import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { SearchResult } from './model/searchresult'

const URL = 'https://localhost:44309/api/search';

@Injectable({
  providedIn: 'root'
})
export class SearchService {

  constructor(private http: HttpClient) {
  }

  search(term: string, market: string): Observable<SearchResult[]> {
    if (term === '')
      return of([])
    let url = URL + '?q=' + term;
    if (market)
      url = url + '&market=' + market;
    return this.http.get<SearchResult[]>(url);
  }
}
