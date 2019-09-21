using System;

namespace Piovra {
    public class Range<T> {
        public Range(T l, T r) => (L, R) = (l, r);
        public T L { get; }
        public T R { get; }
    }

    public class Period : Range<DateTime> {
        public Period(DateTime l, DateTime r) : base(l, r) { }
    }
}