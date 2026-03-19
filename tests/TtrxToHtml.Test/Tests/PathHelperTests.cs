namespace TtrxToHtml.Test.Tests;

public class PathHelperTests : IClassFixture<TestFixture>
{
    private readonly string _testRootDir;
    private readonly AppSettings _appConfig;
    private readonly TestFixture _fixture;

    public PathHelperTests(TestFixture fixture)
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
    public void GetAllFilesBySpecifiedDirPath_ShouldReturnOnlyTrxFiles()
    {
        // Arrange
        var file1 = Path.Combine(_testRootDir, "file1.trx");
        var file2 = Path.Combine(_testRootDir, "file2.trx");
        var file3 = Path.Combine(_testRootDir, "file3.txt");

        File.WriteAllText(file1, "test");
        File.WriteAllText(file2, "test");
        File.WriteAllText(file3, "test");

        // Act
        var result = PathHelper.GetAllFilesBySpecifiedDirPath(_testRootDir);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, file => Assert.EndsWith(".trx", file));
    }

    [Fact]
    public void GetAllFilesBySpecifiedDirPath_ShouldReturnFilesFromSubDirectories()
    {
        // Arrange
        var subDir = Path.Combine(_testRootDir, "SubFolder");
        Directory.CreateDirectory(subDir);

        var file1 = Path.Combine(subDir, "nested.trx");
        File.WriteAllText(file1, "test");

        // Act
        var result = PathHelper.GetAllFilesBySpecifiedDirPath(_testRootDir);

        // Assert
        Assert.Single(result);
        Assert.Contains(result, f => f.EndsWith("nested.trx"));
    }

    [Fact]
    public void GetAllFilesBySpecifiedDirPath_ShouldReturnEmptyList_WhenNoTrxFiles()
    {
        // Arrange
        var file1 = Path.Combine(_testRootDir, "file1.txt");
        File.WriteAllText(file1, "test");

        // Act
        var result = PathHelper.GetAllFilesBySpecifiedDirPath(_testRootDir);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetAllFilesBySpecifiedDirPath_ShouldThrowException_WhenPathIsInvalid()
    {
        // Arrange
        var invalidPath = "Z:\\Invalid\\Path";

        // Act & Assert
        Assert.Throws<DirectoryNotFoundException>(() =>
            PathHelper.GetAllFilesBySpecifiedDirPath(invalidPath));
    }
}