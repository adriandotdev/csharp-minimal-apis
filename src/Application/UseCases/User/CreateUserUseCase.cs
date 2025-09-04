using BCrypt.Net;

public class CreateUserUseCase
{

    private readonly IUserRepository _userRepository;

    public CreateUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Response<CreateUserResponse>> Handle(CreateUserRequest request)
    {
        var user = await _userRepository.GetUserByUsername(request.Username);

        if (user is not null) return new Response<CreateUserResponse>(Status.Conflict, null, "Username already taken");

        user = await _userRepository.GetUserByEmail(request.Email);

        if (user is not null) return new Response<CreateUserResponse>(Status.Conflict, null, "Email already taken");

        if (request.Password.Length < 8) return new Response<CreateUserResponse>(Status.BadRequest, null, "Password must be at least eight (8) characters long");

        var hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password, HashType.SHA512);

        var newUser = request with { Password = hashedPassword };

        var createdUser = await _userRepository.CreateUser(newUser);

        return new Response<CreateUserResponse>(Status.Created,
            new CreateUserResponse(createdUser.Name, createdUser.Username, createdUser.Email, createdUser.Roles, createdUser.Status)
        );
    }
}