namespace TtrxToHtml.Console.Helpers;

public static class DirectoryHelper
{
    public static void CreateDirectory(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);
    }
}
