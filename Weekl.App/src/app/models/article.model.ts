import { SourceItem } from "./source-item.model";

export class Article 
{
    id: number;
    link: string;
    title: string;
    subTitle: string;
    description: string;
    text: string;
    date: Date;
    imageUrl: string;
    unique: string;

    channelId: number;
    channelName: string;

    source: SourceItem;

    constructor() {
        this.source = new SourceItem();
    }
}