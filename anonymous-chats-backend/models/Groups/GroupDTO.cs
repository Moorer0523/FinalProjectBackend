using anonymous_chats_backend.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace anonymous_chats_backend.Models.Groups;

public class CreateGroupDTO()
{
    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    public string Name { get; set; }

    // If this field is missing, use current datetime as the start date
    [DataType(DataType.DateTime)]
    public DateTime StartDate { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime EndDate { get; set; }

    [Required]
    public List<string> UserNames { get; set; } //Need to make a user or group DTO that pulls in the username from when they add it into the create group page
}

public class UpdateGroupDTO()
{
    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    public string Name { get; set; }

    // If this field is missing, use current datetime as the start date
    [DataType(DataType.DateTime)]
    public DateTime StartDate { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime EndDate { get; set; }

}

