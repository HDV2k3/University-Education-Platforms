using System.ComponentModel.DataAnnotations;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Common.Domain;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Domain.Entities
{
    public class Users : DomainEntity<Guid>
    {

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
