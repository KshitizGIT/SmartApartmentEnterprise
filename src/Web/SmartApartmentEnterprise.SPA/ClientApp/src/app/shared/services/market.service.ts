import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
const URL = 'https://localhost:44309/api/markets';

@Injectable({
  providedIn: 'root'
})
export class MarketService {

  constructor(private http: HttpClient) { }

  get(): Observable<string[]> {
    return this.http.get<string[]>(URL);

  }
}
