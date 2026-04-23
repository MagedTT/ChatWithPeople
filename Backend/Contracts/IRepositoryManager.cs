namespace Contracts;

public interface IRepositoryManager
{
    IUserRepository UserRepository { get; }
    IInterestsRepository InterestsRepository { get; }
    IFriendshipsRepository FriendshipsRepository { get; }
    IFriendRequestRepository FriendRequestRepository { get; }
    Task SaveAsync();
}