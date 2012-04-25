using System;
using System.Collections.Generic;

namespace Duplicity.Collections
{
    public static class EnumerableExtensions
    {
        public static void Each<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null) return;

            foreach (var item in source)
                action(item);
        }
    }
}