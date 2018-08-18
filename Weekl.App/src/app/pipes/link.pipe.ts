import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'link', pure: true })
export class LinkPipe implements PipeTransform {

    transform(value: string): any {
        if (!value) {
            return false;
        }

        return this.linkify(value);
    }

    private linkify(source: string) {
        return source.replace(/(https?:\/\/(\S+))/i, '$2').replace(/www./, '').trim();
    }
}