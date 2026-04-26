import { NgClass, NgStyle } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-received-sent-tabs',
  imports: [NgClass, NgStyle],
  templateUrl: './received-sent-tabs.component.html',
  styleUrl: './received-sent-tabs.component.css'
})
export class ReceivedSentTabsComponent {
  @Output() toggleReceivedSentButton = new EventEmitter<string>();
  toggleColor: boolean = false;

  sendToFriendRequeststheToggleReceivedSentButtonStatus(receivedSentStatus: string) {
    this.toggleReceivedSentButton.emit(receivedSentStatus);
  }

  onToggleColor() {
    this.toggleColor = !this.toggleColor;
  }
}
