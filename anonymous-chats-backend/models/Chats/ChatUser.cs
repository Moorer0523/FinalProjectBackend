using System.ComponentModel.DataAnnotations;

namespace anonymous_chats_backend.Models.Chats;

public class ChatUser : BaseModel
{
    [Required]
    [MaxLength(255)]
    public string UserId { get; set; }

    [Required]
    public int ChatId { get; set; }

    [Required]
    [MaxLength(25)]
    public string Pseudonym { get; set; }
}
