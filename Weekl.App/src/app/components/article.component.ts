import { Component, OnInit, Inject, PLATFORM_ID } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TransferState, makeStateKey, Title, Meta } from '@angular/platform-browser';
import { isPlatformServer } from '@angular/common';

import { ApiService } from '../services/api.service';
import { Article } from '../models/article.model';
import { MetaService } from '@ngx-meta/core';

const articleState = makeStateKey('main-article');

@Component({
  selector: 'article',
  templateUrl: './article.component.html'
})
export class ArticleComponent implements OnInit {

    lock: boolean;
    article: Article;

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

        if(this.state.hasKey(articleState)) {
            this.article = this.state.get<Article>(articleState, null);
            this.setMeta(this.article);
            this.state.remove(articleState);
            return;
        }

        var params = this.route.snapshot.params;

        var articleId = +params['articleId'];
        if(articleId) {
            this.fetchArticleById(articleId);
        }

        var source = params['source'];
        var unique = params['unique'];

        if(source && unique) {
            this.fetchArticleByUnique(source, unique);
        }
    }

    fetchArticleById(articleId: number) {
        this.api
            .getArticleById(articleId)
            .subscribe(response => {
                this.article = response;
                this.setMeta(this.article);
                
                if (isPlatformServer(this.platformId)) {
                    this.state.set(articleState, this.article);
                }
            });
    }

    fetchArticleByUnique(source: string, unique: string) {
        this.api
            .getArticleByUnique(source, unique)
            .subscribe(response => {
                this.article = response;
                this.setMeta(this.article);

                if (isPlatformServer(this.platformId)) {
                    this.state.set(articleState, this.article);
                }
            });
    }

    setMeta(article: Article) {
        this.meta.setTitle(article.title);
        this.meta.setTag('description', article.description);

        if(article.imageUrl) {
            this.meta.setTag('image', article.imageUrl);
            this.meta.setTag('og:image', article.imageUrl);
            this.meta.setTag('vk:image', article.imageUrl);
        }
    }
}
