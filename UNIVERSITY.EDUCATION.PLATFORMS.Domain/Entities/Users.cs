using System.ComponentModel.DataAnnotations;
using UNIVERSITY.EDUCATION.PLATFORMS.Domain.Common;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Domain.Entities
{
    public class Users : DomainEntity<Guid>
    {
        public string FullName { get; set; }

        [MaxLength(ListBaseEntityConstants.EmailMaxLength)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(ListBaseEntityConstants.PasswordMaxLength)]
        public string Password { get; set; }

        public bool IsActive { get; set; }

        public int UserTypeId { get; set; }
        public UserType? UserType { get; set; }

        public Students? StudentProfile { get; set; }
    }
}
