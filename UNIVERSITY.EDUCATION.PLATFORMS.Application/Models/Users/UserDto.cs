using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        public int UserTypeId { get; set; }
        public string UserTypeName { get; set; }
    }
}
