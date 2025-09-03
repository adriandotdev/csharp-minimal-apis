public class CreateUserUseCase
{

    private readonly IUserRepository _userRepository;

    public CreateUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Response<User>> Handle(CreateUserRequest request)
    {
        /**
            @TODO

            1.) Check if username is existing
            2.) Check if email is existing
            3.) Check if password length is minimum by eight (8)
            4.) Hash the password
            5.) Create User
        */
        var createdUser = await _userRepository.CreateUser(request);

        return new Response<User>(Status.Created, createdUser);
    }
}