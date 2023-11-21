using Windows.Win32.Foundation;

namespace AutoHelpMe2.Extension
{
    public static class Extension
    {
        internal static LPARAM GetRandomPoint(this RECT rect)
        {
            var random = new Random();
            var x = random.Next(rect.left + 1, rect.right - 1);
            var y = random.Next(rect.top + 1, rect.bottom - 1);
            return x + (y << 16);
        }
    }
}