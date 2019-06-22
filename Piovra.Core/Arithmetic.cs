namespace Piovra {
    public static class Arithmetic {
        public static bool IsPrime(long n) {
            for (var i = 2; i * i <= n; ++i) {
                if (n % i == 0) {
                    return false;
                }
            }
            return true;
        }

        public static long GCD(long a, long b) {
            while (b != 0) {
                var c = a % b;
                a = b;
                b = c;
            }
            return (a > 0) ? a : -a;
        }
    }
}