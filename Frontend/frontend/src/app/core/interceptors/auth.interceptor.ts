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

  const addToken = (request: typeof req) => {
    const accessToken = tokenService.getAccessToken();

    return accessToken ? request.clone({ setHeaders: { Authorization: `Bearer ${accessToken}` } }) : request;
  };

  return next(addToken(req)).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        return from(tokenService.refreshAccessToken()).pipe(
          switchMap(success => {
            if (success) {
              authService.syncUserFromAccessToken();
              return next(addToken(req));
            }

            tokenService.clearTokens();
            router.navigateByUrl('auth/login');
            return throwError(() => error);
          })
        );
      }

      return throwError(() => error);
    })
  );
};
