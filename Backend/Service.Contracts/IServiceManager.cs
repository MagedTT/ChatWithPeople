namespace Service.Contracts;

public interface IServiceManager
{
    IAuthenticationService AuthenticationService { get; }
    IUserService UserService { get; }
    IFriendshipsService FriendshipsService { get; }
    IFriendRequestService FriendRequestService { get; }
    IGroupService GroupService { get; }
    IConversationService ConversationService { get; }
}