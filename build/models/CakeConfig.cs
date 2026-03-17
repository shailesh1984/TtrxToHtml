public class CakeConfig
{
    public Config Config { get; set; }
    public BuildConfig Build { get; set; }
    public PathsConfig Paths { get; set; }
    public TestConfig Test { get; set; }
    public GitHubConfig GitHub { get; set; }
}

public class Config
{
    public string ProjectName { get; set; }
    public string Author { get; set; }
}

public class BuildConfig
{
    public string BuildConfiguration { get; set; }
    public string Runtime { get; set; }
    public string Version { get; set; }
    public string SolutionFile { get; set; }
    public string ArtifactDirectory { get; set; }
    public string OutputDirectory { get; set; }
    public string ZipFileName { get; set; }
    public string ExecutableName { get; set; }
}

public class PathsConfig
{
    public string ReleaseNotes { get; set; }
    public string LogFile { get; set; }
    public string TargetDirectory { get; set; }
    public string InputTextFile { get; set; }
}

public class TestConfig
{
    public string TrxFileName { get; set; }
    public string ResultsDirectory { get; set; }
}

public class GitHubConfig
{
    public string Token { get; set; }
    public string RepoOwner { get; set; }
    public string RepoName { get; set; }
    public string ReleaseTagName { get; set; }
}