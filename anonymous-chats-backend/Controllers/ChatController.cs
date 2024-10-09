using anonymous_chats_backend.Data;
using anonymous_chats_backend.Models.Chats;
using anonymous_chats_backend.Models.Chats.Dto;
using anonymous_chats_backend.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace anonymous_chats_backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChatController : ApiBaseController
{
    private readonly AnonymousDbContext _context;

    public ChatController(AnonymousDbContext context)
    {
        _context = context;
    }


    // GET api/<ChatController>/chats/userId=abc/groupId=2
    [HttpGet("chats/userId={userId}/groupId={groupId}")]
    public async Task<IActionResult> GetChats(string userId, int groupId)
    {
        // Fetch all chats associated with by group
        IQueryable<Chat> chatsByGroup = _context.Chats.Where(x => x.GroupId == groupId);

        // Join ChatUsers that match the passed in userId
        var result = from chat in chatsByGroup
                     join user in _context.ChatUsers on
                     chat.Id equals user.ChatId into UserChats
                     from c in UserChats.DefaultIfEmpty()
                     where c.UserId == userId // filter by userId
                     select new
                     {
                         ChatId = chat.Id,
                         StartDate = chat.StartDate,
                         GroupId = groupId,
                         UserId = userId,
                         Pseudonym = c.Pseudonym
                     };

        return Ok(await result.ToListAsync());
    }


    // GET api/<ChatController>/users/5
    [HttpGet("users/{chatId}")]
    public async Task<IActionResult> GetChatUsers(int chatId)
    {
        IQueryable<ChatUser> chatUsers = _context.ChatUsers.Where(x => x.ChatId == chatId);

        if (chatUsers.IsNullOrEmpty())
        {
            return NotFound();
        }

        return Ok(await chatUsers.ToListAsync());
    }



    // GET api/<ChatController>/messages/5
    [HttpGet("messages/{chatId}")]
    public async Task<IActionResult> GetChatMessages(int chatId)
    {
        IQueryable<ChatMessage> chatMessages = _context.ChatMessages.Where(x => x.ChatId == chatId);

        if (chatMessages.IsNullOrEmpty())
        {
            return NotFound();
        }

        return Ok(await chatMessages.ToListAsync());
    }



    // GET api/<ChatController>/guesses/chatId=5/guesserId=abc
    [HttpGet("guesses/chatId={chatId}/guesserId={guesserId}")]
    public async Task<IActionResult> GetChatGuesses(int chatId, string guesserId)
    {
        IQueryable<ChatGuess> userGuesses = _context.ChatGuesses.Where(x => x.ChatId == chatId && x.GuesserId == guesserId);

        if (userGuesses.IsNullOrEmpty())
        {
            var customResponse = new
            {
                Code = 500,
                Message = $"Could not locate internally maintained guesses for user {guesserId} in chat {chatId}"
            };

            return StatusCode(StatusCodes.Status500InternalServerError, customResponse);
        }

        return Ok(await userGuesses.ToListAsync());
    }



    // POST api/<ChatController>/messages
    [HttpPost("messages")]
    public async Task<IActionResult> CreateChatMessage([FromBody] CreateChatMessageDTO chatMessageDto)
    {
        if (chatMessageDto == null)
        {
            return BadRequest("Invalid request body");
        }

        ChatMessage msg = new ChatMessage();
        msg.CreateToChatMessage(chatMessageDto, GetCurrentUserID());

        await _context.ChatMessages.AddAsync(msg);
        await _context.SaveChangesAsync();

        return Created();
    }



    // PUT api/<ChatController>/guesses
    [HttpPut("guesses")]
    public async Task<IActionResult> UpdateGuess([FromBody] UpdateChatGuessDTO chatGuessDTO)
    {
        if (chatGuessDTO == null)
        {
            return BadRequest("Invalid request body");
        }

        ChatGuess? guess = await _context.ChatGuesses.FindAsync(chatGuessDTO.Id);

        if (guess == null)
        {
            var customResponse = new
            {
                Code = 500,
                Message = $"Could not locate internally maintained chatGuess {chatGuessDTO.Id}"
            };

            return StatusCode(StatusCodes.Status500InternalServerError, customResponse);
        }

        guess.UpdateToChatGuess(chatGuessDTO, GetCurrentUserID());

        _context.ChatGuesses.Update(guess);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
