using System;
using System.Threading.Tasks;

namespace Piovra.Esort;

class Program {
    static async Task Main(string[] args) {
        try {
            if (args.Length == 0) {
                throw new Exception("invalid number of arguments");
            }
            if (args[0] == "-g") {
                FileSort.GenerateSourceFile(ParseGenerateCfg(args));
            } else {
                await FileSort.SortAsync(ParseSortCfg(args));
            }
            Console.WriteLine("OK");
        } catch (Exception e) {
            Console.WriteLine($"ERROR: {e}");
        }

        Console.ReadKey();
    }

    static FileSort.GenerateCfg ParseGenerateCfg(string[] args) {
        var cfg = new FileSort.GenerateCfg();

        var i = 1;
        while (i < args.Length) {
            var s = args[i];
            switch (s) {
                case "-name":
                    cfg.Name = args[++i];
                    break;
                case "-size":
                    cfg.Size = int.Parse(args[++i]);
                    break;
                case "-min":
                    cfg.Min = int.Parse(args[++i]);
                    break;
                case "-max":
                    cfg.Max = int.Parse(args[++i]);
                    break;
                default:
                    throw new Exception($"unexpected arg: {s}");
            }
            ++i;
        }

        return cfg;
    }

    static FileSort.Cfg ParseSortCfg(string[] args) {
        var cfg = new FileSort.Cfg();

        int i = 0;
        while (i < args.Length) {
            var s = args[i];
            switch (s) {
                case "-src":
                    cfg.SrcFile = args[++i];
                    break;
                case "-dest":
                    cfg.DestFile = args[++i];
                    break;
                case "-msize":
                    cfg.MemorySize = int.Parse(args[++i]);
                    break;
                default:
                    throw new Exception($"unexpected arg: {s}");
            }
            ++i;
        }

        return cfg;
    }
}