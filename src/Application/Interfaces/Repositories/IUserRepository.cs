public interface IUserRepository
{
    Task<User> CreateUser(CreateUserRequest request);

    Task<User> GetUserByUsername(string username);

    Task<User> GetUserByEmail(string email);
}