using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace anonymous_chats_backend.Models.Groups;

public class Group : BaseModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    public string Name { get; set; }
}
