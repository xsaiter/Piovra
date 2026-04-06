public class SegTreeTest {
    readonly Node _root;

    public SegTreeTest(List<int> a) {
        _root = new Node(0, a.Count - 1);
        _root.Build(a);
    }

    public int Query(int a, int b) {
        return _root.Query(a, b);
    }

    public class Node {
        public int Value { get; set; }
        public Node L { get; set; }
        public Node R { get; set; }
        public int A { get; set; }
        public int B { get; set; }

        public bool Contains(int a, int b) {
            return a <= A && B <= b;
        }

        public bool Intersects(int a, int b) {
            return !(b < A || a > B);
        }

        public int Query(int a, int b) {
            if (!Intersects(a, b)) {
                return 0;
            }
            if (Contains(a, b)) {
                return Value;
            }
            return L.Query(a, b) + R.Query(a, b);
        }

        public Node(int a, int b) {
            A = a;
            B = b;
        }

        public int Mid() {
            return A + (B - A) / 2;
        }

        public void Build(List<int> a) {
            if (A == B) {
                Value = a[A];
                return;
            }
            var C = Mid();

            var l = new Node(A, C);
            var r = new Node(C + 1, B);

            l.Build(a);
            r.Build(a);

            L = l; R = r;
            Value = L.Value + R.Value;
        }
    }
}
