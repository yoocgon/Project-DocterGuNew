using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorGu
{
    public class CTreeView
    {
        public static TreeNode GetRoot(TreeNode node)
        {
            while (node.Parent != null)
            {
                node = node.Parent;
            }
            return node;
        }
    }
}
