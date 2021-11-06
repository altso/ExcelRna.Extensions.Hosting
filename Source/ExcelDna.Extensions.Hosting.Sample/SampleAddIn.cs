using System.Collections.Generic;
using ExcelDna.Registration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExcelDna.Extensions.Hosting.Sample
{
    public class SampleAddIn : HostedExcelAddIn
    {
        protected override IHostBuilder CreateHostBuilder() => Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddHostedService<IntelliSenseHostedService>();
                services.AddExcelFunctions<SampleFunctions>();
            });

        protected override IEnumerable<ExcelFunctionRegistration> GetExcelFunctions() => base.GetExcelFunctions()
            .ProcessAsyncRegistrations();
    }
}
