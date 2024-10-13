namespace TtrxToHtml.Console.Service;

public class GenerateTrxReportService : IGenerateTrxReportService
{
    private static readonly string CONTENTS_FOLDER = "Contents";

    /// <summary>
    /// Convert trx to html
    /// </summary>
    /// <param name="appSettings"></param>
    /// <param name="commandLineInterfaceValues"></param>
    /// <returns></returns>
    public async Task<TrxReport> ConvertTrxToHtmlAsync(AppSettings appSettings, CommandLineInterfaceValues commandLineInterfaceValues)
    {
        var trxDirPath = commandLineInterfaceValues.TrxPath!;

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

        var trxReport = new TrxReport
        {
            Html = html,
            Path = trxDirPath
        };
        
        return trxReport;
    }

    /// <summary>
    /// Generate trx report in html
    /// </summary>
    /// <param name="trxReport"></param>
    /// <param name="appSettings"></param>
    /// <param name="commandLineInterfaceValues"></param>
    /// <returns></returns>
    public async Task<string> CreateHtmlAsync(TrxReport trxReport, AppSettings appSettings, CommandLineInterfaceValues commandLineInterfaceValues)
    {
        var dateTime = DateTime.Now.ToString(appSettings.DateTimeFormat);
        var outputPath = string.IsNullOrEmpty(commandLineInterfaceValues.OutPutFilePath) ? trxReport.Path! : commandLineInterfaceValues.OutPutFilePath;
        string directoryPath = Path.Combine(outputPath, appSettings.HtmlReportDirectoryFolder!);
        
        DirectoryHelper.CreateDirectory(directoryPath);
        var testReportFileName = string.IsNullOrEmpty(commandLineInterfaceValues.HtmlFileName) ? appSettings.TestReportFileName : string.Concat(commandLineInterfaceValues.HtmlFileName, "_");
        var testReportFile = Path.Combine(directoryPath, testReportFileName + dateTime + appSettings.OutputFileExt);
        var sourceContentsDir = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, CONTENTS_FOLDER));
        TrxHelper.DeepCopy(sourceContentsDir, directoryPath, CONTENTS_FOLDER);

        await File.WriteAllTextAsync(testReportFile, trxReport.Html);
        //Process.Start(@"cmd.exe ", $@"/c {testReportFile}");

        return testReportFile;
    }
}
