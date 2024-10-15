namespace anonymous_chats_backend.Models.Chats.Dto;

public static class ChatUserExtensions
{
    public static void CreateToChatUser(this ChatUser chatUser, string userId, int chatId, string pseudonym)
    {
        chatUser.UserId = userId;
        chatUser.ChatId = chatId;
        chatUser.Pseudonym = pseudonym;
        chatUser.CreatedBy = "system";
    }
}