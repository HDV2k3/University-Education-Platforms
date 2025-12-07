
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.BaseResponse;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Users
{
    public class UserDetailResponse : DomainResponse<Guid>
    {

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? Code { get; set; }

        public string? Description { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public int UserTypeId { get; set; }

        public int UserTypeName { get; set; }

        public bool IsActive { get; set; }

        public int Status { get; set; }

        public List<string> PermissionCodes { get; set; } = new();

        public List<string> RoleNames { get; set; } = new();

        public string? Token { get; set; }
    }
}
