using System.Collections.Generic;

namespace Piovra.Problems.Mesh._01 {
    public class A {
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

    public class B {        
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