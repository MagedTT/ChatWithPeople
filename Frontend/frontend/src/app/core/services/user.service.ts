import { Injectable } from '@angular/core';
import { UserDto } from '../../shared/models/UserDto';
import { HttpClient } from '@angular/common/http';
import { FriendMinimalInformation } from '../../shared/models/FriendMinimalInformation';
import { StateService } from '../../shared/services/state.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private httpClient: HttpClient, private state: StateService) { }

  getUserById(userId: string) {
    return this.httpClient.get<UserDto>(`https://localhost:7079/api/Users/${userId}`);
  }

  getOnlineFriendsByUserId(userId: string | null) {
    return this.httpClient.get<FriendMinimalInformation[]>(`https://localhost:7079/api/Friendships/OnlineFriends/${userId}`);
  }

  getFriendRequestsTotalCountByUserId(userId: string | null) {
    this.httpClient.get<number>(`https://localhost:7079/api/FriendRequests/${userId}/TotalCount`).subscribe((totalCount: number) => {
      this.state.friendRequestsCount.set(totalCount);
    });
  }
}
