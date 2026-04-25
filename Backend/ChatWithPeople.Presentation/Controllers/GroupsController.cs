using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DTOs;
using Shared.RequestFeatures;

namespace ChatWithPeople.Presentation.Controllers;

[ApiController]
[Route("api/Groups")]
public class GroupsController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public GroupsController(IServiceManager serviceManager)
        => _serviceManager = serviceManager;


    [HttpGet]
    [Route("PublicGroups")]
    public async Task<IActionResult> GetAllPublicGroups([FromQuery] GroupParameters groupParameters)
    {
        (IEnumerable<GroupDto> groups, MetaData metaData) = await _serviceManager.GroupService.GetAllPublicGroupsAsync(groupParameters, trackChanges: false);

        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metaData));

        return Ok(groups);
    }

    [HttpGet]
    [Route("{userId:guid}")]
    public async Task<IActionResult> GetAllPublicAndMemberGroupsByUserId(Guid userId, [FromQuery] GroupParameters groupParameters)
    {
        (IEnumerable<GroupDto> groups, MetaData metaData) = await _serviceManager.GroupService.GetAllPublicAndMemberGroupsByUserIdAsync(userId, groupParameters, trackChanges: false);

        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metaData));

        return Ok(groups);
    }

    [HttpPost]
    [Route("{userId:guid}")]
    public async Task<IActionResult> CreateGroup(Guid userId, [FromForm] GroupForCreationDto groupForCreationDto)
    {
        await _serviceManager.GroupService.CreateGroupForUserWithIdAsync(userId, groupForCreationDto);

        return NoContent();
    }

    [HttpDelete]
    [Route("{userId:guid}/{groupId:guid}/DeleteGroup")]
    public async Task<IActionResult> DeleteGroup(Guid userId, Guid groupId)
    {
        await _serviceManager.GroupService.DeleteGroupForUserWithIdAsync(userId, groupId, trackChanges: false);

        return NoContent();
    }
}