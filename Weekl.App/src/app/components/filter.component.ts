import { Component, OnInit, Inject, PLATFORM_ID, ViewChild } from "@angular/core";
import { SourceItem } from "../models/source-item.model";
import { TransferState, makeStateKey } from "@angular/platform-browser";
import { ApiService } from "../services/api.service";
import { isPlatformServer } from "@angular/common";
import { SideBarComponent } from "./sidebar.component";

const sourcesState = makeStateKey('main-sources');

@Component({
    selector: 'filter',
    templateUrl: './filter.component.html'
})
export class FilterComponent implements OnInit {

    @ViewChild(SideBarComponent) 
    sidebar: SideBarComponent;

    subscriptions: number[] = <number[]>[];
    sources: SourceItem[] = <SourceItem[]>[];

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
            // this.state.remove(sourcesState);
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

    selectSource(source: SourceItem) {
        this.subscriptions.push(source.id);
    }

    removeSource(source: SourceItem) {
        var index = this.subscriptions.indexOf(source.id);
        this.subscriptions.splice(index, 1);
    }
}