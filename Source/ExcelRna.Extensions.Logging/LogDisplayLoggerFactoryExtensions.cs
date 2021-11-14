using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace ExcelRna.Extensions.Logging;

/// <summary>
/// Extension methods for the <see cref="ILoggerFactory"/> class.
/// </summary>
public static class LogDisplayLoggerFactoryExtensions
{
    /// <summary>
    /// Adds a debug logger named 'LogDisplay' to the factory.
    /// </summary>
    /// <param name="builder">The extension method argument.</param>
    public static ILoggingBuilder AddLogDisplay(this ILoggingBuilder builder)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, LogDisplayLoggerProvider>());

        return builder;
    }
}
