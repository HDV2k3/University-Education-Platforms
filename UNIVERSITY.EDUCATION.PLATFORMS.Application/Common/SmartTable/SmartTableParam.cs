namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Common.SmartTable
{
    public class SmartTableParam
    {
        public Pagination? Pagination { get; set; }

        public List<Filter>? GroupFilters { get; set; }

        public List<string>? Includes { get; set; }

        public virtual ICollection<Sort>? Sort { get; set; }
    }
}
