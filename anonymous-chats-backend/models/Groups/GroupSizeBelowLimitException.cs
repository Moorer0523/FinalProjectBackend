namespace anonymous_chats_backend.Models.Groups;

public class GroupSizeBelowLimitException : Exception
{
    public override string Message { get; }

    public GroupSizeBelowLimitException(string message)
        : base(message)
    {
        Message = message;
    }

    public GroupSizeBelowLimitException(string message, Exception inner)
        : base(message, inner)
    {
        Message = message;
    }
}
