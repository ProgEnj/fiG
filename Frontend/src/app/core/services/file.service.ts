import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthenticationService } from './authentication.service';
import { MainPageGifsResponse } from '../model/main-page-gifs-response.dto';
import { GifItem } from '../model/gif-item';

@Injectable({
  providedIn: 'root'
})
export class FileService {
  private _http = inject(HttpClient);
  private _authService = inject(AuthenticationService);

  UploadGif(formData: FormData): Observable<any> {
    let headers = new HttpHeaders().set("Authorization", "Bearer " + this._authService.GetToken());

    return this._http.post(environment.host + "/file/upload", formData, 
      { withCredentials: true, observe: "response", headers: headers }
    );
  }

  GetMainPageGifs(): Observable<HttpResponse<MainPageGifsResponse>> {
    return this._http.get<MainPageGifsResponse>(environment.host + "/file/mainpage", { observe: "response" });
  }

  GetGifById(id: string): Observable<HttpResponse<GifItem>> {
    return this._http.get<GifItem>(environment.host + `/file/${id}`, { observe: "response" });
  }

  DeleteGif(id: string): Observable<any> {
    let headers = new HttpHeaders().set("Authorization", "Bearer " + this._authService.GetToken());

    return this._http.post(environment.host + `/file/delete/${id}`, {},{ withCredentials: true, observe: "response", headers: headers });
  }
}
