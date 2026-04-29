import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import * as signalR from '@microsoft/signalr';
import { MessageDto } from '../../shared/models/MessageDto';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class MessageSignalRService {
  private connection: signalR.HubConnection;

  private messageReceived = new Subject<MessageDto>();
  private messagesSeenByUserIdInConversationId = new Subject<{ conversationId: string, userId: string }>();

  messageReceived$ = this.messageReceived.asObservable();
  messagesSeenByUserIdInConversationId$ = this.messagesSeenByUserIdInConversationId.asObservable();

  constructor(private tokenService: TokenService) {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7079/hubs/ConversationHub', {
        accessTokenFactory: () => {
          return this.tokenService.getAccessToken() ?? ''
        }
      })
      .withAutomaticReconnect()
      .build();

    this.registerHandlers();

    this.connection.onreconnected(() => {
      this.registerHandlers();
    });
  }

  registerHandlers(): void {
    this.connection.on('ReceiveMessage', (messageDto: MessageDto) => {
      this.messageReceived.next(messageDto);
    });

    this.connection.on('MessagesSeenByUserIdInConversationWithId', (conversationId: string, userId: string) => {
      this.messagesSeenByUserIdInConversationId.next({ conversationId, userId });
    });
  }

  async startConnection(): Promise<void> {
    try {
      if (this.connection.state === signalR.HubConnectionState.Connected) return;

      await this.connection.start();
      console.log("SignalR connected");
    } catch (err) {
      console.error("SignalR connection failed:", err);
    }
  }

  async stopConnection(): Promise<void> {
    await this.connection.stop();
  }
}
