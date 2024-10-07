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

        if (args == null || args.Length != 0)
        {
            Console.WriteLine("No trx file");
            return;
        }

        string fileExt = Path.GetExtension(args[0]);
        if (fileExt == appSettings.TrxFileExt)
        {
            Console.WriteLine("You have passed wrong file format!");
            return;
        }

        Console.WriteLine("Trx File\n{0}", args[0]);

        //var sourceDirectory = @"C:\Personal\Work\TtrxToHtml\src\TtrxToHtml.Console";
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