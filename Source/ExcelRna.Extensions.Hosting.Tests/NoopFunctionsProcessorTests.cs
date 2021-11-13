using System.Collections.Generic;
using ExcelDna.Registration;
using Xunit;

namespace ExcelRna.Extensions.Hosting.Tests;

public class NoopFunctionsProcessorTests
{
    [Fact]
    public void Process_should_return_the_same_object()
    {
        // ARRANGE
        NoopExcelFunctionsProcessor noopExcelFunctionsProcessor = new();
        var functions = new ExcelFunctionRegistration[42];

        // ACT
        IEnumerable<ExcelFunctionRegistration> processedFunctions = noopExcelFunctionsProcessor.Process(functions);

        // ASSERT
        Assert.Same(processedFunctions, functions);
    }
}
