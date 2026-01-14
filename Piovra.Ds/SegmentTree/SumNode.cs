using System.Collections.Generic;
using System.Numerics;

namespace Piovra.Ds.SegmentTree;

public class SumNode<T>(int a, int b) : Node<T>(a, b) where T : INumber<T> {
    public override void Build(List<T> arr) {
        if (A == B) {
            Value = arr[A];
            return;
        }
        var C = Mid();

        var l = new SumNode<T>(A, C);
        var r = new SumNode<T>(C + 1, B);

        l.Build(arr);
        r.Build(arr);

        L = l; R = r;
        Value = L.Value + R.Value;
    }

    public override T Query(int a, int b) {
        if (!Intersects(a, b)) {
            return default;
        }
        if (Contains(a, b)) {
            return Value;
        }
        return L.Query(a, b) + R.Query(a, b);
    }
}
