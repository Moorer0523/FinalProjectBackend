using Microsoft.AspNetCore.Mvc;

namespace anonymous_chats_backend.Services;

public class ChatService
{
    public string CreateChats(int groupId, List<int> userIds)
    {
        return "Ok";
    }

    public List<string> GeneratePseudonyms()
    {
        return new List<string>();
    }
}
