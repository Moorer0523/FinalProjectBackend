using anonymous_chats_backend.Data;
using anonymous_chats_backend.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace anonymous_chats_backend.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class UserController : ApiBaseController
{
    private readonly AnonymousDbContext _context;

    public UserController(AnonymousDbContext context)
    {
        _context = context;
    }


    // GET: api/<UserController>
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {

        return Ok(await _context.Users.ToListAsync());
    }

    // GET api/<UserController>/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(string id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }
        return Ok(_context.Users.FindAsync(id));
    }

    // POST api/<UserController>
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO user)
    {
        user.Id = GetCurrentUserID();
        if (await _context.Users.FindAsync(user.Id) != null)
            return BadRequest(user);

        User newUser = new();
        newUser.CreateToUser(user, GetCurrentUserID());

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetUser", new { id = user.Id }, user);
    }

    // PUT api/<UserController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDTO updatedUserDTO)
    {
        try
        {
            var currentUser = _context.Users.Find(id);

            if (updatedUserDTO == null)
                return BadRequest();

            if (currentUser == null)
                return NotFound();

            currentUser.UpdateToUser(updatedUserDTO, GetCurrentUserID());
            _context.Users.Update(currentUser);
            await _context.SaveChangesAsync();
        }
        catch (DBConcurrencyException)
        {
            return BadRequest();
        }

        return NoContent();
            

    }

    //Are we allowing users to delete themselves? Not sure if we have included this or not. 

    // DELETE api/<UserController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        catch (DBConcurrencyException)
        {
            return BadRequest();
        }
        return NoContent();            
    }
}
