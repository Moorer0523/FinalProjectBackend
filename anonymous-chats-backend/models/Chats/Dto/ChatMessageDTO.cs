using System.ComponentModel.DataAnnotations;

namespace anonymous_chats_backend.Models.Chats.Dto;

public class CreateChatMessageDTO
{
    [Required]
    public int ChatId { get; set; }

    [Required]
    [MaxLength(500)]
    public string OriginalMessage { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string FilteredMessage { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string CreatedBy { get; set; } // Auditing field

}
