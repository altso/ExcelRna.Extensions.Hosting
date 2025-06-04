using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExcelDna.Integration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace ExcelRna.Extensions.Hosting.Tests;

public class HostedExcelAddInTests
{
    [Fact]
    public void HostedExcelAddIn_should_start_and_stop_host()
    {
        // ARRANGE
        var testExcelAddIn = new TestExcelAddIn();
        IExcelAddIn addIn = testExcelAddIn;

        // ACT & ASSERT
        addIn.AutoOpen();
        Assert.True(testExcelAddIn.IsRunning);

        addIn.AutoClose();
        Assert.False(testExcelAddIn.IsRunning);
    }

    [Fact]
    public void HostedExcelAddIn_should_call_OnException()
    {
        // ARRANGE
        var invalidExcelAddIn = new InvalidExcelAddIn();
        IExcelAddIn addIn = invalidExcelAddIn;

        // ACT & ASSERT
        Assert.Throws<ApplicationException>(addIn.AutoOpen);
        Assert.Throws<ApplicationException>(addIn.AutoClose);
        Assert.Equal(2, invalidExcelAddIn.Exceptions.Count);
    }

    [Fact]
    public void HostedExcelAddIn_AutoClose_without_AutoOpen_should_not_throw()
    {
        // ARRANGE
        var testExcelAddIn = new TestExcelAddIn();
        IExcelAddIn addIn = testExcelAddIn;

        // ACT
        Exception? exception = Record.Exception(addIn.AutoClose);

        // ASSERT
        Assert.Null(exception);
        Assert.False(testExcelAddIn.IsRunning);
    }

    [Fact]
    public void HostedExcelAddIn_AutoClose_can_be_called_multiple_times()
    {
        // ARRANGE
        var testExcelAddIn = new TestExcelAddIn();
        IExcelAddIn addIn = testExcelAddIn;

        addIn.AutoOpen();
        addIn.AutoClose();

        // ACT
        Exception? exception = Record.Exception(addIn.AutoClose);

        // ASSERT
        Assert.Null(exception);
        Assert.False(testExcelAddIn.IsRunning);
    }

    private class TestExcelAddIn : HostedExcelAddIn, IHostedService
    {
        public bool IsRunning { get; private set; }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            IsRunning = true;
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            IsRunning = false;
            return Task.CompletedTask;
        }

        protected override IHostBuilder CreateHostBuilder() => Host.CreateDefaultBuilder()
            .ConfigureServices(services => services.AddHostedService(_ => this));
    }

    private class InvalidExcelAddIn : HostedExcelAddIn, IHostedService
    {
        public List<Exception> Exceptions { get; } = new();

        public Task StartAsync(CancellationToken cancellationToken) => throw new ApplicationException();

        public Task StopAsync(CancellationToken cancellationToken) => throw new ApplicationException();

        protected override IHostBuilder CreateHostBuilder() => new HostBuilder()
            .ConfigureServices(services => services.AddHostedService(_ => this));

        protected override void OnException(Exception e)
        {
            base.OnException(e);
            Exceptions.Add(e);
        }
    }
}
