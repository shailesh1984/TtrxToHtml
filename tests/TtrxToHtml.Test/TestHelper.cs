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

    public static bool IsContainsHTMLElements(string html)
    {
        Regex tagRegex = new(@"<([A-Za-z][A-Za-z0-9]*)\b[^>]*>(.*?)<\/\1>");

        return tagRegex.IsMatch(html);
    }

    public static string ReadFile(string resourceName)
    {
        using Stream resourceStream = Utility.GetEmbeddedResourceStream(resourceName);
        using StreamReader reader = new StreamReader(resourceStream);

        return reader.ReadToEnd();
    }
}
