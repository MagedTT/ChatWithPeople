using Microsoft.AspNetCore.Identity;
using Shared.DTOs;

namespace Service.Contracts;

public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUser(UserForRegisterationDto userForRegisteration);
    Task<bool> ValidateUser(UserForAuthenticationDto userForAuthentication);
    Task<TokenDto> CreateToken(bool populateExp);
    Task<TokenDto> RefreshToken(TokenDto tokenDto);
}