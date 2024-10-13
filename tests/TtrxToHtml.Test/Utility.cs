namespace TtrxToHtml.Test;

public class Utility
{
    public static Stream GetEmbeddedResourceStream(string resourceName)
    {
        return Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)!;
    }

    public static string[] GetEmbeddedResourceNames()
    {
        return Assembly.GetExecutingAssembly().GetManifestResourceNames();
    }

    public static string ProjectPath()
    {
        return Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName;
    }

    public static string TestDataFilePath()
    {
        return Path.Combine(ProjectPath(), GetEmbeddedResourceNames()[0]);
    }
}
