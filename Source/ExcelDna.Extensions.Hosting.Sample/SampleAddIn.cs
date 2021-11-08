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
                services.AddTransient<ICustomService, CustomService>();
                services.AddExcelFunctions(functions =>
                {
                    functions.ConfigureRegistrations(registrations => registrations.ProcessAsyncRegistrations());
                    functions.AddIntelliSense();
                    functions.AddFrom<SampleFunctions>();
                });
                services.AddExcelRibbon<RibbonController>();
            });
    }
}
