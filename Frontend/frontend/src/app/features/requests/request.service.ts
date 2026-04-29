import { Injectable, signal } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { StateService } from '../../shared/services/state.service';
import { FriendRequestDto } from '../../shared/models/FriendRequestDto';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class RequestService {
  currentUserId = signal<string | null>(null);

  constructor(
    private authService: AuthService,
    private httpClient: HttpClient,
    private state: StateService
  ) {
    this.currentUserId.set(this.authService.currentUserId());
  }

  getReceivedFriendRequests() {
    this.httpClient.get<FriendRequestDto[]>(`https://localhost:7079/api/FriendRequests/${this.currentUserId()}/Received`).subscribe((friendRequestDtos: FriendRequestDto[]) => {
      this.state.receivedFriendRequests.set(friendRequestDtos);
    });
  }

  getSentFriendRequests() {
    this.httpClient.get<FriendRequestDto[]>(`https://localhost:7079/api/FriendRequests/${this.currentUserId()}/Sent`).subscribe((friendRequestDtos: FriendRequestDto[]) => {
      this.state.sentFriendRequests.set(friendRequestDtos);
    });
  }

  acceptFriendRequest(friendRequestId: string) {
    this.httpClient.post(`https://localhost:7079/api/FriendRequests/${friendRequestId}/accept`, {}).subscribe(() => {
      this.state.receivedFriendRequests.update(receivedRequest => receivedRequest.filter(x => x.id !== friendRequestId));

      this.state.friendsCount.update(value => value + 1);
    });
  }

  rejectReceivedFriendRequest(friendRequestId: string) {
    this.httpClient.post(`https://localhost:7079/api/FriendRequests/${friendRequestId}/reject`, {}).subscribe(() => {
      this.state.receivedFriendRequests.update(receivedRequest => receivedRequest.filter(x => x.id !== friendRequestId));
      this.state.friendRequestsCount.update(x => x - 1);
    });
  }

  cancelSentFriendRequest(friendRequestId: string) {
    this.httpClient.post(`https://localhost:7079/api/FriendRequests/${friendRequestId}/reject`, {}).subscribe(() => {
      this.state.sentFriendRequests.update(sentRequest => sentRequest.filter(x => x.id !== friendRequestId));
      this.state.friendRequestsCount.update(x => x - 1);
    });
  }
}
