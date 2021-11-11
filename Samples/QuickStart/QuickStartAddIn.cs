using ExcelDna.Extensions.Hosting;
using ExcelDna.Extensions.Hosting.IntelliSense;
using ExcelDna.Registration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace QuickStart
{
    public class QuickStartAddIn : HostedExcelAddIn
    {
        protected override IHostBuilder CreateHostBuilder() => Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddTransient<ICustomService, CustomService>();

                services.AddExcelFunctionsIntelliSense();

                services.AddExcelFunctionsProcessors(functions => functions.ProcessAsyncRegistrations().ProcessParamsRegistrations());

                services.AddExcelFunctions<FunctionsA>();
                services.AddExcelFunctions<FunctionsB>();

                services.AddExcelRibbon<RibbonA>();
                services.AddExcelRibbon<RibbonB>();
            });
    }
}
