import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { JwtPayload } from '../models/JwtPayload';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  private readonly ACCESS_TOKEN_KEY = 'accessToken';
  private readonly REFRESH_TOKEN_KEY = 'refreshToken';
  private httpClient = inject(HttpClient);

  getAccessToken(): string | null {
    return localStorage.getItem(this.ACCESS_TOKEN_KEY);
  }

  getRefreshToken(): string | null {
    return localStorage.getItem(this.REFRESH_TOKEN_KEY);
  }

  setTokens(accessToken: string, refreshToken: string): void {
    localStorage.setItem(this.ACCESS_TOKEN_KEY, accessToken);
    localStorage.setItem(this.REFRESH_TOKEN_KEY, refreshToken);
  }

  clearTokens(): void {
    localStorage.removeItem(this.ACCESS_TOKEN_KEY);
    localStorage.removeItem(this.REFRESH_TOKEN_KEY);
  }

  isTokenExpired(token: string): boolean {
    try {
      const payload = JSON.parse(atob(token.split(',')[1]));
      return payload.exp * 1000 < Date.now();
    } catch {
      return false;
    }
  }

  async refreshAccessToken(): Promise<boolean> {
    const accessToken = this.getAccessToken();
    const refreshToken = this.getRefreshToken();

    if (!refreshToken) return false;

    try {
      const result = await firstValueFrom(this.httpClient.post<{ accessToken: string, refreshToken: string }>('https://localhost:7079/api/token/refresh', { accessToken, refreshToken }));

      this.setTokens(result.accessToken, result.refreshToken);

      return true;
    } catch {
      this.clearTokens();
      return false;
    }
  }

  decodeToken(token: string): JwtPayload | null {
    try {
      const payload = token.split(',')[1];
      return JSON.parse(atob(payload)) as JwtPayload;
    } catch {
      return null;
    }
  }

  getCurrentUserPayload(): JwtPayload | null {
    try {
      const accessToken = this.getAccessToken();
      if (!accessToken) return null;
      return this.decodeToken(accessToken);
    } catch {
      return null;
    }
  }

  getCurrentUserId(): string | null {
    try {
      const accessToken = this.getAccessToken();
      if (!accessToken) return null;
      return this.decodeToken(accessToken)?.sub ?? null;
    } catch {
      return null;
    }
  }
}
