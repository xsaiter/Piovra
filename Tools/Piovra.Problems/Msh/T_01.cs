using System;
using System.Collections.Generic;
using System.Linq;

namespace Piovra.Problems.Msh {
    public class T_01_A {
        public static List<int> GetPrimes(int n, int m) {
            var res = new List<int>();
            for (var i = n; i < m; ++i) {
                if (Numeric.IsPrime(i)) {
                    res.Add(i);
                }
            }
            return res;
        }
    }

    public class T_01_B {
        public static string Expr(List<int> nums, int s) {
            var res = string.Empty;
            var n = nums.Count;
            var signs = new List<char>(Enumerable.Repeat('+', n - 1));
            var m = (int)Math.Pow(2, n - 1);
            var i = 0;
            int b = 0;
            while (i < m) {
                if (Calc(nums, signs) == s) {
                    break;
                }
                ++i;
            }
            return res;
        }

        public static bool Is1(int n, int i) {
            var x = 1 << i;
            return (n & x) > 1;
        }

        public static int Calc(List<int> nums, List<char> sings) {
            var res = 0;
            var n = nums.Count;
            if (n == 0) {
                return res;
            }
            res = nums[0];
            if (n == 1) {
                return res;
            }
            for (var i = 1; i < n; ++i) {
                if (sings[i - 1] == '+') {
                    res += nums[i];
                } else {
                    res += (-1) * nums[i];
                }
            }
            return res;
        }
    }

    public class T_01_C {

    }

    public class T_01_D {

    }

    public class T_01_E {

    }

    public class T_01_F {

    }
}
