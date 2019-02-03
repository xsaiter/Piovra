using System;
using System.Collections.Generic;
using System.Text;

namespace Piovra.Problems.Mesh {
    public class T_01_A_Primes {
        public List<int> Execute(int m, int n) {
            var res = new List<int>();
            for(var i = m; i <= n; ++i) {
                if (Numeric.IsPrime(i)) {
                    res.Add(i);
                }
            }
            return res;
        }
    }

    public class T_01_A_Expr {        
    }
}