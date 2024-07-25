using System.Text;
using Vanara.PInvoke;

namespace AutoHelpMe.Extension;

public static class WinApiExtension
{
    /// <summary>
    /// 获取指定窗口的标题。
    /// </summary>
    /// <param name="hWnd">窗口句柄。</param>
    /// <returns>窗口的标题，如果窗口句柄无效，则返回空字符串。</returns>
    public static string GetWindowTitle(this HWND hWnd)
    {
        if (hWnd.IsNull)
        {
            return string.Empty;
        }
        var length = User32.GetWindowTextLength(hWnd);
        var sb = new StringBuilder(length + 1);
        User32.GetWindowText(hWnd, sb, sb.Capacity);
        return sb.ToString();
    }

    /// <summary>
    /// 获取指定窗口的类名。
    /// </summary>
    /// <param name="hWnd">窗口句柄。</param>
    /// <returns>窗口的类名，如果窗口句柄无效，则返回空字符串。</returns>
    public static string GetWindowClassName(this HWND hWnd)
    {
        if (hWnd.IsNull)
        {
            return string.Empty;
        }
        var sb = new StringBuilder(256);
        User32.GetClassName(hWnd, sb, sb.Capacity);
        return sb.ToString();
    }
}