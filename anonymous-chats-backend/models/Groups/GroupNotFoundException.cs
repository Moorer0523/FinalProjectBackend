namespace anonymous_chats_backend.Models.Groups;

public class GroupNotFoundException : Exception
{
    public override string Message { get; }

    public GroupNotFoundException(string message)
        : base(message)
    {
        Message = message;
    }

    public GroupNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
        Message = message;
    }
}
