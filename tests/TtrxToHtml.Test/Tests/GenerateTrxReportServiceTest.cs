namespace TtrxToHtml.Test.Tests;

public class GenerateTrxReportServiceTest(TestFixture fixture) : IClassFixture<TestFixture>
{
    private readonly AppSettings _appConfig = fixture.AppConfig;
    private readonly TestFixture _fixture = fixture;

    [Fact]
    public async Task Should_CreateHtml_File_Test()
    {
        // ARRANGE
        var args = new[]
        {
            "-f",
            Utility.TestDataFilePath()
        };

        CommandLineInterfaceValues commandLineInterfaceValues = CommandLineInterfaceHelper.ArgumentsHelper(_appConfig, args);

        RazorLightEngine razorEngine = new RazorLightEngineBuilder()
            .UseEmbeddedResourcesProject(typeof(TtrxToHtml.Console.Program).Assembly, "TtrxToHtml.Console.Templates")
            .UseMemoryCachingProvider()
            .Build();

        ILogger<GenerateTrxReportService> logger = new LoggerFactory().CreateLogger<GenerateTrxReportService>();

        GenerateTrxReportService generateTrxReportService = new GenerateTrxReportService(razorEngine, logger);

        // ACT
        TrxReport trxReport = await generateTrxReportService.ConvertTrxToHtmlAsync(_appConfig, commandLineInterfaceValues);
        var testReportPath = Path.Combine(trxReport.Path!, _fixture.AppConfig.HtmlReportDirectoryFolder!);

        _fixture.AddTempDirectory(testReportPath);

        string testReportFile = await generateTrxReportService.CreateHtmlAsync(trxReport, _appConfig, commandLineInterfaceValues);
        bool fileExists = File.Exists(testReportFile);

        // ASSERT
        Assert.NotNull(trxReport);
        Assert.IsType<TrxReport>(trxReport);

        bool res = RegexHelper.DoesItContainsHtml(trxReport.Html!);
        Assert.True(res);

        // check file exists
        Assert.True(fileExists, $"File not found at path: {testReportFile}");

        if (fileExists)
        {
            // Read file content
            var content = File.ReadAllText(testReportFile);
            Assert.NotEmpty(content);

            // Validate content (example)
            Assert.False(string.IsNullOrWhiteSpace(content), "File is empty");

            // Optional: check specific text inside HTML
            Assert.Contains("<html", content, System.StringComparison.OrdinalIgnoreCase);
        }
        else
        {
            Assert.Fail($"File not found: {testReportFile}");
        }
    }
}
