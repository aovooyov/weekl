import { Component, OnInit, Input, Renderer, OnDestroy, Inject, PLATFORM_ID } from "@angular/core";
import { isPlatformBrowser } from "@angular/common";

@Component({
    selector: 'sidebar',
    template: `
    <nav class="sidebar w-100 h-100" [class.open]="open">
        <div class="backdrop fade" (click)="toggle()"></div>
        <div class="container-fluid">
            <div class="row justify-content-end h-100">
                <div class="col-xs-12 col-sm-12 col-md-9 col-lg-6 h-100 bg-light shadow" style="overflow-y: auto">
                    <div class="row mt-3">
                        <div class="col-md">
                            <button class="btn btn-outline-dark" type="button" (click)="toggle()">
                                <i class="fas fa-fw fa-angle-right fa-2x"></i>
                            </button>
                        </div>
                    </div>
                    <ng-content></ng-content>
                </div>
            </div>
        </div>
    </nav>`
})
export class SideBarComponent implements OnInit, OnDestroy {

    open: boolean = false;
    
    @Input()
    width: string = 'w-50';

    constructor(
        private renderer: Renderer,
        @Inject(PLATFORM_ID) 
        private platformId) {        
    }

    ngOnInit() {

    }

    ngOnDestroy(): void {
        if(isPlatformBrowser(this.platformId)) {
            this.renderer.setElementClass(document.body, 'sidebar-open', false);
        }
    }

    toggle() {
        this.open = !this.open;

        if(isPlatformBrowser(this.platformId)) {
            this.renderer.setElementClass(document.body, 'sidebar-open', this.open);
        }
    }
}