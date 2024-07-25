using Serilog;

namespace AutoHelpMe.Helpers;

public static class LogHelper
{
    /// <summary>
    /// 异步记录信息级别的日志。
    /// </summary>
    /// <param name="message">要记录的消息。</param>
    public static async Task InformationAsync(string message)
    {
        await Task.Run(() =>
        {
            Log.Information(message);
            return Task.CompletedTask;
        });
    }

    /// <summary>
    /// 异步记录调试级别的日志。
    /// </summary>
    /// <param name="message">要记录的消息。</param>
    public static async Task DebugAsync(string message)
    {
        await Task.Run(() =>
        {
            Log.Debug(message);
            return Task.CompletedTask;
        });
    }

    /// <summary>
    /// 异步记录错误级别的日志。
    /// </summary>
    /// <param name="message">要记录的消息。</param>
    public static async Task ErrorAsync(string message)
    {
        await Task.Run(() =>
        {
            Log.Error(message);
            return Task.CompletedTask;
        });
    }

    /// <summary>
    /// 异步记录警告级别的日志。
    /// </summary>
    /// <param name="message">要记录的消息。</param>
    public static async Task WarningAsync(string message)
    {
        await Task.Run(() =>
        {
            Log.Warning(message);
            return Task.CompletedTask;
        });
    }

    /// <summary>
    /// 异步记录致命错误级别的日志。
    /// </summary>
    /// <param name="message">要记录的消息。</param>
    public static async Task FatalAsync(string message)
    {
        await Task.Run(() =>
        {
            Log.Fatal(message);
            return Task.CompletedTask;
        });
    }

    /// <summary>
    /// 异步记录详细跟踪级别的日志。
    /// </summary>
    /// <param name="message">要记录的消息。</param>
    public static async Task VerboseAsync(string message)
    {
        await Task.Run(() =>
        {
            Log.Verbose(message);
            return Task.CompletedTask;
        });
    }

    /// <summary>
    /// 记录信息级别的日志。
    /// </summary>
    /// <param name="message">要记录的消息。</param>
    public static void Information(string message)
    {
        Log.Information(message);
    }

    /// <summary>
    /// 记录调试级别的日志。
    /// </summary>
    /// <param name="message">要记录的消息。</param>
    public static void Debug(string message)
    {
        Log.Debug(message);
    }

    /// <summary>
    /// 记录错误级别的日志。
    /// </summary>
    /// <param name="message">要记录的消息。</param>
    public static void Error(string message)
    {
        Log.Error(message);
    }

    /// <summary>
    /// 记录警告级别的日志。
    /// </summary>
    /// <param name="message">要记录的消息。</param>
    public static void Warning(string message)
    {
        Log.Warning(message);
    }

    /// <summary>
    /// 记录致命错误级别的日志。
    /// </summary>
    /// <param name="message">要记录的消息。</param>
    public static void Fatal(string message)
    {
        Log.Fatal(message);
    }

    /// <summary>
    /// 记录详细跟踪级别的日志。
    /// </summary>
    /// <param name="message">要记录的消息。</param>
    public static void Verbose(string message)
    {
        Log.Verbose(message);
    }
}