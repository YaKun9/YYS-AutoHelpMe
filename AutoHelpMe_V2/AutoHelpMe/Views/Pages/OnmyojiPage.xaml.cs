using AutoHelpMe.Extension;
using AutoHelpMe.Helpers;
using AutoHelpMe.ViewModels.Pages;
using Serilog;
using Serilog.Sinks.RichTextBox.Abstraction;
using Wpf.Ui.Controls;

namespace AutoHelpMe.Views.Pages
{
    /// <summary>
    /// OnmyojiPage.xaml 的交互逻辑
    /// </summary>
    public partial class OnmyojiPage : INavigableView<OnmyojiViewModel>
    {
        /// <summary>
        /// 是否初始化完成
        /// </summary>
        private static bool _isInitializationComplete;

        public OnmyojiPage(OnmyojiViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
            Loaded += OnmyojiPage_Loaded;
        }

        private void OnmyojiPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_isInitializationComplete)
            {
                var richTextBox = App.GetService<IRichTextBox>();
                if (richTextBox != null)
                {
                    richTextBox.RichTextBox = OnmyojiLogBox;
                }

                Log.Warning("Make Onmyoji Great Again！！！");
            }

            _isInitializationComplete = true;
        }

        public OnmyojiViewModel ViewModel { get; }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var win = WinApiHelper.GetWindowHandleByTitle("企业微信");
            var bitMap = WinApiHelper.CaptureWindow(win).ConvertBitmapToBitmapSource();

            WindowImage.Source = bitMap;
            Log.Warning("Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！Make Onmyoji Great Again！！！");
        }
    }
}