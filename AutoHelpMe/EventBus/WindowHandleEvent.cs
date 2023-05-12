using Prism.Events;

namespace AutoHelpMe.EventBus;

/// <summary>
/// 句柄获取事件
/// </summary>
public class WindowHandleEvent : PubSubEvent<IntPtr>
{
   
}