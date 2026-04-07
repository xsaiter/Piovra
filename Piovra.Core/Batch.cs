namespace Piovra;

public class Batch<T> : IDisposable where T : IDisposable {
    readonly List<T> _items;
    public IEnumerable<T> Items => _items;
    Batch(IEnumerable<T> items) => _items = [.. items];
    public static Batch<T> New(IEnumerable<T> items) => new(items);

    public void Dispose() {
        Items.Foreach(x => x?.Dispose());
        GC.SuppressFinalize(this);
    }
}
