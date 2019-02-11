using Piovra.Problems.Msh;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Piovra.Problems {
    class Program {
        static void Main(string[] args) {
            int b = 0;
            b = 20;
            var s = Convert.ToString(b, 2);
            var y = 1 << 3;
            var ys = Convert.ToString(y, 2);
            var x = (1 << 3) & b;
            var signs = new List<char>(Enumerable.Repeat('+', 5));
            var res = T_01_B.Calc(new List<int> { 10 }, new List<char> { '+', '+', '+' });
            var tmp = res;
        }
    }
}
