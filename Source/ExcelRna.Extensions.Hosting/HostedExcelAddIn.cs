using System;
using System.Diagnostics;
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

    protected virtual void OnException(Exception e)
    {
        Debug.WriteLine(e);
    }

    protected abstract IHostBuilder CreateHostBuilder();

    void IExcelAddIn.AutoOpen()
    {
        try
        {
            _host = CreateHostBuilder().Build();
            _host.StartAsync().GetAwaiter().GetResult();
            AutoOpen(_host);
        }
        catch (Exception e)
        {
            OnException(e);
            throw;
        }
    }

    void IExcelAddIn.AutoClose()
    {
        try
        {
            AutoClose(_host);
            _host.StopAsync().GetAwaiter().GetResult();
            _host.Dispose();
        }
        catch (Exception e)
        {
            OnException(e);
            throw;
        }
    }
}
