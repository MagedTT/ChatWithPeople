import { computed, inject, Injectable, signal } from '@angular/core';
import { TokenService } from './token.service';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { JwtPayload } from '../models/JwtPayload';
import { Credentials } from '../models/Credentials';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private tokenService = inject(TokenService);
  // private router = inject(Router);
  // private httpClient = inject(HttpClient);

  constructor(private router: Router, private httpClient: HttpClient) {

  }

  private _currentUser = signal<JwtPayload | null>(this.tokenService.getCurrentUserPayload());

  currentUser = this._currentUser.asReadonly();
  currentUserId = computed(() => this._currentUser()?.sub ?? null);
  isAuthenticated = computed(() => this._currentUser() !== null);
  userRoles = computed(() => this._currentUser()?.roles ?? null);

  register(formData: FormData): Observable<any> {
    return this.httpClient.post('https://localhost:7079/api/Authentication/register', formData);
  }

  login(credentials: Credentials) {
    return this.httpClient.post<{ accessToken: string, refreshToken: string }>(
      'https://localhost:7079/api/Authentication/login',
      credentials
    ).pipe(
      tap(({ accessToken, refreshToken }) => {
        this.tokenService.setTokens(accessToken, refreshToken);
        this._currentUser.set(this.tokenService.decodeToken(accessToken));
        this.router.navigate(['discover-home']);
      })
    )
  }

  logout() {
    this.tokenService.clearTokens();
    this._currentUser.set(null);
    this.router.navigateByUrl('login');
  }

  syncUserFromToken() {
    this._currentUser.set(this.tokenService.getCurrentUserPayload());
  }
}
