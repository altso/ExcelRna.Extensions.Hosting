using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ExcelDna.Integration;
using ExcelDna.Registration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ExcelDna.Extensions.Hosting.Tests;

public class ExcelFunctionsProviderTests
{
    [Fact]
    public void WrapInstanceMethod_should_create_lambda()
    {
        // ARRANGE
        var methodInfo = typeof(TestFixture).GetMethod(nameof(TestFixture.TestMethod));
        var serviceProvider = new ServiceCollection()
            .AddTransient<TestFixture>()
            .BuildServiceProvider();

        // ACT
        LambdaExpression lambda = ExcelFunctionsProvider.WrapInstanceMethod(methodInfo, serviceProvider);

        // ASSERT
        var d = lambda.Compile();
        object s = d.DynamicInvoke(123d, 456);
        Assert.Equal("123456", s);
    }

    [Fact]
    public void GetRegistrationFunctions_should_return_ExcelFunctions()
    {
        // ARRANGE
        var serviceProvider = new ServiceCollection()
            .AddExcelFunctions<TestFunctionsA>()
            .AddExcelFunctions<TestFunctionsB>()
            .BuildServiceProvider();
        var functionsProvider = new ExcelFunctionsProvider(serviceProvider);

        // ACT
        List<ExcelFunctionRegistration> registrations = functionsProvider.GetExcelFunctions().ToList();

        // ASSERT
        Assert.Collection(registrations,
            a => Assert.Equal(nameof(TestFunctionsA.FunctionA1), a.FunctionAttribute.Name),
            a => Assert.Equal(nameof(TestFunctionsA.FunctionA2), a.FunctionAttribute.Name),
            b => Assert.Equal(nameof(TestFunctionsB.FunctionB1), b.FunctionAttribute.Name));
    }

    private class TestFixture
    {
        public string TestMethod(double d, int i) => $"{d}{i}";
    }

    private class TestFunctionsA
    {
        [ExcelFunction]
        public int FunctionA1() => default;

        [ExcelFunction]
        public string FunctionA2() => default;
    }

    private class TestFunctionsB
    {
        [ExcelFunction]
        public DateTime FunctionB1(int a, string b) => default;

        [ExcelFunction]
        public static object IgnoredFunction() => default;
    }
}