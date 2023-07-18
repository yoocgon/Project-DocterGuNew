using DoctorGu;
using FluentFTP;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Windows.Forms.ListView;

namespace KCureVDIDataBox
{
    public enum ColStatus
    {
        Local,
        Remote,
        Size,
        Status
    }
    public enum ColList
    {
        Name,
        DateModified,
        Type,
        Size
    }

    /// <summary>
    /// UI에서 공통으로 사용하는 기능
    /// </summary>
    public class CUiCommon
    {
        public const string _KEY_TEMP = "_KEY_TEMP";

        public static void AddTempChild(TreeNode parent)
        {
            parent.Nodes.Clear();
            parent.Nodes.Add(new TreeNode() { Text = _KEY_TEMP, Name = _KEY_TEMP });
        }

        public static TreeNode? FirstNodeOfFullPath(TreeNode parent, string fullPath)
        {
            if (parent.FullPath == fullPath)
            {
                return parent;
            }

            foreach (TreeNode child in parent.Nodes)
            {
                var found = FirstNodeOfFullPath(child, fullPath);
                if (found != null) return found;
            }

            return null;
        }
        public static TreeNode? FirstNodeOfName(TreeNode parent, string name)
        {
            if (parent.Name == name)
            {
                return parent;
            }

            foreach (TreeNode child in parent.Nodes)
            {
                var found = FirstNodeOfName(child, name);
                if (found != null) return found;
            }

            return null;
        }

        /// <summary>
        /// TreeView에서 선택한 폴더의 하위 폴더와 파일 목록이 ListView에 표시된 상황에서,
        /// ListView의 특정 폴더를 선택했다면 TreeView의 폴더를 펼치고 해당 하위 폴더를 선택
        /// </summary>
        /// <param name="tvw"></param>
        /// <param name="item"></param>
        public static void SelectTreeFolderByItem(TreeView tvw, ListViewItem item)
        {
            string name = item.Name;
            var node = tvw.SelectedNode;
            if (node == null) return;

            if (!node.IsExpanded) node.Expand();

            var nodeChild = CUiCommon.FirstNodeOfName(node, name);
            if (nodeChild == null) return;

            tvw.SelectedNode = nodeChild;
        }

        public static void AdjustTreeChild(List<ListViewItem> items, TreeNode node)
        {
            List<string> dirsInTree = items.Select(item => item.Text).ToList();
            List<string> dirsInList = node.Nodes.Cast<TreeNode>()
                .Where(item => item.Name != _KEY_TEMP)
                .Select(item => item.Text).ToList();

            if (dirsInTree.Count == dirsInList.Count 
                && dirsInTree.Count == dirsInTree.Intersect(dirsInList).Count())
            {
                return;
            }

            AddTempChild(node);
            node.Collapse();
        }

        public static string GetFullPathOfName(TreeNode node)
        {
            if (node.Parent == null) return node.Name;

            return GetFullPathOfName(node.Parent) + node.TreeView.PathSeparator + node.Name;
        }

        public static bool ExtractIfOneZip(bool completed, string localDirectory, 
            List<FtpListItem> ftpItems, Form frm,
            out string zipFullPath, out string extractFullDir)
        {
            string zipFileName = ftpItems[0].Name;
            string zipDirLast = Path.GetFileNameWithoutExtension(zipFileName);
            zipFullPath = Path.Combine(localDirectory, zipFileName);
            extractFullDir = Path.Combine(localDirectory, zipDirLast);

            bool oneZip = completed && ftpItems.Count == 1 && Path.GetExtension(ftpItems[0].Name).ToLower() == ".zip";
            if (!oneZip) return false;

            string msg = $"{extractFullDir} 폴더에 {zipFileName} 파일의 압축을 풀겠습니까?";
            var dret = MessageBox.Show(frm, msg, "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dret != DialogResult.Yes) return false;

            if (Directory.Exists(extractFullDir))
            {
                Directory.Delete(extractFullDir, true);
            }
            ZipFile.ExtractToDirectory(zipFullPath, extractFullDir);

            return true;
        }
    }
}
