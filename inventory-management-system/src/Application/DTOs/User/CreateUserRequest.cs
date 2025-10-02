public record CreateUserRequest (

    string Name,
    string Username,
    string Email,
    string Password,
    string Role,
    string Status
);