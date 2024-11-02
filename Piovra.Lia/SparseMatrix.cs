using System;
using System.Collections.Generic;

namespace Piovra.Lia;

public class SparseMatrix<T> where T : IComparable<T> {
    readonly Dictionary<(int, int), T> _m = [];
    public SparseMatrix(int h, int w) => (H, W) = (h, w);

    public int H { get; }
    public int W { get; set; }

    public T this[int i, int j] {
        get {
            var key = (i, j);
            if (_m.TryGetValue(key, out T value)) {
                return value;
            }
            throw new Exception($"no ({i} {j})");
        }
        set {
            var key = (i, j);
            _m[key] = value;
        }
    }
}
