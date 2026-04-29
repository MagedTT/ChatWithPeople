using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DTOs;
using Shared.RequestFeatures;

namespace ChatWithPeople.Presentation.Controllers;

[ApiController]
[Route("api/Users")]
public class UsersController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public UsersController(IServiceManager serviceManager)
        => _serviceManager = serviceManager;

    [HttpGet]
    [Route("{userId:guid}")]
    public async Task<IActionResult> GetUserById(Guid userId)
        => Ok(await _serviceManager.UserService.GetUserByIdAsync(userId, trackChanges: false));

    [HttpGet]
    public async Task<IActionResult> GetAllUsers([FromQuery] UserParameters userParameters)
    {
        (IEnumerable<UserDto> users, MetaData metaData) = await _serviceManager.UserService.GetAllUsersAsync(userParameters: userParameters, trackChanges: false);

        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metaData));

        return Ok(users);
    }

    [HttpGet]
    [Route("Discover/{userId:guid}")]
    public async Task<IActionResult> GetAllUsersForDiscover(Guid userId, [FromQuery] UserParameters userParameters)
    {
        (IEnumerable<UserForDiscoverDTO> users, MetaData metaData) = await _serviceManager.UserService.GetAllUsersForDiscoverWithInterestsAsync(userId, userParameters, trackChanges: false);

        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metaData));

        return Ok(users);
    }
}