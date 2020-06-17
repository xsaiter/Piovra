namespace Piovra.Ds {
    public class FenwickTree {
        readonly int[] _a;

        public FenwickTree(int size) {            
            _a = new int[size + 1];
        }

        public int Capacity => _a.Length;

        public int Sum(int toIndex) {
            var res = 0;
            var i = toIndex;
            while (i > 0) {
                res += _a[i];
                i -= i & (-i);
            }
            return res;
        }

        public int Sum(int fromIndex, int toIndex) {
            return Sum(toIndex) - Sum(fromIndex - 1);
        }

        public FenwickTree Update(int index, int value) {
            var i = index;
            while (i < Capacity) {
                _a[i] += value;
                i += Pk(i);
            }
            return this;
        }

        static int Pk(int k) => k & (-k);
    }
}