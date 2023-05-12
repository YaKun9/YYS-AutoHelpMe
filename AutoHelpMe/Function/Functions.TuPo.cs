namespace AutoHelpMe.Function;

/// <summary>
/// 突破
/// </summary>
public partial class Functions
{
    /// <summary>
    /// 个人突破
    /// </summary>
    /// <param name="taskHelper"></param>
    /// <param name="quitCount">退出次数</param>
    /// <param name="isNeedGreenFlag">是否绿标</param>
    /// <param name="isNeedRedFlag">是否红标</param>
    public static void 个人突破(TaskHelper taskHelper, int quitCount = 4, bool isNeedGreenFlag = false, bool isNeedRedFlag = false)
    {
        taskHelper.Start(() =>
        {
            var keys = new List<string>()
            {
                "十勾玉", "突破攻破0", "突破攻破02", "准备", "结界狗", "结界狗2", "结界战", "自动1", "自动2", "自动3", "探索", "突破",
                "个人突破",
                "阵容_锁定", "突破票0",
                "突破进攻", "突破进攻2", "突破边框", "突破边框2", "突破边框呱", "突破边框呱2", "突破失败",
                "彼岸花血条", "彼岸花血条2","彼岸花血条3", "失败", "失败2", "赢", "赢2", "奖励",
            }.AddExt(BaseKeys);

            var quitKeys = new List<string>()
            {
               "探索", "突破", "个人突破",
                "确认提示", "确认提示2", "失败", "失败2", "突破进攻", "突破进攻2", "退出",
                "阵容_锁定", "突破票0",  "突破失败", "突破边框", "突破边框2", "突破边框呱", "突破边框呱2",
            }.AddExt(BaseKeys);

            var checkKeys = new List<string>() { "探索", "突破", "个人突破", "结界_已攻破" };

        newRound: //一轮刷完
            var succ = 0;
            var fail = 0;
            var lastClick = string.Empty;

            #region 检查是否有已攻破,如果有跳过退出

          
            int retry = 0;
            while (taskHelper.IsRunning && retry<=10)
            {
                taskHelper.WaitForPause();
                var screen = WinHelper.CaptureWindow();
                foreach (var key in checkKeys)
                {
                    taskHelper.WaitForPause();
                    if (!TargetImages.TryGetValue(key, out var tuple)) continue;
                    var (mat, name) = tuple;
                    var rect = WinHelper.Find(screen, mat,0.9);
                    if (rect.IsEmpty) continue;
                    if (name.StartsWith("结界_已攻破"))
                    {
                        Logger.Success("继续进度,跳过退出");
                        goto clean;
                    }
                    WinHelper.Click(rect);
                    WinHelper.RandomDelay();
                    break;
                }
                retry++;
            }

            #endregion 检查是否有已攻破,如果有跳过退出

            #region 退 N 次保星

            while (taskHelper.IsRunning && quitCount > fail)
            {
                taskHelper.WaitForPause();
                var screen = WinHelper.CaptureWindow();
                foreach (var key in quitKeys)
                {
                    taskHelper.WaitForPause();
                    if (!TargetImages.TryGetValue(key, out var tuple)) continue;
                    var (mat, name) = tuple;
                    var threshold = name.StartsWith("突破边框") || name.StartsWith("确认提示") || name.StartsWith("突破进攻")
                        ? 0.85
                        : 0.95;
                    if (name.StartsWith("突破票0"))
                    {
                        threshold = 0.98;
                    }
                    var rect = WinHelper.Find(screen, mat, threshold);
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

                        case "突破票0":
                            Logger.Success("突破票已用完，突破结束");
                            return;
                    }

                    WinHelper.Click(rect);
                    WinHelper.RandomDelay();
                    break;
                }
            }

            #endregion 退 N 次保星

            #region 全清

            clean:
            while (taskHelper.IsRunning && IsRunningExt(0, succ))
            {
                taskHelper.WaitForPause();
                var screen = WinHelper.CaptureWindow();
                foreach (var key in keys)
                {
                    taskHelper.WaitForPause();
                    if (!TargetImages.TryGetValue(key, out var tuple)) continue;
                    var (mat, name) = tuple;
                    var threshold = name.StartsWith("突破边框") || name.StartsWith("突破进攻") ? 0.85 : 0.95;
                    if (name.StartsWith("突破票0") || name.StartsWith("突破攻破0"))
                    {
                        threshold = 0.98;
                    }
                    var rect = WinHelper.Find(screen, mat, threshold);
                    if (rect.IsEmpty) continue;
                    switch (name)
                    {
                        case "结界狗":
                        case "结界狗2":
                        case "结界战":
                        case "自动1":
                        case "自动2":
                        case "自动3":
                            if (isNeedGreenFlag)
                            {
                                GreenFlag1(taskHelper);
                            }
                            goto clean;

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

                        case "突破票0":
                            Logger.Success("突破票已用完，突破结束");
                            return;

                        case "十勾玉":
                        case "突破攻破0":
                        case "突破攻破02":
                            if (lastClick == "奖励")
                            {
                                goto newRound;
                            }
                            continue;
                    }

                    Logger.DeBug(name);
                    lastClick = name;
                    if (lastClick.StartsWith("彼岸花血条") && !isNeedRedFlag) break;
                    if (lastClick.StartsWith("突破攻破0") || lastClick == "十勾玉") break;

                    WinHelper.Click(rect);
                    if (lastClick != "准备")
                    {
                        WinHelper.RandomDelay();
                    }
                    else
                    {
                        WinHelper.Delay(300);
                    }
                    break;
                }
            }

            #endregion 全清
        });
    }

    /// <summary>
    /// 寮突破
    /// </summary>
    /// <param name="retryCount">失败重试次数(暂时不用)</param>
    /// <param name="taskHelper"></param>
    /// <param name="isNeedGreenFlag">是否绿标</param>
    /// <param name="isNeedRedFlag">是否红标</param>
    public static void 寮突破(TaskHelper taskHelper, int retryCount = 4, bool isNeedGreenFlag = false, bool isNeedRedFlag = false)
    {
        taskHelper.Start(() =>
        {
            var keys = new List<string>()
            {
                "探索", "突破", "寮突破", "阵容_锁定", "寮突次数0", "突破进攻", "突破进攻2", "准备", "结界狗","结界狗2", "结界战","自动1","自动2","自动3", "彼岸花血条","彼岸花血条2","彼岸花血条3", "失败", "失败2",
                "赢", "奖励",
                "突破边框", "突破边框2",
            }.AddExt(BaseKeys);
            var succ = 0;
            var fail = 0;
            var scanRetry = 0;
            var lastClick = string.Empty;
        wait: //等待CD
            while (taskHelper.IsRunning && IsRunningExt(0, succ))
            {
                taskHelper.WaitForPause();
                if (scanRetry > 10)
                {
                    Logger.Success("找不到合适的进攻对象，可能是打完了或者输太多，寮突结束");
                    return;
                }

                var screen = WinHelper.CaptureWindow();
                foreach (var key in keys)
                {
                    taskHelper.WaitForPause();
                    if (!TargetImages.TryGetValue(key, out var tuple)) continue;
                    var (mat, name) = tuple;
                    var threshold = name.StartsWith("突破边框") || name.StartsWith("突破进攻") || name.StartsWith("彼岸花血条") ? 0.85 : 0.95;
                    var rect = WinHelper.Find(screen, mat, threshold);
                    if (rect.IsEmpty) continue;
                    scanRetry = 0;
                    switch (name)
                    {
                        case "结界狗":
                        case "结界狗2":
                        case "结界战":
                        case "自动1":
                        case "自动2":
                        case "自动3":
                            if (isNeedGreenFlag)
                            {
                                GreenFlag1(taskHelper);
                            }
                            goto wait;
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

                        case "阵容_锁定":
                            Logger.Success("已解锁阵容");
                            break;

                        case "寮突次数0":
                            Logger.Success("无突破次数，等待CD中");
                            WinHelper.Delay(10000);
                            goto wait;
                    }

                    lastClick = name;

                    if (lastClick.StartsWith("彼岸花血条") && !isNeedRedFlag) break;
                    WinHelper.Click(rect);
                    if (lastClick != "准备")
                    {
                        WinHelper.RandomDelay();
                    }
                    else
                    {
                        WinHelper.Delay(300);
                    }

                    break;
                }

                if (lastClick != string.Empty) continue;
                scanRetry++;
                WinHelper.Delay(6000);
            }
        });
    }

    public static void 道馆(TaskHelper taskHelper, int retryCount = 4, bool isNeedGreenFlag = false, bool isNeedRedFlag = false)
    {
        taskHelper.Start(() =>
        {
            var keys = new List<string>() { "挑战_道馆22", "挑战_道馆12", "准备", "彼岸花血条", "彼岸花血条2", "失败", "失败2", "赢", "赢2", "奖励" }.AddExt(BaseKeys);

            var succ = 0;
            var fail = 0;
            var lastClick = string.Empty;
        wait:
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
                            if (lastClick != "奖励")
                            {
                                succ++;
                                Logger.PrintChallengeCount(succ, fail);
                            }
                            break;
                    }

                    lastClick = name;
                    if (lastClick.StartsWith("彼岸花血条") && !isNeedRedFlag) break;
                    WinHelper.Click(rect);
                    if (lastClick != "准备")
                    {
                        WinHelper.RandomDelay();
                    }
                    break;
                }
            }
        });
    }
}