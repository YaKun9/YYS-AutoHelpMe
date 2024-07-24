using Wpf.Ui.Controls;

namespace AutoHelpMe.ViewModels.Pages;

public partial class OnmyojiViewModel : ObservableObject, INavigationAware
{
    private bool _isInitialized = false;

    public void OnNavigatedTo()
    {
        if (!_isInitialized)
        {
            //todo something
        }
        _isInitialized = true;
    }

    public void OnNavigatedFrom()
    {
    }
}