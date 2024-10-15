using anonymous_chats_backend.Models.Users;

namespace anonymous_chats_backend.Models.Chats.Dto;

public static class ChatGuessExtensions
{
    public static void CreateToChatGuess(this ChatGuess chatGuess, int chatId, string guesserId, string actualId)
    {
        chatGuess.ChatId = chatId;
        chatGuess.GuesserId = guesserId;
        chatGuess.ActualId = actualId;
        chatGuess.CreatedBy = "system";
    }

   public static void UpdateToChatGuess(this ChatGuess chatGuess, UpdateChatGuessDTO updateChatGuessDTO, string authorUsername)
    {
        chatGuess.GuesseeId = updateChatGuessDTO.GuesseeId;
        chatGuess.UpdatedBy = authorUsername;
        chatGuess.UpdatedOn = DateTime.UtcNow;
    }
}
