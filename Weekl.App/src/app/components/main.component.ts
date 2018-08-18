import { Component, OnInit, Inject, PLATFORM_ID } from '@angular/core';
import { TransferState, makeStateKey, Meta, Title } from '@angular/platform-browser';
import { isPlatformServer } from '@angular/common';

import { ApiService } from '../services/api.service';
import { ArticleItemApp, ArticleItem } from '../models/article-item.model';

import * as moment from 'moment';

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

    constructor(
        private api: ApiService,
        private state: TransferState,
        @Inject(PLATFORM_ID) 
        private platformId,
        private title: Title,
        private meta: Meta    
    ) {
    }

    ngOnInit() {

        this.title.setTitle('Актуальные новости и статьи за неделю | weekl');

        if(this.state.hasKey(dateState)) {
            var state = this.state.get(dateState, null);
            this.date = new Date(state.date);
            this.cursor = new Date(state.cursor);
            this.state.remove(dateState);
        }

        if(this.state.hasKey(articlesState)) {
            this.articles = this.state.get<ArticleItemApp[]>(articlesState, null);
            this.state.remove(articlesState);
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
            .getArticles(this.date, this.offset, this.take)
            .subscribe(response => {
                if(response.length > 0) {
                    response.forEach(a => this.articles.push(this.createArticleItemApp(a)));
                    this.lock = false;
                    
                    if (isPlatformServer(this.platformId)) {
                        this.state.set(articlesState, this.articles);
                    }

                    if(this.offset == 0) {
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

    setMeta(articles: ArticleItemApp[]) {
        var article = articles.find(a => a.imageUrl !== null);

        this.meta.addTag({ name: 'description', content: article.description });
        this.meta.addTag({ name: 'image', content: article.imageUrl });
    }
}
