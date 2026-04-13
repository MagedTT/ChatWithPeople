using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
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
    public async Task<IActionResult> GetAllUsers([FromQuery] UserParameters userParameters)
    {
        (IEnumerable<Shared.DTOs.UserDto> users, MetaData metaData) = await _serviceManager.UserService.GetAllUsersAsync(userParameters, trackChanges: false);

        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metaData));

        return Ok(users);
    }
}