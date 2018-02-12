namespace Authentication.Local.Infrastructure.Logging
{
    using System;
    using Microsoft.Extensions.Logging;
    using Models;
    using Syrx;

    public class DatabaseLogger : ILogger
    {
        private readonly string _category;
        private readonly ICommander<DatabaseLogger> _commander;
        private readonly Func<string, bool> _filter;

        public DatabaseLogger(string category,
            ICommander<DatabaseLogger> commander, Func<string, bool> filter)
        {
            _category = category;
            _commander = commander;
            _filter = filter;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                var error = string.Format("{0} -- {1}", exception.Message, exception.StackTrace);
                var @event = new EventLog
                {
                    Date = DateTime.UtcNow,
                    EventId = eventId.Id.ToString(),
                    Level = logLevel,
                    Message = state.ToString(),
                    Exception = error,
                    Logger = _category
                };

                _commander.Execute(@event);
            }
        }

        public bool IsEnabled(LogLevel logLevel) => _filter(_category);

        public IDisposable BeginScope<TState>(TState state) => null;
    }
}