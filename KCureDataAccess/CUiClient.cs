using DoctorGu;
using FluentFTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCureVDIDataBox
{
    /// <summary>
    /// 로컬 파일용 Helper
    /// </summary>
    public class CUiClient
    {
        private TextBox txtClient;
        private TreeView tvwClient;
        private ListView lvwClient;

        /// <summary>
        /// 관련된 모든 컨트롤을 초기화 시에 메모리에 저장하고 재사용하기 위해 지역 변수에 저장, 이벤트 지정
        /// </summary>
        /// <param name="txtClient"></param>
        /// <param name="tvwClient"></param>
        /// <param name="lvwClient"></param>
        public CUiClient(TextBox txtClient, TreeView tvwClient, ListView lvwClient) {
            this.txtClient = txtClient;
            this.tvwClient = tvwClient;
            this.lvwClient = lvwClient;

            this.tvwClient.BeforeExpand += tvwClient_BeforeExpand;
            this.tvwClient.BeforeSelect += tvwClient_BeforeSelect;
            this.tvwClient.NodeMouseClick += tvwClient_NodeMouseClick;
            this.lvwClient.DoubleClick += lvwClient_DoubleClick;
        }

        /// <summary>
        /// 노드를 펼치기 전에 임시 노드인 _KEY_TEMP를 제거하고 자식 노드를 추가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvwClient_BeforeExpand(object? sender, TreeViewCancelEventArgs e)
        {
            if (e.Node == null) return;

            var tag = e.Node.Tag;
            if (tag == null) return;
            
            if (!e.Node.Nodes.ContainsKey(CUiCommon._KEY_TEMP)) return;

            e.Node.Nodes.Clear();
            if (tag is DriveInfo)
            {
                DriveInfo drive = (DriveInfo)tag;
                AddDirectories(e.Node, drive.Name);
            }
            else if (tag is DirectoryInfo)
            {
                DirectoryInfo di = (DirectoryInfo)tag;
                AddDirectories(e.Node, di.FullName);
            }
        }
        /// <summary>
        /// TreeView의 폴더 노드를 선택하면 해당 폴더 안의 하위 폴더와 파일 목록을 ListView에 추가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvwClient_BeforeSelect(object? sender, TreeViewCancelEventArgs e)
        {
            if (e.Node == null) return;

            txtClient.Text = e.Node.FullPath;

            var tag = e.Node.Tag;
            if (tag == null) return;

            string parentFullName = "";
            if (tag is DriveInfo)
            {
                DriveInfo drive = (DriveInfo)tag;
                parentFullName = drive.Name;
            }
            else if (tag is DirectoryInfo)
            {
                DirectoryInfo di = (DirectoryInfo)tag;
                parentFullName = di.FullName;
            }

            if (parentFullName != "")
            {
                lvwClient.Items.Clear();
                List<ListViewItem> dirs = AddDirectories(lvwClient, parentFullName);
                AddFiles(lvwClient, parentFullName);

                CUiCommon.AdjustTreeChild(dirs, e.Node);
            }
        }
        /// <summary>
        /// 우클릭 시에도 해당 노드를 선택하기 위한 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvwClient_NodeMouseClick(object? sender, TreeNodeMouseClickEventArgs e)
        {
            tvwClient.SelectedNode = e.Node;
        }

        /// <summary>
        /// ListView에서 하위 폴더를 더블클릭하면 TreeView에서도 해당 폴더를 선택하기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvwClient_DoubleClick(object? sender, EventArgs e)
        {
            var item = lvwClient.SelectedItems[0];
            if (item.Tag is not DirectoryInfo)
            {
                return;
            }

            CUiCommon.SelectTreeFolderByItem(tvwClient, item);
        }

        /// <summary>
        /// 고정 드라이브 목록을 TreeView에 추가
        /// </summary>
        /// <param name="tvw"></param>
        public void AddDrives(TreeView tvw)
        {
            var drives = CCommon.GetDrives();
            foreach (var drive in drives)
            {
                string text = drive.Name.TrimEnd('\\');
                TreeNode node = new TreeNode(text);
                node.Name = text;
                node.Tag = drive;
                node.ImageKey = "disk";
                node.SelectedImageKey = "disk";
                tvw.Nodes.Add(node);
                CUiCommon.AddTempChild(node);
            }
        }

        /// <summary>
        /// 상위 폴더에 대한 하위 폴더 목록을 TreeView에 추가
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="parentFullPath"></param>
        public void AddDirectories(TreeNode parent, string parentFullPath)
        {
            foreach (var dir in GetDirectories(parentFullPath))
            {
                TreeNode node = new TreeNode(dir.Name);
                node.Name = dir.Name;
                node.Tag = dir;
                node.ImageKey = "folder";
                node.SelectedImageKey = "folder";
                parent.Nodes.Add(node);
                CUiCommon.AddTempChild(node);
            }
        }

        /// <summary>
        /// 상위 폴더에 대한 하위 폴더 목록을 ListView에 추가
        /// </summary>
        /// <param name="lvw"></param>
        /// <param name="parentFullPath"></param>
        /// <returns></returns>
        public List<ListViewItem> AddDirectories(ListView lvw, string parentFullPath)
        {
            List<ListViewItem> list = new List<ListViewItem>();
            foreach (var dir in GetDirectories(parentFullPath))
            {
                string[] items = new string[4];
                items[(int)ColList.Name] = dir.Name;
                items[(int)ColList.DateModified] = dir.LastWriteTime.ToString();
                items[(int)ColList.Type] = "Folder";
                items[(int)ColList.Size] = "";
                
                ListViewItem item = new ListViewItem(items);
                item.Name = dir.Name;
                item.Tag = dir;
                item.ImageKey = "folder";
                lvw.Items.Add(item);

                list.Add(item);
            }

            return list;
        }

        /// <summary>
        /// 상위 폴더에 대한 파일 목록을 ListView에 추가
        /// </summary>
        /// <param name="lvw"></param>
        /// <param name="parentFullPath"></param>
        public void AddFiles(ListView lvw, string parentFullPath)
        {
            foreach (var file in GetFiles(parentFullPath))
            {
                string[] items = new string[4];
                items[(int)ColList.Name] = file.Name;
                items[(int)ColList.DateModified] = file.LastWriteTime.ToString();
                items[(int)ColList.Type] = file.Extension;
                items[(int)ColList.Size] = CMath.ConvertByteToTbGbMbKb(file.Length, 1);

                ListViewItem item = new ListViewItem(items);
                item.Name = file.Name;
                item.Tag = file;
                item.ImageKey = "file";

                lvw.Items.Add(item);
            }
        }

        /// <summary>
        /// 상위 폴더에 대한 하위 폴더 목록을 반환
        /// </summary>
        /// <param name="parentFullPath"></param>
        /// <returns></returns>
        public List<DirectoryInfo> GetDirectories(string parentFullPath)
        {
            DirectoryInfo di = new DirectoryInfo(parentFullPath);
            DirectoryInfo[] dirs = new DirectoryInfo[0];
            try
            {
                dirs = di.GetDirectories("*", SearchOption.TopDirectoryOnly);
            }
            catch (Exception) { }
            if (dirs.Length == 0) return dirs.ToList();

            return dirs.Where(dir => (dir.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden
                    || (dir.Attributes & FileAttributes.System) != FileAttributes.System
                    || (dir.Attributes & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint)
                .OrderBy(dir => dir.Name)
                .ToList();
        }
        /// <summary>
        /// 상위 폴더에 대한 파일 목록을 반환
        /// </summary>
        /// <param name="parentFullPath"></param>
        /// <returns></returns>
        public List<FileInfo> GetFiles(string parentFullPath)
        {
            DirectoryInfo di = new DirectoryInfo(parentFullPath);
            FileInfo[] files = new FileInfo[0];
            try
            {
                files = di.GetFiles("*", SearchOption.TopDirectoryOnly);
            }
            catch (Exception) { }
            if (files.Length == 0) return files.ToList();

            return files.Where(dir => (dir.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden
                    || (dir.Attributes & FileAttributes.System) != FileAttributes.System
                    || (dir.Attributes & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint)
                .OrderBy(file => file.Name)
                .ToList();
        }

        /// <summary>
        /// 파일 반출에 사용할 폴더를 생성
        /// </summary>
        public void CreateDefaultFolder()
        {
            string fullPath = CCommon.GetDefaultFullPath();
            DirectoryInfo di = new DirectoryInfo(fullPath);
            if (!di.Exists)
            {
                di.Create();
            }
        }

        /// <summary>
        /// 파일 반출에 사용할 폴더를 선택
        /// </summary>
        public void SelectDefaultFolder()
        {
            var root = tvwClient.Nodes[0];
            root.Expand();

            string defaultFullPath = CCommon.GetDefaultFullPath();
            var nodeDefault = CUiCommon.FirstNodeOfFullPath(root, defaultFullPath);
            if (nodeDefault == null) return;

            this.tvwClient.SelectedNode = nodeDefault;
            nodeDefault.Expand();
        }
    }
}
