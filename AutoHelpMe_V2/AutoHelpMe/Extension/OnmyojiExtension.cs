using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Vanara.PInvoke;

namespace AutoHelpMe.Extension;

public static class OnmyojiExtension
{
    /// <summary>
    /// 将 Bitmap 对象转换为 BitmapSource 对象，用于在 WPF 中显示图像。
    /// </summary>
    /// <param name="bitmap">要转换的 Bitmap 对象。</param>
    /// <returns>转换后的 BitmapSource 对象。</returns>
    public static BitmapSource BitmapToBitmapSource(this Bitmap bitmap)
    {
        var hBitmap = bitmap.GetHbitmap();
        var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
            hBitmap,
            IntPtr.Zero,
            Int32Rect.Empty,
            BitmapSizeOptions.FromEmptyOptions());
        Gdi32.DeleteObject(hBitmap);
        return bitmapSource;
    }
}