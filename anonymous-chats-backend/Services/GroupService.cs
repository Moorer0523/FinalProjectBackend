using anonymous_chats_backend.Data;
using anonymous_chats_backend.Models.Groups;
using anonymous_chats_backend.Models.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace anonymous_chats_backend.Services;

public class GroupService(AnonymousDbContext context)
{
    private readonly AnonymousDbContext _context = context;
    public async Task<Group> GetGroup(int groupId)
    {
        return await _context.Groups.SingleOrDefaultAsync(x => x.Id == groupId);
    }
    public async Task<List<Group>> GetGroupsForUser(string userId)
    {
        int[] groupDetailIds = _context.GroupDetails
            .Where(x => x.UserID == userId)
            .Select(x=> x.GroupId).ToArray();

        return await _context.Groups.Where(x => groupDetailIds.Contains(x.Id)).ToListAsync();
    }

    public async Task<List<User>> GetUsersFromGroup(int groupId)
    {
        string[] userIds = _context.GroupDetails
            .Where(x => x.GroupId == groupId)
            .Select(x => x.UserID).ToArray();



        return await _context.Users.Where(x => userIds.Contains(x.Id)).ToListAsync();
    }

    public async Task<Group> CreateGroup (CreateGroupDTO createGroupDTO, string authorUsername)
    {

        Group group = new Group();
        group.CreateToGroup(createGroupDTO, authorUsername);

        if (group == null)
            return null;

        await _context.Groups.AddAsync(group);
        await _context.SaveChangesAsync();

        //start for loop for iterating through users and adding them to each new object for the Group Details aspect

        foreach (var user in createGroupDTO.UserIds)
        {
            await _context.GroupDetails.AddAsync(new() { GroupId = group.Id, UserID = user, CreatedBy = authorUsername});
        }
        await _context.SaveChangesAsync();
        return group;
    }

    public async Task<Group> UpdateGroup(int id, UpdateGroupDTO updateGroupDTO, string authorUsername)
    {
        var group = await _context.Groups.FindAsync(id);
        if (group == null)
            return null;

        group.UpdateToGroup(updateGroupDTO, authorUsername);

        _context.Update(group);
        await _context.SaveChangesAsync();

        return group;
        
    }

    public async Task<bool> DeleteGroup(int id, string authorUsername)
    {
        var group = await _context.Groups.FindAsync(id);
        if (group == null)
            return false;
            
        _context.Groups.Remove(group);
        await _context.SaveChangesAsync();
        return true;
    }
}
