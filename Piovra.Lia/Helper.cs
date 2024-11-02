using System;

namespace Piovra.Lia;

public static class Helper {
    public const double EPS = 1e-3;

    public static bool Eq(this double x, double y, double espilon = EPS) => Math.Abs(x - y) <= espilon;
}
