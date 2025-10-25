using Microsoft.EntityFrameworkCore;
using Notes.Domain.Notes;

namespace Notes.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Note> Notes => Set<Note>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Note>(b =>
        {
            b.ToTable("Notes");
            b.HasKey(n => n.Id);
            b.Property(n => n.Title).IsRequired().HasMaxLength(120);
            b.Property(n => n.Content).HasMaxLength(4000);
            b.Property(n => n.Priority).HasConversion<int>();
            b.Property(n => n.CreatedUtc).HasPrecision(0);
            b.Property(n => n.UpdatedUtc).HasPrecision(0);
            b.HasIndex(n => n.CreatedUtc); // for reverse-chronological queries
        });
    }
}
