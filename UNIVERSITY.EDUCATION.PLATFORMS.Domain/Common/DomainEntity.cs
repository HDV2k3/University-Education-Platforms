using System.ComponentModel.DataAnnotations;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Domain.Common
{
    public class DomainEntity<T> : AuditableBaseEntity
    {
        [Key]
        public T Id { get; set; }

        [MaxLength(ListBaseEntityConstants.CodeMaxLength)]
        public string? Code { get; set; }

        [Required]
        [MaxLength(ListBaseEntityConstants.NameMaxLength)]
        public string? Name { get; set; }

        [MaxLength(ListBaseEntityConstants.DescriptionMaxLength)]
        public string? Description { get; set; }
    }
}
