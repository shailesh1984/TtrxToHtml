/// <summary>
/// Converting trx file to html format
/// </summary>

//[assembly: AssemblyVersionAttribute("1.0.0")]

//[HasSubCommands(typeof(NestedCommands))]
public class Program
{
    private static async Task Main(string[] args)
    {
        #region Cocona
            //CoconaApp.Run<Program>(
            //    args,
            //    options =>
            //    {
            //        options.EnableShellCompletionSupport = true;
            //    }
            //);

            //var builder = CoconaApp.CreateBuilder();
            //var app = builder.Build();

            //app.AddCommand(([Argument("n", Description = "Enter name")]string name, [Option("a", Description = "Enter age")]int age) => 
            //{
            //    Console.WriteLine($"Hello {name}, age {age}");
            //});

            //app.Run();
        #endregion

        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        
        IConfigurationRoot configuration = builder.Build();

        var appSettings = new AppSettings();
        configuration.GetSection("AppSettings").Bind(appSettings);

        var commandLineInterfaceValues = CommandLineInterfaceHelper.ArgumentsHelper(appSettings, args);

        if (string.IsNullOrEmpty(commandLineInterfaceValues.TrxPath))
        {
            return;
        }

        var serviceProvider = CreateServices();

        var generateTrxReportService = serviceProvider.GetService<IGenerateTrxReportService>()!;
        var trxReport = await generateTrxReportService.ConvertTrxToHtmlAsync(appSettings, commandLineInterfaceValues);
        var testReportFile = await generateTrxReportService.CreateHtmlAsync(trxReport, appSettings, commandLineInterfaceValues);
        Process.Start(@"cmd.exe ", $"/c \"{testReportFile}\"");
    }

    private static ServiceProvider CreateServices()
    {
        return new ServiceCollection()
            .AddScoped<IGenerateTrxReportService, GenerateTrxReportService>()
            .BuildServiceProvider();
    }

    //[Command(Description = "A dotnet cli program")]
    //public void Hello(
    //    [Option('u', Description = "Description of the option")] bool useOption,
    //    [Argument(Description = "Description of the argument")] string user
    //)
    //{
    //    var output = useOption ? $"{user}" : user;
    //    Console.WriteLine($"Hello {output}");
    //}
}