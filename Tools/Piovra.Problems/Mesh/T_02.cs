using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Piovra.Problems.Mesh {
    public class T_02_A_Primes {

    }

    public class T_02_B_Permut {
        static void Permut(string s, int pos, List<string> res) {
            var n = s.Length;
            var m = n - pos;
            if (m == 2) {
                res.Add(s);
                res.Add(s.Substring(0, pos) + s.Substring(pos, m).ReverseCopy());
            } else {
                Permut(s, pos + 1, res);
            }
        }

        public static string Shift(string s) {
            return s.Last() + s.Substring(0, s.Length - 1);
        }

        public static List<string> Execute(string s) {
            var res = new List<string>();
            var n = s.Length;
            var i = 0;
            var ns = s;
            while(i < n) {
                Permut(ns, i, res);
                ns = Shift(s);
                ++i;
            }
            return res;
        }
    }
}
