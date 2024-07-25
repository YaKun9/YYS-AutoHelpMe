using System.Drawing;
using Serilog;
using Vanara.PInvoke;
using static Vanara.PInvoke.Gdi32;
using static Vanara.PInvoke.User32;

namespace AutoHelpMe.Helpers;

public static class WinApiHelper
{
    /// <summary>
    /// 根据窗口标题获取窗口句柄
    /// </summary>
    /// <param name="windowTitle">窗口标题</param>
    /// <returns>找到的窗口句柄，如果找不到则返回 IntPtr.Zero</returns>
    public static HWND GetWindowHandleByTitle(string windowTitle)
    {
        // 使用 FindWindow 查找窗口，类名参数传递 null，只按标题查找
        var hWnd = User32.FindWindow(null, windowTitle);
        Log.Debug(hWnd == IntPtr.Zero ? $"未找到标题为 '{windowTitle}' 的窗口。" : $"找到标题为 '{windowTitle}' 的窗口，句柄：{hWnd}。");
        return hWnd;
    }

    /// <summary>
    /// 截图指定窗口
    /// </summary>
    public static Bitmap CaptureWindow(HWND windowHandle, int delay = 50)
    {
        // 获取窗口的大小和位置
        if (!GetWindowRect(windowHandle, out var rect))
        {
            Log.Debug("无法获取窗口矩形。");
            return null;
        }

        var width = rect.right - rect.left;
        var height = rect.bottom - rect.top;

        // 获取源设备上下文
        using var hdcSrc = GetDC(windowHandle);
        // 创建兼容的设备上下文
        using var hdcDest = CreateCompatibleDC(hdcSrc);
        // 创建兼容的位图
        using var hBitmap = CreateCompatibleBitmap(hdcSrc, width, height);
        if (hBitmap.IsInvalid)
        {
            Log.Debug("创建位图失败。");
            return null;
        }

        // 选择位图到目标设备上下文并保留旧的位图句柄
        _ = SelectObject(hdcDest, hBitmap);
        if (!BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, Gdi32.RasterOperationMode.SRCCOPY))
        {
            Log.Debug("BitBlt操作失败。");
            return null;
        }

        var bitmap = Image.FromHbitmap(hBitmap.DangerousGetHandle());

        return bitmap;
    }
}