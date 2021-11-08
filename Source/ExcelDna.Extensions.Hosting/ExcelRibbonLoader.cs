using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExcelDna.Integration;
using Microsoft.Extensions.Hosting;

namespace ExcelDna.Extensions.Hosting
{
    public class ExcelRibbonLoader : IHostedService
    {
        private readonly IEnumerable<HostedExcelRibbon> _excelRibbons;

        public ExcelRibbonLoader(IEnumerable<HostedExcelRibbon> excelRibbons)
        {
            _excelRibbons = excelRibbons;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (HostedExcelRibbon excelRibbon in _excelRibbons)
            {
                ExcelComAddInHelper.LoadComAddIn(excelRibbon);
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
