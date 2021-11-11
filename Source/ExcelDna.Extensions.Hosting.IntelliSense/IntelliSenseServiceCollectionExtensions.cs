using Microsoft.Extensions.DependencyInjection;

namespace ExcelDna.Extensions.Hosting.IntelliSense;

public static class IntelliSenseServiceCollectionExtensions
{
    public static IServiceCollection AddExcelFunctionsIntelliSense(this IServiceCollection services)
    {
        return services.AddHostedService<IntelliSenseHostedService>();
    }
}