namespace TtrxToHtml.Test.Tests;

public class TrxHelperTests : IClassFixture<TestFixture>
{
    private readonly string _testRootDir;
    private readonly AppSettings _appConfig;
    private readonly TestFixture _fixture;

    public TrxHelperTests(TestFixture fixture)
    {
        _fixture = fixture;
        _appConfig = fixture.AppConfig;
        var args = new[]
        {
            "-f",
            Utility.TestDataFilePath()
        };
        CommandLineInterfaceValues commandLineInterfaceValues = CommandLineInterfaceHelper.ArgumentsHelper(_appConfig, args);

        // Create a temporary test directory
        var directoryPath = Path.GetDirectoryName(commandLineInterfaceValues.TrxPath)!;
        _testRootDir = Path.Combine(directoryPath, Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testRootDir);

        _fixture.AddTempDirectory(_testRootDir);
    }

    [Fact]
    public void CombineAllTrxFilesToOneTrx_ShouldReturnCombinedJson()
    {
        // Arrange
        var file1 = Path.Combine(_testRootDir, "test1.trx");
        var file2 = Path.Combine(_testRootDir, "test2.trx");

        File.WriteAllText(file1, "<TestRun><Result>Pass</Result></TestRun>");
        File.WriteAllText(file2, "<TestRun><Result>Fail</Result></TestRun>");

        var files = new List<string> { file1, file2 };

        // Act
        var result = TrxHelper.CombineAllTrxFilesToOneTrx(files);

        // Assert
        Assert.False(string.IsNullOrEmpty(result));
        Assert.Contains("TestResults", result);
        Assert.Contains("Pass", result);
        Assert.Contains("Fail", result);
    }

    [Fact]
    public void DeepCopy_ShouldCopyAllFilesAndDirectories()
    {
        // Arrange
        var sourceDir = new DirectoryInfo(_testRootDir);
        var destDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        _fixture.AddTempDirectory(destDir);

        Directory.CreateDirectory(sourceDir.FullName);
        Directory.CreateDirectory(destDir);

        var subDir = Directory.CreateDirectory(Path.Combine(sourceDir.FullName, "SubFolder"));
        var filePath = Path.Combine(subDir.FullName, "test.txt");
        File.WriteAllText(filePath, "sample content");

        // Act
        TrxHelper.DeepCopy(sourceDir, destDir, "Copied");

        var expectedPath = Path.Combine(destDir, "Copied", "SubFolder", "test.txt");

        // Assert
        Assert.True(File.Exists(expectedPath));
        Assert.Equal("sample content", File.ReadAllText(expectedPath));

        // Cleanup
        Directory.Delete(sourceDir.FullName, true);
        Directory.Delete(destDir, true);
    }

    [Fact]
    public void IsPathFileOrDirectory_ShouldIdentifyFileAndDirectory()
    {
        // Arrange
        var filePath = Path.Combine(_testRootDir, "file.txt");
        File.WriteAllText(filePath, "data");

        // Act
        var fileResult = TrxHelper.IsPathFileOrDirectory(filePath);
        var dirResult = TrxHelper.IsPathFileOrDirectory(_testRootDir);
        var invalidResult = TrxHelper.IsPathFileOrDirectory("invalid_path");

        // Assert
        Assert.True(fileResult.Item1);
        Assert.False(fileResult.Item2);

        Assert.False(dirResult.Item1);
        Assert.True(dirResult.Item2);

        Assert.False(invalidResult.Item1);
        Assert.False(invalidResult.Item2);
    }
}
