namespace TtrxToHtml.Test;

public class GenerateTrxReportServiceTest
{
    [Fact]
    public async Task GenerateTrxReportTest()
    {
        // ARRANGE
        var appConfig = TestHelper.InitConfiguration();
        string[] args = { @"-f=C:\Personal\Work\TtrxToHtml\tests\TtrxToHtml.Test\data\xUnit-net8.0-sample.trx" };
        var commandLineInterfaceValues = CommandLineInterfaceHelper.ArgumentsHelper(appConfig, args);

        // ACT
        IGenerateTrxReportService generateTrxReportService = new GenerateTrxReportService();
        var ss = await generateTrxReportService.GenerateTrxReportAsync(appConfig, commandLineInterfaceValues);

        // ASSERT
    }
}
