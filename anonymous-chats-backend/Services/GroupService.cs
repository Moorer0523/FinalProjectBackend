using anonymous_chats_backend.Data;
using anonymous_chats_backend.Models.Groups;
using anonymous_chats_backend.Models.Users;
using Microsoft.EntityFrameworkCore;

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

    public async Task<Group> CreateGroup (CreateGroupDTO createGroupDTO, string authorUsername)
    {

        Group group = new Group();
        group.CreateToGroup(createGroupDTO, authorUsername);

        if (group == null)
            return null;

        await _context.Groups.AddAsync(group);
        await _context.SaveChangesAsync();

        //start for loop for iterating through users and adding them to each new object for the Group Details aspect

        foreach (var user in createGroupDTO.UserNames)
        {
            await _context.GroupDetails.AddAsync(new() { GroupId = group.Id, UserID = user });
        }
        return group;
    }

    //public async Task<Group> UpdateGroup(UpdateGroupDTO updateGroupDTO, )
}
