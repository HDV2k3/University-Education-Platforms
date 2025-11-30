using System.ComponentModel.DataAnnotations;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Users
{
    public class CreateUserRequest
    {
        [Required]
        public string FullName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public int UserTypeId { get; set; }
    }
}
