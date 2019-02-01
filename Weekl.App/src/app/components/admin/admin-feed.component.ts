import { Component, OnInit, Inject, PLATFORM_ID, ViewChild } from "@angular/core";
import { TransferState, makeStateKey } from "@angular/platform-browser";
import { isPlatformServer } from "@angular/common";

import { SideBarComponent } from "../sidebar.component";

import { ApiService } from "../../services/api.service";
import { SourceItem } from "../../models/source-item.model";
import { ChannelItem } from "../../models/channel-item.model";
import { ArticleItem } from "../../models/article-item.model";
import { Article } from "../../models/article.model";

const sourcesState = makeStateKey('admin-feed-sources');

@Component({
    selector: 'admin-feed',
    templateUrl: './admin-feed.component.html'
})
export class AdminFeedComponent implements OnInit {

    @ViewChild(SideBarComponent) 
    sidebar: SideBarComponent;

    sources: SourceItem[] = <SourceItem[]>[];
    channels: ChannelItem[] = <ChannelItem[]>[];
    articles: ArticleItem[] = <ArticleItem[]>[];
    article: Article;
    channel: ChannelItem;

    constructor(
        private api: ApiService,
        private state: TransferState,
        @Inject(PLATFORM_ID) 
        private platformId,
    ) {
        
    }

    ngOnInit() {
        
        if(this.state.hasKey(sourcesState)) {
            this.sources = this.state.get<SourceItem[]>(sourcesState, null);
            return;
        }

        this.fetchSources();
    }

    fetchSources() {
        this.api
            .getSources()
            .subscribe(response => {
                this.sources = response;

                if (isPlatformServer(this.platformId)) {
                    this.state.set(sourcesState, this.sources);
                }
            });
    }

    fetchChannels(source: SourceItem) {
        this.api
            .getChannels(source.id)
            .subscribe(response => {
                this.channels = response;
            });
    }

    fetchArticles(source: SourceItem) {
        this.api
            .getArticles(source.unique, new Date(), 0, 3)
            .subscribe(response => {
                this.articles = response;
            });
    }

    selectSource(source: SourceItem) {
        this.article = null;
        this.articles.splice(0, this.articles.length);
        this.fetchChannels(source);
        this.fetchArticles(source);
    }

    selectChannel(channel: ChannelItem) {
        this.channel = channel;
        this.sidebar.toggle();
    }

    selectArticle(article: Article) {
        this.api
            .getArticleById(article.id)
            .subscribe(response => {
                this.article = response;
            });
    }
}