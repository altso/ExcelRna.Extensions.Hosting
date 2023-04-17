using Microsoft.Extensions.Logging;

namespace ExcelRna.Extensions.Logging;

public class LogDisplayLoggerOptions
{
    public LogLevel AutoShowLogDisplayThreshold { get; set; } = LogLevel.Error;

    /// <summary>
    /// Gets or sets format string used to format timestamp in logging messages. Defaults to <c>null</c>.
    /// </summary>
    public string? TimestampFormat { get; set; }
}
