
public enum Status
{
    OK,
    Created,
    Forbidden,
    NotFound,
    InternalServerError
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
            default:
                return TypedResults.InternalServerError();
        }
    }
}