using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UNIVERSITY.EDUCATION.PLATFORMS.Domain.Entities;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Persistence.Configurations.Entities
{
    public class StudentConfiguration : IEntityTypeConfiguration<Students>
    {
        public void Configure(EntityTypeBuilder<Students> builder)
        {
            builder.HasOne(s => s.User)
                   .WithOne(u => u.StudentProfile)
                   .HasForeignKey<Students>(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
