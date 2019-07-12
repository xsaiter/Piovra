using System;

namespace Piovra.Ds {
    public class RBTree<K, V> where K : IComparable<K> {
        readonly Node _nil;
        Node _root;

        public RBTree() {
            _nil = new Node { Color = BLACK };
            _root = _nil;
        }

        public Result<V> GetValue(K key) {
            var x = _root;

            while (x != _nil) {
                var cmp = key.CompareTo(x.Key);
                if (cmp == 0) {
                    break;
                }
                if (cmp < 0) {
                    x = x.L;
                } else {
                    x = x.R;
                }
            }

            return x != null ? Result<V>.Of(x.Value) : null;
        }

        public void Add(K key, V value) {
            var z = new Node { Key = key, Value = value, Color = RED };

            var y = _nil;
            var x = _root;

            int cmp;
            while (x != _nil) {
                y = x;
                cmp = z.Key.CompareTo(x.Key);
                if (cmp < 0) {
                    x = x.L;
                } else {
                    x = x.R;
                }
            }

            z.P = y;

            if (y == _nil) {
                _root = z;
            } else {
                cmp = z.Key.CompareTo(y.Key);
                if (cmp < 0) {
                    y.L = z;
                } else {
                    y.R = z;
                }
            }

            z.L = _nil;
            z.R = _nil;
            z.Color = RED;

            Restore(z);
        }

        void Restore(Node z) {
            while (z.P.Color == RED) {
                if (z.P == z.P.P.L) {
                    var y = z.P.P.R;
                    if (y.Color == RED) {
                        z.P.Color = BLACK;
                        y.Color = BLACK;
                        z.P.P.Color = RED;
                        z = z.P.P;
                    } else {
                        if (z == z.P.R) {
                            z = z.P;
                            LeftRotate(z);
                        }
                        z.P.Color = BLACK;
                        z.P.P.Color = RED;
                        RightRotate(z.P.P);
                    }
                } else {
                    var y = z.P.P.L;
                    if (y.Color == RED) {
                        z.P.Color = BLACK;
                        y.Color = BLACK;
                        z.P.P.Color = RED;
                        z = z.P.P;
                    } else {
                        if (z == z.P.L) {
                            z = z.P;
                            RightRotate(z);
                        }
                        z.P.Color = BLACK;
                        z.P.P.Color = RED;
                        LeftRotate(z.P.P);
                    }
                }
            }

            _root.Color = BLACK;
        }

        void LeftRotate(Node x) {
            var y = x.R;
            x.R = y.L;

            if (y.L != _nil) {
                y.L.P = x;
            }

            y.P = x.P;

            if (x.P == _nil) {
                _root = y;
            } else if (x == x.P.L) {
                x.P.L = y;
            } else {
                x.P.R = y;
            }

            y.L = x;
            x.P = y;
        }

        void RightRotate(Node x) {
            var y = x.L;
            x.L = y.R;

            if (y.R != _nil) {
                y.R.P = x;
            }

            y.P = x.P;

            if (x.P == _nil) {
                _root = y;
            } else if (x == x.P.R) {
                x.P.R = y;
            } else {
                x.P.L = y;
            }

            y.R = x;
            x.P = y;
        }

        class Node {
            public K Key { get; set; }
            public V Value { get; set; }
            public Colors Color { get; set; }
            public Node P { get; set; }
            public Node L { get; set; }
            public Node R { get; set; }
        }

        const Colors RED = Colors.RED;
        const Colors BLACK = Colors.BLACK;

        enum Colors {
            RED, BLACK
        }
    }
}