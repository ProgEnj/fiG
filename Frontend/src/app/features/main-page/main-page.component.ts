import { Component, inject, OnInit } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { GifSearchComponent } from '../gif-search/gif-search.component';
import { FileService } from '../../core/services/file.service';
import { environment } from '../../../environments/environment';
import { GifItem } from '../../core/model/gif-item';

@Component({
  selector: 'app-main-page',
  imports: [GifSearchComponent, RouterOutlet, RouterLink],
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.scss'
})
export class MainPageComponent implements OnInit {
  private _fileService = inject(FileService);
  private cols: number = 4;
  private gifDisplayGrid: Array<Array<GifItem>> = [];
  private mobileGifDisplayGrid: Array<Array<GifItem>> = [];
  public static: string = environment.static;

  ngOnInit(): void {
    this._fileService.GetMainPageGifs().subscribe(res => {
      let gifs = res.body?.gifItems;
      let len = res.body?.gifItems.length;

      if(len == undefined || len < this.cols) {
        throw new Error("No gifs were send");
      }

      for(let i = 0; i < this.cols; i++) {
        this.gifDisplayGrid.push([]);
      }

      // spread array of gifItems across 4 arrays that represent columns
      let currentCol = 0;
      for(let i = 0; i < len; i++) {
        if(currentCol == 4) {
          currentCol = 0;
        }
        this.gifDisplayGrid[currentCol].push(gifs![i]);
        currentCol += 1;
      }

      // form mobile grid
      this.mobileGifDisplayGrid.push(this.gifDisplayGrid[0].concat(this.gifDisplayGrid[1]));
      this.mobileGifDisplayGrid.push(this.gifDisplayGrid[2].concat(this.gifDisplayGrid[3]));
    });
  }

  GetGrid(): Array<Array<GifItem>> {
    return this.gifDisplayGrid;
  }

  GetMobileGrid(): Array<Array<GifItem>> {
    return this.mobileGifDisplayGrid;
  }
}
