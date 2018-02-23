using System;
using System.Collections.Generic;
using System.Linq;

namespace Forte.Migrations
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> PartiallyOrdered<T>(this IEnumerable<T> source) where T : IComparable<T>
        {
            var list = new List<T>(source);
            while (list.Count > 0)
            {
                T minimum = list.FirstOrDefault();
                foreach (var s in list)
                {
                    if (s.CompareTo(minimum) < 0)
                    {
                        minimum = s;
                    }
                }
                yield return minimum;
                list.Remove(minimum);
            }
        }
    }
}