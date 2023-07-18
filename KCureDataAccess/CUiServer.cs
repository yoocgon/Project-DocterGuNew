using DoctorGu;
using DoctorGu.Encryption;
using FluentFTP;
using KCureDataAccess;
using KCureVDIDataBox.Model;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace KCureVDIDataBox
{
    /// <summary>
    /// FTP 서버용 Helper (참여기관전송, 반입에서 공통 이용)
    /// </summary>
    public class CUiServer
    {
        private CFtpClient client;
        private string fileCode;
        private Form frm;
        private TextBox txtServer;
        private TreeView tvwServer;
        private ListView lvwServer;
        private ListView lvwStatus;
        private ProgressBar prgSub;
        private ProgressBar prgMain;
        private Button btnStop;
        private Button btnRequestFile;
        private bool _stop = false;
        private bool _btnRequestFileVisiblePrev = false;

        private ApplyResult applyResult;

        /// <summary>
        /// 관련된 모든 컨트롤을 초기화 시에 메모리에 저장하고 재사용하기 위해 지역 변수에 저장, 이벤트 지정
        /// </summary>
        /// <param name="forCarry"></param>
        /// <param name="frm"></param>
        /// <param name="txtServer"></param>
        /// <param name="tvwServer"></param>
        /// <param name="lvwServer"></param>
        /// <param name="lvwStatus"></param>
        /// <param name="prgSub"></param>
        /// <param name="prgMain"></param>
        /// <param name="btnStop"></param>
        /// <param name="btnRequestFile"></param>
        public CUiServer(string fileCode, Form frm, 
            TextBox txtServer, TreeView tvwServer, ListView lvwServer, ListView lvwStatus, 
            ProgressBar prgSub, ProgressBar prgMain, Button btnStop, Button btnRequestFile, ApplyResult applyResult)
        {
            client = GetFtpClient();

            this.fileCode = fileCode;
            this.frm = frm;
            this.txtServer = txtServer;
            this.tvwServer = tvwServer;
            this.lvwServer = lvwServer;
            this.lvwStatus = lvwStatus;
            this.prgSub = prgSub;
            this.prgMain = prgMain;
            this.btnStop = btnStop;
            this.btnRequestFile = btnRequestFile;

            this.applyResult = applyResult;

            this.tvwServer.BeforeExpand += tvwServer_BeforeExpand;
            this.tvwServer.BeforeSelect += tvwServer_BeforeSelect;
            this.tvwServer.NodeMouseClick += tvwServer_NodeMouseClick;
            this.lvwServer.DoubleClick += lvwServer_DoubleClick;
        }

        /// <summary>
        /// 노드를 펼치기 전에 임시 노드인 _KEY_TEMP를 제거하고 자식 노드를 추가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvwServer_BeforeExpand(object? sender, TreeViewCancelEventArgs e)
        {
            if (e.Node == null) 
                return;

            if (!e.Node.Nodes.ContainsKey(CUiCommon._KEY_TEMP)) 
                return;

            e.Node.Nodes.Clear();
            AddListItems(e.Node, CUiCommon.GetFullPathOfName(e.Node), FtpObjectType.Directory);
        }
        /// <summary>
        /// TreeView의 폴더 노드를 선택하면 해당 폴더 안의 하위 폴더와 파일 목록을 ListView에 추가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvwServer_BeforeSelect(object? sender, TreeViewCancelEventArgs e)
        {
            if (e.Node == null) 
                return;

            txtServer.Text = e.Node.FullPath;

            var tag = e.Node.Tag;
            if (tag == null || tag is not FtpListItem) 
                return;

            var ftpItem = (FtpListItem)tag;
            if (ftpItem.Type != FtpObjectType.Directory) 
                return;

            lvwServer.Items.Clear();
            List<ListViewItem> dirs = AddListItems(lvwServer, ftpItem.FullName, FtpObjectType.Directory);
            AddListItems(lvwServer, ftpItem.FullName, FtpObjectType.File, this.applyResult);

            CUiCommon.AdjustTreeChild(dirs, e.Node);
        }
        /// <summary>
        /// 우클릭 시에도 해당 노드를 선택하기 위한 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvwServer_NodeMouseClick(object? sender, TreeNodeMouseClickEventArgs e)
        {
            tvwServer.SelectedNode = e.Node;
        }

        /// <summary>
        /// ListView에서 하위 폴더를 더블클릭하면 TreeView에서도 해당 폴더를 선택하기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvwServer_DoubleClick(object? sender, EventArgs e)
        {
            var item = lvwServer.SelectedItems[0];
            if (item.Tag is not FtpListItem) 
                return;

            var ftpItem = (FtpListItem)item.Tag;
            if (ftpItem.Type != FtpObjectType.Directory) 
                return;

            CUiCommon.SelectTreeFolderByItem(tvwServer, item);
        }

        /// <summary>
        /// FTP용 클라이언트 객체를 생성하고 반환
        /// </summary>
        /// <returns></returns>
        private CFtpClient GetFtpClient()
        {
            string host = CCommon.LoginResult.nasUrl;
            string user = CCommon.LoginResult.nasId;
            string pass = CCommon.LoginResult.nasPw;
            int port = Convert.ToInt32(CCommon.LoginResult.nasPort);
            Log("host", host);
            Log("user", user);
            Log("pass", pass);
            Log("port", port.ToString());
            Encoding encoding = GetEncoding(CCommon.LoginResult.nasEncoding);
            var client = new CFtpClient(host, user, pass, port, encoding);
            return client;
        }

        /// <summary>
        /// encoding 문자열에 따른 Encoding 객체를 반환
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        private Encoding GetEncoding(string encoding)
        {
            if (encoding.ToLower() == "euc-kr")
            {
                // euc-kr: 51949
                int codepage = 51949;
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                return Encoding.GetEncoding(codepage);
            }
            else
            {
                return Encoding.GetEncoding(encoding) ?? Encoding.Default;
            }
        }

        /// <summary>
        /// 참여기관전송, 반입 여부에 따라 달라지는 루트 폴더를 반환 (없다면 생성)
        /// </summary>
        /// <param name="tvw"></param>
        public void AddRoot(TreeView tvw)
        {
            var dirs = CCommon.LoginResult.GetUserRootDirs(fileCode);
            if (dirs.Count == 0) 
                return;

            foreach (string dir in dirs)
            {
                if (!client.DirectoryExists(dir))
                {
                    client.CreateDirectory(dir);
                }
                string parentDir = CPath.GetParentPath(dir, '/');
                var items = GetListItems(parentDir, FtpObjectType.Directory).Where(v => v.FullName == dir);
                
                string dirDisplay = CCommon.LoginResult.GetUrlDirectory(Model.UrlDirectoryTypes.TrimBaseFolder, dir);
                var node = new TreeNode(dirDisplay);
                node.Name = dir;
                node.Tag = items.First();
                tvw.Nodes.Add(node);
                CUiCommon.AddTempChild(node);
            }
        }

        /// <summary>
        /// TreeView의 특정 노드의 하위 폴더와 파일 목록 노드를 추가
        /// (폴더인 경우엔 임시 노드를 추가해서 BeforeExpand 이벤트가 일어나게 함)
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parentFullPath"></param>
        /// <param name="type"></param>
        private void AddListItems(TreeNode parent, string parentFullPath, FtpObjectType type)
        {
            client.SetWorkingDirectory("/");

            foreach (var ftpItem in GetListItems(parentFullPath, type))
            {
                TreeNode node = new TreeNode(ftpItem.Name);
                node.Name = ftpItem.Name;
                node.Tag = ftpItem;
                string imageKey = type == FtpObjectType.Directory ? "folder" : "file";
                node.ImageKey = imageKey;
                node.SelectedImageKey = imageKey;
                parent.Nodes.Add(node);
                if (ftpItem.Type == FtpObjectType.Directory)
                {
                    CUiCommon.AddTempChild(node);
                }
            }
        }

        /// <summary>
        /// ListView의 특정 노드의 하위 폴더와 파일 목록 노드를 추가
        /// </summary>
        /// <param name="lvw"></param>
        /// <param name="parentFullPath"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private List<ListViewItem> AddListItems(ListView lvw, string parentFullPath, FtpObjectType type)
        {
            List<ListViewItem> list = new List<ListViewItem>();
            foreach (var ftpItem in GetListItems(parentFullPath, type))
            {
                string[] items = new string[4];
                items[(int)ColList.Name] = ftpItem.Name;
                items[(int)ColList.DateModified] = ftpItem.Modified.ToString();
                items[(int)ColList.Type] = ftpItem.Type == FtpObjectType.Directory ? "Folder" : Path.GetExtension(ftpItem.FullName);
                items[(int)ColList.Size] = ftpItem.Type == FtpObjectType.Directory ? "" : CMath.ConvertByteToTbGbMbKb(ftpItem.Size, 1);

                ListViewItem item = new ListViewItem(items);
                item.Name = ftpItem.Name;
                item.Tag = ftpItem;
                string imageKey = type == FtpObjectType.Directory ? "folder" : "file";
                item.ImageKey = imageKey;
                //
                lvw.Items.Add(item);
                list.Add(item);
            }

            return list;
        }


        /// <summary>
        /// ListView의 특정 노드의 하위 폴더와 파일 목록 노드를 추가
        /// </summary>
        /// <param name="lvw"></param>
        /// <param name="parentFullPath"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private List<ListViewItem> AddListItems(ListView lvw, string parentFullPath, FtpObjectType type, ApplyResult applyResult)
        {
            // gony
            List<string> availableRealFiles = new List<string>();
            List<string> availableFakeFiles = new List<string>();
            List<Dictionary<string, List<Data>>> listDicAutoCode = this.applyResult.authCode.ToList();
            foreach (Dictionary<string, List<Data>> dicData in listDicAutoCode)
                foreach (var keyValuePair in dicData)
                    foreach (Data data in keyValuePair.Value)
                    {
                        // string path = data?.AttfPthNm + data?.AttfNm;
                        string fileReal = data?.AttfNm;
                        string fileFake = data?.AttfStrNm;
                        if (!availableFakeFiles.Contains(fileFake))
                        {
                            availableRealFiles.Add(fileReal);
                            availableFakeFiles.Add(fileFake);
                        }
                    }
            //
            List<ListViewItem> list = new List<ListViewItem>();
            foreach (var ftpItem in GetListItems(parentFullPath, type))
            {
                string[] items = new string[4];
                //
                if (!availableFakeFiles.Contains(ftpItem.Name))
                    continue;
                //
                int index = availableFakeFiles.FindIndex(elem => elem == ftpItem.Name);
                string realName = availableRealFiles[index];
                //
                items[(int)ColList.Name] = realName;
                items[(int)ColList.DateModified] = ftpItem.Modified.ToString();
                items[(int)ColList.Type] = ftpItem.Type == FtpObjectType.Directory ? "Folder" : Path.GetExtension(ftpItem.FullName);
                items[(int)ColList.Size] = ftpItem.Type == FtpObjectType.Directory ? "" : CMath.ConvertByteToTbGbMbKb(ftpItem.Size, 1);
                //
                ListViewItem item = new ListViewItem(items);
                item.Name = ftpItem.Name;
                item.Tag = ftpItem;
                string imageKey = type == FtpObjectType.Directory ? "folder" : "file";
                item.ImageKey = imageKey;
                //
                lvw.Items.Add(item);
                list.Add(item);
            }

            return list;
        }

        /// <summary>
        /// <paramref name="parentFullPath"/>의 하위 폴더와 파일 목록을 이름 순으로 반환
        /// <paramref name="type"/>에 해당하는 것만 반환
        /// </summary>
        /// <param name="parentFullPath"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private List<FtpListItem> GetListItems(string parentFullPath, FtpObjectType type)
        {
            using (new CWaitCursor(frm, new Control[] { tvwServer, lvwServer, btnRequestFile }))
            {
                var list = client.GetListing(parentFullPath, FtpListOption.Auto)
                            .Where(item => item.Type == type)
                            .OrderBy(item => item.Name)
                            .ToList();
                return list;
            }
        }
        /// <summary>
        /// <paramref name="parentFullPath"/>의 하위 폴더와 파일 목록을 이름 순으로 반환
        /// </summary>
        /// <param name="parentFullPath"></param>
        /// <returns></returns>
        private List<FtpListItem> GetListItems(string parentFullPath)
        {
            using (new CWaitCursor(frm, new Control[] { tvwServer, lvwServer, btnRequestFile }))
            {
                var list = client.GetListing(parentFullPath, FtpListOption.Auto)
                            .OrderBy(item => item.Name)
                            .ToList();
                return list;
            }
        }
        
        /// <summary>
        /// 파일을 다운로드 (폴더인 경우엔 재귀 호출 사용)
        /// </summary>
        /// <param name="localDirectory">파일이 다운로드될 로컬 폴더</param>
        /// <param name="ftpItems">다운로드할 폴더 또는 파일 목록</param>
        /// <param name="localExistsAll">
        /// 다운로드할 파일이 존재할 경우에 덮어쓰기, 이어쓰기 등을 할 지의 옵션
        /// (이후 모든 파일에 적용하기 위함)
        /// </param>
        /// <returns></returns>
        public (bool, long, int) DownloadFiles(string localDirectory, List<FtpListItem> ftpItems, FtpLocalExists? localExistsAll = null)
        {
            long fileSizeTotal = 0;
            int fileCount = 0;
            using (new CWaitCursor(frm, new Control[] { tvwServer, lvwServer, btnRequestFile }))
            {
                // gony
                List<string> availableRealFiles = new List<string>();
                List<string> availableFakeFiles = new List<string>();
                List<Dictionary<string, List<Data>>> listDicAutoCode = this.applyResult.authCode.ToList();
                foreach (Dictionary<string, List<Data>> dicData in listDicAutoCode)
                    foreach (var keyValuePair in dicData)
                        foreach (Data data in keyValuePair.Value)
                        {
                            // string path = data?.AttfPthNm + data?.AttfNm;
                            string fileReal = data?.AttfNm;
                            string fileFake = data?.AttfStrNm;
                            if (!availableFakeFiles.Contains(fileFake))
                            {
                                availableRealFiles.Add(fileReal);
                                availableFakeFiles.Add(fileFake);
                            }
                        }
                //
                Action<FtpProgress> progress = delegate (FtpProgress p)
                {
                    Debug.WriteLine($"{p.FileCount}, {p.FileIndex}, {p.TransferredBytes}, {p.LocalPath}, {p.RemotePath}");
                    var ftpItem = ftpItems.FirstOrDefault(item => item.FullName == p.RemotePath);
                    if (ftpItem == null) 
                        return;
                    //
                    var processed = ftpItem.Size > 0 ? p.TransferredBytes * 100 / ftpItem.Size : 0;
                    prgSub.Value = Convert.ToInt32(processed);
                };
                //
                for (int i = 0; i < ftpItems.Count; i++)
                {
                    FtpListItem ftpItem = ftpItems[i];
                    if (ftpItem.Type == FtpObjectType.File)
                    {
                        fileSizeTotal += ftpItem.Size;
                        fileCount++;
                        //
                        if (!availableFakeFiles.Contains(ftpItem.Name))
                            continue;
                        // gony
                        int index = availableFakeFiles.FindIndex(elem => elem == ftpItem.Name);
                        string realName = availableRealFiles[index];
                        string localFullPath = $"{localDirectory}{Path.DirectorySeparatorChar}{realName}";
                        string remoteFullPath = ftpItem.FullName.Replace(ftpItem.Name, realName);
                        //
                        // string localFullPath = $"{localDirectory}{Path.DirectorySeparatorChar}{ftpItem.Name}";
                        //
                        string[] items = new string[4];
                        items[(int)ColStatus.Local] = localFullPath;
                        items[(int)ColStatus.Remote] = remoteFullPath;
                        //items[(int)ColStatus.Remote] = CCommon.LoginResult.GetUrlDirectory(UrlDirectoryTypes.TrimBaseFolder, ftpItem.FullName);
                        items[(int)ColStatus.Size] = CMath.ConvertByteToTbGbMbKb(ftpItem.Size, 1);
                        items[(int)ColStatus.Status] = "";
                        ListViewItem item = new ListViewItem(items);
                        item.Name = localFullPath;
                        lvwStatus.Items.Add(item);
                        Application.DoEvents();

                        var (localExistsCur, localExistsAllCur) = GetLocalExists(localFullPath, localExistsAll);
                        var localExists = localExistsCur;
                        if (localExistsAllCur != null) localExistsAll = localExistsAllCur;
                        client.DownloadFile(localFullPath, ftpItem.FullName, localExists, progress);

                        prgMain.Value = fileCount * 100 / (fileCount + (ftpItems.Count - i + 1));
                        item.SubItems[(int)ColStatus.Status].Text = "OK";
                        item.EnsureVisible();
                        if (_stop) 
                            return (false, fileSizeTotal, fileCount);
                    }
                    else if (ftpItem.Type == FtpObjectType.Directory)
                    {
                        var ftpItemsSub = GetListItems(ftpItem.FullName);
                        var (completedSub, fileSizeTotalSub, fileCountSub) = DownloadFiles(Path.Combine(localDirectory, ftpItem.Name), ftpItemsSub, localExistsAll);
                        fileSizeTotal += fileSizeTotalSub;
                        fileCount += fileCountSub;
                        if (!completedSub)
                        {
                            return (false, fileSizeTotal, fileCount);
                        }
                    }
                }
            }

            return (true, fileSizeTotal, fileCount);
        }

        /// <summary>
        /// 파일을 업로드
        /// </summary>
        /// <param name="remoteDirectory">업로드할 FTP 폴더</param>
        /// <param name="di">업로드할 파일을 가진 로컬 폴더 (FTP 폴더명을 정하기 위해 사용)</param>
        /// <param name="files">업로드할 로컬 파일</param>
        /// <returns></returns>
        public (bool, List<FileInfo>) UploadFiles(string remoteDirectory, DirectoryInfo di, FileInfo[] files)
        {
            string rootFolder = di.FullName;
            var filesSent = new List<FileInfo>();

            FtpRemoteExists? remoteExistsAll = null;
            using (new CWaitCursor(frm, new Control[] { tvwServer, lvwServer, btnRequestFile }))
            {
                Action<FtpProgress> progress = delegate (FtpProgress p)
                {
                    Debug.WriteLine($"{p.FileCount}, {p.FileIndex}, {p.TransferredBytes}, {p.LocalPath}, {p.RemotePath}");
                    var file = files.FirstOrDefault(item => item.FullName == p.LocalPath);
                    if (file == null) return;

                    var processed = file.Length > 0 ? p.TransferredBytes * 100 / file.Length : 0;
                    prgSub.Value = Convert.ToInt32(processed);
                    Log("progress", prgSub.Value.ToString());
                };

                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo file = files[i];

                    string remoteFullPath = CPath.GetRemotePath(remoteDirectory, rootFolder, file.FullName);

                    string[] items = new string[4];
                    items[(int)ColStatus.Local] = file.FullName;
                    items[(int)ColStatus.Remote] = CCommon.LoginResult.GetUrlDirectory(UrlDirectoryTypes.TrimBaseFolder, remoteFullPath);
                    items[(int)ColStatus.Size] = CMath.ConvertByteToTbGbMbKb(file.Length, 1);
                    items[(int)ColStatus.Status] = "";
                    ListViewItem item = new ListViewItem(items);
                    item.Name = file.FullName;
                    lvwStatus.Items.Add(item);
                    Application.DoEvents();

                    var (remoteExistsCur, remoteExistsAllCur) = GetRemoteExists(remoteFullPath, remoteExistsAll);
                    var remoteExists = remoteExistsCur;
                    if (remoteExistsAllCur != null) remoteExistsAll = remoteExistsAllCur;
                    string dir = CPath.GetFolderName(remoteFullPath, '/');
                    if (!client.DirectoryExists(dir))
                    {
                        client.CreateDirectory(dir);
                    }
                    Log("upload file", "start");
                    client.UploadFile(file.FullName, remoteFullPath, remoteExists, progress);
                    Log("upload file", "done");

                    filesSent.Add(file);

                    prgMain.Value = (i + 1) * 100 / files.Length;
                    item.SubItems[(int)ColStatus.Status].Text = "OK";
                    item.EnsureVisible();
                    if (_stop) 
                        return (false, filesSent);
                }
            }

            return (true, filesSent);
        }

        /// <summary>
        /// FTP 폴더의 모든 하위 폴더와 파일을 재귀적으로 삭제
        /// </summary>
        /// <param name="remoteDirectory"></param>
        public void DeleteFiles(string remoteDirectory)
        {
            using (new CWaitCursor(frm, new Control[] { tvwServer, lvwServer, btnRequestFile }))
            {
                if (!client.DirectoryExists(remoteDirectory)) 
                    return;

                var items = client.GetListing(remoteDirectory, FtpListOption.AllFiles);
                foreach (var item in items)
                {
                    if (item.Type == FtpObjectType.Directory)
                    {
                        client.DeleteDirectory(item.FullName);
                    }
                    else
                    {
                        client.DeleteFile(item.FullName);
                    }
                }
            }
        }

        /// <summary>
        /// 다운로드할 파일이 로컬에 존재한다면 무엇을 할지 선택하고 그 결과를 localExistsAll에 저장
        /// (localExistsAll에 값이 있다면 모든 파일에 대해 동일한 작업을 수행)
        /// </summary>
        /// <param name="localFullPath"></param>
        /// <param name="localExistsAll"></param>
        /// <returns></returns>
        private (FtpLocalExists, FtpLocalExists?) GetLocalExists(string localFullPath, FtpLocalExists? localExistsAll)
        {
            FtpLocalExists localExistsCur = FtpLocalExists.Skip;
            FtpLocalExists? localExistsAllCur = null;

            if (localExistsAll != null)
            {
                localExistsCur = localExistsAll.Value;
                localExistsAllCur = localExistsAll.Value;
            }
            else
            {
                if (File.Exists(localFullPath))
                {
                    var dlg = new ChooseFtpExists();
                    dlg.FullPath = localFullPath;
                    var dret = dlg.ShowDialog(frm);
                    if (dret == DialogResult.OK)
                    {
                        localExistsCur = dlg.LocalExists;
                        if (dlg.ApplyToAll)
                        {
                            localExistsAllCur = localExistsCur;
                        }
                    }
                }
            }

            return (localExistsCur, localExistsAllCur);
        }

        /// <summary>
        /// 업로드할 파일이 원격지에 존재한다면 무엇을 할지 선택하고 그 결과를 remoteExistsAll에 저장
        /// (remoteExistsAll에 값이 있다면 모든 파일에 대해 동일한 작업을 수행)
        /// </summary>
        /// <param name="remoteFullPath"></param>
        /// <param name="remoteExistsAll"></param>
        /// <returns></returns>
        private (FtpRemoteExists, FtpRemoteExists?) GetRemoteExists(string remoteFullPath, FtpRemoteExists? remoteExistsAll)
        {
            FtpRemoteExists remoteExistsCur = FtpRemoteExists.Skip;
            FtpRemoteExists? remoteExistsAllCur = null;

            if (remoteExistsAll != null)
            {
                remoteExistsCur = remoteExistsAll.Value;
                remoteExistsAllCur = remoteExistsAll.Value;
            }
            else
            {
                string remoteDir = CPath.GetFolderName(remoteFullPath, '/');
                // DirectoryExists needed first because FileExists raise error when no directory exists
                if (client.DirectoryExists(remoteDir) && client.FileExists(remoteFullPath))
                {
                    var dlg = new ChooseFtpExists();
                    dlg.FullPath = remoteFullPath;
                    var dret = dlg.ShowDialog(frm);
                    if (dret == DialogResult.OK)
                    {
                        remoteExistsCur = dlg.RemoteExists;
                        if (dlg.ApplyToAll)
                        {
                            remoteExistsAllCur = remoteExistsCur;
                        }
                    }
                }
            }

            return (remoteExistsCur, remoteExistsAllCur);
        }

        /// <summary>
        /// 파일 업로드가 시작되었으므로 UI를 변경하고 _stop을 false로 설정
        /// </summary>
        public void Start()
        {
            _stop = false;

            _btnRequestFileVisiblePrev = btnRequestFile.Visible;

            lvwStatus.Items.Clear();
            prgSub.Visible = true;
            prgMain.Visible = true;
            btnStop.Visible = true;
            btnRequestFile.Visible = false;
        }
        /// <summary>
        /// 파일 업로드가 중지되었으므로 UI를 변경하고 _stop을 true로 설정
        /// </summary>
        public void Stop()
        {
            _stop = true;

            prgSub.Visible = false;
            prgMain.Visible = false;
            btnStop.Visible = false;
            btnRequestFile.Visible = _btnRequestFileVisiblePrev;
        }

        /// <summary>
        /// 파일 반출에 사용될 키인 데이터신청번호를 반환
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string GetDataAplcNo(out string msg)
        {
            msg = "";

            if (tvwServer.SelectedNode == null)
            {
                msg = "참여기관전송 폴더를 선택하지 않았습니다.";
                return "";
            }

            string fullPath = CUiCommon.GetFullPathOfName(tvwServer.SelectedNode);

            var dataAplcNos = CCommon.LoginResult.GetDataAplcNos(fileCode);
            var found = dataAplcNos.FirstOrDefault(v => fullPath.IndexOf(v) != -1);
            if (found == null)
            {
                msg = "데이터신청번호를 찾을 수 없습니다.";
                return "";
            }

            return found;
        }

        public void Log(string category, string log)
        {
            Console.WriteLine($"\nDEBUG>>> ({category}) \n{log}");
        }

    }
}
