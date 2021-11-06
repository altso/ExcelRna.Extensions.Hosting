using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ExcelDna.Extensions.Hosting
{
    public static class ExcelFunctionsServiceCollectionExtensions
    {
        public static IServiceCollection AddExcelFunctions<T>(this IServiceCollection services) => services.AddExcelFunctions<T>(ServiceLifetime.Transient);

        public static IServiceCollection AddExcelFunctions<T>(this IServiceCollection services, ServiceLifetime lifetime)
        {
            services.Add(new ServiceDescriptor(typeof(T), typeof(T), lifetime));
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IExcelFunctionsDeclaration, ExcelFunctionsDeclaration<T>>());
            return services;
        }
    }

    public interface IExcelFunctionsDeclaration
    {
        Type ExcelFunctionsType { get; }
    }

    public class ExcelFunctionsDeclaration<T> : IExcelFunctionsDeclaration
    {
        public Type ExcelFunctionsType { get; } = typeof(T);
    }
}
