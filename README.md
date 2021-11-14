# ExcelRna.Extensions.Hosting

[![License](https://img.shields.io/github/license/altso/ExcelRna.Extensions.Hosting?style=flat-square)](https://github.com/altso/ExcelRna.Extensions.Hosting/blob/main/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/ExcelRna.Extensions.Hosting?style=flat-square)](https://www.nuget.org/packages/ExcelRna.Extensions.Hosting/)
[![Build](https://img.shields.io/github/workflow/status/altso/ExcelRna.Extensions.Hosting/Build?style=flat-square)](https://github.com/altso/ExcelRna.Extensions.Hosting/actions/workflows/dotnet.yml)
[![codecov](https://img.shields.io/codecov/c/github/altso/ExcelRna.Extensions.Hosting?style=flat-square&token=CHWDPNBY06)](https://codecov.io/gh/altso/ExcelRna.Extensions.Hosting)

Take advantage of [.NET Generic Host](https://docs.microsoft.com/en-us/dotnet/core/extensions/generic-host) in your [Excel-DNA](https://github.com/Excel-DNA/ExcelDna) add-in.


## Features

This package integrates [`Microsoft.Extensions.*`](https://www.nuget.org/packages?q=Microsoft.Extensions) libraries (dependency injection, configuration, logging, etc.) with [`ExcelDna.AddIn`](https://www.nuget.org/packages/ExcelDna.AddIn/).

```cs
public class QuickStartAddIn : HostedExcelAddIn
{
    protected override IHostBuilder CreateHostBuilder() => Host.CreateDefaultBuilder()
        .ConfigureServices(services =>
        {
            services.AddTransient<IQuickStartService, QuickStartService>();
            services.AddExcelRibbon<QuickStartRibbon>();
            services.AddExcelFunctions<QuickStartFunctions>();
        });
}
```

## Installation

Install [ExcelRna.Extensions.Hosting](https://www.nuget.org/packages/ExcelRna.Extensions.Hosting/) package from nuget.org.

```bash
dotnet add package ExcelRna.Extensions.Hosting
```


## Usage/Examples

#### Check settings in the .dna file

Make sure `ExplicitRegistration` is set to `true` in the .dna file. See [Method Registration](https://github.com/Excel-DNA/ExcelDna/wiki/Method-Registration#explicitregistration-option) for more details. For example:

```xml
<?xml version="1.0" encoding="utf-8"?>
<DnaLibrary Name="QuickStart" RuntimeVersion="v4.0"
            xmlns="http://schemas.excel-dna.net/addin/2018/05/dnalibrary">
  <ExternalLibrary Path="QuickStart.dll" ExplicitRegistration="true" />
</DnaLibrary>
```

#### Define excel functions

[`ExcelFunction`](https://github.com/Excel-DNA/ExcelDna/wiki/ExcelFunction-and-other-attributes) attributes must be declared on **instance** methods, *not* the static ones.
The dependencies are resolved by DI container.

```cs
public class QuickStartFunctions
{
    private readonly IQuickStartService _quickStartService;

    public QuickStartFunctions(IQuickStartService quickStartService)
    {
        _quickStartService = quickStartService;
    }

    [ExcelFunction(Description = "Say hello to somebody")]
    public string SayHello(string name)
    {
        _quickStartService.SayHello(name);
    }
}
```

#### Define excel ribbons

Ribbon classes must be derived from `HostedExcelRibbon` base class.
The presence of this base class stops `ExcelDna` from automatically discovering the ribbon and attempting to instantiate it using the default parameterless constructor.
`HostedExcelAddIn` registers ribbons in Excel at a later stage (`AutoOpen`) after DI container is created.

```cs
[ComVisible(true)]
public class QuickStartRibbon : HostedExcelRibbon
{
    private readonly IQuickStartService _quickStartService;

    public QuickStartRibbon(IQuickStartService quickStartService)
    {
        _quickStartService = quickStartService;
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
                        </group >
                    </tab>
                </tabs>
            </ribbon>
        </customUI>
        ";
    }

    public void OnButtonPressed(IRibbonControl control)
    {
        MessageBox.Show(_quickStartService.SayHello(control.Tag));
    }
}
```

#### Create and configure your add-in

Derive a class from `HostedExcelAddIn` and implement `CreateHostBuilder` abstract method.
Register your services, `ExcelFunction`s container classes, and `ExcelRibbon`s using `ConfigureServices` method.
Here we use `Host.CreateDefaultBuilder()` from `Microsfot.Extensions.Hosting` (must be installed separately).

```cs
public class QuickStartAddIn : HostedExcelAddIn
{
    protected override IHostBuilder CreateHostBuilder() => Host.CreateDefaultBuilder()
        .ConfigureServices(services =>
        {
            services.AddTransient<IQuickStartService, QuickStartService>();
            services.AddExcelRibbon<QuickStartRibbon>();
            services.AddExcelFunctions<QuickStartFunctions>();
        });
}
```


See complete example in [Source/Samples](https://github.com/altso/ExcelRna.Extensions.Hosting/tree/main/Source/Samples).


## FAQ

#### What is the lifetime for `ExcelFunction` container classes and `ExcelRibbon`s?

Both functions and ribbons are registered as singletons. There are no DI scopes as it's hard to define a scope for an Excel add-in.

#### `Microsoft.Extensions.Hosting` has tons of dependencies. How do I reference them all in the .dna file?

You can use msbuild target to automatically register all dependencies. See [discussion](https://github.com/Excel-DNA/ExcelDna/issues/359) or [`QuickStart.csproj`](https://github.com/altso/ExcelRna.Extensions.Hosting/blob/main/Source/Samples/QuickStart/QuickStart.csproj).


## License

[MIT](https://github.com/altso/ExcelRna.Extensions.Hosting/blob/main/LICENSE)

