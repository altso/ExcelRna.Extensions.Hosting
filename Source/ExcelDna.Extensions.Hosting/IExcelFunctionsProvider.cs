using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExcelDna.Integration;
using ExcelDna.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace ExcelDna.Extensions.Hosting;

internal interface IExcelFunctionsProvider
{
    IEnumerable<ExcelFunctionRegistration> GetExcelFunctions();
}

internal class ExcelFunctionsProvider : IExcelFunctionsProvider
{
    private readonly IServiceProvider _serviceProvider;

    public ExcelFunctionsProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IEnumerable<ExcelFunctionRegistration> GetExcelFunctions()
    {
        foreach (var declaration in _serviceProvider.GetRequiredService<IEnumerable<IExcelFunctionsDeclaration>>())
        {
            foreach (MethodInfo methodInfo in declaration.ExcelFunctionsType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                if (TryCreateFunctionRegistration(methodInfo, _serviceProvider, out var registration))
                {
                    yield return registration;
                }
            }
        }
    }

    private static bool TryCreateFunctionRegistration(MethodInfo methodInfo, IServiceProvider serviceProvider, out ExcelFunctionRegistration registration)
    {
        ExcelFunctionAttribute excelFunctionAttribute = methodInfo.GetCustomAttribute<ExcelFunctionAttribute>();
        if (excelFunctionAttribute != null)
        {
            var lambda = WrapInstanceMethod(methodInfo, serviceProvider);
            excelFunctionAttribute.Name ??= lambda.Name;
            var parameters = methodInfo.GetParameters().Select(p => new ExcelParameterRegistration(p));
            registration = new ExcelFunctionRegistration(lambda, excelFunctionAttribute, parameters);
            return true;
        }

        registration = null;
        return false;
    }

    internal static LambdaExpression WrapInstanceMethod(MethodInfo method, IServiceProvider serviceProvider)
    {
        // We wrap a method in class MyType from
        //      public object MyFunction(object arg1, object arg2) {...}
        // with a lambda expression that looks like this
        //      (object arg1, object arg2) => ((MyType)serviceProvider.GetRequiredService(typeof(MyType))).MyFunction(arg1, arg2)

        var functionsType = method.DeclaringType ?? throw new InvalidOperationException();
        var provider = Expression.Constant(serviceProvider);
        MethodInfo getRequiredServiceMethod =
            typeof(ServiceProviderServiceExtensions).GetMethod(
                nameof(ServiceProviderServiceExtensions.GetRequiredService),
                new[] {typeof(IServiceProvider), typeof(Type)})
            ?? throw new InvalidOperationException();
        var instanceObject = Expression.Call(null, getRequiredServiceMethod, provider, Expression.Constant(functionsType));
        var instance = Expression.Convert(instanceObject, functionsType);
        var callParams = method.GetParameters().Select(p => Expression.Parameter(p.ParameterType, p.Name)).ToList();

        var callExpr = Expression.Call(instance, method, callParams);
        return Expression.Lambda(callExpr, method.Name, callParams);
    }
}
