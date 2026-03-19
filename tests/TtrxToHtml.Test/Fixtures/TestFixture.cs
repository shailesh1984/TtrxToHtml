using System.Diagnostics.Metrics;

namespace TtrxToHtml.Test.Fixtures;

/// <summary>
/// IClassFixture<T> => creates ONE instance of TestFixture per test class
/// That fixture is shared across ALL test methods
/// Dispose() of the fixture runs ONLY ONCE after all tests finish
/// </summary>
public class TestFixture : IDisposable
{
    private bool _disposed;

    public AppSettings AppConfig { get; private set; }

    private readonly List<string> _tempDirectories = [];

    public TestFixture()
    {
        AppConfig = TestHelper.InitConfiguration();
    }

    public void AddTempDirectory(string path)
    {
        if (!string.IsNullOrWhiteSpace(path))
        {
            _tempDirectories.Add(path);
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing) 
        {
            foreach (var dir in _tempDirectories.Distinct())
            {
                try
                {
                    // free managed resources here
                    if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
                    {
                        Directory.Delete(dir, recursive: true);
                    }
                }
                catch (Exception ex)
                {
                    // Optional: log instead of throwing
                    System.Console.WriteLine($"Failed to delete {dir}: {ex.Message}");
                }
            }
        }

        // free unmanaged resources here (if any)

        _disposed = true;
    }
}
