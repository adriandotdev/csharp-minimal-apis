using System.Security.Claims;
using BCrypt.Net;
using JsonWebToken;

public class LoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public LoginUseCase(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<Response<LoginResponse>> Handle(LoginRequest request)
    {
        var user = await _userRepository.GetUserByUsername(request.Username);

        if (user is null) return new Response<LoginResponse>(Status.NotFound, null, "Invalid credentials");

        var isPasswordMatch = BCrypt.Net.BCrypt.EnhancedVerify(request.Password, user.Password, HashType.SHA512
        );

        if (!isPasswordMatch) return new Response<LoginResponse>(Status.Unauthorized, null);

        var userScope = user.Roles.ToString() == "Admin" ? "admin_scope" : "staff_scope";

        Console.WriteLine($"{user.Roles.ToString()} - {userScope} - {"access"}");

        var accessToken = JwtService.GenerateJwtToken([
            new("role", user.Roles.ToString()),
            new("scope", userScope),
            new("token_type", "access"),
            new("sub", user.Username)
        ], 15, _configuration);

        var refreshToken = JwtService.GenerateJwtToken([
            new ("role", user.Roles.ToString()),
            new ("scope", userScope),
            new ("token_type", "refresh"),
            new("sub", user.Username)
        ], 30, _configuration);

        return new Response<LoginResponse>(Status.OK, new LoginResponse(accessToken, refreshToken));
    }
}