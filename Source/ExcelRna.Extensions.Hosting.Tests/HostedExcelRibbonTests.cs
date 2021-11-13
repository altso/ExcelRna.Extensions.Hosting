using System;
using System.Reflection;
using ExcelDna.Integration.CustomUI;
using Xunit;

namespace ExcelRna.Extensions.Hosting.Tests;

public class HostedExcelRibbonTests
{
    private static readonly MethodInfo s_isRibbonTypeMethod = typeof(ExcelRibbon).Assembly.GetType("ExcelDna.Integration.AssemblyLoader").GetMethod("IsRibbonType", BindingFlags.Static | BindingFlags.NonPublic);

    [Fact]
    public void HostedExcelRibbon_should_not_be_detected_as_ribbon_by_ExcelDna()
    {
        Assert.False(IsRibbonType(typeof(TestExcelRibbon)));
    }

    private static bool IsRibbonType(Type type)
    {
        return (bool)s_isRibbonTypeMethod.Invoke(null, new object[] { type });
    }

    private class TestExcelRibbon : HostedExcelRibbon
    {
    }
}
