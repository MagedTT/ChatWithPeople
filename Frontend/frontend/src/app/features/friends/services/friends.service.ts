import { Injectable } from '@angular/core';
import { StateService } from '../../../shared/services/state.service';
import { HttpClient } from '@angular/common/http';
import { FriendDto } from '../../../shared/models/FriendDto';

@Injectable({
  providedIn: 'root'
})
export class FriendsService {

  constructor(private state: StateService, private httpClient: HttpClient) { }

  getFriendsByUserId(userId: string | null, friendName: string | null) {
    this.httpClient.get<FriendDto[]>(`https://localhost:7079/api/Friendships/${userId}?SearchTerm=${friendName ?? ''}`).subscribe((friendDtos: FriendDto[]) => {
      this.state.friends.set(friendDtos);
    });
  }

  getTotalFriendsCount(userId: string | null) {
    this.httpClient.get<number>(`https://localhost:7079/api/Friendships/TotalFriendsCount/${userId}`).subscribe((count: number) => {
      this.state.friendsCount.set(count);
    });
  }

  unfriend(userId: string | null, friendId: string | null) {
    this.httpClient.delete(`https://localhost:7079/api/Friendships/${userId}/${friendId}`).subscribe(() => {
      this.state.friends.update(friend => friend.filter(x => x.friendId !== friendId));
      this.state.friendsCount.set(this.state.friends().length);
    });
  }
}
