using OpenCvSharp;

namespace AutoHelpMe.Function;

public partial class Functions
{
    /// <summary>
    /// 图片资源字典
    /// </summary>
    private static Dictionary<string, Tuple<Mat, string>> TargetImages => WinHelper.Instance.LoadImages();

    /// <summary>
    /// Win32帮助类
    /// </summary>
    private static WinHelper WinHelper => WinHelper.Instance;

    /// <summary>
    /// 基础key,包含所有任务都需要处理的
    /// </summary>
    private static readonly List<string> BaseKeys = new() { "拒绝协作", "继续" };

    /// <summary>
    /// 是否运行拓展
    /// </summary>
    /// <param name="maxCount"></param>
    /// <param name="success"></param>
    /// <returns></returns>
    private static bool IsRunningExt(int maxCount, int success)
    {
        return maxCount == 0 || success < maxCount;
    }

    /// <summary>
    /// 调用方法
    /// </summary>
    /// <param name="actionName"></param>
    /// <param name="values"></param>
    public static void Invoke(string actionName, params object[] values)
    {
        // 获取类型
        var type = typeof(Functions);
        // 获取指定名称的方法
        var methodInfo = type.GetMethod(actionName);
        // 判断方法是否存在
        if (methodInfo != null && methodInfo.IsStatic)
        {
            methodInfo.Invoke(null, values);
        }
    }

    /// <summary>
    /// 绿标1号位
    /// </summary>
    /// <param name="taskHelper"></param>
    private static void GreenFlag1(TaskHelper taskHelper)
    {
        var screen = WinHelper.CaptureWindow();
        var keys = new List<string> { "绿标1", "绿标2", "绿标3", "绿标4" };
        foreach (var key in keys)
        {
            taskHelper.WaitForPause();
            if (!TargetImages.TryGetValue(key, out var tuple)) continue;
            var (mat, name) = tuple;
            var img = WinHelper.Find(screen, mat, 0.85);
            if (img.IsEmpty) continue;
            return;
        }

        keys.AddRange(new List<string>() { "归位", "归位2", "失败", "失败2", "赢", "赢2", "奖励" }.AddExt(BaseKeys));
        var rect = new Rectangle(190, 420, 80, 130);
        while (taskHelper.IsRunning)
        {
            screen = WinHelper.CaptureWindow();
            foreach (var key in keys)
            {
                taskHelper.WaitForPause();
                if (!TargetImages.TryGetValue(key, out var tuple)) continue;
                var (mat, name) = tuple;
                var img = WinHelper.Find(screen, mat, 0.85);
                if (img.IsEmpty) continue;
                if (name != "归位") goto end;
                WinHelper.Click(img); //点击归位按钮
                WinHelper.Delay(150);
                break;
            }
            WinHelper.Click(rect);
            WinHelper.Delay(200);
        }
    end:;
    }
}