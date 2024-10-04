using System.ComponentModel.DataAnnotations;

namespace anonymous_chats_backend.Models.Chats;

public class ChatDetail : BaseModel
{
    [Required]
    public int ChatId { get; set; }

    [Required]
    [MaxLength(255)]

    public string UserId { get; set; }
}
