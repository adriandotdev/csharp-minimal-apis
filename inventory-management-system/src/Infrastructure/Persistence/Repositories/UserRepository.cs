
using Microsoft.EntityFrameworkCore;

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
            Roles = Enum.Parse<UserRoles>(request.Role),
            Status = Enum.Parse<UserStatuses>(request.Status)
        };

        _context.Users.Add(newUser);

        await _context.SaveChangesAsync();

        return newUser;
    }

    public async Task<User> GetUserByEmail(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == email);

        return user!;
    }

    public async Task<User> GetUserByUsername(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Username == username);
        
        return user!;
    }
}