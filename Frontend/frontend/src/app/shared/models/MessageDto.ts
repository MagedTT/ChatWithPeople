import { MessageType } from "@microsoft/signalr";

export interface MessageDto {
    id: string;
    senderId: string;
    conversationId: string;
    content: string;
    messageType: MessageType,
    createdAt: Date;
    EditedAt: Date;
    IsDeleted: boolean;
}