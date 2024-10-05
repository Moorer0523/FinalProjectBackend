namespace anonymous_chats_backend.Models.Users;

public static class UserExtensions
{
    public static void updateUser(this User user, UpdateUserDTO updateUserDTO)
    {
        user.Email = updateUserDTO.Email;
        user.UserName = updateUserDTO.UserName;

        //Putting auditing fields in the update user method, not sure if this would be better set in the api call 
        user.UpdatedBy = user.Id;
        user.UpdatedOn = DateTime.Now;

    }
}
