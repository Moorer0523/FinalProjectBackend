using anonymous_chats_backend.Models.Users;
using System.Runtime.CompilerServices;

namespace anonymous_chats_backend.Models.Groups;

public static class GroupExtensions
{
    public static void CreateToGroup(this Group group, CreateGroupDTO createGroupDTO, string authorUsername)
    {
        group.Name = createGroupDTO.Name;
        group.CreatedBy = authorUsername;
    }

    public static void UpdateToGroup(this Group group, UpdateGroupDTO updateGroupDTO, string authorUsername)
    {
        group.Name = updateGroupDTO.Name;
        group.UpdatedBy = authorUsername;
        group.UpdatedOn = DateTime.UtcNow;
    }

}
