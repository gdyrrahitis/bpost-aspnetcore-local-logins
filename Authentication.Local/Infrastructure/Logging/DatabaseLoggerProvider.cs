namespace Authentication.Local.Infrastructure.Logging
{
    using System;
    using Microsoft.Extensions.Logging;
    using Syrx;

    public class DatabaseLoggerProvider: ILoggerProvider
    {
        private readonly ICommander<DatabaseLogger> _commander;
        private readonly Func<string, bool> _filter;

        public DatabaseLoggerProvider(ICommander<DatabaseLogger> commander, Func<string, bool> filter)
        {
            _commander = commander;
            _filter = filter;
        }

        public ILogger CreateLogger(string categoryName) => 
            new DatabaseLogger(categoryName, _commander, _filter);

        public void Dispose()
        {
        }
    }
}