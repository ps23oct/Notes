using Notes.Domain.Common;

namespace Notes.Domain.Notes;

public class Note : AuditableEntity
{
    public string Title { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public Priority Priority { get; private set; } = Priority.Medium;

    // Factory
    private Note() { } // EF
    public Note(string title, string content, Priority priority = Priority.Medium)
    {
        Update(title, content, priority);
    }

    // Behavior
    public void Update(string title, string content, Priority priority)
    {
        Title = (title ?? string.Empty).Trim();
        Content = (content ?? string.Empty).Trim();
        Priority = priority;
        UpdatedUtc = DateTime.UtcNow;
    }
}
