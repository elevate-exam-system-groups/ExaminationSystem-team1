namespace ExaminationSystem.Extensions.Logging
{
    public sealed class OutboxConsoleLoggerProvider : ILoggerProvider
    {
        private static readonly object ConsoleLock = new();

        public ILogger CreateLogger(string categoryName) => new OutboxConsoleLogger(categoryName);

        public void Dispose()
        {
        }

        private sealed class OutboxConsoleLogger(string categoryName) : ILogger
        {
            public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

            public bool IsEnabled(LogLevel logLevel)
            {
                if (logLevel == LogLevel.None)
                {
                    return false;
                }

                return categoryName.StartsWith("MassTransit", StringComparison.Ordinal);
            }

            public void Log<TState>(
                LogLevel logLevel,
                EventId eventId,
                TState state,
                Exception? exception,
                Func<TState, Exception?, string> formatter)
            {
                if (!IsEnabled(logLevel))
                {
                    return;
                }

                var message = formatter(state, exception);
                if (string.IsNullOrWhiteSpace(message) && exception is null)
                {
                    return;
                }

                var previousColor = Console.ForegroundColor;
                var targetColor = ResolveColor(categoryName, logLevel, message);

                lock (ConsoleLock)
                {
                    Console.ForegroundColor = targetColor;
                    Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [OUTBOX] {categoryName}: {message}");

                    if (exception is not null)
                    {
                        Console.WriteLine(exception);
                    }

                    Console.ForegroundColor = previousColor;
                }
            }

            private static ConsoleColor ResolveColor(string loggerCategory, LogLevel level, string message)
            {
                if (loggerCategory.StartsWith("MassTransit.EntityFrameworkCoreIntegration", StringComparison.Ordinal))
                {
                    return ConsoleColor.Cyan;
                }

                if (message.Contains("outbox", StringComparison.OrdinalIgnoreCase))
                {
                    return ConsoleColor.Green;
                }

                if (level >= LogLevel.Warning)
                {
                    return ConsoleColor.Yellow;
                }

                return ConsoleColor.Magenta;
            }
        }
    }
}
