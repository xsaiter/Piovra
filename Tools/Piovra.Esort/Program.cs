using Piovra.Ds;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Piovra.Esort {
    public class FileSort {
        readonly int _memorySize;
        readonly string _inputFile;
        readonly string _outputFile;

        public FileSort(int memorySize, string inputFile, string outputFile) {
            _memorySize = memorySize;
            _inputFile = inputFile;
            _outputFile = outputFile;
        }

        public async Task Sort() {            
            var files = await SplitFile();
            using (AutoClean<FileStream>.New(files)) {
                await Merge(files);
            }            
        }

        public async Task Merge(List<FileStream> streams) {
            using (var outWriter = new BinaryWriter(new FileStream(_outputFile, FileMode.CreateNew, FileAccess.Write))) {
                var pq = PriorityQueue<FileItem>.Min();
                foreach (var stream in streams) {
                    var item = new FileItem(stream);
                    await item.Read();
                    if (!item.IsEmpty) {
                        pq.Enqueue(item);
                    }
                }
                while (!pq.IsEmpty()) {
                    var item = pq.Peek();
                    if (item != null) {
                        outWriter.Write(item.Value);
                        await item.Read();
                        pq.Dequeue();
                        if (!item.IsEmpty) {
                            pq.Enqueue(item);
                        }
                    }
                }
            }
        }

        public async Task MergeAll(string dir) {
            var files = Directory.GetFiles(dir);
            var streams = files.Select(x => new FileStream(x, FileMode.Open, FileAccess.Read)).ToList();
            await Merge(streams);
        }

        async Task<List<FileStream>> SplitFile() {
            EnsureTmpDir();
            var res = new List<FileStream>();
            using (var inStream = new FileStream(_inputFile, FileMode.Open, FileAccess.Read)) {
                var bufferSize = GetBufferSize();
                var buffer = new byte[bufferSize];
                var n = 0;
                var i = 0;
                while ((n = await inStream.ReadAsync(buffer, 0, buffer.Length)) > 0) {
                    var nums = buffer.AsNums();
                    Array.Sort(nums);                    
                    var outStream = new FileStream(Path.Combine(TmpDir, $"{i}_{_inputFile}"), FileMode.CreateNew, FileAccess.ReadWrite);
                    await outStream.WriteAsync(nums.AsBytes(), 0, n);
                    if(outStream.CanSeek) {
                        outStream.Seek(0, SeekOrigin.Begin);
                    }
                    res.Add(outStream);
                    ++i;
                }
            }
            return res;
        }

        int GetBufferSize() => _memorySize - _memorySize % sizeof(int);

        static void EnsureTmpDir() {
            var path = TmpDir;
            if (Directory.Exists(path)) {
                Directory.Delete(path);
            }
            Directory.CreateDirectory(path);
        }

        static string CurrentDir => AppDomain.CurrentDomain.BaseDirectory;
        public static string TmpDir => Path.Combine(CurrentDir, "Tmp");

        public static void GenerateInputFile(string name, int size) {
            using (var bw = new BinaryWriter(new FileStream(name, FileMode.Create))) {
                var nInts = size / sizeof(int);
                var intArray = Enumerable.Range(0, nInts).Select(x => GenerateNumber()).ToArray();
                var byteArray = intArray.AsBytes();
                bw.Write(byteArray);
            }
        }

        static int GenerateNumber() => _random.Next(0, 1 << 10);
        static readonly Random _random = new Random(Guid.NewGuid().GetHashCode());

        public static async Task PrintFile(string name) {
            var buffer = new byte[sizeof(int)];
            var n = 0;
            var i = 0;
            using (var stream = new FileStream(name, FileMode.Open, FileAccess.Read)) {
                while ((n = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0) {
                    var v = buffer.AsNum();
                    Console.WriteLine($"{i}. {v}");
                    ++i;
                }
            }
        }

        public static async Task PrintDir(string dir) {
            var files = Directory.GetFiles(dir);
            foreach (var file in files) {
                await PrintFile(file);
            }
        }

        public class FileItem : IComparable<FileItem> {
            readonly FileStream _stream;

            public FileItem(FileStream stream) {
                _stream = stream;
            }

            public int Value { get; private set; }
            public bool IsEmpty { get; private set; }            

            public async Task Read() {                
                var arr = new byte[sizeof(int)];
                var n = await _stream.ReadAsync(arr, 0, arr.Length);
                IsEmpty = n < arr.Length;
                Value = arr.AsNum();
            }

            public int CompareTo(FileItem other) {
                return Value.CompareTo(other.Value);
            }
        }
    }

    public static class Utils {
        public static byte[] AsBytes(this IEnumerable<int> nums) {
            return nums.SelectMany(x => BitConverter.GetBytes(x)).ToArray();
        }

        public static byte[] AsBytes(this int num) {
            return BitConverter.GetBytes(num);
        }

        public static int AsNum(this byte[] bytes) {
            return BitConverter.ToInt32(bytes);
        }

        public static int[] AsNums(this byte[] bytes) {
            var size = bytes.Length / sizeof(int);
            var res = new int[size];
            for (var i = 0; i < size; ++i) {
                res[i] = BitConverter.ToInt32(bytes, i * sizeof(int));
            }
            return res;
        }
    }

    class Program {
        static async Task Main(string[] args) {           
            //FileSort.GenerateInputFile("input.dat", sizeof(int) * 23);

            await FileSort.PrintFile("input.dat");

            Console.WriteLine("------");
            ///
            //await FileSort.PrintDir(FileSort.OutDir);

            var sort = new FileSort(8, "input.dat", "output.dat");
            await sort.Sort();
            //await sort.MergeAll(FileSort.OutDir);                        

            await FileSort.PrintFile("out.dat");

            Console.WriteLine("press enter");
            Console.ReadKey();
        }
    }
}
