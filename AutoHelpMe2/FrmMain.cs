using AutoHelpMe2.EventBus;
using AutoHelpMe2.Service;
using Furion;
using Furion.EventBus;
using Furion.Logging;
using Microsoft.Extensions.Logging;
using Windows.Win32.Foundation;
using static AutoHelpMe2.EventBus.EventBusConst;

namespace AutoHelpMe2
{
    public partial class FrmMain : Form
    {
        private readonly Win32Service _win32Service;

        public FrmMain()
        {
            InitializeComponent();
            _win32Service = App.GetService<Win32Service>();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            MessageCenter.Subscribe(EventIds.LOG_EVENT, async (data) =>
            {
                Invoke(() =>
                {
                    if (data.Source is LogEventSource log)
                    {
                        if (log.LogLevel != LogLevel.Debug || cbxShowDebug.Checked)
                        {
                            txtLog.Text += log.Payload + Environment.NewLine;
                        }
                    }
                });
                await Task.CompletedTask;
            });

            button2.MouseUp += BtnLock_MouseUp;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Log.Debug("测试");

            var hWnd = new HWND(3939114);

            var title = _win32Service.GetWindowTitle(hWnd);

            Log.Information(title);

            var bitmap = _win32Service.CaptureWindow(hWnd);

            pictureBox1.Image = bitmap;
        }

        private void BtnLock_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Middle) return;
            var screenPoint = button2.PointToScreen(new Point(e.X, e.Y));
            if (Bounds.Contains(screenPoint)) return;
            var hWnd = _win32Service.WindowFromPoint(screenPoint);
            var title = _win32Service.GetWindowTitle(hWnd);
            Log.Information($"选定窗口：{title}");
            Capture = false;
        }
    }
}