using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolHub.Domain.Entities;

namespace SchoolHub.Infrastructure.Persistence.Configurations;

public sealed class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(s => s.Id);

        builder.OwnsOne(s => s.StudentId, sid =>
        {
            sid.Property(p => p.Value)
                .IsRequired();
        });

        builder.Property(s => s.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.BirthDate)
            .IsRequired();

        builder.OwnsOne(s => s.Address, address =>
        {
            address.Property(a => a.City)
                .HasMaxLength(100);

            address.Property(a => a.Street)
                .HasMaxLength(100);

            address.Property(a => a.PostalCode)
                .HasMaxLength(20);
        });
    }
}


