using Prism.Events;

namespace AutoHelpMe.EventBus;

/// <summary>
/// 日志打印事件
/// </summary>
public class PrintLogEvent : PubSubEvent<Tuple<string, Color>>
{
}