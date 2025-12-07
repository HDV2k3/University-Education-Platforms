namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Common.SmartTable
{
    public class Filter
    {
        public List<Filters> Filters { get; set; }

        public Logic Logic { get; set; }

        public Filter()
        {
            Logic = Logic.or;
        }
    }

    public class Filters
    {
        public string? Operator { get; set; }

        public string? Field { get; set; }

        public object? Value { get; set; }
    }
}
