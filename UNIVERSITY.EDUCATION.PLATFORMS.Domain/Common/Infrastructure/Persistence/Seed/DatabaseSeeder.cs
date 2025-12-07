using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Reflection;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Constants;
using UNIVERSITY.EDUCATION.PLATFORMS.Domain.Entities;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Domain.Common.Infrastructure.Persistence.Seed
{
    public static class DatabaseSeeder
    {
        private static readonly DateTime SeedDate =
            new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private static readonly Guid AdminId =
            Guid.Parse("6798ed9c-6dee-48f1-98f3-71012c65e349");

        public static void Seed(this ModelBuilder modelBuilder)
        {
            SeedPermissions(modelBuilder);
            SeedAdminRole(modelBuilder);
            SeedAdminUser(modelBuilder);
            SeedAdminUserRoles(modelBuilder);
            SeedAdminRolePermissions(modelBuilder);
            SeedUserTypes(modelBuilder);
        }

        private static string GetDescription(Enum value)
        {
            return value.GetType()
                .GetField(value.ToString())
                ?.GetCustomAttribute<DescriptionAttribute>()
                ?.Description ?? value.ToString();
        }

        // ======================================
        // 1. PERMISSIONS
        // ======================================
        private static void SeedPermissions(ModelBuilder modelBuilder)
        {
            var permissions = Enum.GetValues(typeof(CommandCode))
                .Cast<CommandCode>()
                .Select((cmd, index) => new Permission
                {
                    Id = index + 1,
                    Name = GetDescription(cmd),
                    Group = cmd.ToString(),
                    IsActive = true,
                    IsDeleted = false,
                    CreatedDate = SeedDate,
                    ModifiedDate = SeedDate,
                    CreatedBy = "system",
                    ModifiedBy = "system"
                })
                .ToList();

            modelBuilder.Entity<Permission>().HasData(permissions);
        }

        // ======================================
        // 2. ROLE ADMIN
        // ======================================
        private static void SeedAdminRole(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(new Role
            {
                Id = 1,
                Name = "Administrator",
                IsActive = true,
                IsDeleted = false,
                CreatedDate = SeedDate,
                ModifiedDate = SeedDate,
                CreatedBy = "system",
                ModifiedBy = "system"
            });
        }
        private static void SeedUserTypes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserType>().HasData(new UserType
            {
                Id = 1,
                Name = "Administrator",
                IsActive = true,
                IsDeleted = false,
                CreatedDate = SeedDate,
                ModifiedDate = SeedDate,
                CreatedBy = "system",
                ModifiedBy = "system"
            });
        }

        // ======================================
        // 3. USER ADMIN
        // ======================================
        private static void SeedAdminUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().HasData(new Users
            {
                Id = AdminId,
                Name = "System Administrator",
                Email = "admin@uep.edu.vn",
                Password = "VietDksh@#!2003", // HASH STATIC
                IsActive = true,
                IsDeleted = false,
                UserTypeId = 1,
                CreatedDate = SeedDate,
                ModifiedDate = SeedDate,
                CreatedBy = "system",
                ModifiedBy = "system"
            });
        }

        // ======================================
        // 4. USER → ROLE
        // ======================================
        private static void SeedAdminUserRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>().HasData(new UserRole
            {
                UserId = AdminId,
                RoleId = 1
            });
        }

        // ======================================
        // 5. ROLE → FULL PERMISSION
        // ======================================
        private static void SeedAdminRolePermissions(ModelBuilder modelBuilder)
        {
            var totalPermissions = Enum.GetNames(typeof(CommandCode)).Length;

            var data = new List<RolePermission>();

            for (int i = 1; i <= totalPermissions; i++)
            {
                data.Add(new RolePermission
                {
                    RoleId = 1,
                    PermissionId = i
                });
            }

            modelBuilder.Entity<RolePermission>().HasData(data);
        }
    }
}
