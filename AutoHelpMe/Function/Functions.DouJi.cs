namespace AutoHelpMe.Function;

public partial class Functions
{
    public static void 斗技挂机(TaskHelper taskHelper, int maxCount, bool deathQuit)
    {
        taskHelper.Start(() =>
        {
            var keys = new List<string>() { "挑战_斗技", "挑战_斗技2", "准备", "自动上阵", "手动", "阵亡", "失败", "失败2", "赢", "赢2", "奖励", }.AddExt(BaseKeys); 

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
                                Logger.PrintChallengeCount(succ, fail);
                            }
                            break;

                        case "赢":
                        case "赢2":
                            if (!lastClick.StartsWith("赢"))
                            {
                                succ++;
                                Logger.PrintChallengeCount(succ, fail);
                            }
                            break;

                        case "阵亡":
                            if (deathQuit) DeathQuit();
                            break;
                    }
                    lastClick = name;
                    if (lastClick != "阵亡")
                    {
                        WinHelper.Click(rect);
                        WinHelper.RandomDelay();
                    }

                    break;
                }
            }
            
            void DeathQuit()
            {
                var quitKeys = new List<string>() { "退出", "确认提示", "确认提示2" };

                var tc = false;
                var qr = false;
                quitKeys.ForEach(key =>
                {
                    while (taskHelper.IsRunning && ((key == "退出" && !tc) || (key.StartsWith("确认提示") && !qr)))
                    {
                        taskHelper.WaitForPause();
                        var screen = WinHelper.CaptureWindow();
                        if (!TargetImages.TryGetValue(key, out var tuple)) continue;
                        var (mat, name) = tuple;
                        var threshold = name.StartsWith("确认提示") ? 0.85 : 0.90;
                        var rect = WinHelper.Find(screen, mat, threshold);
                        if (rect.IsEmpty) continue;
                        WinHelper.Click(rect);

                        switch (name)
                        {
                            case "退出": tc = true; break;
                            default: qr = true; break;
                        }

                        WinHelper.RandomDelay();
                    }
                });
            }
        });
    }
}