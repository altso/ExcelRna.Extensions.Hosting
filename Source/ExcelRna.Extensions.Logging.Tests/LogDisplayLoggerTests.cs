using System;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ExcelRna.Extensions.Logging.Tests;

public class LogDisplayLoggerTests
{
    [Fact]
    public void BeginScope_returns_not_null()
    {
        // ARRANGE
        var logger = new LogDisplayLogger("Test");

        // ACT
        IDisposable scope = logger.BeginScope("Scope");

        // ASSERT
        Assert.NotNull(scope);
    }

    [Fact]
    public void IsEnabled_returns_correct_value()
    {
        // ARRANGE
        var logger = new LogDisplayLogger("Test");

        // ACT & ASSERT
        Assert.True(logger.IsEnabled(LogLevel.Trace));
        Assert.True(logger.IsEnabled(LogLevel.Debug));
        Assert.True(logger.IsEnabled(LogLevel.Information));
        Assert.True(logger.IsEnabled(LogLevel.Warning));
        Assert.True(logger.IsEnabled(LogLevel.Error));
        Assert.True(logger.IsEnabled(LogLevel.Critical));
        Assert.False(logger.IsEnabled(LogLevel.None));
    }

    [Fact]
    public void Log_is_noop_when_not_enabled()
    {
        // ARRANGE
        var logger = new LogDisplayLogger("Test")
        {
            RecordLine = Mock.Of<Action<string, object[]>>(MockBehavior.Strict),
        };

        // ACT
        logger.Log(LogLevel.None, "TestMessage");
    }

    [Fact]
    public void Log_is_noop_when_message_is_empty()
    {
        // ARRANGE
        var logger = new LogDisplayLogger("Test")
        {
            RecordLine = Mock.Of<Action<string, object[]>>(MockBehavior.Strict),
        };

        // ACT
        logger.Log(LogLevel.Information, "");
    }

    [Fact]
    public void Log_throws_when_formatter_is_null()
    {
        // ARRANGE
        var logger = new LogDisplayLogger("Test")
        {
            RecordLine = Mock.Of<Action<string, object[]>>(MockBehavior.Strict),
        };

        // ACT & ASSERT
        Assert.Throws<ArgumentNullException>(() => logger.Log(LogLevel.Information, 0, "State", null, null!));
    }

    [Fact]
    public void Log_includes_exception()
    {
        // ARRANGE
        var logger = new LogDisplayLogger("Test", new LogDisplayLoggerOptions
        {
            TimestampFormat = "G",
        })
        {
            RecordLine = Mock.Of<Action<string, object[]>>(),
        };

        // ACT
        logger.Log(LogLevel.Debug, new Exception("TestException"), "TestMessage");

        // ASSERT
        Mock.Get(logger.RecordLine).Verify(invoke => invoke(
            It.Is<string>(s => s.Contains("TestMessage") && s.Contains("TestException")),
            It.Is<object[]>(p => p.Length == 0)), Times.Once);
    }

    [Fact]
    public void Log_includes_timestamp()
    {
        // ARRANGE
        var logger = new LogDisplayLogger("Test", new LogDisplayLoggerOptions
        {
            TimestampFormat = "yyyy-MM-dd HH:mm:ss ",
        })
        {
            RecordLine = Mock.Of<Action<string, object[]>>(),
            GetCurrentTimestamp = () => new DateTimeOffset(2023, 04, 17, 13, 34, 00, TimeSpan.Zero),
        };

        // ACT
        logger.Log(LogLevel.Trace, new Exception("TestException"), "TestMessage");

        // ASSERT
        Mock.Get(logger.RecordLine).Verify(invoke => invoke(
            It.Is<string>(s => s.Contains("2023-04-17 13:34:00")),
            It.Is<object[]>(p => p.Length == 0)), Times.Once);
    }


    [Theory]
    [InlineData(LogLevel.Error)]
    [InlineData(LogLevel.Critical)]
    public void Log_auto_shows_LogDisplay(LogLevel level)
    {
        // ARRANGE
        var logger = new LogDisplayLogger("Test")
        {
            RecordLine = Mock.Of<Action<string, object[]>>(),
            Show = Mock.Of<Action>(),
        };

        // ACT
        logger.Log(level, new Exception("TestException"), "TestMessage");

        // ASSERT
        Mock.Get(logger.Show).Verify(invoke => invoke(), Times.Once);
    }

    [Theory]
    [InlineData(LogLevel.Trace)]
    [InlineData(LogLevel.Debug)]
    [InlineData(LogLevel.Information)]
    [InlineData(LogLevel.Warning)]
    public void Log_does_not_show_LogDisplay(LogLevel level)
    {
        // ARRANGE
        var logger = new LogDisplayLogger("Test")
        {
            RecordLine = Mock.Of<Action<string, object[]>>(),
            Show = Mock.Of<Action>(),
        };

        // ACT
        logger.Log(level, new Exception("TestException"), "TestMessage");

        // ASSERT
        Mock.Get(logger.Show).Verify(invoke => invoke(), Times.Never);
    }

    [Fact]
    public void Log_throws_when_logLevel_is_wrong()
    {
        // ARRANGE
        var logger = new LogDisplayLogger("Test");

        // ACT & ASSERT
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            logger.Log((LogLevel)100, new Exception("TestException"), "TestMessage");
        });
    }
}
