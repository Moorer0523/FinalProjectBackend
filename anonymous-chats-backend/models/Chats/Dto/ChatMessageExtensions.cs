using anonymous_chats_backend.Models.Users;

namespace anonymous_chats_backend.Models.Chats.Dto;

public static class ChatMessageExtensions
{
    public static void CreateToChatMessage(this ChatMessage chatMessage, CreateChatMessageDTO createChatMessageDto, string authorUsername)
    {
        chatMessage.ChatId = createChatMessageDto.ChatId;
        chatMessage.OriginalMessage = createChatMessageDto.OriginalMessage;
        chatMessage.FilteredMessage = createChatMessageDto.FilteredMessage;
        chatMessage.CreatedBy = authorUsername;
        // CreatedOn is set to current time by default
    }
}
