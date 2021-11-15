using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExcelRna.Extensions.Logging;

/// <summary>
/// The provider for the <see cref="LogDisplayLogger"/>.
/// </summary>
[ProviderAlias("LogDisplay")]
public class LogDisplayLoggerProvider : ILoggerProvider
{
    private readonly IOptionsMonitor<LogDisplayLoggerOptions> _options;
    private readonly IDisposable _optionsReloadToken;
    private readonly ConcurrentDictionary<string, LogDisplayLogger> _loggers = new();

    public LogDisplayLoggerProvider(IOptionsMonitor<LogDisplayLoggerOptions> options)
    {
        _options = options;
        _optionsReloadToken = options.OnChange(ReloadOptions);
    }

    /// <inheritdoc />
    public ILogger CreateLogger(string name)
    {
        return _loggers.GetOrAdd(name, n => new LogDisplayLogger(n, _options.CurrentValue));
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _optionsReloadToken.Dispose();
    }

    private void ReloadOptions(LogDisplayLoggerOptions options)
    {
        foreach (KeyValuePair<string, LogDisplayLogger> logger in _loggers)
        {
            logger.Value.Options = options;
        }
    }
}
