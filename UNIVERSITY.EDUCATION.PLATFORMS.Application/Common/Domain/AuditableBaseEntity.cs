
namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Common.Domain
{
    public class AuditableBaseEntity
    {
        public string CreatedBy { get; set; } 
        public DateTime CreatedDate { get; set; }

        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
