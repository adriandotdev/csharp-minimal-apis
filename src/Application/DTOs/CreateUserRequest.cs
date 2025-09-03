public record CreateUserRequest (

    string Name,
    string Username,
    string Email,
    string Password,
    UserRoles Role,
    UserStatuses Status
);