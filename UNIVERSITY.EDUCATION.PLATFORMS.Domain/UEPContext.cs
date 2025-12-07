using Microsoft.EntityFrameworkCore;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Common.Domain;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Common.Infrastructure;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Service;
using UNIVERSITY.EDUCATION.PLATFORMS.Domain.Common;
using UNIVERSITY.EDUCATION.PLATFORMS.Domain.Common.Infrastructure.Persistence.Configurations;
using UNIVERSITY.EDUCATION.PLATFORMS.Domain.Common.Infrastructure.Persistence.Configurations.Entities;
using UNIVERSITY.EDUCATION.PLATFORMS.Domain.Common.Infrastructure.Persistence.Seed;
using UNIVERSITY.EDUCATION.PLATFORMS.Domain.Entities;



namespace UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Persistence
{
    public partial class UEPContext : BaseDbContext
    {
        private readonly IAuthenticatedUserService _authenticatedUser;

        public UEPContext(DbContextOptions<UEPContext> options, IAuthenticatedUserService authenticatedUser)
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
        public DbSet<RefreshTokenEntity> RefreshTokenEntity { get; set; }
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
                        entry.Entity.CreatedBy = _authenticatedUser.FullName;
                        entry.Entity.ModifiedBy = _authenticatedUser.FullName;
                        entry.Entity.IsDeleted = false;
                        break;

                    case EntityState.Modified:
                        entry.Entity.ModifiedDate = DateTime.Now;
                        entry.Entity.ModifiedBy = _authenticatedUser.FullName;
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
            modelBuilder.Seed();
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
