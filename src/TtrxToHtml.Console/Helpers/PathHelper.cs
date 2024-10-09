namespace TtrxToHtml.Console.Helpers;

public static class PathHelper
{
    public static List<string> GetAllFilesBySpecifiedDirPath(string trxDirPath)
    {
        return Directory.EnumerateFiles(trxDirPath, "*", SearchOption.AllDirectories)
               .Where(s => s.EndsWith(".trx"))
               .ToList();
    }
}
