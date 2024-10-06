namespace anonymous_chats_backend.Models.Users;

public static class UserExtensions
{
    public static void UpdateToUser(this User user, UpdateUserDTO updateUserDTO, string authorUsername)
    {
        user.Email = updateUserDTO.Email;
        user.UserName = updateUserDTO.UserName;

        user.UpdatedBy = authorUsername;
        user.UpdatedOn = DateTime.Now;

    }

    public static void CreateToUser(this User user, CreateUserDTO createUserDTO, string authorUsername)
    {
        user.Id = createUserDTO.Id;
        user.Email = createUserDTO.Email;
        user.UserName = createUserDTO.UserName;
        user.CreatedBy = authorUsername;
        user.CreatedOn = DateTime.Now;
    }
}
