using Microsoft.EntityFrameworkCore;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Services.Interface;
using UNIVERSITY.EDUCATION.PLATFORMS.Domain.Common;
using UNIVERSITY.EDUCATION.PLATFORMS.Domain.Entities;
using UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Persistence.Configurations;
using UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Persistence.Configurations.Entities;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Persistence
{
    public partial class UEPContext : BaseDbContext
    {
        private readonly IAuthenticatedService _authenticatedUser;

        public UEPContext(DbContextOptions<UEPContext> options, IAuthenticatedService authenticatedUser)
            : base(options)
        {
            _authenticatedUser = authenticatedUser;
        }

        #region DbSets
        public DbSet<Users> Users { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Students> Students { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        #endregion

        #region SaveChanges Tracking
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.ModifiedDate = DateTime.Now;
                        entry.Entity.CreatedBy = _authenticatedUser.UserId;
                        entry.Entity.ModifiedBy = _authenticatedUser.UserId;
                        entry.Entity.IsDeleted = false;
                        break;

                    case EntityState.Modified:
                        entry.Entity.ModifiedDate = DateTime.Now;
                        entry.Entity.ModifiedBy = _authenticatedUser.UserId;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StudentConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
