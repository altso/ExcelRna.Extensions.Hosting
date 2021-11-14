using Microsoft.Extensions.Logging;

namespace ExcelRna.Extensions.Logging;

/// <summary>
/// The provider for the <see cref="LogDisplayLogger"/>.
/// </summary>
[ProviderAlias("LogDisplay")]
public class LogDisplayLoggerProvider : ILoggerProvider
{
    /// <inheritdoc />
    public ILogger CreateLogger(string name)
    {
        return new LogDisplayLogger(name);
    }

    /// <inheritdoc />
    public void Dispose()
    {
    }
}
