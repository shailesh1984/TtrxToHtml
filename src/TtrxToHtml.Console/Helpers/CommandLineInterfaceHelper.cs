namespace TtrxToHtml.Console.Helpers;

public static class CommandLineInterfaceHelper
{
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

        var trxDirPath = string.Empty;
        if (arguments.ContainsKey("-tdp") || arguments.ContainsKey("--trx-dir-path"))
        {
            if (arguments.TryGetValue("-tdp", out string? tdpValue))
            {
                trxDirPath = !string.IsNullOrEmpty(tdpValue) ? tdpValue : string.Empty;
            }

            if (arguments.TryGetValue("--trx-dir-path", out string? trxDirPathValue))
            {
                trxDirPath = !string.IsNullOrEmpty(trxDirPathValue) ? trxDirPathValue : string.Empty;
            }

            System.Console.WriteLine($"Trx source directory path: {trxDirPath}");
        }

        //if (arguments.TryGetValue("--output", out string? outputValue))
        //{
        //    System.Console.WriteLine($"Output file: {outputValue}");
        //}

        return trxDirPath;
    }
}
