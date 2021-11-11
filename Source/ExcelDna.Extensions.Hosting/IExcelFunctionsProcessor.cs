using System;
using System.Collections.Generic;
using ExcelDna.Registration;

namespace ExcelDna.Extensions.Hosting;

internal interface IExcelFunctionsProcessor
{
    IEnumerable<ExcelFunctionRegistration> Process(IEnumerable<ExcelFunctionRegistration> registrations);
}

internal class NoopExcelFunctionsProcessor : IExcelFunctionsProcessor
{
    public IEnumerable<ExcelFunctionRegistration> Process(IEnumerable<ExcelFunctionRegistration> registrations) => registrations;
}

internal class ExcelFunctionsProcessor : IExcelFunctionsProcessor
{
    private readonly Func<IEnumerable<ExcelFunctionRegistration>, IEnumerable<ExcelFunctionRegistration>> _process;

    public ExcelFunctionsProcessor(Func<IEnumerable<ExcelFunctionRegistration>, IEnumerable<ExcelFunctionRegistration>> process) => _process = process;

    public IEnumerable<ExcelFunctionRegistration> Process(IEnumerable<ExcelFunctionRegistration> registrations) => _process(registrations);
}
