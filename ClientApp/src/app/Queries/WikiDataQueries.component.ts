import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-queries-component',
  templateUrl: './WikiDataQueries.component.html',
})

export class WikiDataQueriesComponent {
  value: string;
  data: Query = new Query("", "", "", "complex");
  http: HttpClient;
  baseUrl: string;
  queries: Query[] = new Array(10);
  key: string;
  i = 0;
  submitted: boolean;
  newquery: boolean;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
    this.submitted = false;
    this.newquery = false;
  }

  categories = ['שחקנים הישראליים', 'במאים הישראליים', 'מפיקים הישראליים'];
  categories2 = ['שנולדו בעיר', 'שנולדו בשנה', 'שנולדו לאחר שנת'];

  onSubmit() {
    this.submitted = true;
    this.queries[this.i] = this.data;
    this.queries[this.i + 1] = new Query("done", "done", "done", "complex");
    this.http.post(this.baseUrl + "weatherforecast", this.queries).subscribe(
      (response) => console.log(response),
      (error) => console.log(error)
    );
    this.i = 0;
    for (var j = 0; j < 10; j++) {
      this.queries[j] = null;
    }
  }

  newQuery() {
    this.newquery = true
    //this.queries = new Array(10);
    this.queries[this.i] = this.data;
    this.i++;
    this.data = new Query(this.queries[this.i - 1].key1, '', '', "complex");
  }
}

class Query {
  constructor(
    public key1: string,
    public key2: string,
    public value: string,
    public type: string) { }
}
