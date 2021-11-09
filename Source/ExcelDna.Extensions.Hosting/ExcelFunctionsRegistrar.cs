using System.Threading;
using System.Threading.Tasks;
using ExcelDna.Registration;
using Microsoft.Extensions.Hosting;

namespace ExcelDna.Extensions.Hosting
{
    internal class ExcelFunctionsRegistrar : IHostedService
    {
        private readonly IExcelFunctionsProvider _excelFunctionsProvider;
        private readonly IExcelFunctionsProcessor _excelFunctionsProcessor;

        public ExcelFunctionsRegistrar(IExcelFunctionsProvider excelFunctionsProvider, IExcelFunctionsProcessor excelFunctionsProcessor)
        {
            _excelFunctionsProvider = excelFunctionsProvider;
            _excelFunctionsProcessor = excelFunctionsProcessor;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _excelFunctionsProcessor.Process(_excelFunctionsProvider.GetExcelFunctions()).RegisterFunctions();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
