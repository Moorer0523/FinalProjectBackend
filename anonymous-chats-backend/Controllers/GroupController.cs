using anonymous_chats_backend.Data;
using anonymous_chats_backend.Models.Groups;
using anonymous_chats_backend.Models.Users;
using anonymous_chats_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Group = anonymous_chats_backend.Models.Groups.Group;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace anonymous_chats_backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GroupController : ApiBaseController
{
    private readonly GroupService _groupService;

    public GroupController(AnonymousDbContext context)
    {
        _groupService = new(context);
    }


    // GET api/<GroupController>/5
    [HttpGet("{groupId}", Name = "GetGroup")]
    public async Task<IActionResult> GetGroup(int groupId)
    {
        Group group = await _groupService.GetGroup(groupId);

        if (group == null)
            return NotFound();
        return Ok(group);
    }


    // GET api/<GroupController>/GetUserGroups/5
    [HttpGet("GetUserGroups/{userId}")]
    public async Task<IActionResult> GetUserGroups(string userId)
    {
        List<Group> group = await _groupService.GetGroupsForUser(userId);

        if (group == null)
            return NotFound();
        return Ok(group);
    }

    // GET api/<GroupController>/GetGroupUsers/5
    [HttpGet("GetGroupUsers/{groupId}")]
    public async Task<IActionResult> GetGroupUsers(int groupId)
    {
        List<User> users = await _groupService.GetUsersFromGroup(groupId);

        if (users == null)
            return NotFound();
        return Ok(users);
    }


    // POST api/<GroupController>
    [HttpPost]
    public async Task<IActionResult> CreateGroup([FromBody] CreateGroupDTO createGroupDTO) //NOTE need to make sure 
    {
        var group = await _groupService.CreateGroup(createGroupDTO, GetCurrentUserID());
        if (group == null)
            return BadRequest();

        return CreatedAtRoute(nameof(GetGroup),  new {groupId = group.Id }, group);
    }


    // PUT api/<GroupController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGroup(int id, [FromBody] UpdateGroupDTO updateGroupDTO)
    {
        return Ok(await _groupService.UpdateGroup(id, updateGroupDTO, GetCurrentUserID()));
    }


    // DELETE api/<GroupController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroup(int id)
    {
        if(await _groupService.DeleteGroup(id, GetCurrentUserID()))
            return NoContent();
        return NotFound();
            
    }
}
