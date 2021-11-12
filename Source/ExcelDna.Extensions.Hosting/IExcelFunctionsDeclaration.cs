using System;

namespace ExcelDna.Extensions.Hosting;

internal interface IExcelFunctionsDeclaration
{
    Type ExcelFunctionsType { get; }
}

internal class ExcelFunctionsDeclaration<T> : IExcelFunctionsDeclaration
{
    public Type ExcelFunctionsType { get; } = typeof(T);
}
