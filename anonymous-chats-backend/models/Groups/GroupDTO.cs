using anonymous_chats_backend.Models.Users;
using anonymous_chats_backend.Models;
using System.ComponentModel.DataAnnotations;

namespace anonymous_chats_backend.Models.Groups;

public class CreateGroupDTO()
{
    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required]
    [MinLength(Globals.MIN_GROUP_SIZE)]
    public List<string> UserIds { get; set; } //Need to make a user or group DTO that pulls in the username from when they add it into the create group page
}

public class UpdateGroupDTO()
{
    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    public string Name { get; set; }
}

