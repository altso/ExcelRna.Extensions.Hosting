using ExcelDna.Integration;
using Microsoft.Extensions.Hosting;

namespace ExcelDna.Extensions.Hosting;

public abstract class HostedExcelAddIn : IExcelAddIn
{
    private IHost _host;

    protected abstract IHostBuilder CreateHostBuilder();

    void IExcelAddIn.AutoOpen()
    {
        _host = CreateHostBuilder().Build();
        _host.StartAsync().GetAwaiter().GetResult();
    }

    void IExcelAddIn.AutoClose()
    {
        _host.StopAsync().GetAwaiter().GetResult();
        _host.Dispose();
    }
}
