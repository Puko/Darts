using Darts.Contract;
using Darts.Domain.DomainObjects;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Darts.Domain
{
    public static class QueryableExtensions
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> collection, Filter filter, Expression<Func<T, object>> defaultOrder    )
        {
            IOrderedQueryable<T> result;

            if (!string.IsNullOrEmpty(filter?.Column) && filter?.SortDirection != SortDirection.None)
            {
                result = OrderBy(collection, filter.SortDirection, filter.Column);
            }
            else
            {
                result = collection.OrderBy(defaultOrder);
            }

            return result;
        }

        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, SortDirection sortDirection, string propertyName)
        {
            // LAMBDA: x => x.[PropertyName]
            var parameter = Expression.Parameter(typeof(TSource), "x");
            Expression property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);

            // REFLECTION: source.OrderBy(x => x.Property)
            var orderByMethod = typeof(Queryable).GetMethods().First(x => sortDirection == SortDirection.Descending ? x.Name == "OrderByDescending" : x.Name == "OrderBy" && x.GetParameters().Length == 2);
            var orderByGeneric = orderByMethod.MakeGenericMethod(typeof(TSource), property.Type);
            var result = orderByGeneric.Invoke(null, new object[] { source, lambda });

            return (IOrderedQueryable<TSource>)result;
        }
    }
}
