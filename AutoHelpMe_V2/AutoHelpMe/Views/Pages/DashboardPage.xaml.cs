using AutoHelpMe.ViewModels.Pages;
using Wpf.Ui.Controls;
using DashboardViewModel = AutoHelpMe.ViewModels.Pages.DashboardViewModel;

namespace AutoHelpMe.Views.Pages
{
    public partial class DashboardPage : INavigableView<DashboardViewModel>
    {
        public DashboardViewModel ViewModel { get; }

        public DashboardPage(DashboardViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
