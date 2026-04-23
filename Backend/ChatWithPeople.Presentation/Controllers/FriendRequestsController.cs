using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DTOs;
using Shared.RequestFeatures;

namespace ChatWithPeople.Presentation.Controllers;

[ApiController]
[Route("api/FriendRequests")]
public class FriendRequestsController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public FriendRequestsController(IServiceManager serviceManager)
        => _serviceManager = serviceManager;

    [HttpGet]
    [Route("{userId:guid}")]
    public async Task<IActionResult> GetFriendRequests(Guid userId, [FromQuery] FriendRequestParameters friendRequestParameters)
    {
        (IEnumerable<FriendRequestDto> friendRequestDtos, MetaData metaData) = await _serviceManager.FriendRequestService.GetFriendRequestsAsync(userId, friendRequestParameters, trackChanges: false);

        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metaData));

        return Ok(friendRequestDtos);
    }

    [HttpGet]
    [Route("{userId:guid}/TotalCount")]
    public async Task<IActionResult> GetFriendRequestsTotalCount(Guid userId)
        => Ok(await _serviceManager.FriendRequestService.GetFriendRequestsTotalCountAsync(userId));

    [HttpGet]
    [Route("{userId:guid}/SentCount")]
    public async Task<IActionResult> GetFriendRequestsSentCount(Guid userId)
    => Ok(await _serviceManager.FriendRequestService.GetFriendRequestsSentCountAsync(userId));

    [HttpGet]
    [Route("{userId:guid}/ReceivedCount")]
    public async Task<IActionResult> GetFriendRequestsReceivedCount(Guid userId)
    => Ok(await _serviceManager.FriendRequestService.GetFriendRequestsReceivedCountAsync(userId));

    [HttpPost]
    [Route("{friendRequestId}/accept")]
    public async Task<IActionResult> AcceptFriendRequest(Guid friendRequestId)
    {
        await _serviceManager.FriendRequestService.AcceptFriendRequest(friendRequestId);

        return Ok();
    }

    [HttpPost]
    [Route("{friendRequestId}/reject")]
    public async Task<IActionResult> RejectFriendRequest(Guid friendRequestId)
    {
        await _serviceManager.FriendRequestService.DeleteFriendRequest(friendRequestId);

        return Ok();
    }

    [HttpPost]
    [Route("{senderId:guid}/{receiverId:guid}")]
    public async Task<IActionResult> CreateFriendRequest(Guid senderId, Guid receiverId)
    {
        await _serviceManager.FriendRequestService.CreateFriendRequest(senderId, receiverId);

        return NoContent();
    }
}