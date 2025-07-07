import { inject, Injectable } from '@angular/core';
import { host } from '../../app.config';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom, map, Observable } from 'rxjs';
import { UserLoginResponse } from '../model/user-login-response.dto';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  private http: HttpClient = inject(HttpClient);

  private jwtToken = "";

  GetToken(): string {
    return this.jwtToken;
  }

  async RefreshToken(): Promise<boolean> {

      const res = await firstValueFrom(this.http.get<UserLoginResponse>(
        host + '/auth/refreshaccess', 
        { observe: 'response', withCredentials: true }
      ));

      if(res.status === 200) {
        this.jwtToken = res.body!.token;
        return true;
      }
      else {
        return false;
      }
  }
}
