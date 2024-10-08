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

        if (args.Length <= 0)
        {
            Console.WriteLine("No arguments provided.");
            return;
        }

        Console.WriteLine("Converting trx file to html format is in process...");

        string fileExt = Path.GetExtension(args[0]);
        if (fileExt == appSettings.TrxFileExt)
        {
            Console.WriteLine("There is no trx files in this location.");
            return;
        }

        string json = TrxHelper.CombineAllTrxFilesToOneTrx(args[0]);

        var testResult = JsonConvert.DeserializeObject<JsonData>(json)!;

        var engine = new RazorLightEngineBuilder()
            .UseEmbeddedResourcesProject(typeof(Program).Assembly, "TtrxToHtml.Console.Templates")
            .UseMemoryCachingProvider()
            .Build();

        string html = await engine.CompileRenderAsync(appSettings.CshtmlFileName, testResult);

        var path = AppDomain.CurrentDomain.BaseDirectory;
        var dateTime = DateTime.Now.ToString(appSettings.DateTimeFormat);

        string directoryPath = Path.Combine(path, appSettings.HtmlReportDirectoryFolder);

        bool exists = Directory.Exists(directoryPath);

        if (!exists)
            Directory.CreateDirectory(directoryPath);

        var testReportFile = Path.Combine(directoryPath, appSettings.TestReportFileName + dateTime + appSettings.OutputFileExt);
        await File.WriteAllTextAsync(testReportFile, html);
        Process.Start(@"cmd.exe ", $@"/c {testReportFile}");
    }
}