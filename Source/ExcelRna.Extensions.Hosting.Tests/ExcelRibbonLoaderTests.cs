using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExcelDna.Integration;
using Moq;
using Xunit;

namespace ExcelRna.Extensions.Hosting.Tests;

public class ExcelRibbonLoaderTests
{
    [Fact]
    public async Task StartAsync_should_load_ribbons()
    {
        // ARRANGE
        var loadComAddIns = new Mock<Action<ExcelComAddIn>>(MockBehavior.Strict);
        var ribbons = new HostedExcelRibbon[] { new(), new() };
        foreach (HostedExcelRibbon ribbon in ribbons)
        {
            loadComAddIns.Setup(load => load(ribbon)).Verifiable();
        }

        ExcelRibbonLoader excelRibbonLoader = new(ribbons)
        {
            LoadComAddIn = loadComAddIns.Object
        };

        // ACT
        await excelRibbonLoader.StartAsync(CancellationToken.None);

        // ASSERT
        loadComAddIns.Verify();
    }

    [Fact]
    public void StopAsync_is_noop()
    {
        // ARRANGE
        ExcelRibbonLoader excelRibbonLoader = new(Enumerable.Empty<HostedExcelRibbon>());

        // ACT
        Task stopTask = excelRibbonLoader.StopAsync(CancellationToken.None);

        // ASSERT
        Assert.Equal(Task.CompletedTask, stopTask);
    }

    private class TestRibbonA : HostedExcelRibbon
    {
    }

    private class TestRibbonB : HostedExcelRibbon
    {
    }
}
