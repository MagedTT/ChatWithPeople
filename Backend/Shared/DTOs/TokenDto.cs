namespace Shared.DTOs;

public class TokenDto
{
    public string AccessToken { get; private set; }
    public string RefreshToken { get; private set; }

    public TokenDto(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}