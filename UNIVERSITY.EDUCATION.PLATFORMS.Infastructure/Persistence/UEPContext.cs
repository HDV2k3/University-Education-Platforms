using Microsoft.EntityFrameworkCore;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Services.Interface;
using UNIVERSITY.EDUCATION.PLATFORMS.Domain.Common;
using UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Persistence.Configurations;
using UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Persistence.Entities;

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

        public virtual DbSet<Students> Students { get; set; }

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
                        entry.Entity.IsDelete = false;
                        break;

                    case EntityState.Modified:
                        entry.Entity.ModifiedDate = DateTime.Now;
                        entry.Entity.ModifiedBy = _authenticatedUser.UserId;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Students>()
                .HasIndex(s => s.PhoneNumber)
                .HasDatabaseName("IX_STUDENTS_PHONE")
                .IsUnique();
            modelBuilder.Entity<Students>()
                .HasIndex(s => s.Email)
                .HasDatabaseName("IX_STUDENTS_EMAIL")
                .IsUnique();

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
