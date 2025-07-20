import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { catchError, Observable } from 'rxjs';
import { SearchResponse } from '../model/search-response.dto';

@Injectable({
  providedIn: 'root'
})
export class SearchService {
  private _http = inject(HttpClient);

  SearchByName(query: string): Observable<HttpResponse<SearchResponse>> {
    let params = new HttpParams().set('name', query);

    return this._http.get<SearchResponse>(environment.host + "/search/byname", {
      params: params, observe: 'response'
    });
  } 

  SearchByTags(tags: string[]): Observable<HttpResponse<SearchResponse>> {
    let params = new HttpParams();
    tags.forEach(tag => params = params.append("tags", tag));

    return this._http.get<SearchResponse>(environment.host + "/search/bytags", {
      params: params, observe: 'response'
    });
  } 

}
