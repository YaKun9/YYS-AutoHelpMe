using System;
using System.IO;
using System.Threading.Tasks;
using AutoHelpMe.Utils;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;

namespace AutoHelpMe;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        LogTextBox.Text += this.Width + " : " + this.Height;
        return;
        var hWnd = WinApi.GetWindowHandleByTitle("企业微信");
        var bitmap = WinApi.CaptureWindowAsync(hWnd);
        if (bitmap != null)
        {
            using var memoryStream = new MemoryStream();
            // 保存System.Drawing.Bitmap到内存流中
            bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
            memoryStream.Position = 0; // 重置流的位置

            // 从流加载Avalonia的Bitmap
            var avaloniaBitmap = new Bitmap(memoryStream);

            // 设置Image控件的Source属性为新的Bitmap
            //MyAvaloniaImage.Source = avaloniaBitmap;
        }

        Console.WriteLine(this.Width + " : " + this.Height);
    }
}