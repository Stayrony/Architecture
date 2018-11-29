using System;
using System.Collections.Generic;
using System.Linq;

namespace Architecture.Model.Database.Extensions
{
    public static class EnumerableExtensions
    {
        public static string JoinStr(this IEnumerable<string> enumerable, string joiner = ", ") => JoinStr(enumerable, x => x, joiner);

        public static string JoinStr<T>(this IEnumerable<T> enumerable, Func<T, string> selector, string joiner = ", ") => string.Join(joiner, enumerable.Select(selector));

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable) => enumerable == null || !enumerable.Any();

        public static bool IsEmpty<T>(this IEnumerable<T> enumerable) =>
            !enumerable?.Any()
            ?? throw new ArgumentNullException(nameof(enumerable));

        public static IEnumerable<T> Duplicates<T, K>(this IEnumerable<T> enumerable, Func<T, K> groupByFunc)
        {
            if (groupByFunc == null)
                throw new ArgumentNullException(nameof(groupByFunc));

            return enumerable?.GroupBy(groupByFunc)
                              .Where(x => x.Count() > 1)
                              .SelectMany(x => x.AsEnumerable())
                              ?? throw new ArgumentNullException(nameof(enumerable));
        }

        public static IEnumerable<T> DistinctBy<T, K>(this IEnumerable<T> enumerable, Func<T, K> groupByFunc)
        {
            if (groupByFunc == null)
                throw new ArgumentNullException(nameof(groupByFunc));

            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));

            return enumerable.GroupBy(groupByFunc)
                              .Select(x => x.FirstOrDefault());
        }

        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> enumerable) => enumerable ?? Enumerable.Empty<T>();

        public static IEnumerable<T> Duplicates<T>(this IEnumerable<T> enumerable)
            where T : struct
        {
            return Duplicates(enumerable, x => x);
        }

        public static IEnumerable<T> RemoveRange<T, K>(this IEnumerable<T> enumerable, IEnumerable<T> range, Func<T, K> selector) where K : IComparable
        {
            if (range == null)
                throw new ArgumentNullException(nameof(range));

            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return enumerable.Where(x => !range.Any(y => selector(x).CompareTo(selector(y)) == 0));
        }

        public static IEnumerable<T> Intersect<T, K>(this IEnumerable<T> enumerable, IEnumerable<T> range, Func<T, K> selector) where K : IComparable
        {
            if (range == null)
                throw new ArgumentNullException(nameof(range));

            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return enumerable.Where(x => range.Any(y => selector(x).CompareTo(selector(y)) == 0));
        }

        public static IEnumerable<T> Exclude<T, K>(this IEnumerable<T> enumerable, IEnumerable<T> range, Func<T, K> selector) where K : IComparable
        {
            if (range == null)
                throw new ArgumentNullException(nameof(range));

            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return enumerable.Where(x => !range.Any(y => selector(x).CompareTo(selector(y)) == 0));
        }

        public static IEnumerable<T> RemoveRange<T>(this IEnumerable<T> enumerable, IEnumerable<T> range)
        {
            if (range == null)
                throw new ArgumentNullException(nameof(range));

            return enumerable.Where(x => !range.Contains(x));
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source)
            {
                action(element);
            }
        }

        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> enumerable, int size)
        {
            var enumerableList = enumerable.ToList();

            for (var i = 0; i < (float)enumerableList.Count / size; i++)
            {
                yield return enumerableList.Skip(i * size).Take(size);
            }
        }

        public static IEnumerable<string> Duplicates(this IEnumerable<string> enumerable) => Duplicates(enumerable, x => x);

        public static ILookup<object, object> ToLookup(this IEnumerable<string> enumerable, string keyValue, Func<string, string> value) => enumerable.ToLookup(x => (object)keyValue, y => (object)value(y));
    }
}
