using Serilog;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using AutoHelpMe.Extension;
using Vanara.PInvoke;

namespace AutoHelpMe.Helpers;

public static class WinApiHelper
{
    /// <summary>
    /// 根据窗口标题获取窗口句柄。
    /// </summary>
    /// <param name="windowTitle">窗口标题。</param>
    /// <param name="lpClassName">窗口类名（可选）。如果不需要根据类名查找窗口，可以传递 null。</param>
    /// <returns>返回找到的窗口句柄和匹配是否成功</returns>
    public static (HWND hWnd, bool success) GetWindowHandle(string windowTitle, string lpClassName = null)
    {
        var hWnd = User32.FindWindow(lpClassName, windowTitle);
        LogHelper.Debug(hWnd.IsNull
            ? $"未找到标题为 '{windowTitle}' 的窗口。"
            : $"找到标题为 '{windowTitle}' 的窗口，句柄：{hWnd.DangerousGetHandle()}。");
        return (hWnd, !hWnd.IsNull);
    }

    /// <summary>
    /// 截图指定窗口
    /// </summary>
    public static Bitmap CaptureWindow(HWND windowHandle)
    {
        // 获取窗口的大小和位置
        if (!User32.GetWindowRect(windowHandle, out var rect))
        {
            LogHelper.Debug("无法获取窗口矩形。");
            return null;
        }

        var width = rect.right - rect.left;
        var height = rect.bottom - rect.top;

        // 获取源设备上下文
        using var hdcSrc = User32.GetDC(windowHandle);
        // 创建兼容的设备上下文
        using var hdcDest = Gdi32.CreateCompatibleDC(hdcSrc);
        // 创建兼容的位图
        using var hBitmap = Gdi32.CreateCompatibleBitmap(hdcSrc, width, height);
        if (hBitmap.IsInvalid)
        {
            LogHelper.Debug("创建位图失败。");
            return null;
        }

        // 选择位图到目标设备上下文并保留旧的位图句柄
        _ = Gdi32.SelectObject(hdcDest, hBitmap);
        if (!Gdi32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, Gdi32.RasterOperationMode.SRCCOPY))
        {
            LogHelper.Debug("BitBlt操作失败。");
            return null;
        }

        var bitmap = Image.FromHbitmap(hBitmap.DangerousGetHandle());

        return bitmap;
    }

    /// <summary>
    /// 设置全局鼠标钩子，并在检测到鼠标中键点击时返回窗口句柄。
    /// </summary>
    /// <returns>检测到的窗口句柄。</returns>
    public static HWND GetWindowHandleOnMiddleClick()
    {
        using var waitHandle = new ManualResetEvent(false);
        HWND result = default;
        User32.HHOOK mouseHook = default;

        IntPtr MouseProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (User32.WindowMessage)wParam == User32.WindowMessage.WM_MBUTTONUP)
            {
                // 获取鼠标位置
                var hookStruct = Marshal.PtrToStructure<User32.MSLLHOOKSTRUCT>(lParam);
                var pt = hookStruct.pt;

                // 获取窗口句柄
                result = User32.WindowFromPoint(pt);

                // 取消钩子并设置结果
                User32.UnhookWindowsHookEx(mouseHook);
                waitHandle.Set();
            }

            return User32.CallNextHookEx(mouseHook, nCode, wParam, lParam);
        }

        // 设置全局鼠标钩子
        mouseHook = User32.SetWindowsHookEx(User32.HookType.WH_MOUSE_LL, MouseProc, IntPtr.Zero, 0);

        // 等待鼠标中键点击事件
        waitHandle.WaitOne();
        LogHelper.Information($"当前选择窗体名称：{result.GetWindowTitle()}");
        return result;
    }

    /// <summary>
    /// 设置全局鼠标钩子，并在检测到鼠标中键点击时返回窗口句柄。
    /// </summary>
    /// <returns>检测到的窗口句柄。</returns>
    public static Task<HWND> GetWindowHandleOnMiddleClickAsync()
    {
        var tcs = new TaskCompletionSource<HWND>();
        User32.HHOOK mouseHook = default;

        // 设置全局鼠标钩子
        mouseHook = User32.SetWindowsHookEx(User32.HookType.WH_MOUSE_LL, MouseProc, IntPtr.Zero, 0);
        return tcs.Task;

        IntPtr MouseProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (User32.WindowMessage)wParam == User32.WindowMessage.WM_MBUTTONUP)
            {
                // 获取鼠标位置
                var hookStruct = Marshal.PtrToStructure<User32.MSLLHOOKSTRUCT>(lParam);
                var pt = hookStruct.pt;

                // 获取窗口句柄
                var hWnd = User32.WindowFromPoint(pt);
                tcs.SetResult(hWnd);
                // 取消钩子
                User32.UnhookWindowsHookEx(mouseHook);
            }

            return User32.CallNextHookEx(mouseHook, nCode, wParam, lParam);
        }
    }
}