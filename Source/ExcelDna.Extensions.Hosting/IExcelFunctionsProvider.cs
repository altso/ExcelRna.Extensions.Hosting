using System.Collections.Generic;
using ExcelDna.Registration;

namespace ExcelDna.Extensions.Hosting
{
    public interface IExcelFunctionsProvider
    {
        IEnumerable<ExcelFunctionRegistration> GetExcelFunctions();
    }
}
