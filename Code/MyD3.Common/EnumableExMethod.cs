using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Common
{
    public static class EnumableExMethod
    {
        public static IEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, string property, bool descending)
        {
            PropertyInfo prop = typeof(TSource).GetProperty(property);

            if (prop == null)
            {
                throw new Exception("No property '" + property + "' in + " + typeof(TSource).Name + "'");
            }

            if (descending)
            {
                return source.OrderByDescending(x => prop.GetValue(x, null));
            }
            else
            {
                return source.OrderBy(x => prop.GetValue(x, null));
            }
        }
    }
}
