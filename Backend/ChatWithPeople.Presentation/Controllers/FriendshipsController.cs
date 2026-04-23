using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Service.Contracts;
using Shared.DTOs;
using Shared.RequestFeatures;

namespace ChatWithPeople.Presentation.Controllers;

[ApiController]
[Route("api/Friendships")]
public class FriendshipsController : ControllerBase
{
    private readonly IServiceManager _serviceManager;
    public FriendshipsController(IServiceManager serviceManager)
        => _serviceManager = serviceManager;

    [HttpGet]
    [Route("{userId:guid}")]
    public async Task<IActionResult> GetFriendships(Guid userId, [FromQuery] FriendshipsParameters friendshipsParameters)
    {
        (IEnumerable<FriendshipsDto> friendshipsDtos, MetaData metaData) = await _serviceManager.FriendshipsService.GetAllFriendsByUserIdAsync(userId, friendshipsParameters, trackChanges: false);

        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metaData));

        return Ok(friendshipsDtos);
    }

    [HttpGet]
    [Route("{userId:guid}/OnlineFriends")]
    public async Task<IActionResult> GetOnlineFriends(Guid userId)
        => Ok(await _serviceManager.FriendshipsService.GetFriendsWithMinimalInformationByUserIdAsync(userId, trackChanges: false));

    [HttpPost]
    [Route("{user1Id:guid}/{user2Id:guid}")]
    public async Task<IActionResult> CreateFriendship(Guid user1Id, Guid user2Id)
    {
        await _serviceManager.FriendshipsService.CreateFriendshipBetweenUser1IdAndUser2Id(user1Id, user2Id);

        return NoContent();
    }

    [HttpDelete]
    [Route("{user1Id:guid}/{user2Id:guid}")]
    public async Task<IActionResult> DeleteFriendship(Guid user1Id, Guid user2Id)
    {
        await _serviceManager.FriendshipsService.DeleteFriendshipByUsersIds(user1Id, user2Id, trackChanges: false);

        return NoContent();
    }
}