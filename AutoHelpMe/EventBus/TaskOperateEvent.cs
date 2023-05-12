using Prism.Events;

namespace AutoHelpMe.EventBus;

/// <summary>
/// 线程操作事件
/// </summary>
public class TaskOperateEvent : PubSubEvent<TaskOperateType>
{
}

public enum TaskOperateType
{
    Start = 1,
    Stop = 2,
    Pause = 3,
    Restore = 4,
    NoAuth=5,
}