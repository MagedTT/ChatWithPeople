namespace Contracts;

public interface IRepositoryManager
{
    IUserRepository UserRepository { get; }
    IInterestsRepository InterestsRepository { get; }
    IFriendshipsRepository FriendshipsRepository { get; }
    IFriendRequestRepository FriendRequestRepository { get; }
    IGroupRepository GroupRepository { get; }
    IGroupMemberRepository GroupMemberRepository { get; }
    IConversationRepository ConversationRepository { get; }
    IConversationParticipantRepository ConversationParticipantRepository { get; }
    IMessageRepository MessageRepository { get; }
    Task SaveAsync();
}