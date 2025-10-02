
using FluentValidation.Results;

public enum Status
{
    OK,
    Created,
    Forbidden,
    NotFound,
    InternalServerError,
    Conflict,
    BadRequest,
    Unauthorized
}

public class ValidationErrorDto
{
    public string PropertyName { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
}

public class Response<TApiResponse>
{
    public Status Status;

    public TApiResponse? Data;

    public string? Message;

    public Response(Status status, TApiResponse? data, string? message = "Success")
    {
        Status = status;
        Data = data;
        Message = message;
    }

    public static IResult MapResponse<TData>(Status response, TData? data = default, string? Message = "Success")
    {
        switch (response)
        {
            case Status.OK:
                return TypedResults.Ok(new
                {
                    Data = data,
                    Message
                });
            case Status.Created:
                return TypedResults.Created(Message, new
                {
                    Data = data
                });
            case Status.NotFound:
                return TypedResults.NotFound(
                    new
                    {
                        Message
                    }
                );
            case Status.Conflict:
                return TypedResults.Conflict(new
                {
                    Message
                });
            case Status.BadRequest:
                return TypedResults.BadRequest(new
                {
                    Message
                });
            case Status.Unauthorized:
                return TypedResults.Unauthorized();
            default:
                return TypedResults.InternalServerError();
        }
    }

    public static List<ValidationErrorDto> MapErrors(ValidationResult result)
    {
        return result.Errors
            .Select(error => new ValidationErrorDto
            {
               ErrorMessage =  error.ErrorMessage,
                PropertyName = error.PropertyName
            })
            .ToList();
    }
}