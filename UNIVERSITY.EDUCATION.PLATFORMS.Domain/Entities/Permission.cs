using UNIVERSITY.EDUCATION.PLATFORMS.Application.Common.Domain;
namespace UNIVERSITY.EDUCATION.PLATFORMS.Domain.Entities
{
    public class Permission : DomainEntity<int>
    {
        public string? Group { get; set; }
    }
}
