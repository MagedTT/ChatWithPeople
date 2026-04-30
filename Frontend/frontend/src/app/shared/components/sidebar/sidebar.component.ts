import { Component, OnInit, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { UserService } from '../../../core/services/user.service';
import { AuthService } from '../../../core/services/auth.service';
import { FriendMinimalInformation } from '../../models/FriendMinimalInformation';
import { StateService } from '../../services/state.service';
import { FriendsService } from '../../../features/friends/services/friends.service';

@Component({
  selector: 'app-sidebar',
  imports: [RouterLink],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent implements OnInit {
  onlineFriends: FriendMinimalInformation[] | null = null;
  currentUesrId = signal<string | null>(null);
  constructor(
    private authService: AuthService,
    private userService: UserService,
    public state: StateService,
    private friendsService: FriendsService,
    private router: Router
  ) {
    this.currentUesrId.set(this.authService.currentUserId());
  }

  ngOnInit(): void {
    this.userService.getOnlineFriendsByUserId(this.currentUesrId()).subscribe((friends: FriendMinimalInformation[]) => {
      this.onlineFriends = friends;
    });

    this.friendsService.getTotalFriendsCount(this.currentUesrId());

    this.userService.getFriendRequestsTotalCountByUserId(this.currentUesrId());
  }

  navigateToConversation(friendId: string) {
    this.router.navigate(['conversation', friendId]);
  }

  getImageUrl(base64: string | null | undefined): string {
    if (!base64) return 'assets/default.png';
    return `data:image/png;base64,${base64}`;
  }
}
