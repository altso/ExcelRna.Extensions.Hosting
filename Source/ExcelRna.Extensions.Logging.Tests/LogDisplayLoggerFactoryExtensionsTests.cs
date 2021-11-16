using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace ExcelRna.Extensions.Logging.Tests;

public class LogDisplayLoggerFactoryExtensionsTests
{
    [Fact]
    public void AddLogDisplay_adds_logger()
    {
        // ARRANGE & ACT
        var providers = new ServiceCollection()
            .AddLogging(logging => logging.AddLogDisplay())
            .BuildServiceProvider()
            .GetRequiredService<IEnumerable<ILoggerProvider>>();

        // ASSERT
        Assert.Single(providers.OfType<LogDisplayLoggerProvider>());
    }

    [Fact]
    public void AddLogDisplay_adds_logger_with_options()
    {
        // ARRANGE & ACT
        var providers = new ServiceCollection()
            .AddLogging(logging => logging.AddLogDisplay(options => options.AutoShowLogDisplayThreshold = LogLevel.Information))
            .BuildServiceProvider()
            .GetRequiredService<IEnumerable<ILoggerProvider>>();

        // ASSERT
        LogDisplayLoggerProvider provider = Assert.Single(providers.OfType<LogDisplayLoggerProvider>());
        Assert.Equal(LogLevel.Information, provider.Options.AutoShowLogDisplayThreshold);
    }
}
