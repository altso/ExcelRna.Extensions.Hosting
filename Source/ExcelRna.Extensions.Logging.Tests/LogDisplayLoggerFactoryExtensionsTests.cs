using System.Collections.Generic;
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
        Assert.Contains(providers, provider => provider is LogDisplayLoggerProvider);
    }
}
