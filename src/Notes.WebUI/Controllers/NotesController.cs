using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notes.Application.Notes;
using Notes.Domain.Notes;
using Notes.Infrastructure.Persistence;

namespace Notes.WebUI.Controllers;

public class NotesController : Controller
{
    private readonly AppDbContext _db;
    private readonly IValidator<CreateOrUpdateNoteRequest> _validator;

    public NotesController(AppDbContext db, IValidator<CreateOrUpdateNoteRequest> validator)
    {
        _db = db;
        _validator = validator;
    }

    // GET /Notes
    public async Task<IActionResult> Index()
    {
        var items = await _db.Notes
            .OrderByDescending(n => n.CreatedUtc)
            .AsNoTracking()
            .ToListAsync();

        return View(items);
    }

    // POST /Notes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateOrUpdateNoteRequest request)
    {
        var result = await _validator.ValidateAsync(request);
        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            var items = await _db.Notes.OrderByDescending(n => n.CreatedUtc).ToListAsync();
            return View("Index", items); // redisplay with errors
        }

        var note = new Note(request.Title, request.Content, request.Priority);
        _db.Notes.Add(note);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // PATCH /Notes/Edit/{id}   (AJAX inline edit)
    [HttpPatch]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromForm] Guid id, [FromForm] string title, [FromForm] string content, [FromForm] Priority priority)
    {
        var request = new CreateOrUpdateNoteRequest { Id = id, Title = title, Content = content, Priority = priority };
        var result = await _validator.ValidateAsync(request);
        if (!result.IsValid) return BadRequest(result.Errors.Select(e => e.ErrorMessage));

        var note = await _db.Notes.FirstOrDefaultAsync(n => n.Id == id);
        if (note is null) return NotFound();

        note.Update(title, content, priority);
        await _db.SaveChangesAsync();

        return Ok(new { success = true });
    }

    // DELETE /Notes/Delete/{id}  (AJAX)
    [HttpDelete]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete([FromForm] Guid id)
    {
        var note = await _db.Notes.FindAsync(id);
        if (note is null) return NotFound();
        _db.Remove(note);
        await _db.SaveChangesAsync();
        return Ok(new { success = true });
    }
}
