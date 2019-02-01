import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { environment } from '../../environments/environment';
import { ArticleItem } from '../models/article-item.model';
import { Article } from '../models/article.model';
import { SourceItem } from '../models/source-item.model';
import { ChannelItem } from '../models/channel-item.model';

@Injectable()
export class ApiService {

    private host;
    private getArticlesUrl = 'feed/articles/{date}?offset={offset}&take={take}';
    private getArticlesBySourceUrl = 'feed/articles/{source}/{date}?offset={offset}&take={take}';
    private getArticleByIdUrl = 'feed/article/{articleId}';
    private getArticleByUniqueUrl = 'feed/article/{source}/{unique}';
    private getSourcesUrl = 'feed/sources';
    private getChannelsBySourceUrl = 'feed/channels/{sourceId}';

    constructor(
        private http: HttpClient
    ) {
        this.host = environment.host;
    }

    private get<T>(url: string) {
        return this.http.get<T>(this.host + url)
    }

    private post<T>(url: string, body: any) {
        var headers = new HttpHeaders({ 'Content-Type': 'application/json' }); //, 'Access-Control-Allow-Origin': '*'
        var options = { headers: headers }; //, withCredentials: true

        return this.http.post<Response>(this.host + url, JSON.stringify(body), { withCredentials: true }); //headers: headers, 
    }

    public getArticles(source: string, date: Date, offset: number, take: number) {

        let ticks = ((date.getTime() * 10000) + 621355968000000000) - (date.getTimezoneOffset() * 600000000);
        let url: string;

        if(source) {
            url = this.getArticlesBySourceUrl
                .replace(/{source}/, source);
        }
        else {
            url = this.getArticlesUrl;
        }

        url = url
            .replace(/{date}/, ticks.toString())
            .replace(/{offset}/, offset.toString())
            .replace(/{take}/, take.toString());

        return this.get<ArticleItem[]>(url);
    }

    public getArticleById(articleId: number) {

        let url = this.getArticleByIdUrl
            .replace(/{articleId}/, articleId.toString());

        return this.get<Article>(url);
    }

    public getArticleByUnique(source: string, unique: string) {

        let url = this.getArticleByUniqueUrl
            .replace(/{source}/, source)
            .replace(/{unique}/, unique);

        return this.get<Article>(url);
    }

    public getSources() {
        return this.get<SourceItem[]>(this.getSourcesUrl);
    }

    public getChannels(sourceId: number) {
        return this.get<ChannelItem[]>(this.getChannelsBySourceUrl.replace(/{sourceId}/, sourceId.toString()));
    }
}