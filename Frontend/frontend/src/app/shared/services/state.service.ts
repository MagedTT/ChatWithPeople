import { computed, Injectable, Signal, signal, WritableSignal } from '@angular/core';
import { FriendRequestDto } from '../models/FriendRequestDto';

@Injectable({
  providedIn: 'root'
})
export class StateService {
  friendRequestsCount: WritableSignal<number> = signal<number>(0);
  friendsCount: WritableSignal<number> = signal<number>(0);
  receivedFriendRequests: WritableSignal<FriendRequestDto[]> = signal<FriendRequestDto[]>([]);
  sentFriendRequests: WritableSignal<FriendRequestDto[]> = signal<FriendRequestDto[]>([]);


  friendRequestsCountComputed: Signal<number> = computed(() => this.friendRequestsCount());
  friendsCountComputed: Signal<number> = computed(() => this.friendsCount());
  receivedFriendrequestsCountComputed: Signal<number> = computed(() => this.receivedFriendRequests().length);
  sentFriendrequestsCountComputed: Signal<number> = computed(() => this.sentFriendRequests().length);

  constructor() { }
}
