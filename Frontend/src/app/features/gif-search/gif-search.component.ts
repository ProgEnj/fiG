import { Component, inject } from '@angular/core';
import { SearchService } from '../../core/services/search.service';
import { SearchResponse } from '../../core/model/search-response.dto';
import { SearchResponseItem } from '../../core/model/search-response-item';
import { environment } from '../../../environments/environment';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-gif-search',
  imports: [RouterLink],
  templateUrl: './gif-search.component.html',
  styleUrl: './gif-search.component.scss'
})
export class GifSearchComponent {
  private _searchService = inject(SearchService);
  public foundItems: SearchResponseItem[] | undefined = undefined;
  public static: string = environment.static; 

  onInputChange(event: any) {
    let input: string = event.target.value;
    if(input.length > 0) {
      input.includes("#") ? this.SearchByTags(input) : this.SearchByName(input);
    } 
    else {
      this.foundItems = undefined;
    }
  }

  SearchByName(name: string): void {
    this._searchService.SearchByName(name)
    .subscribe(res => {
      res.status == 404 ? "" :  this.foundItems = res.body?.searchItems;
    });
  }

  SearchByTags(tags: string): void {
    this._searchService.SearchByTags(tags.replaceAll('#', '').split(" "))
    .subscribe(res => {
      res.status == 404 ? "" :  this.foundItems = res.body?.searchItems;
    });
  }

}
