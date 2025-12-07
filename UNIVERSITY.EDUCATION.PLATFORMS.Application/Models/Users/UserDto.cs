using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.BaseResponse;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Users
{
    public class UserDto :DomainResponse<Guid>
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        public int UserTypeId { get; set; }
        public string? UserTypeName { get; set; }
        public string? Password { get; set; }

        public List<string> PermissionCodes { get; set; } = new();

        public List<string> RoleNames { get; set; } = new();
    }
}
