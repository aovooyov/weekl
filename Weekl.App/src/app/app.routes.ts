import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { MainComponent } from './components/main.component';
import { ArticleComponent } from './components/article.component';

const routes: Routes = [
  { path: '', component: MainComponent },
  { path: ':source/:unique', component: ArticleComponent },
  { path: 'article/:articleId', component: ArticleComponent },
];

@NgModule({
    imports: [ RouterModule.forRoot(routes, { initialNavigation: 'enabled' }) ],
    exports: [ RouterModule ],
    providers: [
    ]
  })
  export class RoutingModule {}