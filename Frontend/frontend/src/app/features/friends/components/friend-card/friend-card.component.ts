import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { FriendDto } from '../../../../shared/models/FriendDto';
import { UserStatus } from '../../../../shared/models/UserStatus';
import { Router, RouterLink } from "@angular/router";

@Component({
  selector: 'app-friend-card',
  imports: [],
  templateUrl: './friend-card.component.html',
  styleUrl: './friend-card.component.css'
})
export class FriendCardComponent {
  @Input() friend: FriendDto | null = null;
  @Output() friendIdToRemove = new EventEmitter<string>();

  private router = inject(Router);

  getImageUrl(base64: string | null | undefined): string {
    if (!base64) return 'assets/default.png';
    return `data:image/png;base64,${base64}`;
  }

  removeFriend(friendId: string) {
    this.friendIdToRemove.emit(friendId);
  }

  navigateToConversation(friendId: string) {
    this.router.navigate(['conversation', friendId]);
  }
}
