namespace KCureVDIDataBox
{
    partial class LoginBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginBox));
            btnOk = new Button();
            btnCancel = new Button();
            label1 = new Label();
            label2 = new Label();
            txtId = new TextBox();
            txtPassword = new TextBox();
            tmrFocusPassword = new System.Windows.Forms.Timer(components);
            chkUseLocalhost = new CheckBox();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // btnOk
            // 
            btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOk.Location = new Point(291, 254);
            btnOk.Margin = new Padding(4, 5, 4, 5);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(100, 34);
            btnOk.TabIndex = 4;
            btnOk.Text = "로그인(&L)";
            btnOk.Click += btnOk_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Location = new Point(411, 254);
            btnCancel.Margin = new Padding(4, 5, 4, 5);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 34);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "취소(&C)";
            btnCancel.Click += btnCancel_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(82, 123);
            label1.Name = "label1";
            label1.Size = new Size(54, 20);
            label1.TabIndex = 0;
            label1.Text = "아이디";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(67, 172);
            label2.Name = "label2";
            label2.Size = new Size(69, 20);
            label2.TabIndex = 2;
            label2.Text = "비밀번호";
            // 
            // txtId
            // 
            txtId.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtId.Location = new Point(159, 120);
            txtId.Name = "txtId";
            txtId.Size = new Size(352, 27);
            txtId.TabIndex = 1;
            // 
            // txtPassword
            // 
            txtPassword.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtPassword.Location = new Point(159, 169);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(352, 27);
            txtPassword.TabIndex = 3;
            // 
            // tmrFocusPassword
            // 
            tmrFocusPassword.Tick += tmrFocusPassword_Tick;
            // 
            // chkUseLocalhost
            // 
            chkUseLocalhost.AutoSize = true;
            chkUseLocalhost.Location = new Point(35, 255);
            chkUseLocalhost.Name = "chkUseLocalhost";
            chkUseLocalhost.Size = new Size(125, 24);
            chkUseLocalhost.TabIndex = 6;
            chkUseLocalhost.Text = "localhost 사용";
            chkUseLocalhost.UseVisualStyleBackColor = true;
            chkUseLocalhost.CheckedChanged += chkUseLocalhost_CheckedChanged;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = KCureDataAccess.Properties.Resources.logo_b;
            pictureBox1.Location = new Point(15, 17);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(178, 72);
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.TabIndex = 7;
            pictureBox1.TabStop = false;
            // 
            // LoginBox
            // 
            AcceptButton = btnOk;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImageLayout = ImageLayout.None;
            CancelButton = btnCancel;
            ClientSize = new Size(541, 307);
            Controls.Add(pictureBox1);
            Controls.Add(chkUseLocalhost);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            Controls.Add(txtPassword);
            Controls.Add(txtId);
            Controls.Add(label2);
            Controls.Add(label1);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginBox";
            Padding = new Padding(12, 14, 12, 14);
            StartPosition = FormStartPosition.CenterParent;
            Text = "로그인";
            Load += LoginBox_Load;
            MouseClick += LoginBox_MouseClick;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnOk;
        private Button btnCancel;
        private Label label1;
        private Label label2;
        private TextBox txtId;
        private TextBox txtPassword;
        private System.Windows.Forms.Timer tmrFocusPassword;
        private CheckBox chkUseLocalhost;
        private PictureBox pictureBox1;
    }
}
