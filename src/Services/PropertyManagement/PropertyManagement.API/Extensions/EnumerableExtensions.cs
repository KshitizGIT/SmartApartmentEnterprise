using System;
using System.Collections.Generic;
using System.Linq;

namespace PropertyManagement.API.Extensions
{

    public static partial class Enumerable
    {
        /// <summary>Returns distinct elements from a sequence according to a specified key selector function.</summary>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.GroupBy(keySelector).Select(s => s.First());
        }
    }
}
