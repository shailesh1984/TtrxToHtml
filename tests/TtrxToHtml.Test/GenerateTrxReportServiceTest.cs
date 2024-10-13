namespace TtrxToHtml.Test;

public class GenerateTrxReportServiceTest
{
    [Fact]
    public async Task GenerateTrxReportTest()
    {
        // ARRANGE
        var appConfig = TestHelper.InitConfiguration();
        string[] args = [$@"-f={Utility.TestDataFilePath()}"];
        var commandLineInterfaceValues = CommandLineInterfaceHelper.ArgumentsHelper(appConfig, args);

        // ACT
        var generateTrxReportService = new GenerateTrxReportService();
        var actual = await generateTrxReportService.ConvertTrxToHtmlAsync(appConfig, commandLineInterfaceValues);

        // ASSERT
        Assert.NotNull(actual);
        Assert.IsType<TrxReport>(actual);

        var res = RegexHelper.DoesItContainsHtml(actual.Html!);
        Assert.True(res);
    }
}
