import { Component, Input } from '@angular/core';
import { UserForDiscover } from '../../models/UserForDiscover.model';

@Component({
  selector: 'app-user-card',
  imports: [],
  templateUrl: './user-card.component.html',
  styleUrl: './user-card.component.css'
})
export class UserCardComponent {
  @Input() user: UserForDiscover | null = null;

  getImageUrl(base64: string | null): string {
    if (!base64) return 'assets/default.png';
    return `data:image/png;base64,${base64}`;
  }

}
