namespace AutoHelpMe.Function;

/// <summary>
/// 活动
/// </summary>
public partial class Functions
{
    public static void 活动(TaskHelper taskHelper, int maxCount = 50)
    {
        taskHelper.Start(() =>
        {
            var succ = 0;
            var fail = 0;
            var lastClick = string.Empty;
            var keys = new List<string>() { "阵容_解锁", "挑战_鬼童丸", "挑战_言灵", "准备", "失败", "赢", "奖励" };
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