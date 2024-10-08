﻿using anonymous_chats_backend.Data;
using anonymous_chats_backend.Models.Groups;
using anonymous_chats_backend.Models.Users;
using anonymous_chats_backend.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace anonymous_chats_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ApiBaseController
    {
        private readonly GroupService _groupService;

        public GroupController(AnonymousDbContext context)
        {
            _groupService = new(context);
        }
        
        
        [HttpGet("GetGroup/{groupId}")]
        public async Task<IActionResult> GetGroup(int groupId)
        {
            return Ok(await _groupService.GetGroup(groupId));
        }
        // GET api/<GroupController>/5
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserGroups(string userId)
        {
            return Ok(await _groupService.GetGroupsForUser(userId));
        }

        // POST api/<GroupController>
        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupDTO createGroupDTO) //NOTE need to make sure 
        {
            var group = await _groupService.CreateGroup(createGroupDTO, GetCurrentUserID());
            if (group == null)
                return BadRequest();

            return CreatedAtAction("GetGroup", new { id = group.Id }, group);
        }

        // PUT api/<GroupController>/5
        [HttpPut("{id}")]
        public void UpdateGroup(int id, [FromBody] UpdateGroupDTO updateGroupDTO)
        {
        }

        // DELETE api/<GroupController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
