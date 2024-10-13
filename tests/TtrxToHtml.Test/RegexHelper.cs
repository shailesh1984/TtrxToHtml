namespace TtrxToHtml.Test;

/// <summary>
/// https://learn.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-source-generators?pivots=dotnet-8-0
/// </summary>
public static partial class RegexHelper
{
    [GeneratedRegex(@"<([A-Za-z][A-Za-z0-9]*)\b[^>]*>(.*?)<\/\1>", RegexOptions.IgnoreCase)]
    private static partial Regex IsContainsHTMLElements();

    public static bool DoesItContainsHtml(string html) => IsContainsHTMLElements().IsMatch(html);
}