using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service.Contracts;
using Shared.DTOs;

namespace Service;

public class ConversationService : IConversationService
{
    private readonly UserManager<User> _userManager;
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;
    private readonly IMapper _mapper;

    public ConversationService(
        UserManager<User> userManager,
        IRepositoryManager repositoryManager,
        ILoggerManager loggerManager,
        IMapper mapper
        )
    {
        _userManager = userManager;
        _repositoryManager = repositoryManager;
        _loggerManager = loggerManager;
        _mapper = mapper;
    }

    public async Task<ConversationDto> GetConversationByIdAsync(Guid conversationId, bool trackChanges)
    {
        Conversation conversation = await GetConversationAndCheckIfItExistsAsync(conversationId, null, null, trackChanges);

        ConversationDto conversationDto = _mapper.Map<ConversationDto>(conversation);

        return conversationDto;
    }

    public async Task<ConversationDto> GetOrCreateConversationBetweenUserIdAndFriendIdAsync(Guid userId, Guid friendId, bool trackChanges)
    {
        await CheckIfUserExistsAsync(userId);
        await CheckIfUserExistsAsync(friendId);

        if (!await _repositoryManager.FriendshipsRepository.FriendshipExistsAsync(userId, friendId))
            throw new FriendshipNotFound(userId, friendId);

        Conversation? conversation = await _repositoryManager.ConversationRepository.GetConversationByUserIdAndFriendIdAsync(userId, friendId, trackChanges);

        if (conversation is null)
        {
            conversation = new()
            {
                Type = Entities.Enums.ConversationType.Private,
                CreatedAt = DateTime.Now
            };

            _repositoryManager.ConversationRepository.CreateConversation(conversation);

            List<ConversationParticipant> conversationParticipants = new()
            {
                new ConversationParticipant
                {
                    ConversationId = conversation.Id,
                    UserId = userId
                },
                new ConversationParticipant
                {
                    ConversationId = conversation.Id,
                    UserId = friendId
                }
            };

            _repositoryManager.ConversationParticipantRepository.AddConversationParticipantsAsync(conversationParticipants);

            await _repositoryManager.SaveAsync();
        }

        ConversationDto conversationDto = _mapper.Map<ConversationDto>(conversation);

        return conversationDto;
    }

    public async Task DeleteConversationByIdAsync(Guid conversationId, bool trackChanges)
    {
        Conversation conversation = await GetConversationAndCheckIfItExistsAsync(conversationId, null, null, trackChanges);

        _repositoryManager.ConversationRepository.DeleteConversation(conversation);

        await _repositoryManager.SaveAsync();
    }

    private async Task<Conversation> GetConversationAndCheckIfItExistsAsync(Guid? conversationId, Guid? userId, Guid? friendId, bool trackChanges)
    {
        if (conversationId is not null)
        {
            Conversation? conversation = await _repositoryManager.ConversationRepository.GetConversationByIdAsync(conversationId.Value, trackChanges);

            if (conversation is null)
                throw new ConversationNotFound($"Conversation with Id: {conversationId} is not found");

            return conversation;
        }
        else if (userId is not null && friendId is not null)
        {
            Conversation? conversation = await _repositoryManager.ConversationRepository.GetConversationByUserIdAndFriendIdAsync(userId.Value, friendId.Value, trackChanges);

            if (conversation is null)
                throw new ConversationNotFound($"Conversation between users with Ids {userId} and {friendId} is not found");

            return conversation;
        }

        throw new ConversationNotFound("No Conversation found");
    }

    private async Task CheckIfUserExistsAsync(Guid userId)
    {
        if (!await _userManager.Users.AnyAsync(x => x.Id.Equals(userId)))
            throw new UserNotFoundException(userId);
    }

    // public async Task<ConversationDto> CreateConversationBetweenUserIdAndFriendIdAsync(Guid userId, Guid friendId, bool trackChanges)
    // {
    //     await CheckIfUserExistsAsync(userId);
    //     await CheckIfUserExistsAsync(friendId);

    //     if (!await _repositoryManager.FriendshipsRepository.FriendshipExistsAsync(userId, friendId))
    //         throw new FriendshipNotFound(userId, friendId);

    //     Conversation conversation = new()
    //     {
    //         Type = Entities.Enums.ConversationType.Private,
    //         CreatedAt = DateTime.Now
    //     };

    //     _repositoryManager.ConversationRepository.CreateConversation(conversation);

    //     IEnumerable<ConversationParticipant> conversationParticipants = new List<ConversationParticipant>()
    //     {
    //         new ConversationParticipant
    //         {
    //             ConversationId = conversation.Id,
    //             UserId = userId
    //         },
    //         new ConversationParticipant
    //         {
    //             ConversationId = conversation.Id,
    //             UserId = friendId
    //         }
    //     };

    //     ConversationDto conversationDto = _mapper.Map<ConversationDto>(conversation);

    //     _repositoryManager.ConversationParticipantRepository.AddConversationParticipantsAsync(conversationParticipants);

    //     await _repositoryManager.SaveAsync();

    //     return conversationDto;
    // }
}