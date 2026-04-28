using AutoMapper;
using ChatWithPeople.Hubs;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Service.Contracts;
using Service.Contracts.HubContracts;
using Shared.DTOs;
using Shared.RequestFeatures;

namespace Service;

public class MessageService : IMessageService
{
    private readonly UserManager<User> _userManager;
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;
    private readonly IMapper _mapper;
    private readonly IHubContext<ConversationHub, IConversationClient> _hub;

    public MessageService(UserManager<User> userManager, IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper, IHubContext<ConversationHub, IConversationClient> hub)
    {
        _userManager = userManager;
        _repositoryManager = repositoryManager;
        _loggerManager = loggerManager;
        _mapper = mapper;
        _hub = hub;
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

        await _repositoryManager.SaveAsync();

        MessageDto messageDto = _mapper.Map<MessageDto>(message);

        IEnumerable<ConversationParticipant> conversationParticipants = await _repositoryManager.ConversationParticipantRepository.GetAllConversationParticipantsExceptSenderByConversationIdAsync(conversationId, messageForCreationDto.SenderId, trackChanges: false);

        foreach (ConversationParticipant conversationParticipant in conversationParticipants)
        {
            await _hub.Clients.User(conversationParticipant.UserId.ToString()).ReceiveMessage(messageDto);
        }

        return messageDto;
    }

    public async Task MarkAsReadAsync(Guid conversationId, Guid senderId, Guid receiverId)
    {
        await _repositoryManager.MessageRepository.MarkAllMessagesAsReadAsync(conversationId, receiverId);
        await _repositoryManager.SaveAsync();

        await _hub.Clients.User(senderId.ToString()).MessagesSeenByUserIdInConversationWithId(conversationId, receiverId);
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
        Message? message = await _repositoryManager.MessageRepository.GetMessageByIdAsync(messageId, trackChanges);
        if (message is null)
            throw new MessageNotFound($"Message with Id: {messageId} not found");

        return message;
    }
}