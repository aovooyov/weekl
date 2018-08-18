import { SourceItem } from './source-item.model';

export class ArticleItem {
    id: number;
    title: string;
    description: string;
    link: string;
    date: Date;
    imageUrl: string;
    unique: string;
    source: SourceItem;

    constructor() {
        this.source = new SourceItem();
    }
}

export class ArticleItemApp extends ArticleItem {
    header: boolean;
}