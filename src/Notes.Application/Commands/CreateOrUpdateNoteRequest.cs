using FluentValidation;
using Notes.Domain.Notes;

namespace Notes.Application.Notes;

public sealed class CreateOrUpdateNoteRequest
{
    public Guid? Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public Priority Priority { get; init; } = Priority.Medium;
}

public sealed class CreateOrUpdateNoteValidator : AbstractValidator<CreateOrUpdateNoteRequest>
{
    public CreateOrUpdateNoteValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(120);
        RuleFor(x => x.Content).MaximumLength(4000);
        RuleFor(x => x.Priority).IsInEnum();
    }
}
