namespace TtrxToHtml.Console.Helpers;

public static class CommandLineArgumentsHelper
{
    /// <summary>
    /// parsing arguments to dictionary
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static Dictionary<string, string> ParseArguments(string[] args)
    {
        var arguments = new Dictionary<string, string>();

        foreach (var arg in args)
        {
            // Split the argument by '=' to handle key/value pairs
            string[] parts = arg.Split('=');

            // Check if the argument is in the format "key=value"
            if (parts.Length == 2)
            {
                arguments[parts[0]] = parts[1];
            }
            // If not, assume it's just a named argument without a value
            else
            {
                arguments[arg] = null;
            }
        }

        return arguments;
    }

    /// <summary>
    /// print application usage information
    /// </summary>
    public static void PrintHelp()
    {
        System.Console.WriteLine("\nUsage: TtrxToHtml.Console [options]\n");
        System.Console.WriteLine("Options:");
        System.Console.WriteLine("  -h|--help                               Display help.");
        System.Console.WriteLine("  --info                                  Display TtrxToHtml.Console Information.");
        System.Console.WriteLine("  -tdp=<directory-path>|--trx-dir-path=<directory-path>    Specify trx directory path.");
        //System.Console.WriteLine("  --output=<file>    Specify output file");
    }

    /// <summary>
    /// print application --info details
    /// </summary>
    public static void PrintInfo()
    {
        Assembly assembly = typeof(Program).Assembly;
        AssemblyName assemblyName = assembly.GetName();
        Version version = assemblyName.Version!;

        System.Console.WriteLine($"\n{assemblyName.Name}:");
        System.Console.WriteLine($" Version:    {version}");
        System.Console.WriteLine("\nRuntime Environment:");
        System.Console.WriteLine($" OS Name:       Windows");
        System.Console.WriteLine($" OS Version:    {Environment.OSVersion}");
        System.Console.WriteLine($" OS Platform:   Windows");
    }
}
