using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.BaseReponse;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Paged
{
    public class PagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize, int rowCount)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.RowCount = rowCount;
            this.Data = data;
            this.Message = "No message";
            this.Succeeded = true;
            this.Errors = null;
        }

        public PagedResponse(T data, int rowcount)
        {
            this.RowCount = rowcount;
            this.Data = data;
            this.Message = null;
            this.Succeeded = true;
            this.Errors = null;
        }
    }
}
