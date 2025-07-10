import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthenticationService } from './authentication.service';

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
}
