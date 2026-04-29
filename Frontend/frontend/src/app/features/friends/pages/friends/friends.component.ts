import { Component, OnInit, signal, WritableSignal } from '@angular/core';
import { FilterFriendsComponent } from '../../components/filter-friends/filter-friends.component';
import { FriendCardComponent } from '../../components/friend-card/friend-card.component';
import { StateService } from '../../../../shared/services/state.service';
import { FriendsService } from '../../services/friends.service';
import { AuthService } from '../../../../core/services/auth.service';

@Component({
  selector: 'app-friends',
  imports: [FilterFriendsComponent, FriendCardComponent],
  templateUrl: './friends.component.html',
  styleUrl: './friends.component.css'
})
export class FriendsComponent implements OnInit {
  currentUserId: WritableSignal<string | null> = signal<string | null>(null);

  constructor(public state: StateService, private friendsService: FriendsService, private authService: AuthService) {
    this.currentUserId.set(this.authService.currentUserId());
  }

  ngOnInit(): void {
    this.friendsService.getFriendsByUserId(this.currentUserId(), null);
    // console.log(this.state.friends());
  }

  searchForFriends(searchString: string) {
    console.log(`Search Stringgggg: ${searchString}`);
    this.friendsService.getFriendsByUserId(this.currentUserId(), searchString);
  }

  unfriend(friendId: string) {
    this.friendsService.unfriend(this.currentUserId(), friendId);
  }
}
