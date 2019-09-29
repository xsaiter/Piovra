using System.Collections.Generic;
using System.Text;

namespace Piovra.Problems.Mesh._01 {
    public class A_Primes {
        public List<long> Run(Range<long> range) {
            var res = new List<long>();
            for (var i = range.L; i < range.R; ++i) {
                if (Arithmetic.IsPrime(i)) {
                    res.Add(i);
                }
            }
            return res;
        }
    }

    public class B_Expr {
        public string Run(List<long> nums, long s) {
            var ops = new List<bool>();
            int n = nums.Count;
            long x = 0;
            while (true) {
                var r = Calc(nums, x);
                if (r == s) {

                    break;
                }
            }
            if (ops.Empty()) {
                return "no solution";
            }
            var sb = new StringBuilder();

            return sb.ToString();
        }

        long Calc(List<long> nums, long x) {
            var n = nums.Count;
            var res = nums[0];
            for (var i = 1; i < n; ++i) {
                if ((x & (1 << i)) == 0) {
                    res += nums[i];
                } else {
                    res -= nums[i];
                }
            }
            return res;
        }
    }

    public class C {

    }

    public class D {

    }

    public class E {

    }

    public class F {

    }
}