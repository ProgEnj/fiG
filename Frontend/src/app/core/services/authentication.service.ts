import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { host } from '../../app.config';
import { Observable } from 'rxjs';
import { UserRegisterRequest } from '../model/user-register-request.dto';
import { UserLoginRequest } from '../model/user-login-request.dto';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private http: HttpClient = inject(HttpClient);

  Register(dto: UserRegisterRequest): Observable<any> {
    return this.http.post(host + '/auth/register', dto);
  }

  LogIn(dto: UserLoginRequest): Observable<any> {
    return this.http.post(host + '/auth/login', dto, { withCredentials: true });
  }

}
