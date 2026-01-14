using System.Collections.Generic;

namespace Piovra.Ds.SegmentTree;

public abstract class Node<T>(int a, int b) {
    public int A { get; } = a;
    public int B { get; } = b;
    public T Value { get; set; }
    public Node<T> L { get; set; }
    public Node<T> R { get; set; }

    public bool IsLeaf() => A == B;
    public int Mid() => A + (B - A) / 2;
    public bool Contains(int a, int b) => a <= A && B <= b;
    public bool Intersects(int a, int b) => !(b < A || a > B);

    public abstract T Query(int a, int b);
    public abstract void Build(List<T> arr);
}
