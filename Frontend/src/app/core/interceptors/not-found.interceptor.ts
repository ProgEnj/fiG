import { HttpEventType, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs';

export const notFoundInterceptor: HttpInterceptorFn = (req, next) => {
  let router = inject(Router);
  return next(req).pipe(tap(event => {
      if(event.type === HttpEventType.Response) {
        if(event.status == 404) router.navigateByUrl('/**');
      }
  }));
};
