using System;
using System.Threading.Tasks;

namespace Piovra.Esort {

    class Program {
        static async Task Main(string[] args) {
            try {
                var n = args.Length;

                if (n == 0) {
                    throw new Exception("invalid number of arguments");
                }

                if (args[0] == "-g") {
                    var cfg = new FileSort.GenerateCfg();

                    var i = 1;
                    while (i < n) {
                        var s = args[i];
                        if (s == "-name") {
                            cfg.Name = args[++i];
                        } else if (s == "-size") {
                            cfg.Size = int.Parse(args[++i]);
                        } else if (s == "-min") {
                            cfg.Min = int.Parse(args[++i]);
                        } else if (s == "-max") {
                            cfg.Max = int.Parse(args[++i]);
                        } else {
                            throw new Exception($"unexpected arg: {s}");
                        }
                        ++i;
                    }
                    FileSort.GenerateSourceFile(cfg);
                } else {
                    var cfg = new FileSort.Cfg();

                    int i = 0;
                    while (i < n) {
                        var s = args[i];
                        if (s == "-src") {
                            cfg.SrcFile = args[++i];
                        } else if (s == "-dest") {
                            cfg.DestFile = args[++i];
                        } else if (s == "-msize") {
                            cfg.MemorySize = int.Parse(args[++i]);
                        } else {
                            throw new Exception($"unexpected arg: {s}");
                        }
                        ++i;
                    }                    
                    await FileSort.Sort(cfg);
                }
                Console.WriteLine("OK");
            } catch (Exception e) {
                Console.WriteLine($"ERROR: {e}");
            }
            Console.ReadKey();
        }
    }
}