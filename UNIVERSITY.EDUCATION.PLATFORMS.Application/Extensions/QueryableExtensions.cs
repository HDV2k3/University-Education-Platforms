using System.Linq.Expressions;
using System.Reflection;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Common.SmartTable;
namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Extensions
{
    public static class QueryableExtensions
    {
        // =====================================================================
        // ORDER BY (Dynamic)
        // =====================================================================
        public static IQueryable<T> OrderByName<T>(this IQueryable<T> source, string propertyName, bool isDescending, bool isThenBy = false)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            Type type = typeof(T);
            ParameterExpression parameter = Expression.Parameter(type, "p");

            MemberExpression? memberAccess = null;

            foreach (var property in propertyName.Split('.'))
            {
                memberAccess = Expression.Property(memberAccess ?? (parameter as Expression), property);
            }

            if (memberAccess is null)
                return source;

            type = memberAccess.Type;

            var expression = Expression.Property(memberAccess.Expression!, memberAccess.Member.Name);

            var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            var lambda = Expression.Lambda(delegateType, expression, parameter);

            string methodName = isDescending ? "OrderByDescending" : "OrderBy";

            if (isThenBy)
                methodName = isDescending ? "ThenByDescending" : "ThenBy";

            var method = typeof(Queryable)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => m.Name == methodName && m.GetParameters().Length == 2)
                .First()
                .MakeGenericMethod(typeof(T), type);

            return (IQueryable<T>)method.Invoke(null, new object[] { source, lambda })!;
        }


        // =====================================================================
        // FILTER (Dynamic Where)
        // =====================================================================
        public static IQueryable<T> FilterByName<T>(this IQueryable<T> query, Filter filterOpt)
        {
            if (filterOpt?.Filters == null || filterOpt.Filters.Count == 0)
                return query;

            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            Expression? resultCondition = null;

            foreach (var criteria in filterOpt.Filters)
            {
                if (string.IsNullOrWhiteSpace(criteria.Field))
                    continue;

                // Build member access: p.Field1.Field2...
                Expression? memberAccess = parameter;

                foreach (var part in criteria.Field.Split('.'))
                {
                    memberAccess = Expression.PropertyOrField(memberAccess, part);
                }

                if (memberAccess is null)
                    continue;

                Type memberType = memberAccess.Type;

                // Create constant value (safe for .NET 9)
                ConstantExpression filter = Expression.Constant(null, memberType);

                if (criteria.Value != null)
                {
                    try
                    {
                        if (memberType == typeof(Guid))
                        {
                            filter = Expression.Constant(criteria.Value.ToGuid(Guid.Empty), typeof(Guid));
                        }
                        else if (memberType == typeof(Guid?))
                        {
                            filter = Expression.Constant(criteria.Value.ToNullableGuid(), typeof(Guid?));
                        }
                        else
                        {
                            var converted = Convert.ChangeType(criteria.Value, Nullable.GetUnderlyingType(memberType) ?? memberType);
                            filter = Expression.Constant(converted, memberType);
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                var valueExpression = Expression.Convert(filter, memberType);

                Expression? condition = criteria.Operator switch
                {
                    WhereOperation.Equal => Expression.Equal(memberAccess, valueExpression),
                    WhereOperation.NotEqual => Expression.NotEqual(memberAccess, valueExpression),
                    WhereOperation.Greater => Expression.GreaterThan(memberAccess, valueExpression),
                    WhereOperation.GreaterOrEqual => Expression.GreaterThanOrEqual(memberAccess, valueExpression),
                    WhereOperation.Less => Expression.LessThan(memberAccess, valueExpression),
                    WhereOperation.LessEqual => Expression.LessThanOrEqual(memberAccess, valueExpression),
                    WhereOperation.Contains =>
                        Expression.Call(memberAccess,
                            typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) })!,
                            filter),
                    WhereOperation.FirstChar =>
                        Expression.Call(memberAccess,
                            typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(string) })!,
                            filter),
                    _ => null
                };

                if (condition is null)
                    continue;

                if (filterOpt.Logic == Logic.or)
                {
                    resultCondition = resultCondition == null ? condition : Expression.OrElse(resultCondition, condition);
                }
                else
                {
                    resultCondition = resultCondition == null ? condition : Expression.AndAlso(resultCondition, condition);
                }
            }

            if (resultCondition == null)
                return query;

            var lambda = Expression.Lambda<Func<T, bool>>(resultCondition, parameter);

            return query.Where(lambda);
        }
    }
}
