//[assembly: AssemblyVersionAttribute("1.0.0")]

/// <summary>
/// Converting trx file to html format
/// </summary>
public class Program
{
    private static async Task Main(string[] args)
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfigurationRoot configuration = builder.Build();

        var appSettings = new AppSettings();
        configuration.GetSection("AppSettings").Bind(appSettings);

        var trxDirPath = CommandLineInterfaceHelper.ArgumentsHelper(appSettings, args);

        if (string.IsNullOrEmpty(trxDirPath))
        {
            return;
        }

        await GenerateTrxReportService.GenerateTrxReport(appSettings, trxDirPath);
    }
}