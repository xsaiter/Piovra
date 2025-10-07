using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Piovra.Ds;

namespace Piovra.Esort;

public static class FileSort {
    public class Cfg {
        public int MemorySize { get; set; }
        public string SrcFile { get; set; }
        public string DestFile { get; set; }
        public string OutDir { get; set; } = DEFAULT_OUT_DIR;
        public const string DEFAULT_OUT_DIR = "Tmp";
    }

    const int NUM_SIZE = sizeof(int);

    public static async Task SortAsync(Cfg cfg, CancellationToken cancellationToken = default) {
        var files = await SplitFileAsync(cfg.SrcFile, cfg.MemorySize, cfg.OutDir, cancellationToken);
        using (Batch<Stream>.New(files)) {
            await MergeAsync(files, cfg.DestFile, cancellationToken);
        }
    }

    static async Task<List<Stream>> SplitFileAsync(string srcFile, int memorySize, string outDir, CancellationToken cancellationToken = default) {
        EnsureDir(outDir);
        var result = new List<Stream>();
        using var src = new FileStream(srcFile, FileMode.Open, FileAccess.Read);
        var bufferSize = memorySize - memorySize % NUM_SIZE;
        var buffer = new byte[bufferSize];
        var n = 0;
        var i = 0;
        while ((n = await src.ReadAsync(buffer, cancellationToken)) > 0) {
            var nums = buffer.AsNums();
            Array.Sort(nums);
            var destName = $"{outDir}/{i++}_{srcFile}";
            var dest = new FileStream(destName, FileMode.CreateNew, FileAccess.ReadWrite);
            await dest.WriteAsync(nums.AsBytes().AsMemory(0, n), cancellationToken);
            if (dest.CanSeek) {
                dest.Seek(0, SeekOrigin.Begin);
            }
            result.Add(dest);
        }
        return result;
    }

    static void EnsureDir(string dir) {
        if (Directory.Exists(dir)) {
            Directory.Delete(dir);
        }
        Directory.CreateDirectory(dir);
    }

    static async Task MergeAsync(IEnumerable<Stream> streams, string destFile, CancellationToken cancellationToken = default) {
        var pq = PriorityQueue<Feed>.Min();
        foreach (var stream in streams) {
            var x = new Feed(stream);
            await x.ReadAsync(cancellationToken);
            if (x.HasNum) {
                pq.Enqueue(x);
            }
        }
        var fs = new FileStream(destFile, FileMode.CreateNew, FileAccess.Write);
        using var writer = new BinaryWriter(fs);
        while (!pq.IsEmpty()) {
            var x = pq.Peek();
            if (x != null) {
                writer.Write(x.Num);
                pq.Dequeue();
                await x.ReadAsync(cancellationToken);
                if (x.HasNum) {
                    pq.Enqueue(x);
                }
            }
        }
    }

    public static async Task PrintAllFilesAsync(string dir, CancellationToken cancellationToken = default) {
        var files = Directory.GetFiles(dir);
        foreach (var file in files) {
            await PrintFileAsync(file, cancellationToken);
        }
    }

    public static async Task PrintFileAsync(string name, CancellationToken cancellationToken = default) {
        var i = 0;
        var buffer = new byte[NUM_SIZE];
        using var stream = new FileStream(name, FileMode.Open, FileAccess.Read);
        while ((await stream.ReadAsync(buffer, cancellationToken)) > 0) {
            Console.WriteLine($"{i++}. {buffer.AsNum()}");
        }
    }

    public static void GenerateSourceFile(GenerateCfg cfg) {
        var random = new Random(Guid.NewGuid().GetHashCode());
        using var writer = new BinaryWriter(new FileStream(cfg.Name, FileMode.Create));
        var nNums = cfg.Size / NUM_SIZE;
        var nums = Enumerable.Range(0, nNums).Select(x => random.Next(cfg.Min, cfg.Max)).ToArray();
        writer.Write(nums.AsBytes());
    }

    public class GenerateCfg {
        public string Name { get; set; }
        public int Size { get; set; }
        public int Min { get; set; } = 0;
        public int Max { get; set; } = DEFAULT_MAX;
        public const int DEFAULT_MAX = 1 << 10;
    }

    class Feed(Stream stream) : IComparable<Feed> {
        Stream Stream { get; } = stream;
        public int Num { get; private set; }
        public bool HasNum { get; private set; }

        readonly byte[] buffer = new byte[NUM_SIZE];

        public async Task ReadAsync(CancellationToken cancellationToken = default) {
            var n = await Stream.ReadAsync(buffer, cancellationToken);
            HasNum = n == buffer.Length;
            if (HasNum) {
                Num = buffer.AsNum();
            }
        }

        public int CompareTo(Feed other) => Num.CompareTo(other.Num);
    }
}
