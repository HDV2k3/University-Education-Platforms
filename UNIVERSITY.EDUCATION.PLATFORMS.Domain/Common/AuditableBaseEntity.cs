
namespace UNIVERSITY.EDUCATION.PLATFORMS.Domain.Common
{
    public class AuditableBaseEntity
    {
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public Guid? ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
