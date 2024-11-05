using System.Threading.Channels;

namespace AutoHelpMe.Helpers;

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

public static class TaskQueueHelper
{
    private static readonly Channel<(Guid id, Action task)> TaskChannel;
    private static readonly CancellationTokenSource CancellationTokenSource;
    private static readonly ConcurrentDictionary<Guid, Action> TaskDictionary;
    private static Task _consumerTask;

    /// <summary>
    /// 是否是暂停的
    /// </summary>
    /// <remarks>只能通过 <see cref="Resume()"/></remarks>
    public static bool IsPaused { get; private set; }

    /// <summary>
    /// 静态构造函数，用于初始化静态成员
    /// </summary>
    static TaskQueueHelper()
    {
        TaskChannel = Channel.CreateUnbounded<(Guid, Action)>();
        CancellationTokenSource = new CancellationTokenSource();
        TaskDictionary = new ConcurrentDictionary<Guid, Action>();
        IsPaused = false;

        StartConsumer();
    }

    /// <summary>
    /// 启动消费者任务
    /// </summary>
    private static void StartConsumer()
    {
        _consumerTask = Task.Run(async () =>
        {
            await foreach (var (id, task) in TaskChannel.Reader.ReadAllAsync(CancellationTokenSource.Token))
            {
                while (IsPaused)
                {
                    await Task.Delay(500); // 暂停时循环等待
                }

                task(); // 执行任务
                TaskDictionary.TryRemove(id, out _); // 执行后移除
            }
        });
    }

    /// <summary>
    /// 添加任务并返回任务ID
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static Guid EnqueueTask(Action task)
    {
        if (TaskChannel.Writer.TryWrite((Guid.NewGuid(), task)))
        {
            var taskId = Guid.NewGuid();
            TaskDictionary[taskId] = task;
            return taskId;
        }

        throw new InvalidOperationException("Failed to enqueue task.");
    }

    /// <summary>
    /// 异步添加任务并返回任务ID
    /// </summary>
    /// <param name="task">要执行的任务</param>
    /// <returns>任务的唯一ID</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static async Task<Guid> EnqueueTaskAsync(Action task)
    {
        var taskId = Guid.NewGuid();
        await TaskChannel.Writer.WriteAsync((taskId, task));
        TaskDictionary[taskId] = task;
        return taskId;
    }


    /// <summary>
    /// 暂停任务消费
    /// </summary>
    public static void Pause()
    {
        IsPaused = true;
    }

    /// <summary>
    /// 恢复任务消费
    /// </summary>
    public static void Resume()
    {
        IsPaused = false;
    }

    /// <summary>
    /// 清除所有任务
    /// </summary>
    public static void ClearAllTasks()
    {
        while (TaskChannel.Reader.TryRead(out _))
        {
        } // 清空 Channel

        TaskDictionary.Clear(); // 清空字典
    }

    /// <summary>
    /// 清除指定任务
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns></returns>
    public static bool RemoveTask(Guid taskId)
    {
        if (TaskDictionary.TryRemove(taskId, out var task))
        {
            var tempChannel = Channel.CreateUnbounded<(Guid, Action)>();
            while (TaskChannel.Reader.TryRead(out var item))
            {
                if (item.id != taskId)
                {
                    tempChannel.Writer.TryWrite(item); // 保留非指定任务
                }
            }

            TaskChannel.Writer.TryComplete(); // 重建队列
            return true;
        }

        return false;
    }

    /// <summary>
    /// 关闭队列并终止消费者
    /// </summary>
    public static async Task StopAsync()
    {
        TaskChannel.Writer.Complete();
        await CancellationTokenSource.CancelAsync();
        await _consumerTask;
    }
}