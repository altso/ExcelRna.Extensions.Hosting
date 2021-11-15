using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace ExcelRna.Extensions.Logging.Tests;

public class LogDisplayLoggerProviderTests
{
    [Fact]
    public void CreateLogger_creates_LogDisplayLogger()
    {
        // ARRANGE
        var options = new LogDisplayLoggerOptions();
        var optionsMonitor = Mock.Of<IOptionsMonitor<LogDisplayLoggerOptions>>(monitor => monitor.CurrentValue == options);
        var provider = new LogDisplayLoggerProvider(optionsMonitor);

        // ACT
        var logger = provider.CreateLogger("Test");

        // ASSERT
        Assert.IsType<LogDisplayLogger>(logger);
    }

    [Fact]
    public void Dispose_does_not_throw()
    {
        // ARRANGE
        var options = new LogDisplayLoggerOptions();
        var optionsMonitor = Mock.Of<IOptionsMonitor<LogDisplayLoggerOptions>>(monitor => monitor.CurrentValue == options);
        Mock.Get(optionsMonitor)
            .Setup(monitor => monitor.OnChange(It.IsAny<Action<LogDisplayLoggerOptions, string>>()))
            .Returns(Mock.Of<IDisposable>());
        var provider = new LogDisplayLoggerProvider(optionsMonitor);

        // ACT
        provider.Dispose();
    }

    [Fact]
    public void Options_changes_are_applied_to_loggers()
    {
        // ARRANGE
        var optionsMonitor = new TestOptionsMonitor<LogDisplayLoggerOptions>();
        optionsMonitor.Set(Options.DefaultName, new LogDisplayLoggerOptions { AutoShowLogDisplayThreshold = LogLevel.Error });

        var provider = new LogDisplayLoggerProvider(optionsMonitor);
        var logger = (LogDisplayLogger)provider.CreateLogger("Test");

        // ACT & ASSERT
        Assert.Equal(LogLevel.Error, logger.Options.AutoShowLogDisplayThreshold);
        optionsMonitor.Set(Options.DefaultName, new LogDisplayLoggerOptions { AutoShowLogDisplayThreshold = LogLevel.Trace });
        Assert.Equal(LogLevel.Trace, logger.Options.AutoShowLogDisplayThreshold);
    }

    private class TestOptionsMonitor<TOptions> : IOptionsMonitor<TOptions>
        where TOptions : class, new()
    {
        private readonly ConcurrentDictionary<string, TOptions> _options = new();
        private event Action<TOptions, string> OnChangeEvent;

        public TOptions Get(string name)
        {
            return _options.TryGetValue(name ?? Options.DefaultName, out var options) ? options : throw new Exception(nameof(name));
        }

        public void Set(string name, TOptions options)
        {
            _options.TryRemove(name, out _);
            _options.TryAdd(name, options);
            OnChangeEvent?.Invoke(options, name);
        }

        public IDisposable OnChange(Action<TOptions, string> listener)
        {
            var disposable = new ChangeTrackerDisposable(this, listener);
            OnChangeEvent += disposable.OnChange;
            return disposable;
        }

        public TOptions CurrentValue => Get(Options.DefaultName);

        private class ChangeTrackerDisposable : IDisposable
        {
            private readonly Action<TOptions, string> _listener;
            private readonly TestOptionsMonitor<TOptions> _monitor;

            public ChangeTrackerDisposable(TestOptionsMonitor<TOptions> monitor, Action<TOptions, string> listener)
            {
                _listener = listener;
                _monitor = monitor;
            }

            public void OnChange(TOptions options, string name) => _listener.Invoke(options, name);

            public void Dispose() => _monitor.OnChangeEvent -= OnChange;
        }
    }
}
