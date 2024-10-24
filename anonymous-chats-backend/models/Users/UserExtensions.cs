﻿namespace anonymous_chats_backend.Models.Users;

public static class UserExtensions
{
    public static void UpdateToUser(this User user, UpdateUserDTO updateUserDTO, string authorUsername)
    {
        user.Email = updateUserDTO.Email;
        user.UserName = updateUserDTO.UserName;
        user.UpdatedBy = authorUsername;
        user.UpdatedOn = DateTime.UtcNow;

    }

    public static void CreateToUser(this User user, CreateUserDTO createUserDTO)
    {
        user.Id = createUserDTO.Id;
        user.Email = createUserDTO.Email;
        user.UserName = createUserDTO.UserName;
        user.CreatedBy = createUserDTO.Id;
    }
}
