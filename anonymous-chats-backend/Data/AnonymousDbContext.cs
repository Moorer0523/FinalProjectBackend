using anonymous_chats_backend.Models.Chats;
using anonymous_chats_backend.Models.Groups;
using anonymous_chats_backend.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace anonymous_chats_backend.Data;

public class AnonymousDbContext : DbContext
{
    public AnonymousDbContext(DbContextOptions<AnonymousDbContext> options) : base(options) { }

    // Users
    public virtual DbSet<User> Users { get; set; }

    // Groups
    public virtual DbSet<Group> Groups { get; set; }
    public virtual DbSet<GroupDetail> GroupDetails { get; set; }

    // Chats
    public virtual DbSet<Chat> Chats { get; set; }
    public virtual DbSet<ChatDetail> ChatDetails { get; set; }
    public virtual DbSet<ChatUser> ChatUsers { get; set; }
    public virtual DbSet<ChatMessage> ChatMessages { get; set; }
    public virtual DbSet<ChatGuess> ChatGuesses { get; set; }
}
