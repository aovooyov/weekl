import { NgModule, Injectable } from '@angular/core';
import { BrowserModule, BrowserTransferStateModule } from '@angular/platform-browser';
import { HttpClientModule, HttpClient } from "@angular/common/http";
import { FormsModule } from '@angular/forms';
import { Location } from '@angular/common'

import { MetaModule, MetaLoader, MetaStaticLoader } from '@ngx-meta/core';
import { ConfigLoader, ConfigModule, ConfigService } from '@ngx-config/core';
import { ConfigHttpLoader } from '@ngx-config/http-loader';

import { RoutingModule } from './app.routes';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { PipesModule } from './pipes/pipes.module';

import { ApiService } from './services/api.service';
import { AppComponent } from './app.component';
import { MainComponent } from './components/main.component';
import { ArticleComponent } from './components/article.component';
import { SideBarComponent } from './components/sidebar.component';
import { FilterComponent } from './components/filter.component';

import { AdminFeedComponent } from './components/admin/admin-feed.component';

import { PLATFORM_ID, APP_ID, Inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

export function metaFactory(config: ConfigService): MetaLoader {
	return new MetaStaticLoader({
		callback: (key: string) => key,
		pageTitlePositioning: config.getSettings('seo.pageTitlePositioning'),
		pageTitleSeparator: config.getSettings('seo.pageTitleSeparator'),
		applicationName: config.getSettings('system.applicationName'),
		applicationUrl: config.getSettings('system.applicationUrl'),
		defaults: {
			title: config.getSettings('seo.defaultPageTitle'),
			description: config.getSettings('seo.defaultMetaDescription'),
			generator: 'ng-seed',
			'og:site_name': config.getSettings('system.applicationName'),
			'og:type': 'website',
			'og:locale': config.getSettings('i18n.defaultLanguage.culture'),
			'og:locale:alternate': config.getSettings('i18n.availableLanguages')
				.map((language: any) => language.culture)
				.toString()
		}
	});
}

export function configFactory(http: HttpClient): ConfigLoader {
	return new ConfigHttpLoader(http, './assets/config.json');
}

@Injectable()
export class UnstripTrailingSlashLocation extends Location {
	public static stripTrailingSlash(url: string): string {
		return url;
	}
}
Location.stripTrailingSlash = UnstripTrailingSlashLocation.stripTrailingSlash;

@NgModule({
  imports: [
    BrowserModule.withServerTransition({ appId: 'weekl' }),
    BrowserTransferStateModule,
    HttpClientModule,
    FormsModule,
    MetaModule.forRoot({
			provide: MetaLoader,
			useFactory: (metaFactory),
			deps: [
				ConfigService
			]
		}),
		ConfigModule.forRoot({
			provide: ConfigLoader,
			useFactory: (configFactory),
			deps: [HttpClient]
		}),
    RoutingModule,
    InfiniteScrollModule,
    PipesModule
  ],
  declarations: [
    AppComponent,
    MainComponent,
		ArticleComponent,
		SideBarComponent,
		FilterComponent,

		AdminFeedComponent
  ],
  providers: [
    ApiService
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  
  constructor(
    @Inject(PLATFORM_ID) 
    private platformId: Object,
    @Inject(APP_ID) 
    private appId: string) {

    const platform = isPlatformBrowser(platformId) ? "in the browser" : "on the server";
    console.log(`Running ${platform} with appId=${appId}`);
  }
}
