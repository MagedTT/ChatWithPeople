using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DTOs;

namespace ChatWithPeople.Presentation.Controllers;

[ApiController]
[Route("api/Token")]
public class TokenController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public TokenController(IServiceManager serviceManager)
        => _serviceManager = serviceManager;

    [HttpPost]
    [Route("Refresh")]
    public async Task<IActionResult> RefershToken([FromBody] TokenDto tokenDto)
    {
        TokenDto tokenDtoToReturn = await _serviceManager.AuthenticationService.RefreshToken(tokenDto);

        return Ok(tokenDtoToReturn);
    }
}