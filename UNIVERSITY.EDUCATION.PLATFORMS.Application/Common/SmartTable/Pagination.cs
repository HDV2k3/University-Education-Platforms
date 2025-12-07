namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Common.SmartTable
{
    public class Pagination
    {
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public bool IsPaging { get; set; } = true;
    }
}
