import { Component, OnInit } from '@angular/core';
import { ReceivedSentTabsComponent } from '../../components/received-sent-tabs/received-sent-tabs.component';
import { RequestService } from '../../request.service';
import { StateService } from '../../../../shared/services/state.service';

@Component({
  selector: 'app-friend-requests',
  imports: [ReceivedSentTabsComponent],
  templateUrl: './friend-requests.component.html',
  styleUrl: './friend-requests.component.css'
})
export class FriendRequestsComponent implements OnInit {
  received: boolean = false;

  constructor(private requestService: RequestService, public state: StateService) { }

  ngOnInit(): void {
    this.requestService.getReceivedFriendRequests();
    this.requestService.getSentFriendRequests();
  }

  onReceivedSentButtonClicked(value: string) {
    if (value === 'received')
      this.received = true;
    else if (value === 'sent')
      this.received = false;
  }

  cancelSentRequest(requestId: string) {
    this.requestService.cancelSentFriendRequest(requestId);
  }

  rejectReceivedRequest(requestId: string) {
    this.requestService.rejectReceivedFriendRequest(requestId);
  }
}
