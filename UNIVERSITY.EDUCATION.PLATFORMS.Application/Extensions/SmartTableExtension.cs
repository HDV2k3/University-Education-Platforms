using UNIVERSITY.EDUCATION.PLATFORMS.Application.Common.SmartTable;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Extensions
{
    public static class SmartTableExtension
    {
        public static IQueryable<TModel> AppendFilter<TModel>(this IQueryable<TModel> query, List<Filter> groupFilter)
        {
            foreach (var filter in groupFilter)
            {
                List<Filters> filters = filter.Filters != null ? filter.Filters : new List<Filters>();
                int filterLength = 0;
                if (filters != null)
                {
                    filterLength = filters.Count();
                }
                else
                {
                    filters = new List<Filters>();
                }
               
                if (filterLength > 0)
                {
                    foreach (var item in filters)
                    {
                        query = query.FilterByName(filter);
                    }
                }
            }
            return query;
        }
    }
}
