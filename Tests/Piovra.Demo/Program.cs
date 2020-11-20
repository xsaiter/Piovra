using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Piovra.Lia;

namespace Piovra.Demo {
    class Program {
        static bool solve(List<string> v, int n) {
            List<char> a = new List<char>();
            for (int i = 0; i < n; ++i) {
                for (int j = 0; j < (int)v[i].Length; ++j) {
                    a.Add(v[i][j]);
                }
            }
            int len = (int)a.Count;
            if (len % n != 0) return false;

            a.Sort();
            
            var c = a[0];
            int x = 1;
            for (int i = 1; i < len; ++i) {
                if (c == a[i]) {
                    ++x;
                } else {
                    if (x % n != 0) return false;
                    x = 1;
                    c = a[i];
                }
            }
            if (x > 0) {
                if (x % n != 0) return false;
            }
            return true;
        }


        static void Main(string[] args) {
            var v = new List<string> { "caa", "cbb" };
            var res = solve(v, v.Count);
            Console.ReadKey();
        }
    }
}