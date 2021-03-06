import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app- queries - component',
  templateUrl: './Query.component.html',
})

export class QueryComponent {
  value: string;
  data: Query = new Query("", "", "", "single");
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

  categories = ['מידע על אמן - הכנס שם אמן', 'מידע על סרט - הכנס שם סרט', 'שחקנים לפי סרט - הכנס שם סרט'];

  onSubmit() {
    this.submitted = true;
    this.queries[this.i] = this.data;
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
    this.queries[this.i] = this.data;
    this.data = new Query('', '', '', "single");
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
