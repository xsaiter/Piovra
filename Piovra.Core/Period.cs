using System;

namespace Piovra;

public record Period(DateTime L, DateTime R) : Range<DateTime>(L, R);
