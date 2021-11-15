using System;
using ExcelDna.Logging;
using Microsoft.Extensions.Logging;

namespace ExcelRna.Extensions.Logging;

/// <summary>
/// A logger that writes messages LogDisplay window.
/// </summary>
internal sealed class LogDisplayLogger : ILogger
{
    private readonly string _name;

    /// <summary>
    /// Initializes a new instance of the <see cref="LogDisplayLogger"/> class.
    /// </summary>
    /// <param name="name">The name of the logger.</param>
    /// <param name="options">The options of the logger.</param>
    public LogDisplayLogger(string name, LogDisplayLoggerOptions? options = null)
    {
        _name = name;
        Options = options ?? new LogDisplayLoggerOptions();
    }

    internal Action<string, string[]> RecordLine { get; set; } = LogDisplay.RecordLine;

    internal Action Show { get; set; } = LogDisplay.Show;

    internal LogDisplayLoggerOptions Options { get; set; }

    /// <inheritdoc />
    public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

    /// <inheritdoc />
    public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;

    /// <inheritdoc />
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        if (formatter == null)
        {
            throw new ArgumentNullException(nameof(formatter));
        }

        string message = formatter(state, exception);

        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        message = $"{_name} [{logLevel}] {message}";

        if (exception != null)
        {
            message += Environment.NewLine + exception;
        }

        RecordLine(message, Array.Empty<string>());

        if (logLevel >= Options.AutoShowLogDisplayThreshold)
        {
            Show();
        }
    }
}
