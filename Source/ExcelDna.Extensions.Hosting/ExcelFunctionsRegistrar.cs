using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExcelDna.Registration;
using Microsoft.Extensions.Hosting;

namespace ExcelDna.Extensions.Hosting;

internal class ExcelFunctionsRegistrar : IHostedService
{
    private readonly IExcelFunctionsProvider _excelFunctionsProvider;
    private readonly IEnumerable<IExcelFunctionsProcessor> _excelFunctionsProcessors;

    public ExcelFunctionsRegistrar(IExcelFunctionsProvider excelFunctionsProvider, IEnumerable<IExcelFunctionsProcessor> excelFunctionsProcessors)
    {
        _excelFunctionsProvider = excelFunctionsProvider;
        _excelFunctionsProcessors = excelFunctionsProcessors;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _excelFunctionsProcessors
            .Aggregate(_excelFunctionsProvider.GetExcelFunctions(), (functions, processor) => processor.Process(functions))
            .RegisterFunctions();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
