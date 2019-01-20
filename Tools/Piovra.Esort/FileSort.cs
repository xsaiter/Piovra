using Piovra.Ds;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Piovra.Esort {
    public class FileSort {
        readonly Cfg _cfg;

        public FileSort(Cfg cfg) {
            _cfg = cfg;
        }

        public class Cfg {
            public int MemorySize { get; set; }
            public string SrcFile { get; set; }
            public string DestFile { get; set; }
        }

        const int NUM_SIZE = sizeof(int);        
        static string TmpDir => "Tmp";

        public async Task Sort() {
            var files = await SplitFile();
            using (Batch<Stream>.New(files)) {
                await Merge(files);
            }
        }

        async Task<List<Stream>> SplitFile() {
            EnsureTmpDir();
            var res = new List<Stream>();
            using (var src = new FileStream(_cfg.SrcFile, FileMode.Open, FileAccess.Read)) {
                var bufferSize = _cfg.MemorySize - _cfg.MemorySize % NUM_SIZE;
                var buffer = new byte[bufferSize];
                var n = 0;
                var i = 0;
                while ((n = await src.ReadAsync(buffer, 0, buffer.Length)) > 0) {
                    var nums = buffer.AsNums();
                    Array.Sort(nums);
                    var destName = $"{TmpDir}/{i++}_{_cfg.SrcFile}";
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

        static void EnsureTmpDir() {
            var path = TmpDir;
            if (Directory.Exists(path)) {
                Directory.Delete(path);
            }
            Directory.CreateDirectory(path);
        }

        async Task Merge(List<Stream> streams) {
            using (var writer = new BinaryWriter(new FileStream(_cfg.DestFile, FileMode.CreateNew, FileAccess.Write))) {
                var pq = PriorityQueue<FileItem>.Min();
                foreach (var stream in streams) {
                    var item = new FileItem(stream);
                    await item.Read();
                    if (item.HasNum) {
                        pq.Enqueue(item);
                    }
                }
                while (!pq.IsEmpty()) {
                    var item = pq.Peek();
                    if (item != null) {
                        writer.Write(item.Num);
                        pq.Dequeue();
                        await item.Read();
                        if (item.HasNum) {
                            pq.Enqueue(item);
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
                    var num = buffer.AsNum();
                    Console.WriteLine($"{i++}. {num}");
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

            public async Task Read() {
                var buffer = new byte[NUM_SIZE];
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