import { Component, OnInit, signal } from '@angular/core';
import { RouterLink, RouterLinkActive } from "@angular/router";
import { AuthService } from '../../../core/services/auth.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit {
  currentUserId = signal<string | null>(null);
  profilePicture: string = '';

  constructor(private authService: AuthService, private httpClient: HttpClient) {
    this.currentUserId.set(this.authService.currentUserId());
  }

  ngOnInit(): void {
    this.httpClient.get<any>(`https://localhost:7079/api/Users/ProfilePicture/${this.currentUserId()}`).subscribe((profilePictureObject: any) => {
      this.profilePicture = profilePictureObject.profilePicture;
    });
  }

  getImageUrl(base64: string | null | undefined): string {
    if (!base64) return 'assets/default.png';
    return `data:image/png;base64,${base64}`;
  }

  logout() {
    this.authService.logout();
  }
}
