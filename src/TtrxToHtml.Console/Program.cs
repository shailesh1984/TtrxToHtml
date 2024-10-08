//[assembly: AssemblyVersionAttribute("1.0.0")]

/// <summary>
/// Converting trx file to html format
/// </summary>
public class Program
{
    private static async Task Main(string[] args)
    {
        var arguments = CommandLineArgumentsHelper.ParseArguments(args);

        if (arguments.Count == 0 || arguments.ContainsKey("-h") || arguments.ContainsKey("--help"))
        {
            CommandLineArgumentsHelper.PrintHelp();
            return;
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

            Console.WriteLine($"Trx source directory path: {trxDirPath}");
        }

        //if (arguments.TryGetValue("--output", out string? outputValue))
        //{
        //    Console.WriteLine($"Output file: {outputValue}");
        //}

        await GenerateTrxReportService.GenerateTrxReport(trxDirPath);
    }
}