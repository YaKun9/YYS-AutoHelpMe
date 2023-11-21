using Microsoft.VisualBasic.ApplicationServices;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.Input.KeyboardAndMouse;

namespace AutoHelpMe2
{
    public class Win32Helper2
    {
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

        internal static void DragWithCurve(HWND hWnd, Point start, Point end)
        {
            // 计算贝塞尔曲线点
            var points = CalculatePoints(start, end);

            // 模拟鼠标按下
            PInvoke.PostMessage(hWnd, PInvoke.WM_LBUTTONDOWN, new WPARAM(0x0001), MakeLParam(start.X, start.Y));

            // 沿着曲线移动鼠标
            foreach (var point in points)
            {
                PInvoke.PostMessage(hWnd, PInvoke.WM_MOUSEMOVE, new WPARAM(0x0001), MakeLParam(point.X, point.Y));
                Thread.Sleep(50); // 添加延时
            }

            // 模拟鼠标释放
            PInvoke.PostMessage(hWnd, PInvoke.WM_LBUTTONUP, new WPARAM(0x0001), MakeLParam(end.X, end.Y));
        }

        private static LPARAM MakeLParam(int x, int y)
        {
            return x + (y << 16);
            return (LPARAM)((y << 16) | (x & 0xFFFF));
        }

        private static List<Point> CalculatePoints(Point start, Point end)
        {
            var points = new List<Point>();

            for (int i = 1; i < 10; i++)
            {
                var x = (end.X - start.X) / 10 * i;
                var y = (end.Y - start.Y) / 10 * i;
                if (i % 2 == 0)
                {
                    x = -x;
                }

                x = start.X + x;
                y = start.Y + y;
                points.Add(new Point(x, y));
            }

            return points;
        }
    }
}