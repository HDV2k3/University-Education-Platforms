
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Common.Domain;


namespace UNIVERSITY.EDUCATION.PLATFORMS.Domain.Entities
{
    public class RefreshTokenEntity : DomainEntity<int>
    {
        public Guid? UserId { get; set; }
        public string? RefreshToken { get; set; } = string.Empty;
        public DateTime? ExpirationDate { get; set; }
    }
}
