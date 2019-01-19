using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Piovra.Esort {
    public class ExternalSort {
        readonly int _memorySize;
        readonly string _fileName;

        public ExternalSort(int memorySize, string fileName) {
            _memorySize = memorySize;
            _fileName = fileName;
        }

        void SplitFile() {
            using (var fs = new FileStream(_fileName, FileMode.Open)) {

            }
        }

        public static void CreateFile(string name, int size) {
            using (var bw = new BinaryWriter(new FileStream(name, FileMode.Create))) {
                var nInts = size / sizeof(int);
                var intArray = Enumerable.Range(0, nInts).Select(x => GenerateNumber()).ToArray();
                var byteArray = intArray.ToByteArray();
                bw.Write(byteArray);
            }
        }

        public static void PrintFile(string name) {

        }

        static int GenerateNumber() => _random.Next(0, 1 << 10);
        static readonly Random _random = new Random(Guid.NewGuid().GetHashCode());
    }

    public static class Utils {
        public static byte[] ToByteArray(this IEnumerable<int> intArray) {
            return intArray.SelectMany(x => BitConverter.GetBytes(x)).ToArray();
        }

        public static int[] ToIntArray(this byte[] byteArray) {
            var size = byteArray.Length / sizeof(int);
            var res = new int[size];
            for (var i = 0; i < size; ++i) {
                res[i] = BitConverter.ToInt32(byteArray, i * sizeof(int));
            }
            return res;
        }
    }

    class Program {
        static void Main(string[] args) {
        }
    }
}
