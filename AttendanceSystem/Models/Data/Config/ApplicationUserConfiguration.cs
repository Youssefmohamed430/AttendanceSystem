using AttendanceSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceSystem.Models.Data.Config
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(a => a.Name)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Users_Name_Length", "LEN(Name) >= 3");
            });

            builder.Property(a => a.Email)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Users_Email_Format", "Email LIKE '%@%'");
            });

            builder.Property(a => a.PhoneNumber)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Users_PhoneNumber_Format", "Phone NOT LIKE '%[^0-9]%'");
            });

            builder.Property(a => a.UserName)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Users_UserName_Format", "LEN(Name) >= 3");
            });

            builder.ToTable("Users");

        }
    }
}
