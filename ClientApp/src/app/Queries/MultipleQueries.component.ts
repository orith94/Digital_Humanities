import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-queries-component',
  templateUrl: './MultipleQueries.component.html',
})

export class MultipleQueriesComponent {
  value: string;
  data: Query = new Query("", "", "", "multiple");
  http: HttpClient;
  baseUrl: string;
  queries: Query[] = new Array(10);
  key: string;
  i = 0;
  submitted: boolean;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
    this.submitted = false;
  }

  update(value: string) {
    this.key = value;
  }

  categories = ['שם שחקן - הכנס שם שחקן', 'שנה - הכנס שנה',
    'שם במאי - הכנס שם במאי', 'שם מפיק - הכנס שם מפיק', 'שם אמן - הכנס שם אמן', 'שם תסריטאי - הכנס שם תסריטאי'];

  onSubmit() {
    this.submitted = true;
    this.queries[this.i] = this.data;
    this.queries[this.i + 1] = new Query("done", "done", "done", "multiple");
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
    //this.queries = new Array(10);
    this.queries[this.i] = this.data;
    this.data = new Query('', '', '', "multiple");
    this.i++;
  }
}

class Query {
  constructor(
    public key1: string,
    public key2: string,
    public value: string,
    public type: string) { }
}
