using System;

namespace Piovra;

public class Period(DateTime l, DateTime r) : Range<DateTime>(l, r);
