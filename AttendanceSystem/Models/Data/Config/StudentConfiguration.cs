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

            builder.HasOne(a => a.User)
                   .WithOne(s => s.student)
                   .HasForeignKey<Student>(s => s.Id)
                   .OnDelete(DeleteBehavior.Cascade);
                   

            builder.ToTable("Students");
        }
    }
}
