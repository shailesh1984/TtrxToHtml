namespace TtrxToHtml.Test;

public static class TestHelper
{
    public static AppSettings InitConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.test.json", optional: false, reloadOnChange: true)
            .Build();

        var appSettings = new AppSettings();
        configuration.GetSection("AppSettings").Bind(appSettings);

        return appSettings;
    }

    public static string ReadFile(string resourceName)
    {
        using Stream resourceStream = Utility.GetEmbeddedResourceStream(resourceName);
        using StreamReader reader = new(resourceStream);

        return reader.ReadToEnd();
    }
}
