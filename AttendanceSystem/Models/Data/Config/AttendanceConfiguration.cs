using AttendanceSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceSystem.Models.Data.Config
{
    public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Date)
                .HasColumnType("date")
                .IsRequired();

            builder.HasOne(x => x.student)
                   .WithMany(x => x.Attendances)
                   .HasForeignKey(x => x.StudId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Attendances");
        }
    }
}
