using FluentValidation;
using Microsoft.AspNetCore.Mvc;

public static class AuthenticationEndpoint
{
    public static void AddAuthenticationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/auth");

        group.MapPost("/signup", SignUp);
        group.MapPost("/login", Login);
        group.MapPost("/refresh", RefreshToken);
    }

    public static async Task<IResult> SignUp([FromBody] CreateUserRequest request, [FromServices] CreateUserUseCase useCase, IValidator<CreateUserRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return TypedResults.BadRequest(Response<IResult>.MapErrors(validationResult));
        }

        var result = await useCase.Handle(request);

        return Response<IResult>.MapResponse(result.Status, result.Data, result.Message);
    }

    public static async Task<IResult> Login([FromBody] LoginRequest request, [FromServices] LoginUseCase useCase, IValidator<LoginRequest> validator)
    {   
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return TypedResults.BadRequest(Response<IResult>.MapErrors(validationResult));
        }
        
        var result = await useCase.Handle(request);

        return Response<IResult>.MapResponse(result.Status, result.Data, result.Message);
    }

    public static IResult RefreshToken([FromHeader(Name = "Authorization")] string Token, [FromServices] RefreshTokenUseCase useCase)
    {
        var result = useCase.Handle(Token);

        return Response<IResult>.MapResponse(result.Status, result.Data, result.Message);
    }
}