import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { firstValueFrom, Observable } from 'rxjs';
import { UserRegisterRequest } from '../model/user-register-request.dto';
import { UserLoginRequest } from '../model/user-login-request.dto';
import { UserLoginResponse } from '../model/user-login-response.dto';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private http: HttpClient = inject(HttpClient);

  private cred = {username: 'username', jwtToken: 'token'};

  GetToken(): string | null {
    return localStorage.getItem(this.cred.jwtToken);
  }

  SetToken(token: string): void {
    localStorage.setItem(this.cred.jwtToken, token);
  }

  GetUsername(): string | null {
    return localStorage.getItem(this.cred.username);
  }

  SetUsername(username: string): void {
    localStorage.setItem(this.cred.username, username);
  }

  ClearLoginInfo() {
    localStorage.removeItem(this.cred.jwtToken);
    localStorage.removeItem(this.cred.username)
  }

  async RefreshToken(): Promise<boolean> {
      const res = await firstValueFrom(this.http.get<UserLoginResponse>(
        environment.host + '/auth/refreshaccess', 
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
    if(localStorage.getItem(this.cred.jwtToken) != null) {
      return true;
    }
    else {
      return false;
    }
  }

  Register(dto: UserRegisterRequest): Observable<any> {
    return this.http.post(environment.host + '/auth/register', dto, { observe: 'response' });
  }

  LogIn(dto: UserLoginRequest): Observable<UserLoginResponse> {
    return this.http.post<UserLoginResponse>(environment.host + '/auth/login', dto, { withCredentials: true });
  }

  LogOut(): Observable<any> {
    return this.http.post(environment.host + '/auth/logout', null, { withCredentials: true, observe: 'response' });
  }

}
