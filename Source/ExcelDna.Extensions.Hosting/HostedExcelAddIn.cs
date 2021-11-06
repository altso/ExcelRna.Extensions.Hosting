using System.Collections.Generic;
using ExcelDna.Integration;
using ExcelDna.Registration;
using Microsoft.Extensions.Hosting;

namespace ExcelDna.Extensions.Hosting
{
    public abstract class HostedExcelAddIn : IExcelAddIn
    {
        private IHost _host;

        protected abstract IHostBuilder CreateHostBuilder();

        protected virtual IEnumerable<ExcelFunctionRegistration> GetExcelFunctions() => ServiceProviderRegistration.GetExcelFunctions(_host.Services);

        internal virtual void RegisterFunctions(IEnumerable<ExcelFunctionRegistration> registrationEntries) => registrationEntries.RegisterFunctions();

        void IExcelAddIn.AutoOpen()
        {
            _host = CreateHostBuilder().Build();
            RegisterFunctions(GetExcelFunctions());
            _host.StartAsync().GetAwaiter().GetResult();
        }

        void IExcelAddIn.AutoClose()
        {
            _host.StopAsync().GetAwaiter().GetResult();
            _host.Dispose();
        }
    }
}
