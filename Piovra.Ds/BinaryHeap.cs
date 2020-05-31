using System;
using System.Collections.Generic;

namespace Piovra.Ds {
    public class BinaryHeap<T> where T : IComparable<T> {
        readonly List<T> _a;

        public BinaryHeap(int capacity = 1, bool nonIncreasing = true) {
            ARG.EnsureOutOfRange(() => capacity < 1, nameof(capacity));
            _a = Utils.AllocateList<T>(capacity + 1, () => default);
            NonIncreasing = nonIncreasing;
            Size = 0;
        }

        public bool NonIncreasing { get; }

        int Size { get; set; }

        public bool IsEmpty() => Size == 0;

        void AssertNonEmpty() {
            if (IsEmpty()) {
                throw new Exception("heap empty");
            }
        }

        public void Add(T item) {
            if (Size + 1 == _a.Count) {
                _a.Extend(_a.Count, () => default);
            }
            _a[++Size] = item;
            Up(Size);
        }

        public T Top() {
            AssertNonEmpty();
            return _a[1];
        }

        public void Pop() {
            AssertNonEmpty();
            Swap(1, Size--);
            Down(1);
            _a[Size + 1] = default;
        }

        void Up(int i) {
            while (i > 1) {
                var p = Parent(i);
                if (!StrictInequality(p, i)) {
                    break;
                }
                Swap(i, p);
                i = p;
            }
        }

        void Down(int i) {
            while (true) {
                var l = Left(i);
                if (l > Size) {
                    break;
                }
                var r = Right(i);
                if (l < Size && StrictInequality(l, r)) {
                    l++;
                }
                if (!StrictInequality(i, l)) {
                    break;
                }
                Swap(i, l);
                i = l;
            }
        }

        void Swap(int i, int j) {
            var t = _a[i];
            _a[i] = _a[j];
            _a[j] = t;
        }

        bool StrictInequality(int i, int j) => NonIncreasing ? _a[i].Lt(_a[j]) : _a[i].Gt(_a[j]);        

        static int Left(int i) => 2 * i;
        static int Right(int i) => 2 * i + 1;
        static int Parent(int i) => i / 2;
    }
}