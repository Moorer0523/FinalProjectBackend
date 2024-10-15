using anonymous_chats_backend.Models.Chats;
using anonymous_chats_backend.Models.Chats.Dto;
using Microsoft.AspNetCore.Mvc;

namespace anonymous_chats_backend.Services;

public interface IChatService
{
    public Task<Chat?> GetChatById(int chatId);

    public Task<List<Chat>> GetChatsByUserAndGroupId(string userId, int groupId);

    public Task<List<ChatMessage>> GetChatMessages(int chatId);

    public Task<List<ChatUser>> GetChatUsers(int chatId);

    public Task<List<ChatGuess>> GetChatGuesses(int chatId, string guesserId);

    public Task<List<Chat>> CreateChats(int groupId, string requestingUserId);

    public Task CreateChatMessage(CreateChatMessageDTO chatMessageDTO, string authorUsername);

    public Task UpdateChatGuess(UpdateChatGuessDTO chatGuessDTO, string authorUsername);

    public ObjectResult InternalError(string message);
}
