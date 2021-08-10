import { Component, OnInit } from '@angular/core';
import { Observable, of, OperatorFunction } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, switchMap, tap } from 'rxjs/operators';
import { SearchService } from '../shared/services/search.service';
import { MarketService } from '../shared/services/market.service';
import { SearchResult } from '../model/searchresult'

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {

  public model: any;
  searching = false;
  searchFailed = false;
  selectedMarket: string = null;
  options: string[] = []

  constructor(private _service: SearchService, private _marketService: MarketService) { }

  ngOnInit(): void {
    this._marketService.get().subscribe((data) => this.options = data);
  }

  search: OperatorFunction<string, readonly SearchResult[]> = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.searching = true),
      switchMap(term => {
        return this._service.search(term, this.selectedMarket).pipe(map(item => { return item }))
      }
      ),
      tap(() => this.searching = false)
    )
  formatter = (x: { name: string }) => x.name;
}
