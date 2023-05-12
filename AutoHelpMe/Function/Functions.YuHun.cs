namespace AutoHelpMe.Function;

/// <summary>
/// 御魂
/// </summary>
public partial class Functions
{
    public static void 御魂司机(TaskHelper taskHelper, int maxCount = 0)
    {
        taskHelper.Start(() =>
        {
            var keys = new List<string>() { "阵容_解锁", "挑战_组队", "挑战_组队2", "组队_默认邀请", "组队_确认邀请", "赢", "赢2", "奖励", "失败", "失败2", "确认提示", "确认提示2" }.AddExt(BaseKeys);
            var succ = 0;
            var fail = 0;
            var lastClick = string.Empty;
            while (taskHelper.IsRunning && IsRunningExt(maxCount, succ))
            {
                taskHelper.WaitForPause();
                var screen = WinHelper.CaptureWindow();
                foreach (var key in keys)
                {
                    taskHelper.WaitForPause();
                    if (!TargetImages.TryGetValue(key, out var tuple)) continue;
                    var (mat, name) = tuple;
                    var threshold = name.Contains("确认") ? 0.85 : 0.90;
                    var rect = name.StartsWith("挑战_组队") ? WinHelper.FindWithColorTolerance(screen, mat, 0.95, 10) : WinHelper.Find(screen, mat, threshold);
                    if (rect.IsEmpty) continue;
                    switch (name)
                    {
                        case "失败":
                        case "失败2":
                            if (!lastClick.StartsWith("失败"))
                            {
                                fail++;
                                Logger.PrintChallengeCount(succ, fail);
                            }
                            break;

                        case "奖励":
                            if (lastClick != "奖励")
                            {
                                succ++;
                                Logger.PrintChallengeCount(succ, fail);
                            }
                            break;
                    }

                    lastClick = name;
                    WinHelper.Click(rect);
                    WinHelper.RandomDelay();
                    break;
                }
            }
        });
    }

    /// <summary>
    /// 打手，等待别人发起组队邀请
    /// </summary>
    public static void 御魂打手(TaskHelper taskHelper, int maxCount = 0)
    {
        taskHelper.Start(() =>
        {
            var succ = 0;
            var fail = 0;
            var lastClick = string.Empty;
            var keys = new List<string>() { "确认_组队_总是", "确认_组队", "阵容_解锁", "失败", "赢", "赢2", "奖励", "奖励2" }.AddExt(BaseKeys);
            while (taskHelper.IsRunning && IsRunningExt(maxCount, succ))
            {
                taskHelper.WaitForPause();
                var screen = WinHelper.CaptureWindow();
                foreach (var key in keys)
                {
                    taskHelper.WaitForPause();
                    if (!TargetImages.TryGetValue(key, out var tuple)) continue;
                    var (mat, name) = tuple;
                    var rect = WinHelper.Find(screen, mat);
                    if (rect.IsEmpty) continue;
                    switch (name)
                    {
                        case "失败":
                        case "失败2":
                            if (!lastClick.StartsWith("失败"))
                            {
                                fail++;
                                Logger.PrintChallengeCount(succ, fail);
                            }

                            break;

                        case "奖励":
                            if (lastClick != "奖励")
                            {
                                succ++;
                                Logger.PrintChallengeCount(succ, fail);
                            }

                            break;
                    }
                    lastClick = name;
                    WinHelper.Click(rect);
                    WinHelper.RandomDelay();
                    break;
                }
            }
        });
    }

    /// <summary>
    /// 打手，等待别人发起组队邀请
    /// </summary>
    public static void 御魂单刷(TaskHelper taskHelper, int maxCount = 0)
    {
        taskHelper.Start(() =>
        {
            var succ = 0;
            var fail = 0;
            var keys = new List<string>() { "阵容_解锁", "挑战_御魂", "失败", "赢", "奖励" }.AddExt(BaseKeys);
            var lastClick = string.Empty;
            while (taskHelper.IsRunning && IsRunningExt(maxCount, succ))
            {
                var screen = WinHelper.CaptureWindow();
                foreach (var key in keys)
                {
                    taskHelper.WaitForPause();
                    if (!TargetImages.TryGetValue(key, out var tuple)) continue;
                    var (mat, name) = tuple;
                    var rect = WinHelper.Find(screen, mat);
                    if (rect.IsEmpty) continue;

                    switch (name)
                    {
                        case "失败":
                        case "失败2":
                            if (!lastClick.StartsWith("失败"))
                            {
                                fail++;
                                Logger.PrintChallengeCount(succ, fail);
                            }

                            break;

                        case "奖励":
                            if (lastClick != "奖励")
                            {
                                succ++;
                                Logger.PrintChallengeCount(succ, fail);
                            }

                            break;
                    }
                    lastClick = name;
                    WinHelper.Click(rect);
                    WinHelper.RandomDelay();
                    break;
                }
            }
        });
    }
}