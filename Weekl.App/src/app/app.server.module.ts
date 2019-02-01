import { NgModule } from '@angular/core';
import { ServerModule, ServerTransferStateModule } from '@angular/platform-server';
import { ModuleMapLoaderModule } from '@nguniversal/module-map-ngfactory-loader';
import { ConfigLoader, ConfigModule } from '@ngx-config/core';

import { AppModule } from './app.module';
import { AppComponent } from './app.component';

import { Observable } from 'rxjs';
const fs = require('fs');

export class ConfigUniversalLoader implements ConfigLoader {

    constructor(private path: string) {
    }

    loadSettings() {
        return Observable
            .create(observer => {
                observer.next(JSON.parse(fs.readFileSync(this.path, 'utf8')));
                observer.complete();
            })
            .toPromise();
    }
}

export function configUniversalFactory() {
    return new ConfigUniversalLoader('./dist/browser/assets/config.json');
}

@NgModule({
  imports: [
    AppModule,
    ServerModule,
    ServerTransferStateModule,
    ModuleMapLoaderModule,
    ConfigModule.forRoot({
      provide: ConfigLoader,
      useFactory: (configUniversalFactory),
      deps: []
    })
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppServerModule {}
