namespace Piovra.Ds.SegmentTree;

public class Tree<T>(Node<T> root) {
    readonly Node<T> _root = root;
    public T Query(int a, int b) => _root.Query(a, b);
}
