using System.Threading.Channels;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AutoHelpMe.Helpers
{
    public static class TaskQueueHelper
    {
        #region Fields

        private static Channel<(Guid id, Action task)> _taskChannel;
        private static CancellationTokenSource _cancellationTokenSource;
        private static ConcurrentDictionary<Guid, Action> _taskDictionary;
        private static Task _consumerTask;

        #endregion

        #region Properties

        /// <summary>
        /// 是否是暂停的
        /// </summary>
        /// <remarks>只能通过 <see cref="Resume()"/></remarks>
        public static bool IsPaused { get; private set; }

        #endregion

        #region Ctor & Initialization

        /// <summary>
        /// 静态构造函数，用于初始化静态成员
        /// </summary>
        static TaskQueueHelper()
        {
            Initialize();
        }

        /// <summary>
        /// 初始化方法，创建通道、取消令牌和任务字典，并启动消费者任务
        /// </summary>
        private static void Initialize()
        {
            _taskChannel = Channel.CreateUnbounded<(Guid, Action)>();
            _cancellationTokenSource = new CancellationTokenSource();
            _taskDictionary = new ConcurrentDictionary<Guid, Action>();
            IsPaused = false;

            StartConsumer();
        }

        #endregion

        #region Async Methods

        /// <summary>
        /// 异步等待暂停状态解除，每隔指定时间检查一次
        /// </summary>
        /// <param name="ms">等待时间（毫秒）</param>
        public static async void WaitIfPausedAsync(int ms = 500)
        {
            while (IsPaused)
            {
                await Task.Delay(ms);
            }
        }

        /// <summary>
        /// 异步添加任务到队列并返回任务ID
        /// </summary>
        /// <param name="task">要执行的任务</param>
        /// <returns>任务的唯一ID</returns>
        public static async Task<Guid> EnqueueTaskAsync(Action task)
        {
            var taskId = Guid.NewGuid();
            await _taskChannel.Writer.WriteAsync((taskId, task));
            _taskDictionary[taskId] = task;
            return taskId;
        }

        /// <summary>
        /// 停止队列消费，终止通道和消费者任务
        /// </summary>
        public static async Task StopAsync()
        {
            _taskChannel.Writer.Complete(); // 完成通道写入
            await _cancellationTokenSource.CancelAsync(); // 取消消费者
            _consumerTask?.Dispose(); // 等待消费者结束
        }

        #endregion

        #region Methods

        /// <summary>
        /// 同步等待暂停状态解除
        /// </summary>
        /// <param name="ms">等待时间（毫秒）</param>
        public static void WaitIfPaused(int ms = 500)
        {
            while (IsPaused)
            {
                Task.Delay(ms).Wait();
            }
        }

        /// <summary>
        /// 启动消费者任务，持续从通道读取任务并执行
        /// </summary>
        private static void StartConsumer()
        {
            _consumerTask = Task.Run(async () =>
            {
                await foreach (var (id, task) in _taskChannel.Reader.ReadAllAsync(_cancellationTokenSource.Token))
                {
                    WaitIfPaused();
                    task(); // 执行任务
                    _taskDictionary.TryRemove(id, out _); // 执行后移除任务
                }
            });
        }

        /// <summary>
        /// 同步添加任务到队列并返回任务ID
        /// </summary>
        /// <param name="task">要执行的任务</param>
        /// <returns>任务的唯一ID</returns>
        public static Guid EnqueueTask(Action task)
        {
            var taskId = Guid.NewGuid();
            if (_taskChannel.Writer.TryWrite((taskId, task)))
            {
                _taskDictionary[taskId] = task;
                return taskId;
            }

            throw new InvalidOperationException("Failed to enqueue task.");
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
        /// 清除所有任务，清空通道和字典
        /// </summary>
        public static void ClearAllTasks()
        {
            while (_taskChannel.Reader.TryRead(out _))
            {
            } // 清空 Channel

            _taskDictionary.Clear(); // 清空字典
        }

        /// <summary>
        /// 从队列中移除指定ID的任务
        /// </summary>
        /// <param name="taskId">要移除的任务ID</param>
        /// <returns>是否移除成功</returns>
        public static bool RemoveTask(Guid taskId)
        {
            if (_taskDictionary.TryRemove(taskId, out var task))
            {
                var tempChannel = Channel.CreateUnbounded<(Guid, Action)>();
                while (_taskChannel.Reader.TryRead(out var item))
                {
                    if (item.id != taskId)
                    {
                        tempChannel.Writer.TryWrite(item); // 保留非指定任务
                    }
                }

                _taskChannel.Writer.TryComplete(); // 完成当前通道
                _taskChannel = tempChannel; // 替换为新通道
                return true;
            }

            return false;
        }

        /// <summary>
        /// 重置队列以允许重新启动，重新初始化通道和消费者
        /// </summary>
        public static void Reset()
        {
            StopAsync().GetAwaiter().GetResult(); // 等待停止完成
            Initialize(); // 重新初始化资源
        }

        /// <summary>
        /// 停止队列消费，终止通道和消费者任务
        /// </summary>
        public static void Stop()
        {
            _taskChannel.Writer.Complete(); // 完成通道写入
            _cancellationTokenSource.Cancel(); // 取消消费者
            _consumerTask?.Dispose(); // 等待消费者结束
        }

        #endregion
    }
}