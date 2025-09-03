public enum UserRoles
{
    Admin,
    Staff
}

public enum UserStatuses
{
    Active,
    Inactive,
    Suspended
}
public class User
{
    public int ID { get; set; }

    public required string Name { get; set; }

    public required string Username { get; set; }

    public required string Email { get; set; }

    public required string Password { get; set; }

    public UserRoles Roles { get; set; }

    public UserStatuses Status { get; set; }

    public DateTime LastLoginAt { get; set; } = DateTime.UtcNow;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}