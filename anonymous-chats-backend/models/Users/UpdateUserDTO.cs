﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace anonymous_chats_backend.Models.Users;

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

public class CreateUserDTO
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)] // Disabled auto increment
    [Required]
    [MaxLength(255)]
    public string Id { get; set; }

    [Required]
    [MinLength(8)]
    [MaxLength(24)]
    public string UserName { get; set; }

    [Required]
    [MaxLength(255)]
    [EmailAddress]
    public string Email { get; set; }
}

