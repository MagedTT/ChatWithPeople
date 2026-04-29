using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Contracts;
using Entities.ConfigurationModels;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.Contracts;
using Shared.DTOs;

namespace Service;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly JwtConfiguration _jwtConfiguration;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _loggerManager;
    private readonly IRepositoryManager _repositoryManager;

    private User? _user;

    public ILoggerManager LoggerManager => _loggerManager;

    public AuthenticationService(
        UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IOptionsMonitor<JwtConfiguration> optionsMonitorJwtConfiguration,
        IMapper mapper,
        IRepositoryManager repositoryManager,
        ILoggerManager loggerManager
        )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtConfiguration = optionsMonitorJwtConfiguration.CurrentValue;
        _mapper = mapper;
        _repositoryManager = repositoryManager;
        _loggerManager = loggerManager;
    }

    public async Task<IdentityResult> RegisterUser(UserForRegisterationDto userForRegisteration)
    {
        User user = _mapper.Map<User>(userForRegisteration);

        if (userForRegisteration.ProfilePicture is not null)
        {
            user.ProfilePicture = await ConvertIFormFileToByteArray(userForRegisteration.ProfilePicture);
        }

        IdentityResult result = await _userManager.CreateAsync(user, userForRegisteration.Password);

        if (result.Succeeded)
        {
            User? userWithUserInterests = await _repositoryManager.UserRepository.GetUserWithUserInterestsByIdAsync(user.Id, trackChanges: true);

            if (userWithUserInterests is not null)
            {
                userWithUserInterests.Status = Entities.Enums.UserStatus.Online;
                foreach (Guid interestId in userForRegisteration.InterestIds)
                {
                    userWithUserInterests.UserInterests.Add(new UserInterest { UserId = userWithUserInterests.Id, InterestId = interestId });
                }

                await _repositoryManager.SaveAsync();
            }

            foreach (string role in userForRegisteration.Roles ?? new string[0])
            {
                if (await _roleManager.RoleExistsAsync(role))
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }
        }

        return result;
    }

    public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuthentication)
    {
        if (userForAuthentication is null)
            throw new Exception($"{nameof(UserForAuthenticationDto)}: userForAuthenticationDto is NULL");
        _user = await _userManager.FindByNameAsync(userForAuthentication.Username);

        bool result = _user is not null && await _userManager.CheckPasswordAsync(_user, userForAuthentication.Password);

        if (!result)
            LoggerManager.LogWarning($"{nameof(ValidateUser)} Invalid username or password");

        return result;
    }

    public async Task<TokenDto> CreateToken(bool populateExp)
    {
        SigningCredentials signingCredentials = GetSigningCredentials();
        List<Claim> claims = await GetClaims();
        JwtSecurityToken tokenOptions = GenerateTokenOptions(signingCredentials, claims);

        string accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        string refreshToken = GenerateRefreshToken();


        if (_user is not null)
        {
            _user.RefreshToken = refreshToken;

            if (populateExp)
                _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            await _userManager.UpdateAsync(_user);
        }

        return new TokenDto(accessToken, refreshToken);
    }

    private SigningCredentials GetSigningCredentials()
    {
        Console.WriteLine($"====> {Environment.GetEnvironmentVariable("ChatWithPeople__SecretKey")}");
        byte[] key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("ChatWithPeople__SecretKey")!);
        SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(key);

        return new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims()
    {
        List<Claim> claims = new()
        {
            // new Claim(ClaimTypes.Name, _user?.UserName!),
            // new Claim(ClaimTypes.NameIdentifier, _user?.Id.ToString()!)
            new Claim("name", _user?.UserName!),
            new Claim("sub", _user?.Id.ToString()!)
        };

        IList<string> roles = await _userManager.GetRolesAsync(_user ?? new User());

        foreach (string role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        JwtSecurityToken tokenOptions = new JwtSecurityToken(
            issuer: _jwtConfiguration.ValidIssuer,
            audience: _jwtConfiguration.ValidAudience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtConfiguration.ExpiresInMinutes)),
            signingCredentials: signingCredentials
        );

        return tokenOptions;
    }

    public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
    {
        ClaimsPrincipal claimsPrincipal = GetClaimsPrincipalsFromExpiredToken(tokenDto.AccessToken);

        User? user = await _userManager.FindByNameAsync(claimsPrincipal.Identity?.Name!);

        if (user is null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            throw new ArgumentException("Invalid Refresh Token"); // Change To Custom One

        _user = user;

        return await CreateToken(populateExp: false);
    }

    private string GenerateRefreshToken()
    {
        byte[] randomNumber = new byte[32];

        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    private ClaimsPrincipal GetClaimsPrincipalsFromExpiredToken(string token)
    {
        TokenValidationParameters tokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = _jwtConfiguration.ValidIssuer,
            ValidAudience = _jwtConfiguration.ValidAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("ChatWithPeople__SecretKey")!))
        };

        JwtSecurityTokenHandler tokenHandler = new();

        SecurityToken? securityToken;

        ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

        JwtSecurityToken? jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid Token");

        return claimsPrincipal;
    }

    private async Task<byte[]> ConvertIFormFileToByteArray(IFormFile file)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}