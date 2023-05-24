using DevExpress.CodeParser;
using DevExpress.XtraPrinting.Native.Extensions;

namespace AutoHelpMe.Function;

/// <summary>
/// 探索副本
/// </summary>
public partial class Functions
{
    /// <summary>
    /// 探索副本,目前困28
    /// </summary>
    /// <param name="taskHelper"></param>
    /// <param name="maxCount"></param>
    /// <param name="chapter">章节</param>
    /// <param name="isNormal">是否普通,true普通,false困难</param>
    /// <param name="isTuPo">是否当结界票满时进行结界突破</param>
    public static void 探索副本(TaskHelper taskHelper, int maxCount,int chapter=28,bool isNormal=false,bool isTuPo=false)
    {
        taskHelper.Start(() =>
        {
            //是否在副本中
            var isIn = false;
            //左右点击移动次数
            var move = 0;
            var rectR = new Rectangle(800, 500, 400, 100);
            var rectL = new Rectangle(200, 500, 500, 100);
            
            var keys = new List<string>() { "副本_结界票满", "副本_确认","副本_宝箱", "副本_宝箱2", "探索",  "副本_探索", "副本_轮换", "副本_樱饼", "副本_小怪", "副本_BOSS", "准备", "失败", "失败2", "赢", "赢2", "奖励", "副本_小奖励" }.AddExt(BaseKeys);
            keys.Add($"副本_{chapter}");
            keys.Add(isNormal ? "副本_普通" : "副本_困难");

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

                        case "奖励":
                            if (!lastClick.StartsWith("奖励"))
                            {
                                succ++;
                                Logger.PrintChallengeCount(succ, fail);
                            }
                            break;
                        case "副本_结界票满":
                            GlobalConst.FinishReason = "副本结界票满";
                            if (isTuPo)
                            {
                                goto close;
                            }
                            break;
                        default:
                            isIn = false;
                            move = 0;
                            break;
                    }
                    lastClick = name;
                    if (name == "副本_樱饼")
                    {
                        isIn = true;
                    }
                    else
                    {
                        WinHelper.Click(rect);
                        WinHelper.RandomDelay();
                        break;
                    }
                }

                if (isIn)
                {
                    WinHelper.Click(move % 6 < 3 ? rectR : rectL);
                    move++;
                    WinHelper.RandomDelay(2000, 3000);
                }
            }

            close:
            var checkKeys = new List<string>() { "副本_关闭" };
            int retry = 0;
            while (taskHelper.IsRunning && retry <= 10)
            {
                taskHelper.WaitForPause();
                var screen = WinHelper.CaptureWindow();
                foreach (var key in checkKeys)
                {
                    taskHelper.WaitForPause();
                    if (!TargetImages.TryGetValue(key, out var tuple)) continue;
                    var (mat, name) = tuple;
                    var rect = WinHelper.Find(screen, mat, 0.9);
                    if (rect.IsEmpty) continue;
                    WinHelper.Click(rect);
                    WinHelper.RandomDelay();
                    break;
                }
                retry++;
            }
        });
    }

    public static void 剧情(TaskHelper taskHelper,int maxCount)
    {
        taskHelper.Start(() =>
        {
            var keys = new List<string>() { "剧情_任务","剧情_快进","剧情_问号","剧情_跳过","剧情_对话","剧情_天眼", "剧情_CV", "副本_小怪", "准备", "失败", "失败2", "赢", "赢2", "奖励", }.AddExt(BaseKeys);

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