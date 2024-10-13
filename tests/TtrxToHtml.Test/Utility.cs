namespace TtrxToHtml.Test;

public class Utility
{
    public static Stream GetEmbeddedResourceStream(string resourceName) => Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)!;

    public static string[] GetEmbeddedResourceNames() => Assembly.GetExecutingAssembly().GetManifestResourceNames();

    public static string ProjectPath() => Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName;

    public static string TestDataFilePath() => Path.Combine(ProjectPath(), GetEmbeddedResourceNames()[0]);
}
