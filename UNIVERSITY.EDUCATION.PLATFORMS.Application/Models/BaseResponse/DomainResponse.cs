using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Constants;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.BaseResponse
{
    public abstract class DomainResponse<T>
    {
        public T Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public bool IsDeleted { get; set; }

        public string CreateDateText => string.Format(SystemConstants.DateFormatddMMyyyyHHmmss0, CreatedDate);
        public string AlterDateText => string.Format(SystemConstants.DateFormatddMMyyyyHHmmss0, ModifiedDate);
    }
}
