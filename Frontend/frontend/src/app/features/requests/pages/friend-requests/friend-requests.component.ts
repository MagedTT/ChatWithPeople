import { Component } from '@angular/core';
import { ReceivedSentTabsComponent } from '../../components/received-sent-tabs/received-sent-tabs.component';

@Component({
  selector: 'app-friend-requests',
  imports: [ReceivedSentTabsComponent],
  templateUrl: './friend-requests.component.html',
  styleUrl: './friend-requests.component.css'
})
export class FriendRequestsComponent {
  received: boolean = false;

  onReceivedSentButtonClicked(value: string) {
    if (value === 'received')
      this.received = true;
    else if (value === 'sent')
      this.received = false;
  }
}
