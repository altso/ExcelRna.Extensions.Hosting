using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ExcelDna.Registration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace ExcelRna.Extensions.Hosting.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddExcelFunctions_should_add_functions_type()
    {
        // ARRANGE
        var services = new ServiceCollection();

        // ACT
        services.AddExcelFunctions<TestFunctionsA>();
        services.AddExcelFunctions<TestFunctionsB>();

        // ASSERT
        var provider = services.BuildServiceProvider();
        IEnumerable<IExcelFunctionsDeclaration> declarations = provider.GetRequiredService<IEnumerable<IExcelFunctionsDeclaration>>();
        Assert.Collection(declarations,
            a => Assert.Equal(typeof(TestFunctionsA), a.ExcelFunctionsType),
            b => Assert.Equal(typeof(TestFunctionsB), b.ExcelFunctionsType));
    }

    [Fact]
    public void AddExcelRibbon_should_add_loader()
    {
        // ARRANGE
        var services = new ServiceCollection();

        // ACT
        services.AddExcelRibbon<TestRibbonA>();
        services.AddExcelRibbon<TestRibbonB>();

        // ASSERT
        ServiceProvider serviceProvider = services.BuildServiceProvider();
        Assert.Single(serviceProvider.GetRequiredService<IEnumerable<IHostedService>>().OfType<ExcelRibbonLoader>());
        Assert.Collection(serviceProvider.GetRequiredService<IEnumerable<HostedExcelRibbon>>(),
            a => Assert.IsType<TestRibbonA>(a),
            b => Assert.IsType<TestRibbonB>(b));
    }

    [Fact]
    public void AddExcelFunctionsProcessor_should_add_processor()
    {
        // ARRANGE
        var services = new ServiceCollection();
        services.AddTransient<TestDependency>();

        // ACT
        services.AddExcelFunctionsProcessor(functions => functions);
        services.AddExcelFunctionsProcessor((functions, provider) => functions.Concat(new[] { provider.GetRequiredService<TestDependency>().Registration }));

        // ASSERT
        ServiceProvider serviceProvider = services.BuildServiceProvider();
        IExcelFunctionsProcessor processor = serviceProvider.GetRequiredService<IExcelFunctionsProcessor>();
        Assert.Single(processor.Process(Enumerable.Empty<ExcelFunctionRegistration>()));
    }

    private class TestFunctionsA
    {
    }

    private class TestFunctionsB
    {
    }

    private class TestRibbonA : HostedExcelRibbon
    {
    }

    private class TestRibbonB : HostedExcelRibbon
    {
    }

    private class TestDependency
    {
        public ExcelFunctionRegistration Registration { get; } = new(Expression.Lambda(Expression.Constant(null)));
    }
}
