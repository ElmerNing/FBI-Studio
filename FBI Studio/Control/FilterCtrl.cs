using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace FBI_Studio
{
    /// <summary>
    /// edit by ny @ Focus
    /// 2011 09 28
    /// 文件夹列表树形控件</summary>
    public partial class FilterCtrl: UserControl
    {   
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <returns></returns>
        public FilterCtrl()
        {
            InitializeComponent();
        }  

        #region 引发事件

        /// <summary>
        /// 引发树节点被选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnFolderTreeSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            string folder = m_rootfolder;
            string subfoldername = "";
            do
            {
                subfoldername = "\\" + node.Text + subfoldername;
                node = node.Parent;
            } while (node != null);
            m_suplyfolder = folder + subfoldername;
            if (OnSuplyFolderChanged != null)
            {
                OnSuplyFolderChanged();
            }
        }

        /// <summary>
        /// 引发全选事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnSelAllCheckBox(object sender, EventArgs e)
        {
            bool isAllCheck = true;
            foreach (TreeNode node in m_folderTreeView.Nodes)
            {
                if (node.Checked == false)
                {
                    isAllCheck = false;
                }
            }
            foreach (TreeNode node in m_folderTreeView.Nodes)
            {
                node.Checked = !isAllCheck;
            }
        }

        /// <summary>
        /// 鼠标右击节点时,引发设置工作目录事件,
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnSetWorkFolder(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (m_workNode != null)
                    m_workNode.ImageIndex = m_folderTreeView.ImageIndex;

                m_workNode = e.Node;
                TreeNode node = m_workNode;
                string folder = m_rootfolder;
                string subfoldername = "";
                do
                {
                    subfoldername = "\\" + node.Text + subfoldername;
                    node = node.Parent;
                } while (node != null);
                m_workfolder = folder + subfoldername;
                m_workNode.ImageIndex = 0;
                if (OnWorkFolderChanged != null)
                {
                    OnWorkFolderChanged();
                }
            }

        }

        #endregion

        
        private string m_rootfolder = "";
        /// <summary>
        /// 当前的根目录 </summary>
        /// <value></value>
        public string Rootfolder
        {
            get { return m_rootfolder; }
        }
        
        private TreeNode m_workNode = null;
        private string m_workfolder = "";
        /// <summary>
        /// 当前选中的工作目录 </summary>
        /// <value></value>
        public string Workfolder
        {
            get { return m_workfolder; }
        }

        private string m_suplyfolder = "";
        /// <summary>
        /// 当前选中的辅助目录 </summary>
        /// <value></value>
        public string Suplyfolder
        {
            get { return m_suplyfolder; }
        }

        /// <summary>
        /// 获取所有目录 </summary>
        /// <value></value>
        public string[] TotalFolders
        {
            get
            {
                try
                {
                    return Directory.GetDirectories(Rootfolder);  
                }
                catch
                {
                    return null;
                }
                
            }
        }

        /// <summary>
        /// 获取选择的目录 </summary>
        /// <value></value>
        public string[] CheckedFolders
        {
            get
            {
                ArrayList list = new ArrayList();
                foreach (TreeNode node in m_folderTreeView.Nodes)
                {
                    if (node.Checked)
                    {
                        list.Add(m_rootfolder + @"\" + node.Text);
                    }
                }
                string[] str = new string[list.Count];
                list.CopyTo(str);
                return str;
            }
        }

        /// <summary>
        /// 控制事件的委托 </summary>
        public delegate void FilterEventHandle();
        /// <summary>
        /// 工作目录改变事件 </summary>
        /// <value></value>
        public event FilterEventHandle OnWorkFolderChanged = null;
        /// <summary>
        /// 辅助目录改变事件 </summary>
        /// <value></value>
        public event FilterEventHandle OnSuplyFolderChanged = null;

        
        /// <summary>
        /// 设置根目录
        /// </summary>
        /// <param name="rootFolderPath">要设置的根目录</param>
        /// <returns></returns>
        public bool SetRootfolder(string rootFolderPath)
        {
            try
            {
                m_rootfolder = rootFolderPath;
                CreatViewTree();
                m_folderTreeView.SelectedNode = m_folderTreeView.Nodes[0];
                TreeViewEventArgs e = new TreeViewEventArgs(m_folderTreeView.SelectedNode);
                TreeNodeMouseClickEventArgs ec = new TreeNodeMouseClickEventArgs(m_folderTreeView.SelectedNode, MouseButtons.Right, 1, 0, 0);
                //OnFolderTreeSelect(null, e);
                OnSetWorkFolder(null, ec);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
       
        /// <summary>
        /// 设置树节点的checkbox
        /// </summary>
        /// <param name="name">树节点名字</param>
        /// <param name="isCheck">是否选中</param>
        /// <returns></returns>
        public void SetTreeCheckBox(string name, bool isCheck)
        {
            foreach (TreeNode node in m_folderTreeView.Nodes)
            {
                if (node.Text == name)
                {
                    node.Checked = isCheck;
                }
            }
        }

        /// <summary>
        /// 根据rootFolder创建一棵树, 其中maxlevel可以设置树深度, 默认为1
        /// </summary>
        /// <returns></returns>
        private void CreatViewTree()
        {
            string _myShortRootFolder;
            int lastIndex = m_rootfolder.LastIndexOf('\\');
            _myShortRootFolder = m_rootfolder.Substring(lastIndex + 1, m_rootfolder.Length - lastIndex - 1);
            //m_rootLabel.Text = _myShortRootFolder;
            m_folderTreeView.Nodes.Clear();
            int maxlevel = 0; //目录层数
            string[] dir = Directory.GetDirectories(m_rootfolder);

            ArrayList arr = new ArrayList(dir);
            arr.Sort(new FolderStringComparerClass());
            foreach (string subFolder in arr)
            {
                CreateViewTreeNode(subFolder, m_folderTreeView.Nodes, maxlevel);
            }               
            m_folderTreeView.ExpandAll();
        }

        /// <summary>
        /// 创建子节点
        /// </summary>
        /// <param name="parentFolder">当前文件夹名字</param>
        /// <param name="nodes">当前节点</param>
        /// <param name="level">级数</param>
        /// <returns></returns>
        private void CreateViewTreeNode(string parentFolder, TreeNodeCollection nodes, int level)
        {
            string _myShortRootFolder;

            int lastIndex = parentFolder.LastIndexOf('\\');
            _myShortRootFolder = parentFolder.Substring(lastIndex + 1, parentFolder.Length - lastIndex - 1);
            TreeNode tn = new TreeNode(_myShortRootFolder);
            nodes.Add(tn);

            if (level == 0) return;

            string[] dir = Directory.GetDirectories(parentFolder);
            if (dir == null) return;

            ArrayList arr = new ArrayList(dir);
            arr.Sort(new FolderStringComparerClass());
            foreach (string subFolder in arr)
            {
                CreateViewTreeNode(subFolder, nodes[nodes.Count - 1].Nodes, level-1);
            }
        }

    }

    /// <summary>
    /// edit by ny @ Focus
    /// 2011 09 28
    /// 文件夹的字符串比较接口</summary>
    public class FolderStringComparerClass : IComparer
    {
        int IComparer.Compare(Object x, Object y)
        {
            string a = (string)x;
            string b = (string)y;
            a = a.Substring(a.LastIndexOf('\\') + 1);
            b = b.Substring(b.LastIndexOf('\\') + 1);
            int lengthdiff = Math.Abs(a.Length - b.Length);
            int pos = 0;
            if (a.Substring(0,2) == "GR" || a.Substring(0,2) == "IR")
            {
                pos = 3;
            }
            if (a.Length > b.Length)
            {
                for (int i = 0; i < lengthdiff; i++)
                {
                    b = b.Insert(pos, "0");
                }
            }
            else
            {
                for (int i = 0; i < lengthdiff; i++)
                {
                    a = a.Insert(pos, "0");
                }
            }
            return ((new CaseInsensitiveComparer()).Compare(a, b));
        }

    }
}
