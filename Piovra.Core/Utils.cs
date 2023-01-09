using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Piovra;

public static class Utils {
    public static bool Empty(this string s) => string.IsNullOrEmpty(s);

    public static bool NonEmpty(this string s) => !s.Empty();

    public static bool Empty<T>(this IEnumerable<T> items) => !items.NonEmpty();

    public static bool NonEmpty<T>(this IEnumerable<T> items) => items.Any();

    public static List<T> AsList<T>(this T obj) => new() { obj };

    public static IEnumerable<T> AsSeq<T>(this T obj) {
        yield return obj;
    }

    public static bool SameSequences<T, TKey>(this IEnumerable<T> items, IEnumerable<T> other, Func<T, TKey> keySelector)
        where TKey : IComparable<TKey> => items.OrderBy(keySelector).SequenceEqual(other.OrderBy(keySelector));

    public static bool SameSequences<T>(this IEnumerable<T> items, IEnumerable<T> other)
        where T : IComparable<T> => items.SameSequences(other, x => x);

    public static bool Same(this string s, string t) {
        if (s is null || t is null) {
            return false;
        }
        var n = Math.Min(s.Length, t.Length);
        return s[..n].Equals(s[..n]);
    }

    public static bool SameIgnoreCase(this string s, string t) => string.Equals(s, t, StringComparison.OrdinalIgnoreCase);

    public static string Mirror(this string s) => new(s.ToCharArray().Reverse().ToArray());

    public static string SetAtIndex(this string s, int index, char c) {
        var a = s.ToCharArray();
        a[index] = c;
        return new string(a);
    }

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
        var result = new List<T>(capacity);
        result.Extend(capacity, create);
        return result;
    }

    public static T Coalesce<T>(params T[] items) where T : class => items.FirstOrDefault(x => x != null);

    public static bool IsKthBit(int x, int k) => (x & (1 << (k - 1))) > 0;

    public static async Task StepByStep<T>(this IEnumerable<T> way, Func<IEnumerable<T>, int, Task> processStep, int stepSize) {
        var number = 0;
        int nextStepSize;
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

    public static int Rows<T>(this T[,] a) => a.GetLength(0);
    public static int Cols<T>(this T[,] a) => a.GetLength(1);

    public static byte[] AsBytes(this IEnumerable<int> nums) => nums.SelectMany(x => BitConverter.GetBytes(x)).ToArray();

    public static byte[] AsBytes(this int num) => BitConverter.GetBytes(num);

    public static int AsNum(this byte[] bytes) => BitConverter.ToInt32(bytes);

    public static int[] AsNums(this byte[] bytes) {
        var size = bytes.Length / sizeof(int);
        var result = new int[size];
        for (var i = 0; i < size; ++i) {
            result[i] = BitConverter.ToInt32(bytes, i * sizeof(int));
        }
        return result;
    }
}