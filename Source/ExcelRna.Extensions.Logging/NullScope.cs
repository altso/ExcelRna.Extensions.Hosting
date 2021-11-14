using System;

namespace ExcelRna.Extensions.Logging;

/// <summary>
/// An empty scope without any logic
/// </summary>
internal sealed class NullScope : IDisposable
{
    public static NullScope Instance { get; } = new();

    private NullScope()
    {
    }

    /// <inheritdoc />
    public void Dispose()
    {
    }
}
