namespace Piovra {
    public class Range<T> {
        public Range(T l, T r) {
            L = l;
            R = r;
        }
        public T L { get; }
        public T R { get; }
    }
}