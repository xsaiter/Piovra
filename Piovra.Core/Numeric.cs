using System;
using System.Collections.Generic;
using System.Text;

namespace Piovra {
    public class Numeric {
        public static bool IsPrime(int n) {
            for (var i = 2; i * i <= n; ++i) {
                if (n % i == 0) {
                    return false;
                }
            }
            return true;
        }        
    }
}
