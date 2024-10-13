namespace TtrxToHtml.Console.Helpers;

public static class CommandLineInterfaceHelper
{
    private static string TRX_PATH = string.Empty;
    
    private static readonly CommandLineInterfaceValues commandLineInterfaceValues = new();

    public static CommandLineInterfaceValues ArgumentsHelper(AppSettings appSettings, string[] args)
    {
        var arguments = CommandLineArgumentsHelper.ParseArguments(args);

        if (arguments.Count == 0 || arguments.ContainsKey("-h") || arguments.ContainsKey("--help"))
        {
            CommandLineArgumentsHelper.PrintHelp();
            return commandLineInterfaceValues;
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
                return commandLineInterfaceValues;
            }
            
            Tuple<bool, bool> isPathFileOrDirectory = TrxHelper.IsPathFileOrDirectory(TRX_PATH);

            if (!isPathFileOrDirectory.Item2 && isPathFileOrDirectory.Item1)
            {
                System.Console.WriteLine($"You need to specify the trx source directory path, not file path");
                return commandLineInterfaceValues;
            }

            var files = PathHelper.GetAllFilesBySpecifiedDirPath(TRX_PATH);
            if (files.Count == 0)
            {
                System.Console.WriteLine($"There is no trx files in the specified directory {TRX_PATH} path.");
                return commandLineInterfaceValues;
            }

            System.Console.WriteLine($"Trx source directory path: {TRX_PATH}");
            commandLineInterfaceValues.TrxPath = TRX_PATH;
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
                return commandLineInterfaceValues;
            }

            Tuple<bool, bool> isPathFileOrDirectory = TrxHelper.IsPathFileOrDirectory(TRX_PATH);

            if (isPathFileOrDirectory.Item2 && !isPathFileOrDirectory.Item1)
            {
                System.Console.WriteLine($"You need to specify the trx source file path, not directory path");
                return commandLineInterfaceValues;
            }

            string fileExt = Path.GetExtension(TRX_PATH);
            if (fileExt != appSettings.TrxFileExt)
            {
                System.Console.WriteLine($"The specified file {Path.GetFileName(TRX_PATH)} could not be be processed.");
                return commandLineInterfaceValues;
            }

            System.Console.WriteLine($"Trx source file path: {TRX_PATH}");
            commandLineInterfaceValues.TrxPath = TRX_PATH;
        }

        if (arguments.ContainsKey("-hfn") || arguments.ContainsKey("--html-file-name"))
        {
            if (arguments.TryGetValue("-hfn", out string? hfileName))
            {
                commandLineInterfaceValues.HtmlFileName = hfileName;
            }

            if (arguments.TryGetValue("--html-file-name", out string? hFileName))
            {
                commandLineInterfaceValues.HtmlFileName = hFileName;
            }
        }

        if (arguments.ContainsKey("-o") || arguments.ContainsKey("--output"))
        {
            if (arguments.TryGetValue("-o", out string? oFilePathValue))
            {
                commandLineInterfaceValues.OutPutFilePath = oFilePathValue;
            }

            if (arguments.TryGetValue("--output", out string? outputFilePathValue))
            {
                commandLineInterfaceValues.OutPutFilePath = outputFilePathValue;
            }
        }

        return commandLineInterfaceValues;
    }
}
