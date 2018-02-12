namespace Authentication.Local.Models
{
    using System;
    using Microsoft.Extensions.Logging;

    public class EventLog
    {
        public DateTime Date { get; set; }
        public LogLevel Level { get; set; }
        public string EventId { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
    }
}