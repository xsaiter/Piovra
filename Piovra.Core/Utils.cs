using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Piovra {
    public static class Utils {
        public static bool Empty(this string s) => string.IsNullOrEmpty(s);

        public static bool NonEmpty(this string s) => !s.Empty();

        public static bool Empty<T>(this IEnumerable<T> items) => !items.NonEmpty();

        public static bool NonEmpty<T>(this IEnumerable<T> items) => items.Any();

        public static List<T> AsList<T>(this T obj) => new List<T> { obj };

        public static bool EqSeq<T, TKey>(this IEnumerable<T> items, IEnumerable<T> other, Func<T, TKey> keySelector)
        where TKey : IComparable<TKey> =>
            items.OrderBy(keySelector).SequenceEqual(other.OrderBy(keySelector));

        public static bool EqSeq<T>(this IEnumerable<T> items, IEnumerable<T> other)
        where T : IComparable<T> => items.EqSeq(other, x => x);

        public static bool Eq(this string s, string t) {
            if (s == null || t == null) {
                return false;
            }
            var n = Math.Min(s.Length, t.Length);
            return s.Substring(0, n).Equals(s.Substring(0, n));
        }

        public static bool EqIgnoreCase(this string s, string t) => string.Equals(s, t, StringComparison.OrdinalIgnoreCase);

        public static string Mirror(this string s) => new string(s.ToCharArray().Reverse().ToArray());

        public static int ComputeHash(this IEnumerable<object> items) =>
            items.Select(x => x == null ? 0 : x.GetHashCode()).Aggregate(23, (hash, itemHash) => hash * 31 + itemHash);

        public static IEnumerable<T> Enumerate<T>(params T[] items) => items;

        public static IEnumerable<object> EnumerateObjects(params object[] items) => items.Cast<object>();

        public static void Extend<T>(this ICollection<T> items, IEnumerable<T> newItems) {
            foreach (var newItem in newItems) {
                items.Add(newItem);
            }
        }

        public static void Extend<T>(this ICollection<T> items, int n, Func<T> create) =>
            Enumerable.Range(0, n).Foreach(x => items.Add(create()));

        public static void Foreach<T>(this IEnumerable<T> items, Action<T> action) {
            foreach (var item in items) {
                action(item);
            }
        }

        public static List<T> AllocateList<T>(int capacity, Func<T> create) {
            var res = new List<T>(capacity);
            res.Extend(capacity, create);
            return res;
        }

        public static T Coalesce<T>(params T[] items) where T : class => items.FirstOrDefault(x => x != null);

        public static bool IsKthBit(int x, int k) => (x & (1 << (k - 1))) > 0;

        public static async Task StepByStep<T>(this IEnumerable<T> way, Func<IEnumerable<T>, int, Task> processStep, int stepSize) {
            var number = 0;
            var nextStepSize = 0;
            do {
                var items = way.Skip(number * stepSize).Take(stepSize).ToList();
                nextStepSize = items.Count;
                if (nextStepSize == 0) {
                    break;
                }
                await processStep(items, number);
                ++number;
            } while (nextStepSize == stepSize);
        }

        public static bool Le<T>(this T x, T y) where T : IComparable<T> => x.CompareTo(y) <= 0;
        public static bool Lt<T>(this T x, T y) where T : IComparable<T> => x.CompareTo(y) < 0;

        public static bool Ge<T>(this T x, T y) where T : IComparable<T> => x.CompareTo(y) >= 0;
        public static bool Gt<T>(this T x, T y) where T : IComparable<T> => x.CompareTo(y) > 0;

        public static bool Eq<T>(this T x, T y) where T : IComparable<T> => x.CompareTo(y) == 0;
        public static bool Ne<T>(this T x, T y) where T : IComparable<T> => x.CompareTo(y) != 0;

        public static int Rows<T>(this T[, ] a) => a.GetLength(0);
        public static int Cols<T>(this T[, ] a) => a.GetLength(1);

        public static byte[] AsBytes(this IEnumerable<int> nums) {
            return nums.SelectMany(x => BitConverter.GetBytes(x)).ToArray();
        }

        public static byte[] AsBytes(this int num) {
            return BitConverter.GetBytes(num);
        }

        public static int AsNum(this byte[] bytes) {
            return BitConverter.ToInt32(bytes);
        }

        public static int[] AsNums(this byte[] bytes) {
            var size = bytes.Length / sizeof(int);
            var res = new int[size];
            for (var i = 0; i < size; ++i) {
                res[i] = BitConverter.ToInt32(bytes, i * sizeof(int));
            }
            return res;
        }
    }

    public class Asserts {
        public static void True(bool condition, string message = null) {
            if (!condition) {
                throw new Exception(message);
            }
        }
        public static void False(bool condition, string message = null) {
            True(!condition, message);
        }
    }

    public class Result<T> {
        public T Value { get; set; }
        public static Result<T> Of(T value) => new Result<T> { Value = value };
    }

    public class Batch<T> : IDisposable where T : IDisposable {
        public IEnumerable<T> Items { get; }
        Batch(IEnumerable<T> items) => Items = items;
        public void Dispose() => Items.Foreach(x => x.Dispose());
        public static Batch<T> New(IEnumerable<T> items) => new Batch<T>(items);
    }
}