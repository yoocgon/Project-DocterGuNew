using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;
using System.Text.Json;
using KCureVDIDataBox.Model;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Text.Json.Nodes;
using DoctorGu;

namespace KCureVDIDataBox
{
    /// <summary>
    /// 로그인 박스
    /// </summary>
    partial class LoginBox : Form
    {
        public LoginBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 로그인 박스가 표시될 때 설정을 불러오고 포커스를 이동
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginBox_Load(object sender, EventArgs e)
        {
            RestoreSettings();

            this.txtId.Text = "TEST01@naver.com";
            this.txtPassword.Text = "1q2w3e4r!@#";

            chkUseLocalhost.Visible = (CCommon.GetAppSetting<bool>(ConfigAppKeys.ShowLocalHost, false));

            if (txtId.Text != "")
            {
                tmrFocusPassword.Enabled = true;
            }
        }
        /// <summary>
        /// 우클릭 시에 어플리케이션의 정보를 표시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                AboutBox frm = new AboutBox();
                frm.ShowDialog();
            }
        }

        /// <summary>
        /// API 호출 주소를 localhost을 이용할 지 여부 지정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkUseLocalhost_CheckedChanged(object sender, EventArgs e)
        {
            CCommon.useLocalhost = chkUseLocalhost.Checked;
        }

        /// <summary>
        /// 로그인 시도하고 성공하면 DialogResult.OK를 반환
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                string id;
                string password;
                bool useLocalhost;
                string msg;
                if (!IsValid(out id, out password, out useLocalhost, out msg))
                {
                    MessageBox.Show(this, msg, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                using (new CWaitCursor(this, new Control[] { btnOk }))
                {
                    (bool success, string errorMsg, LoginResult result) = await CCommon.Api.Login(id, password);
                    if (!success)
                    {
                        MessageBox.Show(this, errorMsg, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    CCommon.LoginId = id;
                    CCommon.LoginResult = result;
                }
                SaveSettings();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                CCommon.Log.WriteLog(ex);
            }
        }

        /// <summary>
        /// 로그인 취소
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 폼이 로드된 후 비밀번호 텍스트박스에 포커스를 이동하기 위해 타이머를 사용
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrFocusPassword_Tick(object sender, EventArgs e)
        {
            txtPassword.Focus();
            tmrFocusPassword.Enabled = false;
        }

        /// <summary>
        /// 로그인 전에 유효성 검사 (Debug 모드일 때만 자동으로 비밀번호 지정)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <param name="useLocalhost"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool IsValid(out string id, out string password, out bool useLocalhost, out string msg)
        {
            id = txtId.Text.Trim();
            msg = "";
            useLocalhost = chkUseLocalhost.Visible && chkUseLocalhost.Checked;

            password = txtPassword.Text;
//#if DEBUG
//            if (id == "TEST01@naver.com")
//            {
//                password = "1q2w3e4r!@#";
//            }
//            else if (id == "TEST05@test.com")
//            {
//                password = "rhdxhd12";
//            }
//#endif

            if (string.IsNullOrEmpty(id))
            {
                msg = "아이디는 필수 입력입니다.";
                return false;
            }
            if (string.IsNullOrEmpty(password))
            {
                msg = "비밀번호는 필수 입력입니다.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 설정 불러와서 적용
        /// </summary>
        private void RestoreSettings()
        {
            txtId.Text = CCommon.XmlConfigUser.GetSetting("Id", "");
            chkUseLocalhost.Checked = CCommon.XmlConfigUser.GetSetting("UseLocalhost", "false") == "true";
        }
        /// <summary>
        /// 설정 저장
        /// </summary>
        private void SaveSettings()
        {
            CCommon.XmlConfigUser.SaveSetting("Id", txtId.Text);
            CCommon.XmlConfigUser.SaveSetting("UseLocalhost", chkUseLocalhost.Checked ? "true" : "false");
        }

        public void Log(string category, string log)
        {
            Console.WriteLine($"\nDEBUG>>> ({category}) \n{log}");
        }
    }
}
