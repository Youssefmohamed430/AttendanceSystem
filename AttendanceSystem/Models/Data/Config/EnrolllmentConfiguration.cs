using AttendanceSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceSystem.Models.Data.Config
{
    public class EnrolllmentConfiguration : IEntityTypeConfiguration<Enrolllment>
    {
        public void Configure(EntityTypeBuilder<Enrolllment> builder)
        {
            builder.HasKey(e => new { e.StudId, e.CrsId });

            builder.HasOne(e => e.student)
                   .WithMany(s => s.Enrolllments)
                   .HasForeignKey(e => e.StudId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.course)
                .WithMany(c => c.Enrolllments)
                .HasForeignKey(e => e.CrsId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
