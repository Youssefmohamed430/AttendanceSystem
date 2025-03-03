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

            builder.Property(a => a.Email)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(a => a.PhoneNumber)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(a => a.UserName)
                   .HasMaxLength(255)
                   .IsRequired();

            builder.Property(x => x.PhoneNumber)
                .IsRequired(false);


            builder.ToTable("Users", t =>
            {
                t.HasCheckConstraint("CK_Users_Name_Length", "LEN(Name) >= 3");
                t.HasCheckConstraint("CK_Users_Email_Format", "Email LIKE '%@%'");
                t.HasCheckConstraint("CK_Users_PhoneNumber_Format", "PhoneNumber NOT LIKE '%[^0-9]%'");
                t.HasCheckConstraint("CK_Users_UserName_Format", "LEN(UserName) >= 3");
            });

        }
    }
}
