using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExcelDna.Integration;
using ExcelDna.Registration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace ExcelDna.Extensions.Hosting.Tests
{
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

            internal override void RegisterFunctions(IEnumerable<ExcelFunctionRegistration> registrationEntries)
            {
                // noop as real implementation requires Excel
            }
        }
    }
}
