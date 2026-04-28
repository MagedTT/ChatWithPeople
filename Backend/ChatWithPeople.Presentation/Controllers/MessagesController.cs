using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DTOs;
using Shared.RequestFeatures;

namespace ChatWithPeople.Presentation.Controllers;

[ApiController]
[Route("api/Conversations/{conversationId:guid}/Messages")]
public class MessagesController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public MessagesController(IServiceManager serviceManager)
        => _serviceManager = serviceManager;


    [HttpGet]
    public async Task<IActionResult> GetAllMessagesForConversation(Guid conversationId, [FromQuery] MessageParameters messageParameters)
    {
        (IEnumerable<MessageDto> messageDtos, MessageMetaData messageMetaData) = await _serviceManager.MessageService.GetAllMessagesByConversationIdAsync(conversationId, messageParameters, trackChanges: false);

        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(messageMetaData));

        return Ok(messageDtos);
    }

    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> CreateMessage(Guid conversationId, [FromBody] MessageForCreationDto messageForCreationDto)
        => Ok(await _serviceManager.MessageService.CreateMessageForConversationAsync(conversationId, messageForCreationDto));

    [HttpPost]
    [Route("MarkAsRead/{senderId:guid}/{receiverId:guid}")]
    public async Task<IActionResult> MarkAsRead(Guid conversationId, Guid senderId, Guid receiverId)
    {
        await _serviceManager.MessageService.MarkAsReadAsync(conversationId, senderId, receiverId);

        return NoContent();
    }

    [HttpDelete]
    [Route("Delete/{messageId:guid}")]
    public async Task<IActionResult> DeleteMessageForConversation(Guid conversationId, Guid messageId)
    {
        await _serviceManager.MessageService.DeleteMessageByConversationIdAsync(conversationId, messageId, trackChanges: false);
        return NoContent();
    }
}