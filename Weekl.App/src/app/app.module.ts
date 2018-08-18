import { NgModule} from '@angular/core';
import { BrowserModule, BrowserTransferStateModule } from '@angular/platform-browser';
import { HttpClientModule } from "@angular/common/http";
import { FormsModule }    from '@angular/forms';

import { RoutingModule } from './app.routes';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { PipesModule } from './pipes/pipes.module';

import { ApiService } from './services/api.service';
import { AppComponent } from './app.component';
import { MainComponent } from './components/main.component';
import { ArticleComponent } from './components/article.component'

import { PLATFORM_ID, APP_ID, Inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

@NgModule({
  imports: [
    BrowserModule.withServerTransition({ appId: 'weekl' }),
    BrowserTransferStateModule,
    HttpClientModule,
    FormsModule,
    RoutingModule,
    InfiniteScrollModule,
    PipesModule
  ],
  declarations: [
    AppComponent,
    MainComponent,
    ArticleComponent
  ],
  providers: [
    ApiService
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  
  constructor(
    @Inject(PLATFORM_ID) private platformId: Object,
    @Inject(APP_ID) private appId: string) {

    const platform = isPlatformBrowser(platformId) 
      ? 'in the browser' 
      : 'on the server';

    console.log(`Running ${platform} with appId=${appId}`);
  }
}
