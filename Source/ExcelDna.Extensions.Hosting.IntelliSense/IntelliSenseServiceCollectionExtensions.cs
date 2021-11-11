using Microsoft.Extensions.DependencyInjection;

namespace ExcelDna.Extensions.Hosting.IntelliSense;

public static class IntelliSenseServiceCollectionExtensions
{
    public static IServiceCollection AddExcelIntelliSenseServer(this IServiceCollection services)
    {
        return services.AddHostedService<IntelliSenseHostedService>();
    }
}
