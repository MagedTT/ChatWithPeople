import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, shareReplay } from 'rxjs';
import { UserForDiscover } from '../models/UserForDiscover.model';

@Injectable({
  providedIn: 'root'
})
export class DiscoverService {

  constructor(private httpClient: HttpClient) { }

  getAllUsersForDiscover(): Observable<UserForDiscover[]> {
    return this.httpClient.get<UserForDiscover[]>('https://localhost:7079/api/Users/Discover');
  }

  getImageUrl(base64: string | null): string {
    if (!base64) return 'assets/default.png';
    return `data:image/png;base64,${base64}`;
  }

  // getImageUrl(byteArray: number[]): string {
  //   const binary = byteArray.map(b => String.fromCharCode(b)).join('');
  //   const base64 = window.btoa(binary);
  //   return `data:image/png;base64,${base64}`;
  // }
}
