namespace anonymous_chats_backend.Models.Chats.Dto;

public static class ChatExtensions
{
    public static void CreateToChat(this Chat chat, int groupId)
    {
        chat.GroupId = groupId;
        chat.CreatedBy = "system";
    }
}
