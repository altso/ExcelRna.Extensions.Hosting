using ExcelDna.Integration;
using Microsoft.Extensions.Hosting;

namespace ExcelRna.Extensions.Hosting;

public abstract class HostedExcelAddIn : IExcelAddIn
{
    private IHost _host;

    protected virtual void AutoOpen(IHost host)
    {
    }

    protected virtual void AutoClose(IHost host)
    {
    }

    protected abstract IHostBuilder CreateHostBuilder();

    void IExcelAddIn.AutoOpen()
    {
        _host = CreateHostBuilder().Build();
        _host.StartAsync().GetAwaiter().GetResult();
        AutoOpen(_host);
    }

    void IExcelAddIn.AutoClose()
    {
        AutoClose(_host);
        _host.StopAsync().GetAwaiter().GetResult();
        _host.Dispose();
    }
}
