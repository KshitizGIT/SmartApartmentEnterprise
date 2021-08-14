import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { from, Observable, of } from 'rxjs';
import { SearchResult } from '../../model/searchresult'
import { AuthService } from './auth.service';

const URL = 'https://localhost:44309/api/search';

@Injectable({
  providedIn: 'root'
})
export class SearchService {

  constructor(private http: HttpClient, private auth: AuthService) {
  }

  search(term: string, market: string): Observable<SearchResult[]> {
    let url = URL + '?q=' + term;
    if (market) {
      url = url + '&market=' + market;
    }
    // default limit 6 for autocomplete
    url = url + '&limit=6';
    return this.http.get<SearchResult[]>(url);
  }
}
