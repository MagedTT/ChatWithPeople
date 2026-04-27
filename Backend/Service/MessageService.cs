using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Service.Contracts;
using Shared.DTOs;
using Shared.RequestFeatures;

namespace Service;

public class MessageService : IMessageService
{
    private readonly UserManager<User> _userManager;
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;
    private readonly IMapper _mapper;

    public MessageService(UserManager<User> userManager, IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper)
    {
        _userManager = userManager;
        _repositoryManager = repositoryManager;
        _loggerManager = loggerManager;
        _mapper = mapper;
    }

    public async Task<(IEnumerable<MessageDto> messages, MessageMetaData messageMetaData)> GetAllMessagesByConversationIdAsync(Guid conversationId, MessageParameters messageParameters, bool trackChanges)
    {
        await CheckIfConversationExistsAsync(conversationId);

        CursoredList<Message> messagesWithCursorMetaData = await _repositoryManager.MessageRepository.GetAllMessagesByConversationIdAsync(conversationId, messageParameters, trackChanges);

        IEnumerable<MessageDto> messageDtos = _mapper.Map<IEnumerable<MessageDto>>(messagesWithCursorMetaData);

        return (messages: messageDtos, messageMetaData: messagesWithCursorMetaData.MessageMetaData);
    }

    public async Task<MessageDto> CreateMessageForConversationAsync(Guid conversationId, MessageForCreationDto messageForCreationDto)
    {
        await CheckIfConversationExistsAsync(conversationId);

        Message message = _mapper.Map<Message>(messageForCreationDto);

        message.ConversationId = conversationId;
        message.MessageType = Entities.Enums.MessageType.Text;
        message.CreatedAt = DateTime.Now;
        message.EditedAt = DateTime.Now;
        message.IsDeleted = false;

        _repositoryManager.MessageRepository.CreateMessage(message);

        MessageRead messageRead = new()
        {
            UserId = messageForCreationDto.SenderId,
            MessageId = message.Id,
            ReadAt = DateTime.Now
        };

        _repositoryManager.MessageReadRepository.CreateMessageRead(messageRead);

        await _repositoryManager.SaveAsync();

        message.MessagesRead.Add(messageRead);

        MessageDto messageDto = _mapper.Map<MessageDto>(message);

        return messageDto;
    }

    public async Task DeleteMessageByConversationIdAsync(Guid conversationId, Guid messageId, bool trackChanges)
    {
        await CheckIfConversationExistsAsync(conversationId);
        Message message = await GetMessageAndCheckIfMessageExistsAsync(messageId, trackChanges);

        _repositoryManager.MessageRepository.DeleteMessage(message);

        await _repositoryManager.SaveAsync();
    }

    private async Task CheckIfConversationExistsAsync(Guid conversationId)
    {
        if (!await _repositoryManager.ConversationRepository.CheckIfConversationExistsByConversationIdAsync(conversationId))
            throw new ConversationNotFound($"Conversation with Id {conversationId} not found");
    }

    private async Task<Message> GetMessageAndCheckIfMessageExistsAsync(Guid messageId, bool trackChanges)
    {
        Message? message = await _repositoryManager.MessageRepository.GetMessageByidAsync(messageId, trackChanges);
        if (message is null)
            throw new MessageNotFound($"Message with Id: {messageId} not found");

        return message;
    }
}