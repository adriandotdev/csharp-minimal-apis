public interface IUserRepository
{
    Task<User> CreateUser(CreateUserRequest request);
}