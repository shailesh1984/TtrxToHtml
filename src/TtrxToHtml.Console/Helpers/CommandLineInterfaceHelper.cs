using System.IO;

namespace TtrxToHtml.Console.Helpers;

public static class CommandLineInterfaceHelper
{
    private static string TRX_PATH = string.Empty;

    public static string ArgumentsHelper(string[] args)
    {
        var arguments = CommandLineArgumentsHelper.ParseArguments(args);

        if (arguments.Count == 0 || arguments.ContainsKey("-h") || arguments.ContainsKey("--help"))
        {
            CommandLineArgumentsHelper.PrintHelp();
            return string.Empty;
        }

        if (arguments.ContainsKey("--info"))
        {
            CommandLineArgumentsHelper.PrintInfo();
        }

        if (arguments.ContainsKey("-d") || arguments.ContainsKey("--tdp"))
        {
            if (arguments.TryGetValue("-d", out string? tdirValue))
            {
                TRX_PATH = tdirValue;
            }

            if (arguments.TryGetValue("--tdp", out string? tdpValue))
            {
                TRX_PATH = tdpValue;
            }

            if (string.IsNullOrEmpty(TRX_PATH))
            {
                System.Console.WriteLine($"You need to specify the trx source directory path");
                return string.Empty;
            }
            
            Tuple<bool, bool> isPathFileOrDirectory = TrxHelper.IsPathFileOrDirectory(TRX_PATH);

            bool isFile = isPathFileOrDirectory.Item1;
            bool isDirectory = isPathFileOrDirectory.Item2;

            if (!isDirectory && isFile)
            {
                System.Console.WriteLine($"You need to specify the trx source directory path, not file path");
                return string.Empty;
            }

            System.Console.WriteLine($"Trx source directory path: {TRX_PATH}");
        }

        if (arguments.ContainsKey("-f") || arguments.ContainsKey("--tfp"))
        {
            if (arguments.TryGetValue("-f", out string? tfileValue))
            {
                TRX_PATH = tfileValue;
            }

            if (arguments.TryGetValue("--tfp", out string? tfpValue))
            {
                TRX_PATH = tfpValue;
            }

            if (string.IsNullOrEmpty(TRX_PATH))
            {
                System.Console.WriteLine($"You need to specify the trx source file path");
                return string.Empty;
            }

            Tuple<bool, bool> isPathFileOrDirectory = TrxHelper.IsPathFileOrDirectory(TRX_PATH);

            bool isFile = isPathFileOrDirectory.Item1;
            bool isDirectory = isPathFileOrDirectory.Item2;

            if (isDirectory && !isFile)
            {
                System.Console.WriteLine($"You need to specify the trx source file path, not directory path");
                return string.Empty;
            }

            System.Console.WriteLine($"Trx source file path: {TRX_PATH}");
        }

        //if (arguments.TryGetValue("--output", out string? outputValue))
        //{
        //    System.Console.WriteLine($"Output file: {outputValue}");
        //}

        return TRX_PATH;
    }
}
