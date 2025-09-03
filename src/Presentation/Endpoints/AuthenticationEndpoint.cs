using System;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;

public static class AuthenticationEndpoint
{

    public static void AddAuthenticationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/auth");


        group.MapPost("/signup", SignUp);
    }

    public static async Task<IResult> SignUp([FromBody] CreateUserRequest request, [FromServices] CreateUserUseCase useCase)
    {
        var result = await useCase.Handle(request);

        return Response<IResult>.MapResponse(result.Status, result.Data, result.Message);
    }
}