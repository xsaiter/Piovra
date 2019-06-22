using System.Collections.Generic;

namespace Piovra.Problems.Mesh {
    public class _01_A {
        public List<long> GetPrimes(Range<long> range) {
            var res = new List<long>();
            for (var i = range.L; i < range.R; ++i) {
                if (Arithmetic.IsPrime(i)) {
                    res.Add(i);
                }
            }
            return res;
        }
    }

    public class _01_B {        
    }

    public class _01_C {

    }

    public class _01_D {

    }

    public class _01_E {

    }

    public class _01_F {

    }
}