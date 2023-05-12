using AutoHelpMe.EventBus;
using SageTools.Extension;

namespace AutoHelpMe;

public class TaskHelper
{
    private Task _task;
    private readonly CancellationTokenSource _tokenSource;
    private readonly ManualResetEvent _resetEvent;

    public TaskHelper()
    {
        _task = Task.CompletedTask;
        _tokenSource = new CancellationTokenSource();
        _resetEvent = new ManualResetEvent(true);
    }

    public string TaskName { get; set; }

    /// <summary>
    /// 线程是否已结束
    /// </summary>
    public bool IsStop => _tokenSource.Token.IsCancellationRequested;

    /// <summary>
    /// 线程是否还在运行
    /// </summary>
    public bool IsRunning => !IsStop;

    /// <summary>
    /// 创建一个对象
    /// </summary>
    /// <returns></returns>
    public static TaskHelper NewInstance()
    {
        return new TaskHelper();
    }

    /// <summary>
    /// 开始一个任务
    /// </summary>
    public void Start(Action action)
    {
        Logger.Success($"任务【{TaskName}】已启动...");
        GlobalConst.LastTask = TaskName;
        EventBusHelper.EventAggregator.GetEvent<TaskOperateEvent>().Publish(TaskOperateType.Start);
        _task = Task.Run(action).ContinueWith(task =>
        {
            var tag = GlobalConst.FinishReason.IsNotNullOrWhiteSpace() ? $"，原因：{GlobalConst.FinishReason}" : "";
            Logger.Success($"任务【{TaskName}】已结束{tag}");
            EventBusHelper.EventAggregator.GetEvent<TaskOperateEvent>().Publish(TaskOperateType.Stop);
        });
    }

    /// <summary>
    /// 结束任务
    /// </summary>
    public void Stop()
    {
        _tokenSource.Cancel();
    }

    /// <summary>
    /// 暂停任务
    /// </summary>
    public void Pause()
    {
        _resetEvent.Reset();
        EventBusHelper.EventAggregator.GetEvent<TaskOperateEvent>().Publish(TaskOperateType.Pause);
    }

    /// <summary>
    /// 恢复任务
    /// </summary>
    public void Restore()
    {
        _resetEvent.Set();
        EventBusHelper.EventAggregator.GetEvent<TaskOperateEvent>().Publish(TaskOperateType.Restore);
    }

    /// <summary>
    /// 暂停功能支持
    /// </summary>
    public void WaitForPause()
    {
        _resetEvent.WaitOne();
    }
}