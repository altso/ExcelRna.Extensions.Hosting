using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExcelDna.Registration;
using Moq;
using Xunit;

namespace ExcelRna.Extensions.Hosting.Tests;

public class ExcelFunctionsRegistrarTests
{
    [Fact]
    public async Task StartAsync_should_register_functions()
    {
        // ARRANGE
        var excelFunctionsProvider = new Mock<IExcelFunctionsProvider>();
        bool registered = false;
        ExcelFunctionsRegistrar excelFunctionsRegistrar =
            new(excelFunctionsProvider.Object, new[] { new TestProcessor() })
            {
                RegisterFunctions = _ => { registered = true; }
            };

        // ACT
        await excelFunctionsRegistrar.StartAsync(CancellationToken.None);

        // ASSERT
        Assert.True(registered);
    }

    [Fact]
    public void StopAsync_is_noop()
    {
        // ARRANGE
        var excelFunctionsProvider = new Mock<IExcelFunctionsProvider>();
        ExcelFunctionsRegistrar excelFunctionsRegistrar =
            new(excelFunctionsProvider.Object, new[] { new TestProcessor() });

        // ACT
        Task stopTask = excelFunctionsRegistrar.StopAsync(CancellationToken.None);

        // ASSERT
        Assert.Equal(Task.CompletedTask, stopTask);
    }

    private class TestProcessor : IExcelFunctionsProcessor
    {
        public IEnumerable<ExcelFunctionRegistration> Process(IEnumerable<ExcelFunctionRegistration> registrations) =>
            registrations;
    }
}
