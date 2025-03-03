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

            builder.HasOne(u => u.User)
                .WithOne(i => i.instructor)
                .HasForeignKey<Instructor>(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.course)
                .WithOne(x => x.instructor)
                .HasForeignKey<Instructor>(x => x.CrsId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Instructors");
        }
    }
}
