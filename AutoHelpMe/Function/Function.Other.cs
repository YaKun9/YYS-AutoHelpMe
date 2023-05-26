namespace AutoHelpMe.Function;

/// <summary>
/// 其他功能定义，包括：
/// 御灵 2023-2-25 00:09:40
/// </summary>
public partial class Functions
{
    public static void 御灵(TaskHelper taskHelper, int maxCount)
    {
        taskHelper.Start(() =>
        {
            var succ = 0;
            var fail = 0;
            var keys = new List<string> { "阵容_解锁", "挑战_御灵", "准备", "失败", "赢", "奖励" }.AddExt(BaseKeys);
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
                    var rect = WinHelper.Find(screen, mat);
                    if (rect.IsEmpty) continue;
                    switch (name)
                    {
                        case "失败":
                        case "失败2":
                            if (!lastClick.StartsWith("失败"))
                            {
                                fail++;
                                Logger.PrintChallengeCount(succ, fail, maxCount);
                            }

                            break;

                        case "奖励":
                        case "奖励2":
                            if (!lastClick.StartsWith("奖励"))
                            {
                                succ++;
                                Logger.PrintChallengeCount(succ, fail, maxCount);
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

    public static void 抽草纸(TaskHelper taskHelper, int maxCount)
    {
        taskHelper.Start(() =>
        {
            var succ = 1;
            var keys = new List<string> { "召唤", "普通召唤", "再次召唤" };
            while (IsRunningExt(maxCount, succ))
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
                        case "再次召唤":
                            succ++;
                            Logger.PrintChallengeCount(succ, maxCount: maxCount);
                            break;
                    }

                    WinHelper.Click(rect);
                    WinHelper.RandomDelay();
                    break;
                }
            }
        });
    }

    public static void 秘闻(TaskHelper taskHelper, int maxCount)
    {
        taskHelper.Start(() =>
        {
            var keys = new List<string>() { "挑战_秘闻", "挑战_秘闻2", "准备", "失败", "失败2", "赢", "赢2", "奖励", "秘闻通关" }.AddExt(BaseKeys);

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
                    var rect = WinHelper.Find(screen, mat);
                    if (rect.IsEmpty) continue;
                    switch (name)
                    {
                        case "失败":
                        case "失败2":
                            if (!lastClick.StartsWith("失败"))
                            {
                                fail++;
                                Logger.PrintChallengeCount(succ, fail, maxCount);
                            }
                            break;

                        case "奖励":
                        case "秘闻通关":
                            if (!lastClick.StartsWith("奖励") || lastClick != "秘闻通关")
                            {
                                succ++;
                                Logger.PrintChallengeCount(succ, fail, maxCount);
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

    public static void 关闭加成(TaskHelper taskHelper, int maxCount)
    {
        taskHelper.Start(() =>
        {
            var keys = new List<string>() { "加成_觉醒", "加成_御魂", "加成_入口1", "加成_入口2", "加成_入口3" }.AddExt(BaseKeys);
            while (taskHelper.IsRunning)
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

                    if (!name.StartsWith("加成_入口"))
                    {
                        goto second;
                    }
                    WinHelper.Click(rect);
                    WinHelper.RandomDelay();
                    break;
                }
            }
        second:
            keys = new List<string>() { "加成_开着", "加成_开着2" };

            int check = 0;
           
            while (taskHelper.IsRunning && IsRunningExt(100, check))
            {
                bool click = false;
                taskHelper.WaitForPause();
                var screen = WinHelper.CaptureWindow();
                foreach (var key in keys)
                {
                    taskHelper.WaitForPause();
                    if (!TargetImages.TryGetValue(key, out var tuple)) continue;
                    var (mat, name) = tuple;
                    var rect = WinHelper.Find(screen, mat);
                    if (rect.IsEmpty) continue;
                    click = true;
                    check = 0;
                    WinHelper.Click(rect);
                    WinHelper.RandomDelay();
                    break;
                }

                if (!click)
                {
                    check++;
                }
            }
        });
    }

    public static void Demo(TaskHelper taskHelper, int maxCount)
    {
        taskHelper.Start(() =>
        {
            var keys = new List<string>() { "探索", "准备", "失败", "失败2", "赢", "赢2", "奖励", }.AddExt(BaseKeys);

            var succ = 0;
            var fail = 0;
            var lastClick = string.Empty;
            while (taskHelper.IsRunning && IsRunningExt(0, succ))
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
                        case "奖励2":
                            if (!lastClick.StartsWith("奖励"))
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