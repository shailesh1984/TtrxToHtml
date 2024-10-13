namespace TtrxToHtml.Test;

public class GenerateTrxReportServiceTest
{
    [Fact]
    public async Task GenerateTrxReportTest()
    {
        var path = Utility.TestDataFilePath();

        // ARRANGE
        var appConfig = TestHelper.InitConfiguration();
        string[] args = { $@"-f={path}" };
        var commandLineInterfaceValues = CommandLineInterfaceHelper.ArgumentsHelper(appConfig, args);

        // ACT
        IGenerateTrxReportService generateTrxReportService = new GenerateTrxReportService();
        var actual = await generateTrxReportService.GenerateTrxReportAsync(appConfig, commandLineInterfaceValues);

        // ASSERT
        Assert.NotNull(actual);
        Assert.IsType<TrxReport>(actual);

        var res = TestHelper.IsContainsHTMLElements(actual.Html!);
        Assert.True(res);
    }
}
