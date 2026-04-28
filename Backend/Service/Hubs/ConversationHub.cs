using Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Service.Contracts.HubContracts;

namespace ChatWithPeople.Hubs;

[Authorize]
public class ConversationHub : Hub<IConversationClient>
{
    public async Task MarkAsReadAsync(Guid conversationId, Guid userId)
    {
        await Clients.User(Context.UserIdentifier).MessagesSeenByUserIdInConversationWithId(conversationId, userId);
    }

    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception);
    }
}