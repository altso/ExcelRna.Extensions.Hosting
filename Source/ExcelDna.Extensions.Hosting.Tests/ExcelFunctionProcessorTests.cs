using System.Collections.Generic;
using System.Linq;
using ExcelDna.Registration;
using Xunit;

namespace ExcelDna.Extensions.Hosting.Tests;

public class ExcelFunctionProcessorTests
{
    [Fact]
    public void Process_should_be_delegated()
    {
        // ARRANGE
        IEnumerable<ExcelFunctionRegistration> TestProcess(IEnumerable<ExcelFunctionRegistration> functions)
        {
            return functions.Reverse();
        }

        ExcelFunctionsProcessor excelFunctionsProcessor = new(TestProcess);
        var functions = new ExcelFunctionRegistration[] { new(typeof(TestFunctions).GetMethod(nameof(TestFunctions.Function1))), new(typeof(TestFunctions).GetMethod(nameof(TestFunctions.Function2))) };

        // ACT
        IEnumerable<ExcelFunctionRegistration> processedFunctions = excelFunctionsProcessor.Process(functions);

        // ASSERT
        Assert.Equal(processedFunctions.Reverse(), functions);
    }

    private class TestFunctions
    {
        public static int Function1() => default;

        public static string Function2() => default;
    }
}
