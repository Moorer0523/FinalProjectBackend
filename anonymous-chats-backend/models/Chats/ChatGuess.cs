using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace anonymous_chats_backend.Models.Chats;

public class ChatGuess : BaseModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int ChatId { get; set; }

    [Required]
    [MaxLength(255)]
    public string GuesserId { get; set; }

    [Required]
    [MaxLength(255)]
    public string GuesseeId { get; set; }

    [Required]
    [MaxLength(255)]
    public string ActualId { get; set; }

}
