import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { TokenService } from '../services/token.service';
import { Router } from '@angular/router';
import { catchError, from, switchMap, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const tokenService = inject(TokenService);
  const authService = inject(AuthService);
  const router = inject(Router);

  const appToken = (request: typeof req) => {
    const token = tokenService.getAccessToken();

    return token ? request.clone({ setHeaders: { Authorization: `Bearer ${token}` } }) : request;
  };

  return next(appToken(req)).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        return from(tokenService.refreshAccessToken()).pipe(
          switchMap((success) => {
            if (success) {
              authService.syncUserFromToken();
              return next(appToken(req));
            }
            tokenService.clearTokens(); //////////////////////////////////////////////// <------
            router.navigateByUrl('login');
            return throwError(() => error);
          })
        );
      }

      return throwError(() => error);
    })
  );
};
