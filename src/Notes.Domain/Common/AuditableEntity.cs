namespace Notes.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedUtc { get; set; }
}
