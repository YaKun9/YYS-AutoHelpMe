using AutoHelpMe2.Service;
using Furion;
using Furion.EventBus;
using Furion.Logging;
using static AutoHelpMe2.EventBus.EventBusConst;

namespace AutoHelpMe2
{
    public partial class FrmMain : Form
    {
        private readonly Win32Service _win32Service;
        private readonly OpenCvService _openCvService;
        private readonly IEventBusFactory _eventBusFactory;

        public FrmMain()
        {
            InitializeComponent();
            _win32Service = App.GetService<Win32Service>();
            _openCvService = App.GetService<OpenCvService>();
            _eventBusFactory = App.GetService<IEventBusFactory>();
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

        private void FrmMain_Load(object sender, EventArgs e)
        {
            MessageCenter.Subscribe(EventIds.LOG_EVENT, async (data) =>
            {
                await Task.CompletedTask;
            });
        }
    }
}