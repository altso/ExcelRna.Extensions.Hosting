using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExcelDna.Integration;
using ExcelDna.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace ExcelDna.Extensions.Hosting
{
    public class ExcelFunctionsProvider : IExcelFunctionsProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public ExcelFunctionsProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<ExcelFunctionRegistration> GetExcelFunctions() =>
            from declaration in _serviceProvider.GetRequiredService<IEnumerable<IExcelFunctionsDeclaration>>()
            from methodInfo in declaration.ExcelFunctionsType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
            where methodInfo.GetCustomAttribute<ExcelFunctionAttribute>() != null
            select new ExcelFunctionRegistration(WrapInstanceMethod(methodInfo, _serviceProvider));

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
            var allParams = new List<ParameterExpression>();
            allParams.AddRange(callParams);

            var callExpr = Expression.Call(instance, method, callParams);
            return Expression.Lambda(callExpr, method.Name, allParams);
        }
    }
}
