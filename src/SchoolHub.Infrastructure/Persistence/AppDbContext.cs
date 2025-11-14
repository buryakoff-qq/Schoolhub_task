using Microsoft.EntityFrameworkCore;
using SchoolHub.Domain.Entities;

namespace SchoolHub.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Student>  Students { get; set; }
    public DbSet<SchoolClass> SchoolClasses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>().OwnsOne(s => s.StudentId, sb =>
        {
            sb.Property(p => p.Value)
                .IsRequired();
        });
        
        modelBuilder.Entity<Student>().OwnsOne(s => s.Address, ab =>
        {
            ab.Property(a => a.City);
            ab.Property(a => a.Street);
            ab.Property(a => a.PostalCode);
        });
    }
}