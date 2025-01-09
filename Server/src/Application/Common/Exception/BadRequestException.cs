namespace Application.Common.Exception;

public class BadRequestException(string message, List<string> errors) : System.Exception(message)
{
    public List<string> Errors { get; } = errors;
}
