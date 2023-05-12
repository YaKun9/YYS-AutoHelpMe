using DevExpress.XtraEditors;
using System.Text;

namespace AutoHelpMe
{
    public partial class FrmAuth : XtraForm
    {
        public FrmAuth()
        {
            InitializeComponent();
        }

        public FrmAuth(string title, string content) : this()
        {
            this.lab_提示.Text = content;
            this.Text = title;
        }

        private void btn_确认_Click(object sender, EventArgs e)
        {
            try
            {
               //todo verify authcode
            }
            catch
            {
                XtraMessageBox.Show("保存失败!");
                return;
            }

            this.Close();
        }
    }
}