namespace Piovra.Lia;

public class SparseMatrix<T> where T : IComparable<T> {
    readonly Dictionary<(int, int), T> _m = [];
    public SparseMatrix(int h, int w) {
        Requires.True(h > 0, "Height must be positive");
        Requires.True(w > 0, "Width must be positive");

        (H, W) = (h, w);
    }

    public int H { get; }
    public int W { get; }

    public T this[int i, int j] {
        get {
            ValidateIndices(i, j);
            var key = (i, j);
            if (_m.TryGetValue(key, out var value)) {
                return value;
            }
            throw new Exception($"no ({i} {j})");
        }
        set {
            ValidateIndices(i, j);
            var key = (i, j);
            _m[key] = value;
        }
    }

    void ValidateIndices(int i, int j) {
        Requires.True(i >= 0 && i < H, $"Index i({i}) out of range [0..{H - 1}]");
        Requires.True(j >= 0 && j < W, $"Index j({j}) out of range [0..{W - 1}]");
    }
}
