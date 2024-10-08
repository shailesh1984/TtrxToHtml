namespace TtrxToHtml.Console.Service;

public static class GenerateTrxReportService
{
    /// <summary>
    /// Generate trx report in html
    /// </summary>
    /// <param name="trxDirPath"></param>
    public static async Task GenerateTrxReport(string trxDirPath)
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfigurationRoot configuration = builder.Build();

        var appSettings = new AppSettings();
        configuration.GetSection("AppSettings").Bind(appSettings);

        string fileExt = Path.GetExtension(trxDirPath);
        if (fileExt == appSettings.TrxFileExt)
        {
            System.Console.WriteLine("There are no trx files in the provided.");
            return;
        }

        System.Console.WriteLine("Processing the trx file into html format.");
        string json = TrxHelper.CombineAllTrxFilesToOneTrx(trxDirPath);

        var testResult = JsonConvert.DeserializeObject<JsonData>(json)!;

        var engine = new RazorLightEngineBuilder()
            .UseEmbeddedResourcesProject(typeof(Program).Assembly, "TtrxToHtml.Console.Templates")
            .UseMemoryCachingProvider()
            .Build();

        string html = await engine.CompileRenderAsync(appSettings.CshtmlFileName, testResult);

        var dateTime = DateTime.Now.ToString(appSettings.DateTimeFormat);

        string directoryPath = Path.Combine(trxDirPath, appSettings.HtmlReportDirectoryFolder!);

        bool exists = Directory.Exists(directoryPath);

        if (!exists)
            Directory.CreateDirectory(directoryPath);

        var testReportFile = Path.Combine(directoryPath, appSettings.TestReportFileName + dateTime + appSettings.OutputFileExt);

        var sourceContentsDir = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "Contents"));
        TrxHelper.DeepCopy(sourceContentsDir, directoryPath);

        await File.WriteAllTextAsync(testReportFile, html);
        Process.Start(@"cmd.exe ", $@"/c {testReportFile}");
    }
}
