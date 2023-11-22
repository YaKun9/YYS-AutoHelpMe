namespace AutoHelpMe2
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            pictureBox1 = new PictureBox();
            txtLog = new RichTextBox();
            panelLog = new Panel();
            btnOpenLogDir = new LinkLabel();
            cbxShowDebug = new CheckBox();
            button2 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panelLog.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(12, 28);
            button1.Name = "button1";
            button1.Size = new Size(187, 35);
            button1.TabIndex = 0;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Button1_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(12, 101);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(187, 321);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // txtLog
            // 
            txtLog.BorderStyle = BorderStyle.None;
            txtLog.Location = new Point(3, 26);
            txtLog.Name = "txtLog";
            txtLog.Size = new Size(276, 537);
            txtLog.TabIndex = 2;
            txtLog.Text = "";
            // 
            // panelLog
            // 
            panelLog.Controls.Add(btnOpenLogDir);
            panelLog.Controls.Add(cbxShowDebug);
            panelLog.Controls.Add(txtLog);
            panelLog.Dock = DockStyle.Right;
            panelLog.Location = new Point(844, 0);
            panelLog.Name = "panelLog";
            panelLog.Size = new Size(282, 566);
            panelLog.TabIndex = 3;
            // 
            // btnOpenLogDir
            // 
            btnOpenLogDir.AutoSize = true;
            btnOpenLogDir.Location = new Point(194, 6);
            btnOpenLogDir.Name = "btnOpenLogDir";
            btnOpenLogDir.Size = new Size(80, 17);
            btnOpenLogDir.TabIndex = 4;
            btnOpenLogDir.TabStop = true;
            btnOpenLogDir.Text = "打开日志文件";
            // 
            // cbxShowDebug
            // 
            cbxShowDebug.AutoSize = true;
            cbxShowDebug.Location = new Point(113, 5);
            cbxShowDebug.Name = "cbxShowDebug";
            cbxShowDebug.Size = new Size(75, 21);
            cbxShowDebug.TabIndex = 3;
            cbxShowDebug.Text = "调试信息";
            cbxShowDebug.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(372, 165);
            button2.Name = "button2";
            button2.Size = new Size(187, 35);
            button2.TabIndex = 4;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = true;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1126, 566);
            Controls.Add(button2);
            Controls.Add(panelLog);
            Controls.Add(pictureBox1);
            Controls.Add(button1);
            Name = "FrmMain";
            Text = "Form1";
            Load += FrmMain_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panelLog.ResumeLayout(false);
            panelLog.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private PictureBox pictureBox1;
        private RichTextBox txtLog;
        private Panel panelLog;
        private CheckBox cbxShowDebug;
        private LinkLabel btnOpenLogDir;
        private Button button2;
    }
}
