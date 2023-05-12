using Prism.Events;

namespace AutoHelpMe.EventBus;

/// <summary>
/// EventBus帮助类
/// </summary>
public class EventBusHelper
{
    private static IEventAggregator _eventAggregator;

    /// <summary>
    /// 单例 IEventAggregator
    /// </summary>
    public static IEventAggregator EventAggregator
    {
        get { return _eventAggregator ??= new EventAggregator(); }
    }
}