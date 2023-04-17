using System;
using System.Text;
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

    internal Action<string, object[]> RecordLine { get; set; } = LogDisplay.RecordLine;

    internal Action Show { get; set; } = LogDisplay.Show;

    internal LogDisplayLoggerOptions Options { get; set; }

    internal Func<DateTimeOffset> GetCurrentTimestamp { get; set; } = () => DateTimeOffset.Now;

    /// <inheritdoc />
    public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

    /// <inheritdoc />
    public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;

    /// <inheritdoc />
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
        Func<TState, Exception?, string> formatter)
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

        StringBuilder builder = new();

        if (Options.TimestampFormat != null)
        {
            DateTimeOffset dateTimeOffset = GetCurrentTimestamp();
            builder.Append(dateTimeOffset.ToString(Options.TimestampFormat));
            builder.Append(" ");
        }

        builder.Append("[");
        builder.Append(logLevel);
        builder.Append("] ");
        builder.Append(_name);
        builder.Append(": ");
        builder.Append(message);

        if (exception != null)
        {
            builder.AppendLine();
            builder.Append(exception);
        }

        RecordLine(builder.ToString(), Array.Empty<object>());

        if (logLevel >= Options.AutoShowLogDisplayThreshold)
        {
            Show();
        }
    }
}
