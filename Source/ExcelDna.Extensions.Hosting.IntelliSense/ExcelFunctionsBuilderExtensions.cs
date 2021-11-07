using Microsoft.Extensions.DependencyInjection;

namespace ExcelDna.Extensions.Hosting
{
    public static class ExcelFunctionsBuilderExtensions
    {
        public static IExcelFunctionsBuilder AddIntelliSense(this IExcelFunctionsBuilder builder)
        {
            builder.Services.AddHostedService<IntelliSenseHostedService>();
            return builder;
        }
    }
}
