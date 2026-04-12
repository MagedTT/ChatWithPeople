using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DTOs;

namespace ChatWithPeople.Presentation.Controllers;

[ApiController]
[Route("api/Authentication")]
public class AuthenticationController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public AuthenticationController(IServiceManager serviceManager)
        => _serviceManager = serviceManager;

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromForm] UserForRegisterationDto userForRegisteration)
    {
        IdentityResult result = await _serviceManager.AuthenticationService.RegisterUser(userForRegisteration);

        if (!result.Succeeded)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }

        return StatusCode(201);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto userForAuthentication)
    {
        bool result = await _serviceManager.AuthenticationService.ValidateUser(userForAuthentication);

        if (!result)
            return Unauthorized();

        TokenDto tokenDto = await _serviceManager.AuthenticationService.CreateToken(populateExp: true);

        return Ok(tokenDto);
    }
}