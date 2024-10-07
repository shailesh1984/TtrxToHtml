public class Program
{
    private const string TEMPLATE_FOLDER = "Templates";

    private const string OUTPUT_FILE_EXT = ".html";
    private static async Task Main(string[] args)
    {
        var sourceDirectory = @"C:\Personal\Work\TtrxToHtml\src\TtrxToHtml.Console"; // need to modify
        string json = TrxHelper.CombineAllTrxFilesToOneTrx(sourceDirectory);

        var testResult = JsonConvert.DeserializeObject<JsonData>(json)!;

        var engine = new RazorLightEngineBuilder()
            .UseEmbeddedResourcesProject(typeof(Program).Assembly, "TtrxToHtml.Console.Templates")
            .UseMemoryCachingProvider()
            .Build();

        string html = await engine.CompileRenderAsync("Index", testResult);

        var path = AppDomain.CurrentDomain.BaseDirectory;
        var dateTime = DateTime.Now.ToString("yyyy-dd-M-HH-mm-ss");

        string directoryPath = Path.Combine(path, "Test_Reports");

        bool exists = Directory.Exists(directoryPath);

        if (!exists)
            Directory.CreateDirectory(directoryPath);

        var testReportFile = Path.Combine(directoryPath, @"TestReportFile_" + dateTime + OUTPUT_FILE_EXT);
        await File.WriteAllTextAsync(testReportFile, html);
        Process.Start(@"cmd.exe ", $@"/c {testReportFile}");
    }
}