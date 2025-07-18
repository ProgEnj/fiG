import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, RouterOutlet } from '@angular/router';
import { FileService } from '../../core/services/file.service';
import { GifItem } from '../../core/model/gif-item';
import { GifSearchComponent } from '../gif-search/gif-search.component';

@Component({
  selector: 'app-gif-page',
  imports: [RouterOutlet, GifSearchComponent],
  templateUrl: './gif-page.component.html',
  styleUrl: './gif-page.component.scss',
})
export class GifPageComponent implements OnInit {
  private _fileService: FileService = inject(FileService);
  private _activatedRoute: ActivatedRoute = inject(ActivatedRoute);
  public gif: GifItem | null  = null;

  ngOnInit(): void {
    let id = this._activatedRoute.snapshot.paramMap.get("id");
    this._fileService.GetGifById(id!).subscribe(res => {
      this.gif = res.body!;
    });
  }

}
