using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExcelDna.Integration;
using Microsoft.Extensions.Hosting;

namespace ExcelRna.Extensions.Hosting;

internal class ExcelRibbonLoader : IHostedService
{
    private readonly IEnumerable<HostedExcelRibbon> _excelRibbons;

    public ExcelRibbonLoader(IEnumerable<HostedExcelRibbon> excelRibbons)
    {
        _excelRibbons = excelRibbons;
    }

    internal Action<ExcelComAddIn> LoadComAddIn { get; set; } = ExcelComAddInHelper.LoadComAddIn;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (HostedExcelRibbon excelRibbon in _excelRibbons)
        {
            LoadComAddIn(excelRibbon);
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
