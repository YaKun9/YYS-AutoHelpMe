using Serilog;

namespace AutoHelpMe.ViewModels.Pages
{
    public partial class DashboardViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _counter = 0;

        [RelayCommand]
        private void OnCounterIncrement()
        {
            Log.Information("我点点点");
            Counter++;
        }
    }
}
