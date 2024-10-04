namespace anonymous_chats_backend.Models;

public abstract class BaseModel
{
    public string CreatedBy { get; set; } // User who created the entity
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow; // Timestamp for when the entity was created
    public string UpdatedBy { get; set; } // User who last updated the entity
    public DateTime? UpdatedOn { get; set; } // Nullable, because it may not have been updated yet
}
