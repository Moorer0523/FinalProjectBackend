using anonymous_chats_backend.Models.Users;
using System.Runtime.CompilerServices;

namespace anonymous_chats_backend.Models.Groups;

public static class GroupExtensions
{
    public static void CreateToGroup(this Group group, CreateGroupDTO createGroupDTO, string authorUsername)
    {
        group.Name = createGroupDTO.Name;
        group.StartDate = createGroupDTO.StartDate;
        group.EndDate = createGroupDTO.EndDate;
        group.CreatedBy = authorUsername;
        group.CreatedOn = DateTime.Now;
    }

    public static void UpdateToGroup(this Group group, UpdateGroupDTO updateGroupDTO, string authorUsername)
    {
        group.Name = updateGroupDTO.Name;
        group.StartDate = updateGroupDTO.StartDate;
        group.EndDate = updateGroupDTO.EndDate;
        group.UpdatedBy = authorUsername;
        group.UpdatedOn = DateTime.Now;
    }

}
