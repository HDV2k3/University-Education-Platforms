using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Common.Domain;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Users
{
    public class UpdateUserRequest : DomainUpdate<Guid>
    {
        [Required]
        public string FullName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public int UserTypeId { get; set; }
    }
}
