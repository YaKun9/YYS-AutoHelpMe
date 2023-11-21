using Windows.Win32;

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
            //584 417
            var ss = PInvoke.FindWindow(null, "н╒пе");
            var sw = Win32Helper2.CaptureWindow(ss);
            pictureBox1.Image=sw;

            Win32Helper2.DragWithCurve(ss,new Point(584,417),new Point(584,617));

        }
    }
}