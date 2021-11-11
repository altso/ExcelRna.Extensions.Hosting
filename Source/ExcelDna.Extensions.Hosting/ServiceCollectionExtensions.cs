using System;
using System.Collections.Generic;
using ExcelDna.Registration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ExcelDna.Extensions.Hosting;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExcelFunctions<T>(this IServiceCollection services)
        where T : class
    {
        services.AddTransient<IExcelFunctionsProvider, ExcelFunctionsProvider>();
        services.AddHostedService<ExcelFunctionsRegistrar>();

        services.AddSingleton<T>();
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IExcelFunctionsDeclaration, ExcelFunctionsDeclaration<T>>());

        return services;
    }

    public static IServiceCollection AddExcelRibbon<T>(this IServiceCollection services)
        where T : HostedExcelRibbon
    {
        services.AddHostedService<ExcelRibbonLoader>();
        services.TryAddEnumerable(ServiceDescriptor.Singleton<HostedExcelRibbon, T>());
        return services;
    }

    public static IServiceCollection AddExcelFunctionsProcessors(this IServiceCollection services, Func<IEnumerable<ExcelFunctionRegistration>, IEnumerable<ExcelFunctionRegistration>> process)
    {
        return services.AddSingleton<IExcelFunctionsProcessor>(new ExcelFunctionsProcessor(process));
    }
}