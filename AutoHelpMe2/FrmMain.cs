using Windows.Win32;
using Windows.Win32.Foundation;
using AutoHelpMe2.Helper;

namespace AutoHelpMe2
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var ss = PInvoke.FindWindow(null, "��ҵ΢��");
            var sw = Win32Helper.CaptureWindow(ss);
            pictureBox1.Image = sw;

            var rect = OpenCvHelper.FindImage(sw, "Resources/tapd.png");
            
            Win32Helper.Click_Left(ss, rect);
        }
    }
}