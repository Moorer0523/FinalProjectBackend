namespace anonymous_chats_backend.Models.Chats;

public class MissingChatComponentException : Exception
{
    public override string Message { get; }

    public MissingChatComponentException(string message)
        : base(message)
    {
        Message = message;
    }

    public MissingChatComponentException(string message, Exception inner)
        : base(message, inner)
    {
        Message = message;
    }
}
