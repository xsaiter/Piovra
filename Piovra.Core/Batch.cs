using System;
using System.Collections.Generic;

namespace Piovra {
    public class Batch<T> : IDisposable where T : IDisposable {
        public IEnumerable<T> Items { get; }
        Batch(IEnumerable<T> items) => Items = items;
        public void Dispose() => Items.Foreach(x => x?.Dispose());
        public static Batch<T> New(IEnumerable<T> items) => new Batch<T>(items);
    }
}