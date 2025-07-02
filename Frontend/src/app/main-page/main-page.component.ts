import { Component } from '@angular/core';
import { GifSearchComponent } from '../gif-search/gif-search.component';

@Component({
  selector: 'app-main-page',
  imports: [GifSearchComponent],
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.scss'
})
export class MainPageComponent {

}
