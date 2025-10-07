using System;
using System.Collections.Generic;
using System.Linq;

namespace Piovra;

public static class Utils {
    public static bool Empty(this string s) => string.IsNullOrEmpty(s);
    public static bool NonEmpty(this string s) => !s.Empty();
    public static List<T> AsList<T>(this T obj) => [obj];

    public static IEnumerable<T> AsSeq<T>(this T obj) {
        yield return obj;
    }

    public static bool Same(this string s, string t) {
        if (s is null || t is null) {
            return false;
        }
        var n = Math.Min(s.Length, t.Length);
        return s[..n].Equals(s[..n]);
    }

    public static bool SameIgnoreCase(this string s, string t) =>
        string.Equals(s, t, StringComparison.OrdinalIgnoreCase);

    public static string Mirror(this string s) {
        var arr = s.ToCharArray();
        Array.Reverse(arr);
        return new(arr);
    }

    public static string SetAtIndex(this string s, int index, char c) {
        var a = s.ToCharArray();
        a[index] = c;
        return new(a);
    }

    public static int ComputeHash(this IEnumerable<object> items) =>
        items.Select(x => x == null ? 0 : x.GetHashCode())
            .Aggregate(23, (hash, itemHash) => hash * 31 + itemHash);

    public static bool IsKthBit(int x, int k) => (x & (1 << (k - 1))) > 0;

    public static bool Le<T>(this T x, T y) where T : IComparable<T> => x.CompareTo(y) <= 0;
    public static bool Lt<T>(this T x, T y) where T : IComparable<T> => x.CompareTo(y) < 0;

    public static bool Ge<T>(this T x, T y) where T : IComparable<T> => x.CompareTo(y) >= 0;
    public static bool Gt<T>(this T x, T y) where T : IComparable<T> => x.CompareTo(y) > 0;

    public static bool Eq<T>(this T x, T y) where T : IComparable<T> => x.CompareTo(y) == 0;
    public static bool Ne<T>(this T x, T y) where T : IComparable<T> => x.CompareTo(y) != 0;

    public static int Rows<T>(this T[,] a) => a.GetLength(0);
    public static int Cols<T>(this T[,] a) => a.GetLength(1);

    public static byte[] AsBytes(this IEnumerable<int> nums) =>
        nums.SelectMany(x => BitConverter.GetBytes(x)).ToArray();

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
