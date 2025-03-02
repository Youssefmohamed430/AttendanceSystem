using AttendanceSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceSystem.Models.Data.Config
{
    public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Instructors_Name_Length", "LEN(Name) >= 3");
            });

            builder.Property(x => x.Email)
                .HasMaxLength(255)
                .IsRequired();

            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Instructors_Email_Format", "Email LIKE '%@%'");
            });

            builder.Property(r => r.Role)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.ToTable("Instructors");
        }
    }
}
