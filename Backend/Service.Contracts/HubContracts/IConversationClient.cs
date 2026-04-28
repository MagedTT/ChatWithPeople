using Shared.DTOs;

namespace Service.Contracts.HubContracts;

public interface IConversationClient
{
    Task ReceiveMessage(MessageDto messageDto);
    Task MessagesSeenByUserIdInConversationWithId(Guid conversationId, Guid userId);
}