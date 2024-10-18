using anonymous_chats_backend.Data;
using anonymous_chats_backend.Hubs;
using anonymous_chats_backend.Models.Chats;
using anonymous_chats_backend.Models.Chats.Dto;
using anonymous_chats_backend.Models.Groups;
using anonymous_chats_backend.Models.Users;
using anonymous_chats_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace anonymous_chats_backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChatController : ApiBaseController
{
    private readonly ChatService _chatService;

    public ChatController(AnonymousDbContext context, IHubContext<ChatHub> hubContext)
    {
        _chatService = new(context, hubContext);
    }



    // GET api/<ChatController>/Chats/userId=abc/groupId=2
    [HttpGet("Chats/userId={userId}/groupId={groupId}")]
    public async Task<IActionResult> GetChats(string userId, int groupId)
    {
        try
        {
            List<Chat> chats = await _chatService.GetChatsByUserAndGroupId(userId, groupId);

            return Ok(chats);
        }
        catch (GroupNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }



    // GET api/<ChatController>/Messages/5
    [HttpGet("Messages/{chatId}")]
    public async Task<IActionResult> GetChatMessages(int chatId)
    {
        // Verify chat
        if (await _chatService.GetChatById(chatId) == null)
        {
            return NotFound($"Chat {chatId} could not be found");
        }

        return Ok(await _chatService.GetChatMessages(chatId));
    }



    // GET api/<ChatController>/Users/5
    [HttpGet("Users/{chatId}")]
    public async Task<IActionResult> GetChatUsers(int chatId)
    {
        // Verify chat
        if (await _chatService.GetChatById(chatId) == null)
        {
            return NotFound($"Chat {chatId} could not be found");
        }

        try
        {
            List<ChatUser> chatUsers = await _chatService.GetChatUsers(chatId);
            return Ok(chatUsers);
        } 
        catch (MissingChatComponentException ex) 
        {
            return _chatService.InternalError(ex.Message);
        }
    }



    // GET api/<ChatController>/Guesses/chatId=5/guesserId=abc
    [HttpGet("Guesses/chatId={chatId}/guesserId={guesserId}")]
    public async Task<IActionResult> GetChatGuesses(int chatId, string guesserId)
    {
        // Verify chat
        if (await _chatService.GetChatById(chatId) == null)
        {
            return NotFound($"Chat {chatId} could not be found");
        }

        try
        {
            List<ChatGuess>? chatGuesses = await _chatService.GetChatGuesses(chatId, guesserId);
            return Ok(chatGuesses);
        }
        catch (MissingChatComponentException ex)
        {
            return _chatService.InternalError(ex.Message);
        }
    }



    // POST api/<ChatController>/Chats
    [HttpPost("Chats")]
    public async Task<IActionResult> CreateChats(int groupId)
    {
        try
        {
            List<Chat> chats = await _chatService.CreateChats(groupId, GetCurrentUserID());
            return Ok(chats);
        }
        catch (Exception ex)
        {
            Func<string, IActionResult> responseMethod;
            switch (ex)
            {
                case GroupNotFoundException:
                    responseMethod = NotFound; break;
                case UnauthorizedUserException:
                    responseMethod = Unauthorized; break;
                case GroupSizeBelowLimitException:
                    responseMethod = UnprocessableEntity; break;
                default:
                    return BadRequest(ex.StackTrace);
            }
            return responseMethod(ex.Message);
        }
    }



    // POST api/<ChatController>/Messages
    [HttpPost("Messages")]
    public async Task<IActionResult> CreateChatMessage([FromBody] CreateChatMessageDTO chatMessageDTO)
    {
        if (chatMessageDTO == null || !ModelState.IsValid)
        {
            return BadRequest("Invalid request body");
        }

        ChatMessage msg = await _chatService.CreateChatMessage(chatMessageDTO, GetCurrentUserID());

        return Ok(msg);
    }



    // PUT api/<ChatController>/Guesses
    [HttpPut("Guesses")]
    public async Task<IActionResult> UpdateChatGuess([FromBody] UpdateChatGuessDTO chatGuessDTO)
    {
        if (chatGuessDTO == null || !ModelState.IsValid)
        {
            return BadRequest("Invalid request body");
        }

        await _chatService.UpdateChatGuess(chatGuessDTO, GetCurrentUserID());

        return NoContent();
    }
}
