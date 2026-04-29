import { Component, Input } from '@angular/core';
import { MessageDto } from '../../../../shared/models/MessageDto';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-sent-message',
  imports: [DatePipe],
  templateUrl: './sent-message.component.html',
  styleUrl: './sent-message.component.css'
})
export class SentMessageComponent {
  @Input() message!: MessageDto;
}
