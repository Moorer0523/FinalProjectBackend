using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace anonymous_chats_backend.Models.Groups;

public class GroupDetail : BaseModel
{
    [Required]
    public int GroupId { get; set; }

    [Required]
    [MaxLength(255)]
    public string UserID { get; set; }
}
