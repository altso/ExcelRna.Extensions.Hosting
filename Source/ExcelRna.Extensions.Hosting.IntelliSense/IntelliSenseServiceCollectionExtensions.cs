using Microsoft.Extensions.DependencyInjection;

namespace ExcelRna.Extensions.Hosting.IntelliSense;

public static class IntelliSenseServiceCollectionExtensions
{
    public static IServiceCollection AddExcelIntelliSenseServer(this IServiceCollection services)
    {
        return services.AddHostedService<IntelliSenseHostedService>();
    }
}
