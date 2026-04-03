namespace Piovra.Esort;

class Program {
    static async Task Main(string[] args) {
        try {
            if (args.Length == 0) {
                throw new Exception("Invalid number of arguments");
            }
            if (args[0] == "-g") {
                FileSort.GenerateSourceFile(ParseGenerateCfg(args));
            } else {
                await FileSort.SortAsync(ParseSortCfg(args));
            }
            Console.WriteLine("OK");
        } catch (Exception ex) {
            Console.WriteLine($"ERROR: {ex}");
        }

        Console.ReadKey();
    }

    static FileSort.GenerateCfg ParseGenerateCfg(string[] args) {
        string? name = null;
        int? size = null;
        int? min = null;
        int? max = null;

        var i = 1;
        while (i < args.Length) {
            var arg = args[i];
            switch (arg) {
                case "-name":
                    name = args[++i];
                    break;
                case "-size":
                    size = int.Parse(args[++i]);
                    break;
                case "-min":
                    min = int.Parse(args[++i]);
                    break;
                case "-max":
                    max = int.Parse(args[++i]);
                    break;
                default:
                    throw new Exception($"Unexpected {nameof(arg)}: {arg}");
            }
            ++i;
        }

        Requires.NotNull(name);
        Requires.NotNull(size);

        min ??= FileSort.GenerateCfg.DEFAULT_MIN;
        max ??= FileSort.GenerateCfg.DEFAULT_MAX;

        return new FileSort.GenerateCfg(name, size.Value, min.Value, max.Value);
    }

    static FileSort.Cfg ParseSortCfg(string[] args) {
        string? srcFile = null;
        string? destFile = null;
        int? memorySize = 0;

        int i = 0;
        while (i < args.Length) {
            var arg = args[i];
            switch (arg) {
                case "-src":
                    srcFile = args[++i];
                    break;
                case "-dest":
                    destFile = args[++i];
                    break;
                case "-msize":
                    memorySize = int.Parse(args[++i]);
                    break;
                default:
                    throw new Exception($"Unexpected {nameof(arg)}: {arg}");
            }
            ++i;
        }

        Requires.NotNull(srcFile);
        Requires.NotNull(destFile);
        Requires.NotNull(memorySize);

        return new FileSort.Cfg(SrcFile: srcFile, DestFile: destFile, MemorySize: memorySize.Value);
    }
}
