using System;
using Piovra.Ds;
using Piovra.Lia;

namespace Piovra.Demo {   
    class Program {       
        static void Main(string[] args) {            
            var x = new double[2, 3] { { 2, 2, 2 }, { 2, 2, 2 } };

            var mx = new Matrix(x);

            mx.Transform(b => 10);

            var my = mx * 10;

            x[1, 1] = 10;
            x[1, 2] = 20;            

            var l1 = x.GetLength(0);
            var l2 = x.GetLength(1);

            var heap = new BinaryHeap<int>(1, false);
            heap.Add(10);
            heap.Add(5);
            heap.Add(15);
            heap.Add(25);
            heap.Add(45);
            heap.Add(1);
            heap.Add(3);

            while (!heap.IsEmpty()) {
                var t = heap.Top();
                heap.Pop();
                Console.WriteLine(t);
            }

            Console.ReadKey();
        }
    }
}