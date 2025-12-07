using UNIVERSITY.EDUCATION.PLATFORMS.Application.Common.Domain;


namespace UNIVERSITY.EDUCATION.PLATFORMS.Domain.Entities
{
    public class Role : DomainEntity<int>
    {
        public Role()
        {
            RolePermissions = new List<RolePermission>();
            UserRoles = new List<UserRole>();
        }

        public List<RolePermission> RolePermissions { get; set; }
        public List<UserRole> UserRoles { get; set; }
    }
}
