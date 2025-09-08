using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

public class RefreshTokenUseCase
{

    private readonly IConfiguration _configuration;


    public RefreshTokenUseCase(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Response<LoginResponse> Handle(string refreshToken)
    {
        var token = refreshToken.Split(" ")[1];

        try
        {
            ClaimsPrincipal? claims = JwtService.ValidateJwtToken(token, _configuration);

            string? tokenType = claims?.FindFirst("token_type")?.Value;
            string? role = claims?.FindFirst(ClaimTypes.Role)?.Value;
            string? scope = claims?.FindFirst("scope")?.Value;

            if (tokenType != "refresh" || role is null || scope is null)
                return new Response<LoginResponse>(Status.BadRequest, null, "Invalid token");

            var newAccessToken = JwtService.GenerateJwtToken([
                new(ClaimTypes.Role, role),
                new("scope", scope),
                new("token_type", "access")
            ], 15, _configuration);

            return new Response<LoginResponse>(Status.OK, new LoginResponse(newAccessToken, token));
        }
        catch (SecurityTokenValidationException ex)
        {
            // @TODO: Must use "ex" to logger
            return new Response<LoginResponse>(Status.Unauthorized, null);
        }
    }
}