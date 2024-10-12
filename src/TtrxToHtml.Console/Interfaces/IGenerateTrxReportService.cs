namespace TtrxToHtml.Console.Interfaces;

public interface IGenerateTrxReportService
{
    public Task<string> GenerateTrxReportAsync(AppSettings appSettings, CommandLineInterfaceValues commandLineInterfaceValues);
}
