namespace TtrxToHtml.Console.Helpers;

public static class DirectoryHelper
{
    public static void CreateDirectory(string directoryPath)
    {
        bool exists = Directory.Exists(directoryPath);

        if (!exists)
            Directory.CreateDirectory(directoryPath);
    }
}
