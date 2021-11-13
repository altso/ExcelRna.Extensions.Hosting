using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExcelDna.Registration;
using Microsoft.Extensions.Hosting;

namespace ExcelRna.Extensions.Hosting;

internal class ExcelFunctionsRegistrar : IHostedService
{
    private readonly IExcelFunctionsProvider _excelFunctionsProvider;
    private readonly IExcelFunctionsProcessor _excelFunctionsProcessor;

    public ExcelFunctionsRegistrar(IExcelFunctionsProvider excelFunctionsProvider, IExcelFunctionsProcessor excelFunctionsProcessor)
    {
        _excelFunctionsProvider = excelFunctionsProvider;
        _excelFunctionsProcessor = excelFunctionsProcessor;
    }

    internal Action<IEnumerable<ExcelFunctionRegistration>> RegisterFunctions { get; set; } = functions => functions.RegisterFunctions();

    public Task StartAsync(CancellationToken cancellationToken)
    {
        IEnumerable<ExcelFunctionRegistration> functions = _excelFunctionsProcessor
            .Process(_excelFunctionsProvider.GetExcelFunctions());
        RegisterFunctions(functions);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
