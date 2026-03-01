using Microsoft.Extensions.Logging;
using RazorLight;

namespace TtrxToHtml.Test;

public class GenerateTrxReportServiceTest
{
    [Fact]
    public async Task GenerateTrxReportTest()
    {
        // ARRANGE
        AppSettings appConfig = TestHelper.InitConfiguration();
        var args = new[] 
        { 
            "-f", 
            Utility.TestDataFilePath() 
        };
        CommandLineInterfaceValues commandLineInterfaceValues = CommandLineInterfaceHelper.ArgumentsHelper(appConfig, args);

        RazorLightEngine razorEngine = new RazorLightEngineBuilder()
            .UseEmbeddedResourcesProject(typeof(TtrxToHtml.Console.Program).Assembly, "TtrxToHtml.Console.Templates")
            .UseMemoryCachingProvider()
            .Build();

        ILogger<GenerateTrxReportService> logger = new LoggerFactory().CreateLogger<GenerateTrxReportService>();

        GenerateTrxReportService generateTrxReportService = new GenerateTrxReportService(razorEngine, logger);

        // ACT
        TrxReport actual = await generateTrxReportService.ConvertTrxToHtmlAsync(appConfig, commandLineInterfaceValues);

        // ASSERT
        Assert.NotNull(actual);
        Assert.IsType<TrxReport>(actual);

        bool res = RegexHelper.DoesItContainsHtml(actual.Html!);
        Assert.True(res);
    }
}
