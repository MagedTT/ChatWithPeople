import { Injectable } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { ActivatedRoute } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { MessageDto } from '../../../shared/models/MessageDto';

export enum ConversationType {
  Private = 1,
  Public = 2
}

export interface ConversationDto {
  id: string;
  conversationType: ConversationType;
  createdAt: Date;
}

@Injectable({
  providedIn: 'root'
})
export class ConversationService {

  constructor(private httpClient: HttpClient) { }

  async getOrCreateConversation(userId: string | null, friendId: string): Promise<ConversationDto> {
    return await firstValueFrom(this.httpClient.get<ConversationDto>(`https://localhost:7079/api/Conversations/GetOrCreateConversation/${userId}/${friendId}`));
  }

  deleteConversation(conversationId: string) {
    return this.httpClient.delete(`https://localhost:7079/api/Conversations/${conversationId}`);
  }
}
