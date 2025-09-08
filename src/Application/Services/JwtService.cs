using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class JwtService
{
    public static string GenerateJwtToken(List<Claim> claims, double expiration, IConfiguration configuration)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Authentication:Schemes:Bearer:IssuerSigningKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration["Authentication:Schemes:Bearer:ValidIssuer"],
            audience: configuration["Authentication:Schemes:Bearer:ValidAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(expiration),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static ClaimsPrincipal? ValidateJwtToken(string token, IConfiguration configuration)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(configuration["Authentication:Schemes:Bearer:IssuerSigningKey"]!);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = configuration["Authentication:Schemes:Bearer:ValidIssuer"],

            ValidateAudience = true,
            ValidAudience = configuration["Authentication:Schemes:Bearer:ValidAudience"],

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
        };

        var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
        return principal;
    }
}