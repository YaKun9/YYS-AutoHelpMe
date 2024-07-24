using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace AutoHelpMe.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _applicationTitle = "AutoHelpMe";

        [ObservableProperty]
        private ObservableCollection<NavigationViewItem> _menuItems =
        [
            new NavigationViewItem()
            {
                Content = "首页",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(Views.Pages.DashboardPage)
            },

            new NavigationViewItem()
            {
                Content = "Onmyoji",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Games24 },
                TargetPageType = typeof(Views.Pages.OnmyojiPage)
            },

            new NavigationViewItem()
            {
                Content = "菜单2",
                Icon = new SymbolIcon { Symbol = SymbolRegular.ConvertRange24 },
                TargetPageType = typeof(Views.Pages.DataPage)
            },
            new NavigationViewItem()
            {
                Content = "菜单3",
                Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
                TargetPageType = typeof(Views.Pages.DataPage)
            },
            new NavigationViewItem()
            {
                Content = "菜单4",
                Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
                TargetPageType = typeof(Views.Pages.DataPage)
            },
            new NavigationViewItem()
            {
                Content = "菜单5",
                Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
                TargetPageType = typeof(Views.Pages.DataPage)
            },
        ];

        [ObservableProperty]
        private ObservableCollection<NavigationViewItem> _footerMenuItems =
        [
            new NavigationViewItem()
            {
                Content = "设置",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(Views.Pages.SettingsPage)
            }
        ];

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems =
        [
            new MenuItem { Header = "Home", Tag = "tray_home" }
        ];
    }
}