import { Component } from '@angular/core';
import { FilterFriendsComponent } from '../../components/filter-friends/filter-friends.component';
import { FriendCardComponent } from '../../components/friend-card/friend-card.component';

@Component({
  selector: 'app-friends',
  imports: [FilterFriendsComponent, FriendCardComponent],
  templateUrl: './friends.component.html',
  styleUrl: './friends.component.css'
})
export class FriendsComponent {

}
