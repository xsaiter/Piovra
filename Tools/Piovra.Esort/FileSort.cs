using Piovra.Ds;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Piovra.Esort {
    public class FileSort {
        public class Cfg {
            public int MemorySize { get; set; }
            public string SrcFile { get; set; }
            public string DestFile { get; set; }
            public string OutDir { get; set; } = "Tmp";
        }

        const int NUM_SIZE = sizeof(int);

        public static async Task Sort(Cfg cfg) {
            var files = await SplitFile(cfg.SrcFile, cfg.MemorySize, cfg.OutDir);
            using (Batch<Stream>.New(files)) {
                await Merge(files, cfg.DestFile);
            }
        }

        static async Task<List<Stream>> SplitFile(string srcFile, int memorySize, string outDir) {
            EnsureDir(outDir);
            var res = new List<Stream>();
            using (var src = new FileStream(srcFile, FileMode.Open, FileAccess.Read)) {
                var bufferSize = memorySize - memorySize % NUM_SIZE;
                var buffer = new byte[bufferSize];
                var n = 0;
                var i = 0;
                while ((n = await src.ReadAsync(buffer, 0, buffer.Length)) > 0) {
                    var nums = buffer.AsNums();
                    Array.Sort(nums);
                    var destName = $"{outDir}/{i++}_{srcFile}";
                    var dest = new FileStream(destName, FileMode.CreateNew, FileAccess.ReadWrite);
                    await dest.WriteAsync(nums.AsBytes(), 0, n);
                    if (dest.CanSeek) {
                        dest.Seek(0, SeekOrigin.Begin);
                    }
                    res.Add(dest);
                }
            }
            return res;
        }

        static void EnsureDir(string dir) {
            if (Directory.Exists(dir)) {
                Directory.Delete(dir);
            }
            Directory.CreateDirectory(dir);
        }

        static async Task Merge(List<Stream> streams, string destFile) {
            using (var writer = new BinaryWriter(new FileStream(destFile, FileMode.CreateNew, FileAccess.Write))) {
                var pq = PriorityQueue<FileItem>.Min();
                foreach (var stream in streams) {
                    var x = new FileItem(stream);
                    await x.Read();
                    if (x.HasNum) {
                        pq.Enqueue(x);
                    }
                }
                while (!pq.IsEmpty()) {
                    var x = pq.Peek();
                    if (x != null) {
                        writer.Write(x.Num);
                        pq.Dequeue();
                        await x.Read();
                        if (x.HasNum) {
                            pq.Enqueue(x);
                        }
                    }
                }
            }
        }

        public static async Task PrintFile(string name) {
            var n = 0;
            var i = 0;
            var buffer = new byte[NUM_SIZE];
            using (var stream = new FileStream(name, FileMode.Open, FileAccess.Read)) {
                while ((n = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0) {
                    Console.WriteLine($"{i++}. {buffer.AsNum()}");
                }
            }
        }

        public static async Task PrintAllFiles(string dir) {
            var files = Directory.GetFiles(dir);
            foreach (var file in files) {
                await PrintFile(file);
            }
        }

        public static void GenerateSourceFile(GenerateCfg cfg) {
            var random = new Random(Guid.NewGuid().GetHashCode());
            using (var writer = new BinaryWriter(new FileStream(cfg.Name, FileMode.Create))) {
                var nNums = cfg.Size / NUM_SIZE;
                var nums = Enumerable.Range(0, nNums).Select(x => random.Next(cfg.Min, cfg.Max)).ToArray();
                writer.Write(nums.AsBytes());
            }
        }

        public class GenerateCfg {
            public string Name { get; set; }
            public int Size { get; set; }
            public int Min { get; set; } = 0;
            public int Max { get; set; } = 1 << 10;
        }

        class FileItem : IComparable<FileItem> {
            public FileItem(Stream stream) => Stream = stream;

            Stream Stream { get; }
            public int Num { get; private set; }
            public bool HasNum { get; private set; }

            readonly byte[] buffer = new byte[NUM_SIZE];

            public async Task Read() {
                var n = await Stream.ReadAsync(buffer, 0, buffer.Length);
                HasNum = n == buffer.Length;
                if (HasNum) {
                    Num = buffer.AsNum();
                }
            }

            public int CompareTo(FileItem other) {
                return Num.CompareTo(other.Num);
            }
        }
    }
}