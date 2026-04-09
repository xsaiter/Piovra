namespace Piovra;

public record Range<T> where T : IComparable<T> {
    public Range(T l, T r) {
        Requires.True(l.CompareTo(r) <= 0, "L must be <= R");
        (L, R) = (l, r);
    }

    public void Deconstruct(out T l, out T r) => (l, r) = (L, R);

    public T L { get; init; }
    public T R { get; init; }

    public bool Contains(T value) {
        return value.CompareTo(L) >= 0 && value.CompareTo(R) <= 0;
    }
}
