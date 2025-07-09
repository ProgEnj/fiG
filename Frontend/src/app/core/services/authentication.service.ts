import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { host } from '../../app.config';
import { firstValueFrom, Observable } from 'rxjs';
import { UserRegisterRequest } from '../model/user-register-request.dto';
import { UserLoginRequest } from '../model/user-login-request.dto';
import { UserLoginResponse } from '../model/user-login-response.dto';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private http: HttpClient = inject(HttpClient);

  GetToken(): string | null {
    return localStorage.getItem("token");
  }

  SetToken(token: string): void {
    localStorage.setItem("token", token);
  }

  async RefreshToken(): Promise<boolean> {
      const res = await firstValueFrom(this.http.get<UserLoginResponse>(
        host + '/auth/refreshaccess', 
        { observe: 'response', withCredentials: true }
      ));

      if(res.status === 200) {
        this.SetToken(res.body!.token);
        return true;
      }
      else {
        return false;
      }
  }

  IsLoggedIn(): boolean {
    if(localStorage.getItem("token") != null) {
      return true;
    }
    else {
      return false;
    }
  }

  Register(dto: UserRegisterRequest): Observable<any> {
    return this.http.post(host + '/auth/register', dto);
  }

  LogIn(dto: UserLoginRequest): Observable<UserLoginResponse> {
    return this.http.post<UserLoginResponse>( host + '/auth/login', dto, { withCredentials: true } );
  }

}
