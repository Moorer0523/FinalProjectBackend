namespace anonymous_chats_backend.Models.Users;

public class UnauthorizedUserException : Exception
{
    public override string Message { get; }

    public UnauthorizedUserException(string message)
        : base(message)
    {
        Message = message;
    }

    public UnauthorizedUserException(string message, Exception inner)
        : base(message, inner)
    {
        Message = message;
    }
}
