using System;
using System.Collections.Generic;
using System.Text;

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
            if (n == 0) {
                return res;
            }
            if (n == 1) {
                return $"{nums[0]}";
            }
            var m = (int)Math.Pow(2, n - 1);
            var i = 0;
            while (i < m) {
                if (Calc(nums, i) == s) {
                    var sb = new StringBuilder();
                    var j = 0;
                    sb.Append($"{nums[j]}");
                    for (j = 1; j < n; ++j) {
                        if (!Numeric.IsKthBit(i, j)) {
                            sb.Append($" + {nums[j]}");
                        } else {
                            sb.Append($" - {nums[j]}");
                        }
                    }
                    res = sb.ToString();
                    break;
                }
                ++i;
            }
            return res;
        }

        static int Calc(List<int> nums, int i) {
            var res = nums[0];
            for (var j = 1; j < nums.Count; ++j) {
                if (!Numeric.IsKthBit(i, j)) {
                    res += nums[j];
                } else {
                    res += (-1) * nums[j];
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
