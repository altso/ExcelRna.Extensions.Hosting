using System.Runtime.InteropServices;
using System.Windows.Forms;
using ExcelDna.Extensions.Hosting;
using ExcelDna.Extensions.Hosting.IntelliSense;
using ExcelDna.Integration;
using ExcelDna.Integration.CustomUI;
using ExcelDna.Registration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace QuickStart;

public class QuickStartAddIn : HostedExcelAddIn
{
    protected override IHostBuilder CreateHostBuilder() => Host.CreateDefaultBuilder()
        .ConfigureServices(services =>
        {
            services.AddTransient<IQuickStartService, QuickStartService>();

            services.AddExcelFunctionsIntelliSense();

            services.AddExcelFunctionsProcessors(functions => functions
                .ProcessParamsRegistrations()
                .ProcessAsyncRegistrations()
            );

            services.AddExcelFunctions<QuickStartFunctions>();

            services.AddExcelRibbon<QuickStartRibbon>();
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

    public QuickStartRibbon(IQuickStartService quickStartService) => _quickStartService = quickStartService;

    public override string GetCustomUI(string ribbonId)
    {
        return @"
<customUI xmlns='http://schemas.microsoft.com/office/2006/01/customui'>
  <ribbon>
    <tabs>
      <tab id='tab1' label='Quick Start'>
        <group id='group1' label='Hosting'>
          <button id='button1' label='Say Hello' tag='Ribbon' onAction='OnButtonPressed'/>
        </group >
      </tab>
    </tabs>
  </ribbon>
</customUI>
";
    }

    public void OnButtonPressed(IRibbonControl control) => MessageBox.Show(_quickStartService.SayHello(control.Tag));
}
