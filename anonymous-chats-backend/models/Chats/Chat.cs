using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace anonymous_chats_backend.Models.Chats;

public class Chat : BaseModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int GroupId { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime StartDate { get; set; }
}
