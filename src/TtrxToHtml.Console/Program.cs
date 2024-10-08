//[assembly: AssemblyVersionAttribute("1.0.0")]

/// <summary>
/// Converting trx file to html format
/// </summary>
public class Program
{
    private static async Task Main(string[] args)
    {
        var trxDirPath = CommandLineInterfaceHelper.ArgumentsHelper(args);

        if (string.IsNullOrEmpty(trxDirPath))
        {
            return;
        }

        await GenerateTrxReportService.GenerateTrxReport(trxDirPath);
    }
}