using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace AutoHelpMe.Views.Pages
{
    /// <summary>
    /// ChatGPTPage.xaml 的交互逻辑
    /// </summary>
    public partial class ChatGPTPage : Page
    {
        private const string Url = "https://ikunai.top";

        public ChatGPTPage()
        {
            InitializeComponent();
            Clipboard.SetText(Url);
        }

        private void OpenBrowser_OnClick(object sender, RoutedEventArgs e)
        {
            // 使用默认浏览器打开网页
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = Url,
                UseShellExecute = true
            });
        }
    }
}