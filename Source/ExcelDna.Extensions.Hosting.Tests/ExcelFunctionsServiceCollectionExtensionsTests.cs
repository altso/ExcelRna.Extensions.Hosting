using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ExcelDna.Extensions.Hosting.Tests
{
    public class ExcelFunctionsServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddExcelFunctions_should_add_functions_type()
        {
            // ARRANGE
            var services = new ServiceCollection();

            // ACT
            services.AddExcelFunctions<TestFunctionsA>();
            services.AddExcelFunctions<TestFunctionsB>();

            // ASSERT
            var provider = services.BuildServiceProvider();
            IEnumerable<IExcelFunctionsDeclaration> declarations = provider.GetRequiredService<IEnumerable<IExcelFunctionsDeclaration>>();
            Assert.Collection(declarations,
                a => Assert.Equal(typeof(TestFunctionsA), a.ExcelFunctionsType),
                b => Assert.Equal(typeof(TestFunctionsB), b.ExcelFunctionsType));
        }

        private class TestFunctionsA
        {
        }

        private class TestFunctionsB
        {
        }
    }
}
