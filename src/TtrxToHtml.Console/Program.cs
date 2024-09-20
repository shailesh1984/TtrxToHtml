public class Program
{
    private const string TemplatesFolder = "Templates";

    private static async Task Main(string[] args)
    {
        var sourceDirectory = @"C:\Personal\Work\TtrxToHtml\src\TtrxToHtml.Console"; // need to modify
        string json = TrxHelper.CombineAllTrxFilesToOneTrx(sourceDirectory);

        var model = JsonConvert.DeserializeObject<JsonData>(json)!;

        var engine = new RazorLightEngineBuilder()
            .UseEmbeddedResourcesProject(typeof(Program).Assembly, "TtrxToHtml.Console.Templates")
            .UseMemoryCachingProvider()
            .Build();

        string html = await engine.CompileRenderAsync("JsonData", model);

        var path = AppDomain.CurrentDomain.BaseDirectory;
        var testReportFile = Path.Combine(path, "TestReportFile.html");
        await File.WriteAllTextAsync(testReportFile, html);
        Process.Start(@"cmd.exe ", $@"/c {testReportFile}");
    }


    //public static string ReadEmbeddedResource(Assembly assembly, string resourceName)
    //{
    //    using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
    //    using (StreamReader reader = new StreamReader(resourceStream))
    //    {
    //        return reader.ReadToEnd();
    //    }
    //}

    //internal static string GenerateHtml<T>(string resourceName, T model)
    //{
    //    var rootType = typeof(RootTypeForEmbeddedResources);
    //    var rootTypeAssembly = rootType.Assembly;
    //    var rootTypeNamespace = rootType.Namespace;

    //    var layoutResource = ReadEmbeddedResource(rootTypeAssembly, rootTypeNamespace + "._Layout.cshtml");
    //    var engine = new RazorLightEngineBuilder()
    //        .UseEmbeddedResourcesProject(typeof(RootTypeForEmbeddedResources).Assembly, typeof(RootTypeForEmbeddedResources).Namespace)
    //        .AddDynamicTemplates(new Dictionary<string, string>()
    //            {
    //                    {"_Layout.cshtml",layoutResource },
    //            })
    //        .SetOperatingAssembly(operatingAssembly)
    //        .UseMemoryCachingProvider()
    //        .EnableDebugMode()
    //        .Build();

    //    string html = engine.CompileRenderAsync<T>(resourceName, model).Result;
    //    return html;
    //}
}

// To convert JSON text contained in string json into an XML node
//XmlDocument doc = JsonConvert.DeserializeXmlNode(json);


