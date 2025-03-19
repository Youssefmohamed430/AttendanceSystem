using AttendanceSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceSystem.Models.Data.Config
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IsRead).IsRequired(false);

            builder.HasOne(x => x.student)
                .WithMany(x => x.Notifications)
                .HasForeignKey(x => x.StudentId);


            builder.ToTable("Notifications");
        }
    }
}
