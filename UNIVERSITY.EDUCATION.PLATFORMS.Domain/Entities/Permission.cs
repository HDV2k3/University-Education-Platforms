using UNIVERSITY.EDUCATION.PLATFORMS.Domain.Common;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Domain.Entities
{
    public class Permission : DomainEntity<int>
    {
        public string? Group { get; set; }
    }
}
