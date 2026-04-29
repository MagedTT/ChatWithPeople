import { Injectable } from '@angular/core';
import { MessageDto } from '../../shared/models/MessageDto';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { MessageForCreateionDto } from '../../shared/models/MessageForCreationDto';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  constructor(private httpClient: HttpClient) { }

  getAllMessage(conversationId: string, cursorId: string | null, cursorTime: Date | null): Observable<MessageDto[]> {
    return this.httpClient.get<MessageDto[]>(`https://localhost:7079/api/Conversations/${conversationId}/Messages?cursorId=${cursorId}&cursorTime=${cursorTime ? cursorTime.toISOString() : null}`);
  }

  createMessage(conversationId: string, messageForCreationDto: MessageForCreateionDto): Observable<MessageDto> {
    return this.httpClient.post<MessageDto>(`https://localhost:7079/api/Conversations/${conversationId}/Messages/Create`, messageForCreationDto);
  }

  markAsRead(conversationId: string, senderId: string, receiverId: string) {
    return this.httpClient.post(`https://localhost:7079/api/Conversations/${conversationId}/Messages/MarkAsRead/${senderId}/${receiverId}`, {});
  }

  deleteMessageForConversation(conversationId: string, messageId: string) {
    return this.httpClient.post(`https://localhost:7079/api/Conversations/${conversationId}/Messages/Delete/${messageId}`, {});
  }
}
