using Furion.DependencyInjection;

namespace AutoHelpMe2.Service
{
    public class CommonService : ISingleton
    {
        internal void RandomDelay(int min = 50, int max = 100)
        {
            Thread.Sleep(new Random().Next(min, max));
        }
    }
}