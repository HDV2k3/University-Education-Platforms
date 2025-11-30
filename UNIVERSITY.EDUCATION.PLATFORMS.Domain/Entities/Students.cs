using System.ComponentModel.DataAnnotations;
using UNIVERSITY.EDUCATION.PLATFORMS.Domain.Common;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Domain.Entities
{
    public class Students : DomainEntity<Guid>
    {
        public Guid UserId { get; set; }
        public Users User { get; set; }

        [MaxLength(ListBaseEntityConstants.StudentCodeMaxLength)]
        public string StudentCode { get; set; }

        [MaxLength(ListBaseEntityConstants.ThousandMaxLength)]
        public string Program { get; set; }

        [MaxLength(ListBaseEntityConstants.ThousandMaxLength)]
        public string TranningProgram { get; set; }

        [MaxLength(ListBaseEntityConstants.ThousandMaxLength)]
        public string Department { get; set; }

        [MaxLength(ListBaseEntityConstants.ThousandMaxLength)]
        public string SchoolYear { get; set; }

        [MaxLength(ListBaseEntityConstants.ThousandMaxLength)]
        public string Major { get; set; }
    }
}
