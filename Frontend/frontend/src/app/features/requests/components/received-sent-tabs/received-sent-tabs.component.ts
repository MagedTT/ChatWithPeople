import { NgStyle } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-received-sent-tabs',
  imports: [NgStyle],
  templateUrl: './received-sent-tabs.component.html',
  styleUrl: './received-sent-tabs.component.css'
})
export class ReceivedSentTabsComponent {
  @Input() receivedRequestsCount: number = 0;
  @Input() sentRequestsCount: number = 0;

  @Output() toggleReceivedSentButton = new EventEmitter<string>();
  toggleColor: boolean = false;

  sendToFriendRequeststheToggleReceivedSentButtonStatus(receivedSentStatus: string) {
    this.toggleReceivedSentButton.emit(receivedSentStatus);
  }

  onToggleColor() {
    this.toggleColor = !this.toggleColor;
  }
}
