using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TtrxToHtml.Console;

//[assembly: AssemblyVersionAttribute("1.0.0")]

/// <summary>
/// Converting trx file to html format
/// </summary>
public class Program
{
    private static async Task Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                IConfiguration configuration = context.Configuration;
                services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

                RazorLightEngine razorEngine = new RazorLightEngineBuilder()
                    .UseEmbeddedResourcesProject(typeof(Program).Assembly, "TtrxToHtml.Console.Templates")
                    .UseMemoryCachingProvider()
                    .Build();

                services.AddSingleton<RazorLightEngine>(razorEngine);
                services.AddScoped<IGenerateTrxReportService, GenerateTrxReportService>();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            })
            .Build();

        using IServiceScope scope = host.Services.CreateScope();
        IServiceProvider servicesProvider = scope.ServiceProvider;
        ILogger<Program> logger = servicesProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            IConfiguration configuration = host.Services.GetRequiredService<IConfiguration>();
            AppSettings appSettings = new AppSettings();
            configuration.GetSection("AppSettings").Bind(appSettings);

            CommandLineInterfaceValues commandLineInterfaceValues = CommandLineInterfaceHelper.ArgumentsHelper(appSettings, args);

            if (string.IsNullOrEmpty(commandLineInterfaceValues.TrxPath))
            {
                logger.LogWarning("No trx path provided. Exiting.");
                return;
            }

            IGenerateTrxReportService generateTrxReportService = servicesProvider.GetRequiredService<IGenerateTrxReportService>();
            TrxReport trxReport = await generateTrxReportService.ConvertTrxToHtmlAsync(appSettings, commandLineInterfaceValues);
            string testReportFile = await generateTrxReportService.CreateHtmlAsync(trxReport, appSettings, commandLineInterfaceValues);

            logger.LogInformation("Opening generated report: {ReportPath}", testReportFile);

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = testReportFile,
                UseShellExecute = true
            };

            Process.Start(startInfo);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled exception while generating trx report.");
            throw;
        }
        finally
        {
            await host.StopAsync();
        }
    }
}