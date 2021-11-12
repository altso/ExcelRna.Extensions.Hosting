using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace ExcelDna.Extensions.Hosting.Tests;

public class ExcelFunctionsRegistrarTests
{
    [Fact]
    public async Task StartAsync_should_register_functions()
    {
        // ARRANGE
        var excelFunctionsProvider = new Mock<IExcelFunctionsProvider>();
        bool registered = false;
        ExcelFunctionsRegistrar excelFunctionsRegistrar = new(excelFunctionsProvider.Object, new NoopExcelFunctionsProcessor()) { RegisterFunctions = _ => { registered = true; } };

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
        ExcelFunctionsRegistrar excelFunctionsRegistrar = new(excelFunctionsProvider.Object, new NoopExcelFunctionsProcessor());

        // ACT
        Task stopTask = excelFunctionsRegistrar.StopAsync(CancellationToken.None);

        // ASSERT
        Assert.Equal(Task.CompletedTask, stopTask);
    }
}
