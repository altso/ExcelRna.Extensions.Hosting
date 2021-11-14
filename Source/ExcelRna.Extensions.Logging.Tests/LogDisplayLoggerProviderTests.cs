using Xunit;

namespace ExcelRna.Extensions.Logging.Tests;

public class LogDisplayLoggerProviderTests
{
    [Fact]
    public void CreateLogger_creates_LogDisplayLogger()
    {
        // ARRANGE
        var provider = new LogDisplayLoggerProvider();

        // ACT
        var logger = provider.CreateLogger("Test");

        // ASSERT
        Assert.IsType<LogDisplayLogger>(logger);
    }

    [Fact]
    public void Dispose_does_not_throw()
    {
        // ARRANGE
        var provider = new LogDisplayLoggerProvider();

        // ACT
        provider.Dispose();
    }
}
