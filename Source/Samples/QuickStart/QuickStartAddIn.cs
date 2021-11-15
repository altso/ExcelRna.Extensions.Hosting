using System.Runtime.InteropServices;
using ExcelDna.Integration;
using ExcelDna.Integration.CustomUI;
using ExcelDna.IntelliSense;
using ExcelDna.Logging;
using ExcelDna.Registration;
using ExcelRna.Extensions.Hosting;
using ExcelRna.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace QuickStart;

public class QuickStartAddIn : HostedExcelAddIn
{
    protected override void AutoOpen(IHost host) => IntelliSenseServer.Install();

    protected override void AutoClose(IHost host) => IntelliSenseServer.Uninstall();

    protected override IHostBuilder CreateHostBuilder() => Host.CreateDefaultBuilder()
        .ConfigureLogging(logging => { logging.AddLogDisplay(); })
        .ConfigureServices(services =>
        {
            services.AddTransient<IQuickStartService, QuickStartService>();

            services.AddExcelFunctionsProcessor(functions => functions
                .ProcessParamsRegistrations()
                .ProcessAsyncRegistrations()
            );

            services.AddExcelRibbon<QuickStartRibbon>();

            services.AddExcelFunctions<QuickStartFunctions>();
        });
}

public interface IQuickStartService
{
    string SayHello(string name);
}

public class QuickStartService : IQuickStartService
{
    public string SayHello(string name) => $"Hello {name}!";
}

public class QuickStartFunctions
{
    private readonly IQuickStartService _quickStartService;

    public QuickStartFunctions(IQuickStartService quickStartService) => _quickStartService = quickStartService;

    [ExcelFunction(Description = "Say hello to somebody")]
    public string SayHello([ExcelArgument(Name = "Name", Description = "The name to say hello to")] string name) => _quickStartService.SayHello(name);
}

[ComVisible(true)]
public class QuickStartRibbon : HostedExcelRibbon
{
    private readonly IQuickStartService _quickStartService;
    private readonly ILogger<QuickStartRibbon> _logger;

    public QuickStartRibbon(IQuickStartService quickStartService, ILogger<QuickStartRibbon> logger)
    {
        _quickStartService = quickStartService;
        _logger = logger;
    }

    public override string GetCustomUI(string ribbonId)
    {
        return @"
<customUI xmlns='http://schemas.microsoft.com/office/2006/01/customui'>
  <ribbon>
    <tabs>
      <tab id='tab1' label='Quick Start'>
        <group id='group1' label='Hosting'>
          <button id='button1' label='Say Hello' tag='Ribbon' onAction='OnButtonPressed'/>
          <button id='button2' label='Show Log Display' onAction='OnLogDisplay'/>
        </group >
      </tab>
    </tabs>
  </ribbon>
</customUI>
";
    }

    public void OnButtonPressed(IRibbonControl control) => _logger.LogInformation(_quickStartService.SayHello(control.Tag));

    public void OnLogDisplay(IRibbonControl control) => LogDisplay.Show();
}
