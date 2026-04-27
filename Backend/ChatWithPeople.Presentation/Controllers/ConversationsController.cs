using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DTOs;
using Shared.RequestFeatures;

namespace ChatWithPeople.Presentation.Controllers;

[ApiController]
[Route("api/Conversations")]
public class ConversationsController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public ConversationsController(IServiceManager serviceManager)
        => _serviceManager = serviceManager;


    [HttpGet]
    [Route("ConversationById/{conversationId:guid}")]
    public async Task<IActionResult> CreateConversationById(Guid conversationId)
        => Ok(await _serviceManager.ConversationService.GetConversationByIdAsync(conversationId, trackChanges: false));

    [HttpGet]
    [Route("GetOrCreateConversation/{userId:guid}/{friendId:guid}")]
    public async Task<IActionResult> GetOrCreateConversation(Guid userId, Guid friendId)
        => Ok(await _serviceManager.ConversationService.GetOrCreateConversationBetweenUserIdAndFriendIdAsync(userId, friendId, trackChanges: false));
}