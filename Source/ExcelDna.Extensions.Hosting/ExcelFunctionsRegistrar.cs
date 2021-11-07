using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExcelDna.Registration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ExcelDna.Extensions.Hosting
{
    public class ExcelFunctionsRegistrar : IHostedService
    {
        private readonly IExcelFunctionsProvider _excelFunctionsProvider;
        private readonly IOptions<ExcelFunctionRegistrationOptions> _options;

        public ExcelFunctionsRegistrar(IExcelFunctionsProvider excelFunctionsProvider, IOptions<ExcelFunctionRegistrationOptions> options)
        {
            _excelFunctionsProvider = excelFunctionsProvider;
            _options = options;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _options.Value.Register(_options.Value.Configure(_excelFunctionsProvider.GetExcelFunctions()));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }

    public class ExcelFunctionRegistrationOptions
    {
        public Func<IEnumerable<ExcelFunctionRegistration>, IEnumerable<ExcelFunctionRegistration>> Configure { get; set; } = x => x;

        public Action<IEnumerable<ExcelFunctionRegistration>> Register { get; set; } = x => x.RegisterFunctions();
    }
}
