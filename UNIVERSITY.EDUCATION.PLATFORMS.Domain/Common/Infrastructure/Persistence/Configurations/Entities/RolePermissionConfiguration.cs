using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UNIVERSITY.EDUCATION.PLATFORMS.Domain.Entities;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Domain.Common.Infrastructure.Persistence.Configurations.Entities
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });
        }
    }
}
