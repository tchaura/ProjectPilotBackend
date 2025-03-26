namespace Common;

public class WebApiException(int statusCode, string message): Exception(message)
{
    public int StatusCode { get; } = statusCode;
}