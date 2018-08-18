import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MomentPipe } from "./moment.pipe";
import { LinkPipe } from './link.pipe';

@NgModule({
    imports: [
        CommonModule
    ],
    declarations: [
        MomentPipe,
        LinkPipe
    ],
    exports: [
        MomentPipe,
        LinkPipe
    ]
})
export class PipesModule { }
