using System;

namespace Piovra;
public class Period : Range<DateTime> {
    public Period(DateTime l, DateTime r) : base(l, r) { }
}