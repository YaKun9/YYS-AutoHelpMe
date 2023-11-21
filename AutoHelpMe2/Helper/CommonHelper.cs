namespace AutoHelpMe2.Helper
{
    public static class CommonHelper
    {
        internal static void RandomDelay(int min = 50, int max = 100)
        {
            Thread.Sleep(new Random().Next(min, max));
        }
    }
}