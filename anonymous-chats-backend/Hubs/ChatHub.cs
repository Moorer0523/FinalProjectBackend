using anonymous_chats_backend.Models.Chats;
using Microsoft.AspNetCore.SignalR;

namespace anonymous_chats_backend.Hubs;

public class ChatHub : Hub
{
    public async Task JoinChatGroup(int chatId)
    {
        string groupName = GetGroupName(chatId);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("UserJoined", $"{Context.ConnectionId} has joined {groupName}");
    }

    public async Task SendMessageToGroup(ChatMessage chatMessage)
    {
        string groupName = GetGroupName(chatMessage.ChatId);
        await Clients.Group(groupName).SendAsync("ReceiveMessage", chatMessage);
    }


    // Helper method to get the group name
    public static string GetGroupName(int chatId)
    {
        return $"Chat-{chatId}";
    }
}
