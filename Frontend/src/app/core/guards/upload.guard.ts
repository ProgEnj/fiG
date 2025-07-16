import { CanActivateFn, Router } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';
import { inject } from '@angular/core';

export const uploadGuard: CanActivateFn = (route, state) => {
  let authService = inject(AuthenticationService);
  let router = inject(Router);
  if(authService.IsLoggedIn()) {
    return true;
  }
  else {
    router.navigate([router.url, 'login']);
    return false;
  }
};
