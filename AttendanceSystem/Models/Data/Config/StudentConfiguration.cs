using AttendanceSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceSystem.Models.Data.Config
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x =>x.Email)
                .HasMaxLength(255)
                .IsRequired();

            
            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Students_Name_Length", "LEN(Name) >= 3");
            });

            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Students_Email_Format", "Email LIKE '%@%'");
            });

            builder.Property(x => x.Phone)
                .HasMaxLength(255)
                .IsRequired();

            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Students_Phone_Format", "Phone NOT LIKE '%[^0-9]%'");
            });

            builder.Property(r => r.Role)
                .HasMaxLength(255)
                .IsRequired();

            builder.ToTable("Students");
        }
    }
}
