import { Component, Input } from '@angular/core';
import { MessageDto } from '../../../../shared/models/MessageDto';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-received-message',
  imports: [DatePipe],
  templateUrl: './received-message.component.html',
  styleUrl: './received-message.component.css'
})
export class ReceivedMessageComponent {
  @Input() message!: MessageDto;


}
