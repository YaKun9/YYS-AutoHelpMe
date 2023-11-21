using AutoHelpMe2.Helper;
using AutoHelpMe2.Service;
using Furion;
using Furion.Logging;

namespace AutoHelpMe2
{
    public partial class FrmMain : Form
    {
        private readonly Win32Service _win32Service;
        private readonly OpenCvService _openCvService;
        public FrmMain()
        {
            InitializeComponent();
            _win32Service = App.GetService<Win32Service>();
            _openCvService = App.GetService<OpenCvService>();
        }
        


        private void button1_Click(object sender, EventArgs e)
        {
            Log.Warning("测试");
            var ss = _win32Service.FindWindow("企业微信", true);
            var sw = _win32Service.CaptureWindow(ss);
            pictureBox1.Image = sw;

            var rect = _openCvService.FindImage(sw, "Resource/tapd.png");

            _win32Service.Click_Left(ss, rect);
        }
    }
}