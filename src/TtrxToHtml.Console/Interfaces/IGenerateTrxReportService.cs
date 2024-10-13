namespace TtrxToHtml.Console.Interfaces;

public interface IGenerateTrxReportService
{
    public Task<TrxReport> ConvertTrxToHtmlAsync(AppSettings appSettings, CommandLineInterfaceValues commandLineInterfaceValues);
    public Task<string> CreateHtmlAsync(TrxReport trxReport, AppSettings appSettings, CommandLineInterfaceValues commandLineInterfaceValues);
}
