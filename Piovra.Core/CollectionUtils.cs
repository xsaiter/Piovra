using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Piovra;

public static class CollectionUtils {
    public static bool Empty<T>(this IEnumerable<T> items) => !items.NonEmpty();
    public static bool NonEmpty<T>(this IEnumerable<T> items) => items.Any();

    public static bool SameSequences<T, TKey>(this IEnumerable<T> items, IEnumerable<T> other, Func<T, TKey> keySelector)
        where TKey : IComparable<TKey> => items.OrderBy(keySelector).SequenceEqual(other.OrderBy(keySelector));

    public static bool SameSequences<T>(this IEnumerable<T> items, IEnumerable<T> other)
        where T : IComparable<T> => items.SameSequences(other, x => x);

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

    public static T Coalesce<T>(params T[] items) where T : class =>
        items.FirstOrDefault(x => x != null);
}
