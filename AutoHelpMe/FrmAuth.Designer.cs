namespace AutoHelpMe
{
    partial class FrmAuth
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAuth));
            lab_提示 = new DevExpress.XtraEditors.LabelControl();
            label1 = new Label();
            txt_授权码 = new HZH_Controls.Controls.TextBoxEx();
            btn_确认 = new DevExpress.XtraEditors.SimpleButton();
            SuspendLayout();
            // 
            // lab_提示
            // 
            lab_提示.Appearance.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lab_提示.Appearance.Options.UseFont = true;
            lab_提示.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            lab_提示.Location = new Point(26, 29);
            lab_提示.Name = "lab_提示";
            lab_提示.Size = new Size(304, 19);
            lab_提示.TabIndex = 0;
            lab_提示.Text = "占位提示";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(24, 105);
            label1.Name = "label1";
            label1.Size = new Size(69, 19);
            label1.TabIndex = 1;
            label1.Text = "授权码：";
            // 
            // txt_授权码
            // 
            txt_授权码.DecLength = 2;
            txt_授权码.InputType = HZH_Controls.TextInputType.NotControl;
            txt_授权码.Location = new Point(99, 103);
            txt_授权码.MaxValue = new decimal(new int[] { 1000000, 0, 0, 0 });
            txt_授权码.MinValue = new decimal(new int[] { 1000000, 0, 0, int.MinValue });
            txt_授权码.MyRectangle = new Rectangle(0, 0, 0, 0);
            txt_授权码.Name = "txt_授权码";
            txt_授权码.OldText = null;
            txt_授权码.PromptColor = Color.Gray;
            txt_授权码.PromptFont = new Font("微软雅黑", 15F, FontStyle.Regular, GraphicsUnit.Pixel);
            txt_授权码.PromptText = "";
            txt_授权码.RegexPattern = "";
            txt_授权码.Size = new Size(231, 23);
            txt_授权码.TabIndex = 2;
            txt_授权码.WordWrap = false;
            // 
            // btn_确认
            // 
            btn_确认.Location = new Point(139, 139);
            btn_确认.Name = "btn_确认";
            btn_确认.Size = new Size(75, 23);
            btn_确认.TabIndex = 3;
            btn_确认.Text = "确认";
            btn_确认.Click += btn_确认_Click;
            // 
            // FrmAuth
            // 
            Appearance.BackColor = Color.White;
            Appearance.Options.UseBackColor = true;
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(377, 174);
            ControlBox = false;
            Controls.Add(btn_确认);
            Controls.Add(txt_授权码);
            Controls.Add(label1);
            Controls.Add(lab_提示);
            IconOptions.ColorizeInactiveIcon = DevExpress.Utils.DefaultBoolean.False;
            IconOptions.LargeImage = (Image)resources.GetObject("FrmAuth.IconOptions.LargeImage");
            IconOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("FrmAuth.IconOptions.SvgImage");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmAuth";
            StartPosition = FormStartPosition.CenterParent;
            Text = "授权";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lab_提示;
        private Label label1;
        private HZH_Controls.Controls.TextBoxEx txt_授权码;
        private DevExpress.XtraEditors.SimpleButton btn_确认;
    }
}