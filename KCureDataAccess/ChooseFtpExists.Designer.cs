namespace KCureVDIDataBox
{
    partial class ChooseFtpExists
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblLocalExists = new System.Windows.Forms.Label();
            this.rdoResume = new System.Windows.Forms.RadioButton();
            this.rdoSkip = new System.Windows.Forms.RadioButton();
            this.rdoOverwrite = new System.Windows.Forms.RadioButton();
            this.chkApplyToAll = new System.Windows.Forms.CheckBox();
            this.txtFullPath = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(279, 241);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 34);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "확인(&O)";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(399, 241);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 34);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "취소(&C)";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblLocalExists
            // 
            this.lblLocalExists.AutoEllipsis = true;
            this.lblLocalExists.Location = new System.Drawing.Point(30, 24);
            this.lblLocalExists.Name = "lblLocalExists";
            this.lblLocalExists.Size = new System.Drawing.Size(469, 25);
            this.lblLocalExists.TabIndex = 0;
            this.lblLocalExists.Text = "다음 파일이 이미 존재합니다.";
            // 
            // rdoResume
            // 
            this.rdoResume.AutoSize = true;
            this.rdoResume.Location = new System.Drawing.Point(305, 126);
            this.rdoResume.Name = "rdoResume";
            this.rdoResume.Size = new System.Drawing.Size(90, 24);
            this.rdoResume.TabIndex = 4;
            this.rdoResume.Text = "이어쓰기";
            this.rdoResume.UseVisualStyleBackColor = true;
            // 
            // rdoSkip
            // 
            this.rdoSkip.AutoSize = true;
            this.rdoSkip.Checked = true;
            this.rdoSkip.Location = new System.Drawing.Point(34, 126);
            this.rdoSkip.Name = "rdoSkip";
            this.rdoSkip.Size = new System.Drawing.Size(90, 24);
            this.rdoSkip.TabIndex = 2;
            this.rdoSkip.TabStop = true;
            this.rdoSkip.Text = "건너뛰기";
            this.rdoSkip.UseVisualStyleBackColor = true;
            // 
            // rdoOverwrite
            // 
            this.rdoOverwrite.AutoSize = true;
            this.rdoOverwrite.Location = new System.Drawing.Point(171, 126);
            this.rdoOverwrite.Name = "rdoOverwrite";
            this.rdoOverwrite.Size = new System.Drawing.Size(90, 24);
            this.rdoOverwrite.TabIndex = 3;
            this.rdoOverwrite.Text = "덮어쓰기";
            this.rdoOverwrite.UseVisualStyleBackColor = true;
            // 
            // chkApplyToAll
            // 
            this.chkApplyToAll.AutoSize = true;
            this.chkApplyToAll.Location = new System.Drawing.Point(34, 175);
            this.chkApplyToAll.Name = "chkApplyToAll";
            this.chkApplyToAll.Size = new System.Drawing.Size(197, 24);
            this.chkApplyToAll.TabIndex = 5;
            this.chkApplyToAll.Text = "모든 파일에 위 선택 적용";
            this.chkApplyToAll.UseVisualStyleBackColor = true;
            // 
            // txtFullPath
            // 
            this.txtFullPath.Location = new System.Drawing.Point(32, 52);
            this.txtFullPath.Multiline = true;
            this.txtFullPath.Name = "txtFullPath";
            this.txtFullPath.ReadOnly = true;
            this.txtFullPath.Size = new System.Drawing.Size(467, 54);
            this.txtFullPath.TabIndex = 1;
            // 
            // ChooseFtpExists
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(528, 304);
            this.Controls.Add(this.txtFullPath);
            this.Controls.Add(this.chkApplyToAll);
            this.Controls.Add(this.rdoOverwrite);
            this.Controls.Add(this.rdoSkip);
            this.Controls.Add(this.rdoResume);
            this.Controls.Add(this.lblLocalExists);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChooseFtpExists";
            this.Padding = new System.Windows.Forms.Padding(12, 14, 12, 14);
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "파일 쓰기 방식 선택";
            this.Load += new System.EventHandler(this.ChooseFtpExists_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnOk;
        private Button btnCancel;
        private Label lblLocalExists;
        private RadioButton rdoResume;
        private RadioButton rdoSkip;
        private RadioButton rdoOverwrite;
        private CheckBox chkApplyToAll;
        private TextBox txtFullPath;
    }
}
