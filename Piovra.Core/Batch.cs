using System;
using System.Collections.Generic;

namespace Piovra;

public class Batch<T> : IDisposable where T : IDisposable {
    public IEnumerable<T> Items { get; }
    Batch(IEnumerable<T> items) => Items = items;

    public static Batch<T> New(IEnumerable<T> items) => new(items);

    public void Dispose() {
        Items.Foreach(x => x?.Dispose());
        GC.SuppressFinalize(this);
    }
}