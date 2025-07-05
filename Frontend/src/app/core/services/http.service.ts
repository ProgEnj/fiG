import { HttpClient } from '@angular/common/http';
import { host } from "../../app.config";
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpService {
  private http = inject(HttpClient);

}
