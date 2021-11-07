using System;
using System.Collections.Generic;
using ExcelDna.Registration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ExcelDna.Extensions.Hosting
{
    public interface IExcelFunctionsBuilder
    {
        IServiceCollection Services { get; }
    }

    internal class ExcelFunctionsBuilder : IExcelFunctionsBuilder
    {
        public ExcelFunctionsBuilder(IServiceCollection services) => Services = services;

        public IServiceCollection Services { get; }
    }

    public static class ExcelFunctionsBuilderExtensions
    {
        public static IServiceCollection AddExcelFunctions(this IServiceCollection services, Action<IExcelFunctionsBuilder> configure)
        {
            services.AddTransient<IExcelFunctionsProvider, ExcelFunctionsProvider>();
            services.AddHostedService<ExcelFunctionsRegistrar>();

            configure(new ExcelFunctionsBuilder(services));

            return services;
        }

        public static IExcelFunctionsBuilder ConfigureRegistrations(this IExcelFunctionsBuilder builder, Func<IEnumerable<ExcelFunctionRegistration>, IEnumerable<ExcelFunctionRegistration>> configure)
        {
            builder.Services.Configure<ExcelFunctionRegistrationOptions>(options => options.Configure = configure);
            return builder;
        }

        public static IExcelFunctionsBuilder AddFrom<T>(this IExcelFunctionsBuilder builder) => builder.AddFrom<T>(ServiceLifetime.Transient);

        public static IExcelFunctionsBuilder AddFrom<T>(this IExcelFunctionsBuilder builder, ServiceLifetime lifetime)
        {
            builder.Services.Add(new ServiceDescriptor(typeof(T), typeof(T), lifetime));
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IExcelFunctionsDeclaration, ExcelFunctionsDeclaration<T>>());
            return builder;
        }
    }
}
