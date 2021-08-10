import { Component } from '@angular/core';
import { Observable, of, OperatorFunction } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, switchMap, tap } from 'rxjs/operators';
import { SearchService } from '../search.service';
import {SearchResult} from '../model/searchresult'

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent {

  public model: any;
  searching = false;
  searchFailed = false;
  selectedMarket: string = null;
  options: string[] = ["San francisco", "DFW"]

  constructor(private _service: SearchService) { }

  search: OperatorFunction<string, readonly SearchResult[]> = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      tap(() => this.searching = true),
      switchMap(term => {
        return this._service.search(term, this.selectedMarket).pipe(map(item => {return item }))
      }
      ),
      tap(() => this.searching = false)
    )
    formatter = (x: {name: string}) => x.name;
}
