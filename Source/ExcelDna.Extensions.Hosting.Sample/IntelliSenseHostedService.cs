using System.Threading;
using System.Threading.Tasks;
using ExcelDna.IntelliSense;
using Microsoft.Extensions.Hosting;

namespace ExcelDna.Extensions.Hosting.Sample
{
    public class IntelliSenseHostedService : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            IntelliSenseServer.Install();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            IntelliSenseServer.Uninstall();
            return Task.CompletedTask;
        }
    }
}
