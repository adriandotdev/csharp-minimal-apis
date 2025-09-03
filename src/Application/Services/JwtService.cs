using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class JwtService
{
    public static string GenerateJwtToken(List<Claim> claims, double expiration, IConfiguration configuration)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("sample_key_of_json_web_token_sample_key_must_be_16_chars"));
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
}