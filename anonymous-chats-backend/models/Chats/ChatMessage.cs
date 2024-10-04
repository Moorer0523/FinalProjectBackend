using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace anonymous_chats_backend.Models.Chats;

public class ChatMessage : BaseModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int ChatId { get; set; }

    [Required]
    [MaxLength(500)]
    public string OriginalMessage { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string FilteredMessage { get; set; } = string.Empty;
}
