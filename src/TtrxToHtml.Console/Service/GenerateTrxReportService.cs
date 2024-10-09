namespace TtrxToHtml.Console.Service;

public static class GenerateTrxReportService
{
    private static string CONTENTS_FOLDER = "Contents";

    /// <summary>
    /// Generate trx report in html
    /// </summary>
    /// <param name="trxDirPath"></param>
    public static async Task GenerateTrxReport(AppSettings appSettings, string trxDirPath)
    {
        string fileExt = Path.GetExtension(trxDirPath);

        var trxFiles = new List<string>();
        if (string.IsNullOrEmpty(fileExt))
        {
            trxFiles = PathHelper.GetAllFilesBySpecifiedDirPath(trxDirPath);
        }
        else
        {
            trxFiles.Add(trxDirPath);
            trxDirPath = Path.GetDirectoryName(trxDirPath)!;
        }

        System.Console.WriteLine("Processing the trx file into html format.");
        string json = TrxHelper.CombineAllTrxFilesToOneTrx(trxFiles);

        var testResult = JsonConvert.DeserializeObject<JsonData>(json)!;

        var engine = new RazorLightEngineBuilder()
            .UseEmbeddedResourcesProject(typeof(Program).Assembly, "TtrxToHtml.Console.Templates")
            .UseMemoryCachingProvider()
            .Build();

        string html = await engine.CompileRenderAsync(appSettings.CshtmlFileName, testResult);

        var dateTime = DateTime.Now.ToString(appSettings.DateTimeFormat);

        string directoryPath = Path.Combine(trxDirPath, appSettings.HtmlReportDirectoryFolder!);
        
        DirectoryHelper.CreateDirectory(directoryPath);

        var testReportFile = Path.Combine(directoryPath, appSettings.TestReportFileName + dateTime + appSettings.OutputFileExt);

        var sourceContentsDir = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, CONTENTS_FOLDER));
        TrxHelper.DeepCopy(sourceContentsDir, directoryPath, CONTENTS_FOLDER);

        await File.WriteAllTextAsync(testReportFile, html);
        Process.Start(@"cmd.exe ", $@"/c {testReportFile}");
    }
}
