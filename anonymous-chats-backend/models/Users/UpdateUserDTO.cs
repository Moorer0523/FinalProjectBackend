using System.ComponentModel.DataAnnotations;

namespace anonymous_chats_backend.Models.Users
{
    public class UpdateUserDTO
    {
        [Required]
        [MinLength(8)]
        [MaxLength(24)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
