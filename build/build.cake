//////////////////////////////////////////////////////////////////////
// ADDINS
//////////////////////////////////////////////////////////////////////

#addin nuget:?package=Cake.Git
#addin nuget:?package=Cake.Json
#addin nuget:?package=Cake.GitHub&version=1.0.0
#addin nuget:?package=Cake.FileHelpers&version=7.0.0
#addin nuget:?package=Newtonsoft.Json&version=13.0.4

//////////////////////////////////////////////////////////////////////
// TOOLS
//////////////////////////////////////////////////////////////////////

#tool dotnet:?package=GitReleaseManager.Tool&version=0.20.0

#tool nuget:?package=GitVersion.Tool
#tool nuget:?package=GitReleaseManager&version=0.20.0
#tool nuget:?package=GitVersion.CommandLine
#tool nuget:?package=Newtonsoft.Json&version=13.0.4

//////////////////////////////////////////////////////////////////////
// NAMESPACES
//////////////////////////////////////////////////////////////////////
#load "./models/CakeConfig.cs"
#load "./services/JsonConfigReader.cs"

//////////////////////////////////////////////////////////////////////
// VARIABLES
//////////////////////////////////////////////////////////////////////
var target = Argument("target", "Default");
//var target = Argument("target", "TrxToHtmlConverterRelease");
var cakeConfigPath = "./cakeConfig.json";

CakeConfig config;
string outputDir = "";
string zipFile = "";
string releaseTagName = "";

Setup(context =>
{
    Information("Read All Config");
    config = JsonConfigReader.LoadConfig(cakeConfigPath);
    outputDir = config.Build.ArtifactDirectory + config.Build.OutputDirectory;
    zipFile = config.Build.ArtifactDirectory + config.Build.ZipFileName;
    releaseTagName = string.Format(config.GitHub.ReleaseTagName, config.Build.Version);
});

Task("Clean")
.Does(() =>
{
    Information("Clean started");
    //Information(GetFiles("./**/*.csproj"));

    CleanDirectory(config.Build.ArtifactDirectory);
    CleanDirectory(config.Test.ResultsDirectory);
});

Task("Restore")
.Does(() =>
{
    Information("Restore started");
    DotNetRestore(config.Build.SolutionFile);
});

Task("Build")
.Does(() =>
{
    Information("Build started");
    DotNetBuild(config.Build.SolutionFile, new DotNetBuildSettings
    {
        Configuration = config.Build.BuildConfiguration,
        MSBuildSettings = new DotNetMSBuildSettings()
            .WithProperty("NoWarn", "ALL")
    });
});

Task("Test")
.Does(() =>
{
    Information("Test started");
    DotNetTest(config.Build.SolutionFile, new DotNetTestSettings
    {
        Configuration = config.Build.BuildConfiguration,
        NoBuild = true,
        Loggers = new[] { $"trx;LogFileName={string.Format(config.Test.TrxFileName, DateTime.UtcNow.ToString("yyyyMMddHHmmss"))}" },
        ResultsDirectory = config.Test.ResultsDirectory
    });
});

Task("Publish")
.Does(() =>
{
    Information("Publish started");
    DotNetPublish("./../src/TtrxToHtml.Console/TtrxToHtml.Console.csproj", new DotNetPublishSettings
    {
        Configuration = config.Build.BuildConfiguration,
        Runtime = config.Build.Runtime,
        SelfContained = true,
        PublishSingleFile = true,
        OutputDirectory = outputDir,
        MSBuildSettings = new DotNetMSBuildSettings()
            .WithProperty("AssemblyName", string.Format(config.Build.ExecutableName, config.Build.Version))
            .WithProperty("PublishSingleFile", "true")
            .WithProperty("PublishTrimmed", "false")
            .WithProperty("IncludeAllContentForSelfExtract", "true")
    });
});

Task("Zip")
.Does(() =>
{
    Information("Zip started");
    Zip(outputDir, zipFile);
});

Task("CreateReleaseNotes")
.Does(() =>
{
    Information("Create Release Notes started");

    // Initial release note content
    var header = $"Release version {config.Build.Version} of {config.Config.ProjectName} by {config.Config.Author}{Environment.NewLine}{Environment.NewLine}";
    
    System.IO.File.WriteAllText(config.Paths.ReleaseNotes, header);

    var sourceFilePath = config.Paths.InputTextFile; 

    if (System.IO.File.Exists(sourceFilePath))
    {
        // Read all content from source file
        var content = System.IO.File.ReadAllText(sourceFilePath);

        if (!string.IsNullOrWhiteSpace(content))
        {
            System.IO.File.AppendAllText(config.Paths.ReleaseNotes, content);
            Information("Content appended to release notes");
            System.IO.File.WriteAllText(sourceFilePath, string.Empty);
            Information("Source text file cleared");
        }
        else
        {
            Information("Source text file is empty");
        }
    }
    else
    {
        Warning($"Source file not found: {sourceFilePath}");
    }

    var outputFilePath = MakeAbsolute(File(config.Paths.ReleaseNotes)).FullPath;
    Information($"ReleaseNotes Path: {outputFilePath}");
});

Task("CreateGitHubRelease")
.Does(() =>
{
    Information("Create GitHub Release started");
    GitReleaseManagerCreate(config.GitHub.Token, config.GitHub.RepoOwner, config.GitHub.RepoName, new GitReleaseManagerCreateSettings
    {
        Name = releaseTagName,
        InputFilePath = MakeAbsolute(File(config.Paths.ReleaseNotes)).FullPath,
        Prerelease = false,
        TargetCommitish = "master",
        TargetDirectory = MakeAbsolute(File(config.Paths.TargetDirectory)).FullPath,
        LogFilePath = MakeAbsolute(File(config.Paths.LogFile)).FullPath
    });
});

Task("Upload-Asset")
.Does(() =>
{
    Information("Upload Asset to GitHub Release started");
    GitReleaseManagerAddAssets(config.GitHub.Token, config.GitHub.RepoOwner, config.GitHub.RepoName, releaseTagName, MakeAbsolute(File(zipFile)).FullPath, new GitReleaseManagerAddAssetsSettings
    {
        TargetDirectory = MakeAbsolute(File(config.Paths.TargetDirectory)).FullPath,
        LogFilePath = MakeAbsolute(File(config.Paths.LogFile)).FullPath
    });
});

Task("Publish-Release")
.Does(() =>
{
    Information("Publishing GitHub Release");

    GitReleaseManagerPublish(config.GitHub.Token, config.GitHub.RepoOwner, config.GitHub.RepoName, releaseTagName,
        new GitReleaseManagerPublishSettings
        {
            TargetDirectory = MakeAbsolute(File(config.Paths.TargetDirectory)).FullPath,
            LogFilePath = MakeAbsolute(File(config.Paths.LogFile)).FullPath
        });
});

Task("Default")
.IsDependentOn("Clean")
.IsDependentOn("Restore")
.IsDependentOn("Build")
.IsDependentOn("Test")
.IsDependentOn("Publish");

Task("TrxToHtmlConverterRelease")
.IsDependentOn("Clean")
.IsDependentOn("Restore")
.IsDependentOn("Build")
.IsDependentOn("Test")
.IsDependentOn("Publish")
.IsDependentOn("Zip")
.IsDependentOn("CreateReleaseNotes")
.IsDependentOn("CreateGitHubRelease")
.IsDependentOn("Upload-Asset")
.IsDependentOn("Publish-Release");

RunTarget(target);