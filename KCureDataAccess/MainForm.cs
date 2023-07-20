using DoctorGu;
using FluentFTP;
using KCureVDIDataBox.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KCureVDIDataBox
{
    /// <summary>
    /// 메인 폼
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>로컬</summary>
        private CUiClient uiClient;
        /// <summary>참여기관전송</summary>
        private CUiServer uiServerPrti;
        /// <summary>반입</summary>
        private CUiServer uiServerCarry;

        public MainForm()
        {
            try
            {
                InitializeComponent();

                this.Text = Application.ProductName;

                ApplyResult? applyResult = CCommon.LoginResult.applyResult;

                // 각 파일, FTP용 클래스 생성
                uiClient = new CUiClient(txtClient, tvwClient, lvwClient, CCommon.LoginId);
                uiServerPrti = new CUiServer("03", this, txtServerPrti, tvwServerPrti, lvwServerPrti, lvwStatus,
                    prgSub, prgMain, btnStop, btnRequestFile, applyResult);
                uiServerCarry = new CUiServer("04", this, txtServerCarry, tvwServerCarry, lvwServerCarry, lvwStatus,
                    prgSub, prgMain, btnStop, btnRequestFile, applyResult);
            }
            catch (Exception ex)
            {
                CCommon.Log.WriteLog(ex);
                throw new Exception("Main", ex);
            }
        }

        /// <summary>
        /// 폼이 로드될 때 필요한 초기 설정을 실행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                tsslLoginId.Text = CCommon.LoginId;
                btnRequestFile.Visible = CCommon.LoginResult.HasAuthCode(CCommon.Const.USER_AU006);
                CCommon.WriteLog(txtLog, $"{CCommon.LoginId} 로그인");

                RestoreSettings();

                uiClient.CreateDefaultFolder();
                uiClient.AddDrives(tvwClient);
                uiClient.SelectDefaultFolder();

                uiServerPrti.AddRoot(tvwServerPrti);
                uiServerCarry.AddRoot(tvwServerCarry);
            }
            catch (Exception ex)
            {
                CCommon.Log.WriteLog(ex);
            }
        }
        /// <summary>
        /// 폼이 종료되기 전 설정을 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        /// <summary>
        /// 파일의 다운로드/업로드를 중지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, EventArgs e)
        {
            uiServerPrti.Stop();
            uiServerCarry.Stop();
        }

        /// <summary>
        /// 반출 신청
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnRequestFile_Click(object sender, EventArgs e)
        {
            DirectoryInfo di;
            string msg;
            if (!IsValid(out di, out msg))
            {
                MessageBox.Show(this, msg, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string dataAplyNo = uiServerPrti.GetDataAplcNo(out msg);
            if (string.IsNullOrEmpty(dataAplyNo))
            {
                MessageBox.Show(this, msg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string remoteDir = CCommon.LoginResult.GetAplyUrl(dataAplyNo, CCommon.LoginId);
            string remoteDirDisplay = CCommon.LoginResult.GetUrlDirectory(UrlDirectoryTypes.TrimBaseFolder, remoteDir);

            Log("remoteDir", remoteDir);
            Log("remoteDirDisplay", remoteDirDisplay);

            msg = $"다음과 같이 반출 신청하시겠습니까?\n\n데이터신청번호: {dataAplyNo}\n\n{di.FullName}\n->\n{remoteDirDisplay}";
            var dret = MessageBox.Show(this, msg, "Question",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (dret != DialogResult.OK) return;

            FileInfo[] files = GetFilesToRequest(di);

            var tm = new CTimeMeasure();
            CCommon.WriteLog(txtLog, "파일 업로드 시작");
            uiServerPrti.Start();

            CCommon.WriteLog(txtLog, $"{remoteDirDisplay} 폴더의 파일 삭제");
            uiServerPrti.DeleteFiles(remoteDir);

            CCommon.WriteLog(txtLog, $"{remoteDirDisplay} 폴더에 파일 업로드");
            (bool completed, List<FileInfo> filesSent) = uiServerPrti.UploadFiles(remoteDir, di, files);
            uiServerPrti.Stop();
            int fileCount = filesSent.Count;
            long fileSizeTotal = filesSent.Sum(f => f.Length);
            CCommon.WriteLog(txtLog, $"파일 업로드 {(completed ? "완료" : "중단")} (개수: {fileCount}, 크기: {CMath.ConvertByteToTbGbMbKb(fileSizeTotal, 1)}, 시간: {tm.GetElapsedText()})");
            //
            if (!completed)
            {
                CCommon.WriteLog(txtLog, $"{remoteDirDisplay} 폴더의 파일 삭제");
                uiServerPrti.DeleteFiles(remoteDir);
                DeleteZipFileInDefaultDirectory(di);
                MessageBox.Show(this, "반출 신청이 취소되었습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //
            List<string> cryOutFiles = files
                                    .Select(f => CCommon.LoginResult
                                    .GetUrlDirectory(UrlDirectoryTypes.TrimBaseFolder,
                                       CPath.GetRemotePath(remoteDir, di.FullName, Path.Combine(di.FullName, f.Name)))).ToList();
            //
            (bool success, string errorMsg, AplyResponse response) = await CCommon.Api.Aply(dataAplyNo, CCommon.LoginId, cryOutFiles);
            Log("dataAplyNo", dataAplyNo);
            Log("CCommon.LoginId", CCommon.LoginId);
            foreach (string file in cryOutFiles)
                Log("file", file);
            //
            Log("api result", success.ToString());
            Log("api error message", errorMsg);
            Log("api result", response.result.getString());
            if (!success)
            {
                CCommon.WriteLog(txtLog, $"{remoteDirDisplay} 폴더의 파일 삭제");
                uiServerPrti.DeleteFiles(remoteDir);
                DeleteZipFileInDefaultDirectory(di);
                MessageBox.Show(this, errorMsg, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //
            var selectedNode = tvwServerPrti.SelectedNode;
            tvwServerPrti.SelectedNode = null;
            tvwServerPrti.SelectedNode = selectedNode;
            //
            DeleteZipFileInDefaultDirectory(di);
            MessageBox.Show(this, "반출 신청이 완료되었습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 종료 단추
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 팝업메뉴를 열기 전 이벤트 (팝업메뉴의 하위 메뉴의 Visible 속성을 설정)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuPopup_Opening(object sender, CancelEventArgs e)
        {
            if (!SetVisibleBySource(mnuPopup.SourceControl))
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 팝업메뉴에서 다운로드를 선택하면 선택한 파일 또는 폴더 안의 파일을 다운로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiDownload_Click(object sender, EventArgs e)
        {
            var ctl = mnuPopup.SourceControl;
            if (ctl == null)
                return;
            //
            bool forCarry = ctl.Name == tvwServerCarry.Name || ctl.Name == lvwServerCarry.Name;
            TreeView tvwServer = !forCarry ? tvwServerPrti : tvwServerCarry;
            ListView lvwServer = !forCarry ? lvwServerPrti : lvwServerCarry;
            CUiServer uiServer = !forCarry ? uiServerPrti : uiServerCarry;
            //
            string localDirectory = tvwClient.SelectedNode.FullPath;
            List<FtpListItem> ftpItems = new List<FtpListItem>();
            if (ctl.Name == tvwServer.Name)
            {
                FtpListItem ftpItem = (FtpListItem)tvwServer.SelectedNode.Tag;
                ftpItems.Add(ftpItem);
            }
            else if (ctl.Name == lvwServer.Name)
            {
                ftpItems = lvwServer.SelectedItems.OfType<ListViewItem>()
                                    .Select(item => (FtpListItem)item.Tag).ToList();
            }
            //
            if (ftpItems.Count == 0)
            {
                MessageBox.Show(this, "다운로드할 파일이 없습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //
            var tm = new CTimeMeasure();
            CCommon.WriteLog(txtLog, "다운로드 시작");
            uiServer.Start();
            (bool completed, long fileSizeTotal, int fileCount) = uiServer.DownloadFiles(localDirectory, ftpItems);
            uiServer.Stop();
            CCommon.WriteLog(txtLog, $"다운로드 {(completed ? "완료" : "중단")} (개수: {fileCount}, 크기: {CMath.ConvertByteToTbGbMbKb(fileSizeTotal, 1)}, 시간: {tm.GetElapsedText()})");
            //
            string zipFullPath;
            string extractFullDir;
            bool extracted = CUiCommon.ExtractIfOneZip(completed, localDirectory, ftpItems, this, out zipFullPath, out extractFullDir);
            if (extracted)
            {
                CCommon.WriteLog(txtLog, $"압축 풀기 완료\n{zipFullPath} ->\n{extractFullDir}");
            }
            // gony
            uiClient.UpdateLastSelectedTreeViewNode();
        }
        /// <summary>
        /// 팝업메뉴에서 열기를 선택하면 선택된 파일 또는 폴더를 탐색기에서 열기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiOpen_Click(object sender, EventArgs e)
        {
            var ctl = mnuPopup.SourceControl;
            if (ctl == null) return;

            if (ctl.Name == tvwClient.Name)
            {
                Process.Start("explorer.exe", tvwClient.SelectedNode.FullPath);
            }
            else if (ctl.Name == lvwClient.Name)
            {
                var tag = lvwClient.SelectedItems[0].Tag;
                if (tag == null) return;
                if (tag is DirectoryInfo)
                {
                    var di = (DirectoryInfo)tag;
                    Process.Start("explorer.exe", di.FullName);
                }
                else if (tag is FileInfo)
                {
                    var fi = (FileInfo)tag;
                    Process.Start("explorer.exe", fi.FullName);
                }
            }
        }

        /// <summary>
        /// 반출 신청 전 유효성 검사
        /// </summary>
        /// <param name="di"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool IsValid(out DirectoryInfo di, out string msg)
        {
            string localDirectory = CCommon.GetDefaultFullPath();
            msg = "";

            string defaultFullPath = CCommon.GetDefaultFullPath();
            di = new DirectoryInfo(defaultFullPath);

            if (!di.Exists)
            {
                msg = $"{defaultFullPath} 경로가 존재하지 않습니다.";
                return false;
            }

            if (di.GetFiles("*", SearchOption.AllDirectories).Length == 0)
            {
                msg = $"{defaultFullPath} 경로에 파일이 없습니다.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 반출 신청 대상 파일을 리턴함 (설정에 따라 압축파일이나 전체 파일로 구분)
        /// </summary>
        /// <param name="di"></param>
        /// <returns></returns>
        private FileInfo[] GetFilesToRequest(DirectoryInfo di)
        {
            if (CCommon.GetAppSetting<bool>(ConfigAppKeys.CompressWhenRequest, false))
            {
                string zipFullPath = CCommon.CreateZip(di.FullName);
                return new FileInfo[] { new FileInfo(zipFullPath) };
            }
            else
            {
                return di.GetFiles("*", SearchOption.AllDirectories);
            }
        }
        /// <summary>
        /// 반출 신청용 기본 압축 파일을 삭제
        /// </summary>
        /// <param name="di"></param>
        private void DeleteZipFileInDefaultDirectory(DirectoryInfo di)
        {

            string zipFileName = CCommon.GetAppSetting<string>(ConfigAppKeys.DefaultZipFileName, "");
            string fullPath = Path.Combine(di.FullName, zipFileName);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        /// <summary>
        /// 팝업 메뉴의 하위 메뉴를 현재 선택한 컨트롤에 따라 보이거나 숨김
        /// </summary>
        /// <param name="ctl">선택한 컨트롤</param>
        /// <returns></returns>
        private bool SetVisibleBySource(Control? ctl)
        {
            if (ctl == null)
                return false;

            var name = ctl.Name;
            if (name != tvwClient.Name
                && name != lvwClient.Name
                && name != tvwServerPrti.Name
                && name != lvwServerPrti.Name
                && name != tvwServerCarry.Name
                && name != lvwServerCarry.Name)
            {
                return false;
            }

            tsmiDownload.Visible = name == tvwServerPrti.Name || name == lvwServerPrti.Name
                                || name == tvwServerCarry.Name || name == lvwServerCarry.Name;
            tsmiOpen.Visible = name == tvwClient.Name || name == lvwClient.Name;
            return true;
        }

        /// <summary>
        /// UI 변경 정보를 XML 파일에서 읽어와 적용
        /// </summary>
        private void RestoreSettings()
        {
            double distanceRatio = Convert.ToDouble(CCommon.XmlConfigUser.GetSetting("BodyContentSplitterDistanceRatio", 0.5));
            if (distanceRatio < 0.1 || distanceRatio > 0.9)
            {
                distanceRatio = 0.5;
            }
            int distance = Convert.ToInt32(this.Width * distanceRatio);
            splitBodyContent.SplitterDistance = distance;
        }
        /// <summary>
        /// UI 변경 정보를 XML 파일에 저장
        /// </summary>
        private void SaveSettings()
        {
            double distanceRatio = splitBodyContent.SplitterDistance * 1.0 / this.Width;
            CCommon.XmlConfigUser.SaveSetting("BodyContentSplitterDistanceRatio", distanceRatio);
        }


        public void Log(string category, string log)
        {
            Console.WriteLine($"\nDEBUG>>> ({category}) \n{log}");
        }
    }
}
