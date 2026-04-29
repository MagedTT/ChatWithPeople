import { Component, Input } from '@angular/core';
import { UserForDiscover } from '../../models/UserForDiscover.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-card',
  imports: [],
  templateUrl: './user-card.component.html',
  styleUrl: './user-card.component.css'
})
export class UserCardComponent {
  @Input() user: UserForDiscover | null = null;

  constructor(private router: Router) { }

  getImageUrl(base64: string | null): string {
    if (!base64) return 'assets/default.png';
    return `data:image/png;base64,${base64}`;
  }

  navigateToConversation(id: string | undefined) {
    this.router.navigate(['conversation', id]);
  }
}
