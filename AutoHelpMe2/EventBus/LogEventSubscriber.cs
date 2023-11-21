using Furion.DependencyInjection;
using Furion.EventBus;
using Microsoft.Extensions.Logging;

namespace AutoHelpMe2.EventBus
{
    public class LogEventSubscriber : IEventSubscriber, ITransient
    {
        [EventSubscribe("LogEvent")]
        public async Task ExceptionHandler(EventHandlerExecutingContext context)
        {
            if (context.Source is LogEventSource source)
            {
            }
        }
    }

    public class LogEventSource(LogLevel logLevel, string log) : IEventSource
    {
        public LogLevel LogLevel { get; } = logLevel;
        public string EventId => "LogEvent";
        public object Payload { get; } = log;
        public DateTime CreatedTime { get; } = DateTime.Now;
        public CancellationToken CancellationToken { get; set; }
    }
}