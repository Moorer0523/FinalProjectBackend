using anonymous_chats_backend.Data;
using anonymous_chats_backend.Models;
using anonymous_chats_backend.Models.Chats;
using anonymous_chats_backend.Models.Chats.Dto;
using anonymous_chats_backend.Hubs;
using anonymous_chats_backend.Models.Groups;
using anonymous_chats_backend.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace anonymous_chats_backend.Services;


public class ChatService : IChatService
{
    private readonly AnonymousDbContext _context;
    private readonly GroupService _groupService;
    private readonly IHubContext<ChatHub> _hubContext;

    public ChatService(AnonymousDbContext context, IHubContext<ChatHub> hubContext)
    {
        _context = context;
        _groupService = new(context);
        _hubContext = hubContext;
    }



    public async Task<Chat?> GetChatById(int chatId)
    {
        return await _context.Chats.FindAsync(chatId);
    }



    public async Task<List<Chat>> GetChatsByUserAndGroupId(string userId, int groupId)
    {
        // Verify group
        if (await _groupService.GetGroup(groupId) == null)
            throw new GroupNotFoundException($"Group {groupId} could not be found");

        // Fetch all chats associated with by group
        IQueryable<Chat> chatsByGroup = _context.Chats.Where(x => x.GroupId == groupId);

        // Join ChatUsers that match the passed in userId
        IQueryable<Chat> result = 
            from chat in chatsByGroup
            join user in _context.ChatUsers on
            chat.Id equals user.ChatId into UserChats
            from c in UserChats.DefaultIfEmpty()
            where c.UserId == userId  // filter by userId
            select chat;

        return await result.ToListAsync();
    }



    public async Task<List<ChatMessage>> GetChatMessages(int chatId)
    {
        return await _context.ChatMessages.Where(x => x.ChatId == chatId).ToListAsync();
    }



    public async Task<List<ChatUser>> GetChatUsers(int chatId)
    {
        List<ChatUser> chatUsers = await _context.ChatUsers.Where(x => x.ChatId == chatId).ToListAsync();

        if (chatUsers.IsNullOrEmpty())
        {
            throw new MissingChatComponentException($"Could not locate internally maintained chat users in Chat {chatId}");
        }

        return chatUsers;
    }
    


    public async Task<List<ChatGuess>> GetChatGuesses(int chatId, string guesserId)
    {
        List<ChatGuess> chatGuesses = await _context.ChatGuesses.Where(x => x.ChatId == chatId && x.GuesserId == guesserId).ToListAsync();

        if (chatGuesses.IsNullOrEmpty())
        {
            throw new MissingChatComponentException($"Could not locate internally maintained guesses for user {guesserId} in chat {chatId}");
        }

        return chatGuesses;
    }



    public async Task<List<Chat>> CreateChats(int groupId, string requestingUserId)
    {
        try
        {
            // Verify group and user information before creating chats
            List<User> users = await VerifyChatCreation(groupId, requestingUserId);

            // Create random sub groups of original group
            List<string> userIds = users.Select(x => x.Id).ToList();
            List<string[]> chatGroups = RandomizeChatGroups(userIds);

            // Create Chat objects
            List<Chat> chatObjects = new List<Chat>(new Chat[chatGroups.Count]);
            for (int i = 0; i < chatGroups.Count; i++) 
            {
                Chat chat = new Chat();
                chat.CreateToChat(groupId);
                chatObjects[i] = chat;
                await _context.Chats.AddAsync(chat);
            }
            await _context.SaveChangesAsync();

            await CreateChatUsersAndGuesses(chatObjects, chatGroups, userIds);
            return chatObjects;
        }
        catch (Exception)
        {
            throw;
        }
    }
    


    public async Task<ChatMessage> CreateChatMessage(CreateChatMessageDTO chatMessageDTO, string authorUsername)
    {
        ChatMessage msg = new ChatMessage();
        msg.CreateToChatMessage(chatMessageDTO, authorUsername);

        await _context.ChatMessages.AddAsync(msg);
        await _context.SaveChangesAsync();

        // Relay message to other users in the chat
        string chatGroupName = ChatHub.GetGroupName(msg.ChatId);
        await _hubContext.Clients.Group(chatGroupName).SendAsync("ReceiveMessage", msg);

        return msg;
    }



    public async Task CreateChatUsersAndGuesses(List<Chat> chatObjects, List<string[]> chatGroups, List<string> userIds)
    {
        List<string> pseudonyms = await GeneratePseudonyms(userIds.Count);
        int userIdx = 0;

        for (int i = 0; i < chatGroups.Count; i++)
        {
            for (int j = 0; j < chatGroups[i].Length; j++)
            {
                ChatUser chatUser = new();

                //something is off here.
                chatUser.CreateToChatUser(chatGroups[i][j],     // UserId
                                          chatObjects[i].Id,    // ChatId
                                          pseudonyms[userIdx]); // Pseudonym
                await _context.ChatUsers.AddAsync(chatUser);

                ////something is off here. old code testing new fix
                //chatUser.CreateToChatUser(userIds[userIdx],     // UserId
                //                          chatObjects[i].Id,    // ChatId
                //                          pseudonyms[userIdx]); // Pseudonym
                //await _context.ChatUsers.AddAsync(chatUser);

                userIdx++;

                // Create ChatGuesses for each user to all others in the chat
                for (int k = 0; k < chatGroups[i].Length; k++)
                {
                    // GuesserId should not equal ActualId
                    if (j == k)
                    {
                        continue;
                    }

                    ChatGuess chatGuess = new();
                    chatGuess.CreateToChatGuess(chatObjects[i].Id,  // ChatId
                                                chatGroups[i][j],   // GuesserId
                                                chatGroups[i][k]);  // ActualId
                    await _context.ChatGuesses.AddAsync(chatGuess);
                }
            }
        }
        await _context.SaveChangesAsync();
    }



    public async Task UpdateChatGuess(UpdateChatGuessDTO chatGuessDTO, string authorUsername)
    {
        
        ChatGuess? guess = await _context.ChatGuesses.FindAsync(chatGuessDTO.Id);

        if (guess == null)
        {
            throw new MissingChatComponentException($"Could not locate internally maintained chatGuess {chatGuessDTO.Id}");
        }

        guess.UpdateToChatGuess(chatGuessDTO, authorUsername);

        _context.ChatGuesses.Update(guess);
        await _context.SaveChangesAsync();
    }



    public ObjectResult InternalError(string message = "The server failed to respond")
    {
        ObjectResult result = new ObjectResult(message);
        result.StatusCode = 500;
        return result;
    }


    /*   ------- Helper Methods -------   */
    private async Task<List<User>> VerifyChatCreation(int groupId, string requestingUserId)
    {
        // Verify group exists
        Group? group = await _groupService.GetGroup(groupId);
        if (group == null)
            throw new GroupNotFoundException($"Group {groupId} could not be found");

        // Verify user making request is group admin
        if (group.CreatedBy != requestingUserId)
        {
            throw new UnauthorizedUserException($"{requestingUserId} is not authorized to create chats for group {groupId}");
        }

        // Verify size of group
        List<User> users = await _groupService.GetUsersFromGroup(groupId);
        if (users == null || users.Count < Globals.MIN_GROUP_SIZE)
        {
            throw new GroupSizeBelowLimitException($"Number of users in group {groupId} falls below the minimum required group size");
        }

        return users;
    }



    private static List<string[]> RandomizeChatGroups(List<string> userIds)
    {
        // Determine chat sizes from total group size
        int minChatSize;
        switch (userIds.Count)
        {
            case >= 19:
                minChatSize = 5; break;
            case >= 13 and <= 16:
                minChatSize = 4; break;
            default:
                minChatSize = 3; break;
        }

        // Randomize user order
        Random rnd = new Random();
        userIds = userIds.OrderBy(_ => rnd.Next()).ToList();

        // Divide users into random groups with a minimum size
        List<string[]> chatGroups = userIds.Chunk(minChatSize).ToList();

        // Disperse leftover users into existing random groups
        foreach (string remainder in chatGroups.Last())
        {
            int accomodatingChatIdx = rnd.Next(0, chatGroups.Count - 1);
            chatGroups[accomodatingChatIdx] = chatGroups[accomodatingChatIdx].Append(remainder).ToArray();
        }
        chatGroups.RemoveAt(chatGroups.Count - 1);

        return chatGroups;
    }



    private static string[] ParseWordGenResponse(string content)
    {
        return content
            .Substring(1, content.Length - 2)
            .Split(',')
            .Select(x => x.Trim().Substring(1, x.Length - 2))
            .Select(x => char.ToUpper(x[0]) + x.Substring(1)) // Capitalize
            .ToArray();
    }



    private static async Task<List<string>> GeneratePseudonyms(int n)
    {
        // Call word generating API to get random adjectives and nouns
        HttpClient client = new HttpClient();

        string adjectivesRaw = await client.GetAsync($"https://random-word-form.herokuapp.com/random/adjective?count={n}").Result.Content.ReadAsStringAsync();

        string nounsRaw = await client.GetAsync($"https://random-word-form.herokuapp.com/random/noun?count={n}").Result.Content.ReadAsStringAsync();

        // Clean and parse data
        string[] adjectives = ParseWordGenResponse(adjectivesRaw);
        string[] nouns = ParseWordGenResponse(nounsRaw);
        return adjectives.Zip(nouns, (adj, noun) => adj + noun).ToList();
    }
}
