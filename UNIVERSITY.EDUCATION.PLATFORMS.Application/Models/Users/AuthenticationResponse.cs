using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Users
{
    public class AuthenticationResponse
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsDelete { get; set; }
        public List<string> Roles { get; set; }
        public List<string> Permissions { get; set; }
        public bool IsVerified { get; set; }
        public string Token { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset LockoutEnd { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; }
        public string DeviceId { get; set; }
    }
}
