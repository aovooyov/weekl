import { Component, OnInit, Inject, PLATFORM_ID } from '@angular/core';
import { TransferState, makeStateKey, Meta, Title } from '@angular/platform-browser';
import { isPlatformServer } from '@angular/common';

import { ApiService } from '../services/api.service';
import { ArticleItemApp, ArticleItem } from '../models/article-item.model';

import * as moment from 'moment';
import { ActivatedRoute } from '@angular/router';
import { MetaService } from '@ngx-meta/core';
import { SourceItem } from '../models/source-item.model';

const articlesState = makeStateKey('main-articles');
const dateState = makeStateKey('main-date');

@Component({
  selector: 'main',
  templateUrl: './main.component.html'
})
export class MainComponent implements OnInit {

    lock: boolean;
    articles: ArticleItemApp[] = <ArticleItemApp[]>[];
    cursor: Date;

    offset: number = 0;
    take: number = 10;
    date: Date = new Date();
    sourceUnique: string;
    source: SourceItem;

    constructor(
        private api: ApiService,
        private route: ActivatedRoute,
        private state: TransferState,
        @Inject(PLATFORM_ID) 
        private platformId,
        private meta: MetaService
    ) {
    }

    ngOnInit() {

        this.meta.setTitle('Новости и статьи за неделю');
        this.meta.setTag('description', 'Новостной агрегатор Weekl - новости и статьи за неделю');

        this.sourceUnique = this.route.snapshot.params['source'];
        this.route.params.subscribe(params => {
            let source = params['source'];

            if(source === this.sourceUnique) {
                return;
            }

            this.sourceUnique = source;
            this.date = new Date();
            this.cursor = null;

            this.offset = 0;
            this.articles.splice(0, this.articles.length);
            this.fetchArticles();
        });

        if(this.state.hasKey(dateState)) {
            var state = this.state.get(dateState, null);
            this.date = new Date(state.date);
            this.cursor = new Date(state.cursor);
            this.state.remove(dateState);
        }

        if(this.state.hasKey(articlesState)) {
            this.articles = this.state.get<ArticleItemApp[]>(articlesState, null);
            this.state.remove(articlesState);
            this.setSource(this.articles);
            this.setMeta(this.articles);
            return;
        }

        this.fetchArticles();
    }

    fetch() {
        if(this.lock) {
            return;
        }

        this.lock = true;
        this.offset += 10;
        this.fetchArticles();
    }

    fetchArticles() {
        this.api
            .getArticles(this.sourceUnique, this.date, this.offset, this.take)
            .subscribe(response => {
                if(response.length > 0) {
                    response.forEach(a => this.articles.push(this.createArticleItemApp(a)));
                    this.lock = false;
                    
                    if (isPlatformServer(this.platformId)) {
                        this.state.set(articlesState, this.articles);
                    }

                    if(this.offset == 0) {
                        this.setSource(this.articles);
                        this.setMeta(this.articles);
                    }
                }
            });
    }

    createArticleItemApp(article: ArticleItem) : ArticleItemApp {

        if(!this.cursor) {
            this.cursor = article.date;
       
            if (isPlatformServer(this.platformId)) {
                this.state.set(dateState, { date: this.date, cursor: this.cursor });
            }

            var item = <ArticleItemApp>article;
            item.header = true;
            return item;
        }

        if(moment(this.cursor).isAfter(article.date, 'day')) {
            this.cursor = article.date;

            if (isPlatformServer(this.platformId)) {
                this.state.set(dateState, { date: this.date, cursor: this.cursor });
            }

            var item = <ArticleItemApp>article;
            item.header = true;
            return item;
        }

        return <ArticleItemApp>article;
    }

    setSource(articles: ArticleItemApp[]) {
        if(!this.sourceUnique) {
            return;
        }

        if(articles.length === 0) {
            return;
        }

        var article = articles[0];

        if(article) {
            this.selectSource(article.source);
        }
    }

    selectSource(source: SourceItem) {
        this.source = source;
    }

    setMeta(articles: ArticleItemApp[]) {

        if(this.source) {
            this.meta.setTitle(this.source.name);
        }

        var article = articles.find(a => !!a.imageUrl);        
        if(article) {
            this.meta.setTag('image', article.imageUrl);
            this.meta.setTag('og:image', article.imageUrl);
            this.meta.setTag('vk:image', article.imageUrl);
        }
    }
}
