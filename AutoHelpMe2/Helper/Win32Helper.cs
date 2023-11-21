using AutoHelpMe2.Extension;
using System.Drawing.Imaging;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;

namespace AutoHelpMe2.Helper
{
    public class Win32Helper
    {
        /// <summary>
        /// 查找窗口句柄
        /// </summary>
        /// <param name="windowTitle">窗口名称</param>
        /// <param name="isWait">是否等待窗口出现</param>
        /// <param name="retry">窗口等待次数(间隔3秒)</param>
        /// <returns></returns>
        internal static HWND FindWindow(string windowTitle, bool isWait = false, int retry = 10)
        {
            var handle = PInvoke.FindWindow(null, windowTitle);
            if (handle == HWND.Null && isWait)
            {
                while (handle == HWND.Null && retry > 0)
                {
                    handle = PInvoke.FindWindow(null, windowTitle);
                    retry--;
                }
            }
            return handle;
        }

        internal static HWND FindWindow(Point point)
        {
            return PInvoke.WindowFromPoint(point);
        }

        /// <summary>
        /// 窗口截屏
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <returns></returns>
        internal static Bitmap CaptureWindow(HWND hWnd)
        {
            PInvoke.GetWindowRect(hWnd, out var rect);
            var width = rect.right - rect.left;
            var height = rect.bottom - rect.top;

            var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            using var g = Graphics.FromImage(bmp);
            var hdcBmp = g.GetHdc();
            var hdcWindow = PInvoke.GetWindowDC(hWnd);

            PInvoke.BitBlt((HDC)hdcBmp, 0, 0, width, height, hdcWindow, 0, 0, ROP_CODE.SRCCOPY);

            g.ReleaseHdc(hdcBmp);
            PInvoke.ReleaseDC(hWnd, hdcWindow);

            return bmp;
        }

        /// <summary>
        /// 单击鼠标左键
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="rect">点击区域</param>
        internal static void Click_Left(HWND hWnd, RECT rect)
        {
            var point = rect.GetRandomPoint();
            PInvoke.PostMessage(hWnd, PInvoke.WM_LBUTTONDOWN, new WPARAM(0x0001), point);
            CommonHelper.RandomDelay();
            PInvoke.PostMessage(hWnd, PInvoke.WM_LBUTTONUP, new WPARAM(0x0001), point);
        }

        /// <summary>
        /// 单击鼠标左键
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="x">x轴坐标</param>
        /// <param name="y">y轴坐标</param>
        internal static void Click_Left(HWND hWnd, int x, int y)
        {
            var point = x + (y << 16);
            PInvoke.PostMessage(hWnd, PInvoke.WM_LBUTTONDOWN, new WPARAM(0x0001), point);
            CommonHelper.RandomDelay();
            PInvoke.PostMessage(hWnd, PInvoke.WM_LBUTTONUP, new WPARAM(0x0001), point);
        }
    }
}