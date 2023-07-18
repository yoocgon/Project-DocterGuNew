using FluentFTP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KCureVDIDataBox
{
    /// <summary>
    /// FTP 파일 전송 시에 파일이 이미 존재할 때의 처리 방법을 선택하는 폼
    /// </summary>
    partial class ChooseFtpExists : Form
    {
        /// <summary>
        /// 이미 존재하는 파일의 전체 경로
        /// </summary>
        public string FullPath { get; set; } = "";

        /// <summary>
        /// 로컬 파일이 이미 존재할 때의 처리 방법
        /// </summary>
        public FtpLocalExists LocalExists { get; private set; } = FtpLocalExists.Overwrite;
        /// <summary>
        /// 원격지 파일이 이미 존재할 때의 처리 방법
        /// </summary>
        public FtpRemoteExists RemoteExists
        {
            get
            {
                switch (LocalExists)
                {
                    case FtpLocalExists.Skip: return FtpRemoteExists.Skip;
                    case FtpLocalExists.Overwrite: return FtpRemoteExists.Overwrite;
                    case FtpLocalExists.Resume: return FtpRemoteExists.Resume;
                    default: throw new Exception("Wrong LocalExists");
                }
            }
        }

        /// <summary>
        /// 이후 모든 파일에 대해 동일한 처리 방법을 적용할 지 여부
        /// </summary>
        public bool ApplyToAll { get; set; } = false;
        
        public ChooseFtpExists()
        {
            InitializeComponent();
        }

        private void ChooseFtpExists_Load(object sender, EventArgs e)
        {
            txtFullPath.Text = FullPath;

            RestoreSettings();
        }
        
        /// <summary>
        /// 처리 방법을 적용
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            SaveSettings();

            this.LocalExists = GetLocalExists();
            this.ApplyToAll = chkApplyToAll.Checked;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// 취소
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 이전 다운로드/업로드 시에 선택한 처리 방법을 복원해서 적용
        /// </summary>
        private void RestoreSettings()
        {
            string LocalExists = CCommon.XmlConfigUser.GetSetting("LocalExists", "Overwrite");
            switch (LocalExists)
            {
                case "Skip": rdoSkip.Checked = true; break;
                case "Overwrite": rdoOverwrite.Checked = true; break;
                case "Resume": rdoResume.Checked = true; break;
            }

            bool ApplyToAll = CCommon.XmlConfigUser.GetSetting("ApplyToAll", "false") == "true";
            chkApplyToAll.Checked = ApplyToAll;
        }

        /// <summary>
        /// 이후 다운로드/업로드 시에 표시할 선택 방법을 XML에 저장
        /// </summary>
        private void SaveSettings()
        {
            var LocalExists = GetLocalExists();
            CCommon.XmlConfigUser.SaveSetting("LocalExists", LocalExists.ToString());
            CCommon.XmlConfigUser.SaveSetting("ApplyToAll", chkApplyToAll.Checked ? "true" : "false");
        }

        /// <summary>
        /// 선택한 처리 방법을 가져옴
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private FtpLocalExists GetLocalExists()
        {
            if (rdoSkip.Checked)
            {
                return FtpLocalExists.Skip;
            }
            else if (rdoOverwrite.Checked)
            {
                return FtpLocalExists.Overwrite;
            }
            else if (rdoResume.Checked)
            {
                return FtpLocalExists.Resume;
            }
            else
            {
                throw new Exception("Wrong LocalExists");
            }
        }
    }
}
