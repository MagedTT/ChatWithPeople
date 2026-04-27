using Shared.DTOs;
using Shared.RequestFeatures;

namespace Service.Contracts;

public interface IMessageService
{
    Task<(IEnumerable<MessageDto> messages, MessageMetaData messageMetaData)> GetAllMessagesByConversationIdAsync(Guid conversationId, MessageParameters messageParameters, bool trackChanges);
    Task<MessageDto> CreateMessageForConversationAsync(Guid conversationId, MessageForCreationDto messageForCreationDto);
    Task DeleteMessageByConversationIdAsync(Guid conversationId, Guid messageId, bool trackChanges);
}