using ExcelDna.Integration.CustomUI;

namespace ExcelDna.Extensions.Hosting;

public class HostedExcelRibbon : ExcelRibbon
{
    // Ribbons with this base type are not detected as ribbons by ExcelDna, thus not instantiated by ExcelDna and not loaded automatically.
    // See IsRibbonType detection logic here: // https://github.com/Excel-DNA/ExcelDna/blob/d103b8d4bfb79ebb05ddd842cc395182336bc402/Source/ExcelDna.Integration/AssemblyLoader.cs#L325-L341.
}