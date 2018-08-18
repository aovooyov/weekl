import { Pipe, PipeTransform, ChangeDetectorRef } from '@angular/core';
import * as moment from 'moment';

@Pipe({ name: 'moment', pure: true })
export class MomentPipe implements PipeTransform {

    constructor(
        public ref: ChangeDetectorRef) {
        moment.locale('ru');
    }

    transform(value: any, args: any): any {
        if (!args) {
            return moment(value).format("LLL");
        }

        switch (args) {
            case "ago":
                return moment(value).fromNow();
            case "LL":
                return moment(value).format("LL");
            case "month":
                return moment(value).format("MMMM");
            case "year":
                return moment(value).format("YYYY");
            case "dayWeek": 
                var momentDate = moment(value);
                return momentDate.isSame(moment(), 'day') ? 'сегодня' : momentDate.format('dddd');
            case "day":
                var momentDate = moment(value);
                return momentDate.isSame(moment(), 'day') ? momentDate.format('dddd D MMMM') : momentDate.format('D MMMM');            
            default:
                return moment(value).format(args);
        }
    }
}
