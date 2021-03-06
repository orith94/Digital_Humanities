import { BrowserModule } from '@angular/platform-browser';
import { NgModule, Query } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { MultipleQueriesComponent } from './Queries/MultipleQueries.component';
import { QueryComponent } from './Queries/Query.component';
import { WikiDataQueriesComponent } from './Queries/WikiDataQueries.component';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    MultipleQueriesComponent,
    QueryComponent,
    WikiDataQueriesComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'query', component: MultipleQueriesComponent },
      { path: 'personsQuery', component: QueryComponent },
      { path: 'complexQuery', component: WikiDataQueriesComponent }

    ]),
    BrowserAnimationsModule
  ],

  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
