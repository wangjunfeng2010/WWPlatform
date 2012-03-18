using System.Data.Objects;
using System.Linq;

namespace WWPlatform.DataAccess.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Include<T>(this IQueryable<T> source, string path)
        {
            ObjectQuery<T> objectQuery = source as ObjectQuery<T>;
            if (objectQuery != null)
            {
                return objectQuery.Include(path);
            }

            return source;
        }

        public static IQueryable<T> Top<T>(this IQueryable<T> source, int count,params ObjectParameter[] parameters)
        {
            ObjectQuery<T> objectQuery = source as ObjectQuery<T>;
            if (objectQuery != null)
            {
                return objectQuery.Top(count.ToString(), parameters);
            }

            return source;
        }
    }
}
