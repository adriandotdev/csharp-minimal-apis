
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

    public Response(Status status, TApiResponse? data)
    {
        Status = status;
        Data = data;
    }

    public static IResult MapResponse<TData>(Status response, TData? data = default, string? Message = "")
    {

        switch (response)
        {
            case Status.OK:
                return TypedResults.Ok(data);
            case Status.Created:
                return TypedResults.Created(Message, data);
            case Status.NotFound:
                return TypedResults.NotFound(Message);
            default:
                return TypedResults.InternalServerError();
        }
    }
}