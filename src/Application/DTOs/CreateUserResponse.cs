public record CreateUserResponse(

    string Name,
    string Username,
    string Email,
    UserRoles Role,
    UserStatuses Status
);