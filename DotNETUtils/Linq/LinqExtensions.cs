using System;
using System.Collections.Generic;
using System.Linq;



namespace Roslan.DotNetUtils.Linq {



    /// <summary>
    /// Contains extension methods for LINQ.
    /// </summary>
    public static class LinqExtensions {



        /// <summary>
        /// Unnests a collection of objects that by themselves contain a collection of objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TCollection"></typeparam>
        /// <param name="source"></param>
        /// <param name="collectionSelector"></param>
        /// <param name="resultSelector"></param>
        /// <returns></returns>
        public static IEnumerable<T> Unnest<T, TCollection>(this IEnumerable<T> source, Func<T, IEnumerable<TCollection>> collectionSelector, Func<T, TCollection, T> resultSelector) {

            var result = source
                .SelectMany(item => collectionSelector(item)
                .Select(singleItem => resultSelector(item, singleItem)));

            return result;
        }
    }
}
