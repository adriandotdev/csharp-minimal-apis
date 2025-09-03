
public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> CreateUser(CreateUserRequest request)
    {
        var newUser = new User()
        {
            Name = request.Name,
            Username = request.Username,
            Email = request.Email,
            Password = request.Password,
            Roles = request.Role,
            Status = request.Status
        };

        _context.Users.Add(newUser);

        await _context.SaveChangesAsync();

        return newUser;
    }
}