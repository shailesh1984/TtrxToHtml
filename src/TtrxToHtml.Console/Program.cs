public class Program
{
    private const string TemplatesFolder = "Templates";

    private static async Task Main(string[] args)
    {
        var sourceDirectory = @"C:\Personal\Work\TtrxToHtml\src\TtrxToHtml.Console"; // need to modify
        string json = TrxHelper.CombineAllTrxFilesToOneTrx(sourceDirectory);

        var res = JsonConvert.DeserializeObject<JsonData>(json)!;
        
        var razorTemplateEngine = new TemplateEngine();
        var htmlContent = await razorTemplateEngine.RenderTemplateAsync(res);

        var tempFile = Path.Combine(Path.GetTempPath(), "temp.html");
        await File.WriteAllTextAsync(tempFile, htmlContent);
        Process.Start(@"cmd.exe ", $@"/c {tempFile}");
    }
}

// To convert JSON text contained in string json into an XML node
//XmlDocument doc = JsonConvert.DeserializeXmlNode(json);


