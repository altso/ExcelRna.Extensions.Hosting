using ExcelDna.Registration;
using Microsoft.Extensions.Hosting;

namespace ExcelDna.Extensions.Hosting.Sample
{
    public class SampleAddIn : HostedExcelAddIn
    {
        protected override IHostBuilder CreateHostBuilder() => Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddExcelFunctions(functions =>
                {
                    functions.ConfigureRegistrations(registrations => registrations.ProcessAsyncRegistrations());
                    functions.AddIntelliSense();
                    functions.AddFrom<SampleFunctions>();
                });
            });
    }
}
