namespace Piovra.Numerics {
    public static class Numeric {
        public static bool IsPrime(int n) {
            for (var i = 2; i * i <= n; ++i) {
                if (n % i == 0) {
                    return false;
                }
            }
            return true;
        }

        public static bool IsKthBit(int x, int k) {
            return (x & (1 << (k - 1))) > 0;
        }
    }
}