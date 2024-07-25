using System.Drawing;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Vanara.PInvoke;

namespace AutoHelpMe.Extension;

public static class OnmyojiExtension
{
    public static BitmapSource ConvertBitmapToBitmapSource(this Bitmap bitmap)
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