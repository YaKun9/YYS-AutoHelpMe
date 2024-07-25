using AutoHelpMe.ViewModels.Pages;
using Serilog.Sinks.RichTextBox.Abstraction;
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
            Loaded += OnmyojiPage_Loaded;
        }

        private void OnmyojiPage_Loaded(object sender, RoutedEventArgs e)
        {
            var richTextBox = App.GetService<IRichTextBox>();
            if (richTextBox != null)
            {
                richTextBox.RichTextBox = OnmyojiLogBox;
            }
        }

        public OnmyojiViewModel ViewModel { get; }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
        }
    }
}