using Microsoft.Extensions.Logging;

namespace ExcelRna.Extensions.Logging;

public class LogDisplayLoggerOptions
{
    public LogLevel AutoShowLogDisplayThreshold { get; set; } = LogLevel.Error;
}
