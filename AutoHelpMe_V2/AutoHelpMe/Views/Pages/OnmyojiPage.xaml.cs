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
        private static bool _isInitialized;

        public OnmyojiViewModel ViewModel { get; }

        public OnmyojiPage(OnmyojiViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
            Loaded += OnmyojiPage_Loaded;
        }

        private void OnmyojiPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_isInitialized)
            {
                var richTextBox = App.GetService<IRichTextBox>();
                if (richTextBox != null)
                {
                    richTextBox.RichTextBox = OnmyojiLogBox;
                }
                LogHelper.Warning("Make Onmyoji Great Again！！！");
            }

            _isInitialized = true;
        }

        /// <summary>
        /// 选择窗体按钮点击事件
        /// </summary>
        private async void ChooseWindowButton_OnClick(object sender, RoutedEventArgs e)
        {
            await LogHelper.InformationAsync("等待按下鼠标中键(滚轮)来选择一个窗口...");
            var hWnd = await WinApiHelper.GetWindowHandleOnMiddleClickAsync();
            if (!hWnd.IsNull)
            {
                ChooseWindowButton.Content = "已选定窗口";
                ChooseWindowButton.Icon = new SymbolIcon(SymbolRegular.CalendarLock24);
                ChooseWindowButton.ToolTip = $"当前已选定窗口【{hWnd.GetWindowTitle()}】，点击可重新选择";
                await LogHelper.InformationAsync($"当前已选定窗口【{hWnd.GetWindowTitle()}】，点击可重新选择");
            }
            else
            {
                await LogHelper.InformationAsync("选定的窗口无效，请重新选定");
            }
        }
    }
}