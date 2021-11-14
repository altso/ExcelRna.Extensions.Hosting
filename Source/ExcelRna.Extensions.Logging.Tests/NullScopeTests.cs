using Xunit;

namespace ExcelRna.Extensions.Logging.Tests;

public class NullScopeTests
{
    [Fact]
    public void Instance_is_singleton()
    {
        Assert.Same(NullScope.Instance, NullScope.Instance);
    }

    [Fact]
    public void Dispose_is_noop()
    {
        NullScope.Instance.Dispose();
        NullScope.Instance.Dispose();
    }
}
