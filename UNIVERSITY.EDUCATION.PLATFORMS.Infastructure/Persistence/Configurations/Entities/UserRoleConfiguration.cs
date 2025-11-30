using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UNIVERSITY.EDUCATION.PLATFORMS.Domain.Entities;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Persistence.Configurations.Entities
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(ur => new { ur.UserId, ur.RoleId });
        }
    }
}
