using AutoHelpMe.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace AutoHelpMe.Views.Pages
{
    /// <summary>
    /// OnmyojiPage.xaml 的交互逻辑
    /// </summary>
    public partial class OnmyojiPage : INavigableView<OnmyojiViewModel>
    {
        public OnmyojiPage(OnmyojiViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }

        public OnmyojiViewModel ViewModel { get; }
    }
}