using Furion.EventBus;
using Microsoft.Extensions.Logging;

namespace AutoHelpMe2.EventBus
{
    public class LogEventSource(LogLevel logLevel, string log) : IEventSource
    {
        public LogLevel LogLevel { get; } = logLevel;
        public string EventId => EventBusConst.EventIds.LOG_EVENT;
        public object Payload { get; } = log;
        public DateTime CreatedTime { get; } = DateTime.Now;
        public CancellationToken CancellationToken { get; set; }
    }
}