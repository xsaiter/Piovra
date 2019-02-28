using System;

namespace Piovra.Lia {
    public static class Help {
        public const double EPSILON = 1e-3;

        public static bool Eq(this double x, double y, double espilon = EPSILON) {
            return Math.Abs(x - y) <= espilon;
        }
    }
}