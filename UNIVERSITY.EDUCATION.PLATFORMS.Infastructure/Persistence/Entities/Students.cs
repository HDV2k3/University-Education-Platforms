using System.ComponentModel.DataAnnotations;
using UNIVERSITY.EDUCATION.PLATFORMS.Domain.Common;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Persistence.Entities
{
    public class Students : DomainEntity<int>
    {
        [MaxLength(ListBaseEntityConstants.PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; }

        [MaxLength(ListBaseEntityConstants.AddressMaxLength)]
        public string Address { get; set; }

        [MaxLength(ListBaseEntityConstants.EmailMaxLength)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(ListBaseEntityConstants.PasswordMaxLength)]
        public string Password { get; set; }

        public bool IsActive { get; set; }
    }
}
