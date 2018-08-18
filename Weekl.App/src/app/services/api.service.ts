import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { environment } from '../../environments/environment';
import { ArticleItem } from '../models/article-item.model';
import { Article } from '../models/article.model';

@Injectable()
export class ApiService {

    private host;
    private getArticlesUrl = 'feed/articles/{date}?offset={offset}&take={take}';
    private getArticleByIdUrl = 'feed/article/{articleId}';
    private getArticleByUniqueUrl = 'feed/article/{source}/{unique}';

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

    public getArticles(date: Date, offset: number, take: number) {

        var ticks = ((date.getTime() * 10000) + 621355968000000000) - (date.getTimezoneOffset() * 600000000);

        var url = this.getArticlesUrl
            .replace(/{date}/, ticks.toString())
            .replace(/{offset}/, offset.toString())
            .replace(/{take}/, take.toString());

        return this.get<ArticleItem[]>(url);
    }

    public getArticleById(articleId: number) {

        var url = this.getArticleByIdUrl
            .replace(/{articleId}/, articleId.toString());

        return this.get<Article>(url);
    }

    public getArticleByUnique(source: string, unique: string) {

        var url = this.getArticleByUniqueUrl
            .replace(/{source}/, source)
            .replace(/{unique}/, unique);

        return this.get<Article>(url);
    }
}