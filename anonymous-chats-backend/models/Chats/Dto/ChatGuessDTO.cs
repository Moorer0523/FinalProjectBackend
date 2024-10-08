using System.ComponentModel.DataAnnotations;

namespace anonymous_chats_backend.Models.Chats.Dto;

public class UpdateChatGuessDTO
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string GuesseeId { get; set; }
}
