using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Collections;
using System.IO;
using System.Drawing.Drawing2D;

namespace FBI_Studio
{
    /// <summary>
    /// edit by ny @ Focus
    /// 2011 11 1
    /// 配置管理图形界面
    /// </summary>
    public partial class WorkConfigForm : Form
    {
        #region 公共方法和属性
        public WorkConfig Config
        {
            get { return m_workconfig; }
            private set { m_workconfig = value; }
        }
        
        public WorkConfigForm()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(WorkConfig config)
        {
            this.Config = config;
            InitalEx();
            return ShowDialog();
        }
        #endregion

        #region 私有方法和字段
        private WorkConfig m_workconfig = null;
        private Image m_strechImage = null;
        private Timer m_longPressTimer = new Timer();
        private Timer m_rockItemTimer = new Timer();
        private bool m_isSetting = false;
        private ListViewItem m_hotItem = null;
        private ListViewItem m_dragItem = null;
        private Random m_rand = new Random(DateTime.Now.Millisecond);

        private readonly string m_iconPath = "skin\\icon";
        private readonly string m_skinPath = "skin";

        ListViewItem GetItemByLoction(int x, int y)
        {
            int w = m_listView.LargeImageList.ImageSize.Width;
            int h = m_listView.LargeImageList.ImageSize.Height;
            foreach (ListViewItem item in m_listView.Items)
            {
                int top = item.Bounds.Top + 2;
                int left = item.Bounds.Left + (item.Bounds.Width - w) / 2;
                if (new Rectangle(left,top,w,h).Contains(x, y))
                    return item;
            }
            return null;
        }

        int GetInsertIndex(int x, int y)
        {
            int[] dis = new int[m_listView.Items.Count];
            for (int i = 0; i < m_listView.Items.Count; i++ )
            {
                Rectangle bound = m_listView.Items[i].Bounds;
                int upDis = Math.Abs(y - bound.Top);
                int downDis = Math.Abs(y - bound.Bottom);
                int leftDis = Math.Abs(x - bound.Left);
                int RightDis = Math.Abs(x - bound.Right);
                int direct = leftDis < RightDis ? -1 : 1;
                dis[i] = Math.Min(upDis, downDis) + Math.Min(leftDis, RightDis);
                dis[i] *= direct;
             }

            int insertIndex = 0;
            for (int i = 1; i < m_listView.Items.Count; i++ )
            {
                if (Math.Abs(dis[i]) < Math.Abs(dis[insertIndex]))
                {
                    insertIndex = i;
                }
            }
            if (dis[insertIndex] > 0)
            {
                insertIndex += 1;
            }
            return insertIndex;
        }

        Image GetIconByProjectName(string prjName, string[] paths, string[] specialPaths)
        {
            string selectPath = null;
            foreach (string path in specialPaths)
            {
                string filename = Path.GetFileName(path);
                filename = filename.Substring(0, filename.Length - 4);
                if (filename == prjName)
                {
                    selectPath = path;
                    break;
                }
            }
            if (selectPath == null)
            {
                int index = m_rand.Next(paths.Length - 1);
                selectPath = paths[index];
            }

            Image src = Bitmap.FromFile(selectPath);
            Bitmap dst = new Bitmap(128, 128);
            Graphics g = Graphics.FromImage(dst);
            g.DrawImage(src, new Rectangle(0, 0, 128, 128), new Rectangle(0, 0, src.Width, src.Height), GraphicsUnit.Pixel);
            return dst;
        }

        Project ItemToProject(ListViewItem item)
        {
            return new Project(item.Text, item.SubItems[1].Text, item.SubItems[2].Text);
        }

        ListViewItem ProjectToItem(Project pjt)
        {
            string[] paths = Directory.GetFiles("skin\\icon", "*.png");
            string[] specialPaths = Directory.GetFiles("skin\\icon\\special", "*.png");
            Image dst = GetIconByProjectName(pjt.PjtName, paths, specialPaths);
            m_listView.ImageListNF.Add(dst);
            m_listView.LargeImageList.Images.Add(dst);
            ListViewItem item = new ListViewItem(pjt.PjtName, m_listView.ImageListNF.Count-1);
            item.SubItems.Add(pjt.SampleFolder);
            item.SubItems.Add(pjt.PointFolder);
            return item;
        }

        bool IsProjectNameExist(string pjtname)
        {
            foreach (ListViewItem item in m_listView.Items)
            {
                if (item.Text == pjtname)
                {
                    return true;
                }
            }
            return false;
        }

        void InitalEx()
        {
            #region 背景图片
            m_strechImage = Image.FromFile("skin\\background.jpg");
            Bitmap bmp = new Bitmap(m_listView.Width, m_listView.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawImage(m_strechImage, new Rectangle(0, 0, m_listView.Width, m_listView.Height), new Rectangle(0, 0, m_strechImage.Width, m_strechImage.Height), GraphicsUnit.Pixel);
            m_listView.BackgroundImage = bmp;
            #endregion

            #region 定时器
            m_longPressTimer.Tick += new EventHandler(OnLongPress);
            m_rockItemTimer.Tick += new EventHandler(OnRockItem);
            m_rockItemTimer.Interval = 150;
            m_rockItemTimer.Start();
            #endregion

            #region 初始化Item
            m_listView.View = View.LargeIcon;
            // Allow the user to edit item text.
            m_listView.LabelEdit = true;
            // Allow the user to rearrange columns.
            m_listView.AllowColumnReorder = true;
            // Display check boxes.
            m_listView.CheckBoxes = false;
            // Select the item and subitems when selection is made.
            m_listView.FullRowSelect = true;
            // Display grid lines.
            m_listView.GridLines = true;
            // Sort the items in the list in ascending order.
            m_listView.Sorting = SortOrder.None;
            // Aisable multiselect
            m_listView.MultiSelect = false;

            //初始化大图显示模式
            m_listView.LargeImageList = new ImageList();
            m_listView.LargeImageList.ImageSize = new Size(128, 128);
            m_listView.LargeImageList.ColorDepth = ColorDepth.Depth32Bit;

            //由于ImageList的索引太慢，使用List来保存图片
            m_listView.ImageListNF = new List<Image>();

            string[] paths = Directory.GetFiles("skin\\icon", "*.png");
            string[] specialPaths = Directory.GetFiles("skin\\icon\\special", "*.png");
            m_listView.Items.Clear();
            for (int index = 0; index < m_workconfig.ProjectArray.Count; index++)
            {
                Project pjt = m_workconfig.ProjectArray[index];
                m_listView.Items.Add(ProjectToItem(pjt));
            }
            #endregion
        }
        #endregion

        #region 私有类
        class ListViewItemComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                return ((ListViewItem)x).Index.CompareTo(((ListViewItem)x).Index);
            }
        }
        #endregion

        #region 引发事件
        
        private void OnLongPress(object sender, EventArgs e)
        {
            m_isSetting = true;
            m_listView.RedrawItems(0, m_listView.Items.Count - 1, true);
        }

        private float m_rockAngle = 1.0f;
        private void OnRockItem(object sender, EventArgs e)
        {
            if (m_isSetting)
            {
                m_rockAngle = m_rockAngle*(-1.0f);
                m_listView.RedrawItems(0, m_listView.Items.Count-1,false);
            }
        }

        private void OnDrawItem(object sender, DrawListViewItemEventArgs e)
        {
            Graphics g = e.Graphics;
            //画图标
            Image img = ((ListViewNF)(e.Item.ListView)).ImageListNF[e.Item.ImageIndex];
            Rectangle bounds = e.Bounds;
            int imgStartX = -img.Width / 2;
            int imgStartY = -img.Height / 2;
            g.TranslateTransform(bounds.Left + bounds.Width/2, bounds.Top + 2 + img.Height / 2);
            if (m_isSetting)
            {
                g.RotateTransform(m_rockAngle);
                
            }
            if (e.Item == m_hotItem)//(e.State & ListViewItemStates.Selected) != 0)
            {
                //选中的暗色处理
                ImageAttributes attr = new ImageAttributes();
                float[][] colorMatrixElements = { 
                    new float[] {0.5f,  0,  0,  0, 0},
                    new float[] {0,  0.5f,  0,  0, 0},
                    new float[] {0,  0,  0.5f,  0, 0},
                    new float[] {0,  0,  0,  1, 0},
                    new float[] {0.1f, 0.2f, 0.2f, 0, 1}};
                ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
                attr.SetColorMatrix(colorMatrix,ColorMatrixFlag.Default,ColorAdjustType.Bitmap);
                g.DrawImage(img, new Rectangle(imgStartX, imgStartY, img.Width , img.Height ), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, attr);
            }
            else
                g.DrawImage(img, new Point(imgStartX, imgStartY));

            //画删除小圈
            if (m_isSetting)
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.FillEllipse(new SolidBrush(Color.White), new Rectangle(imgStartX, imgStartY-2, 18, 18));
                g.FillEllipse(new SolidBrush(Color.Black), new Rectangle(imgStartX + 2, imgStartY, 14, 14));
                g.DrawLine(new Pen(Color.White), new Point(imgStartX + 4, imgStartY + 2), new Point(imgStartX + 14, imgStartY + 12));
                g.DrawLine(new Pen(Color.White), new Point(imgStartX + 14, imgStartY + 2), new Point(imgStartX + 4, imgStartY + 12));
            }

            //画文字
            e.DrawText(TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter);
        }

        private void OnReSize(object sender, EventArgs e)
        {
            if (m_listView.Width == 0 || m_listView.Height == 0)
            {
                return;
            }
            Bitmap bmp = new Bitmap(m_listView.Width, m_listView.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            //消除锯齿 
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawImage(m_strechImage, new Rectangle(0, 0, m_listView.Width, m_listView.Height), new Rectangle(0, 0, m_strechImage.Width, m_strechImage.Height), GraphicsUnit.Pixel);
            m_listView.BackgroundImage = bmp;
            m_listView.Refresh();
         }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            ListViewItem item = GetItemByLoction(e.X, e.Y);
            if (item == null && m_isSetting == true)
            {
                m_isSetting = false;
                m_listView.RedrawItems(0, m_listView.Items.Count - 1, false);
                return;
            }
            if (item != null)
            {
                m_longPressTimer.Enabled = true;
                m_longPressTimer.Interval = 1000;
            }

            if (e.Clicks > 1)
            {
                m_longPressTimer.Enabled = false;
            }

            if ( e.Clicks > 1 && e.Button == MouseButtons.Right && item == null && m_isSetting == false)
            {
                PjtConfigForm pjtForm = new PjtConfigForm();
                Project pjt = null;
                if (pjtForm.ShowDialog(ref pjt) != DialogResult.OK)
                {
                    return;
                }
                if (IsProjectNameExist(pjt.PjtName))
                    MessageBox.Show(string.Format("当前项目名{0}已存在", pjt.PjtName));
                else
                    m_listView.Items.Add(ProjectToItem(pjt));
            }
            if ( e.Clicks > 1 && e.Button == MouseButtons.Right && item != null && m_isSetting == false)
            {
                PjtConfigForm pjtForm = new PjtConfigForm();
                Project pjt = ItemToProject(item);
                if (pjtForm.ShowDialog(ref pjt) != DialogResult.OK)
                {
                    return;
                }
                item.SubItems[1].Text = pjt.SampleFolder;
                item.SubItems[2].Text = pjt.PointFolder;
            }
            if (e.Clicks > 1 && e.Button == MouseButtons.Left && item != null && m_isSetting == false)
            {
                m_workconfig.ProjectArray.Clear();
                foreach (ListViewItem subitem in m_listView.Items)
                {
                    m_workconfig.ProjectArray.Add(ItemToProject(subitem));
                }
                m_workconfig.CurrentProject = ItemToProject(item);
                this.Close();
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            m_longPressTimer.Enabled = false;
            this.Cursor = Cursors.Default;

            if (m_dragItem != null)
            {
                int index = GetInsertIndex(e.X, e.Y);
                if (index >= m_listView.Items.Count)
                {
                    m_listView.Items.Add(m_dragItem);
                }
                else
                    m_listView.Items.Insert(index, m_dragItem);
            }
            m_listView.ListViewItemSorter = new ListViewItemComparer();
            m_dragItem = null;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            //Console.WriteLine(e.Location);
            ListViewItem oldhotItem = m_hotItem;
            m_hotItem = GetItemByLoction(e.X, e.Y);
            if (m_hotItem != null)
            {
                if (oldhotItem != m_hotItem)
                {
                    m_listView.RedrawItems(m_hotItem.Index, m_hotItem.Index, false);
                    if (oldhotItem != null)
                        m_listView.RedrawItems(oldhotItem.Index, oldhotItem.Index, false);
                }
                if (m_dragItem == null && m_isSetting)
                {
                    this.Cursor = Cursors.Hand;
                }
            }
            else if (oldhotItem != null )
            {
                m_listView.RedrawItems(oldhotItem.Index, oldhotItem.Index, false);
                if (m_dragItem == null)
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void OnItemDrag(object sender, ItemDragEventArgs e)
        {
            if (!m_isSetting)
                return;

            m_dragItem = (ListViewItem)(e.Item);
            if (m_hotItem == m_dragItem)
            {
                m_hotItem = null;
            }
            Image img = m_dragItem.ImageList.Images[m_dragItem.ImageIndex];
            Bitmap bmp = new Bitmap(img);
            this.Cursor = new Cursor(bmp.GetHicon());
            m_listView.Items.Remove(m_dragItem);
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            ListViewItem item = GetItemByLoction(e.X, e.Y);
            if (item == null)
                return;

            if (e.Button == MouseButtons.Left && m_isSetting)
            {
                Rectangle rect = new Rectangle(item.Bounds.Left + (item.Bounds.Width - m_listView.LargeImageList.ImageSize.Width) / 2, item.Bounds.Top, 20, 20);
                if (rect.Contains(e.X, e.Y))
                {
                    m_listView.Items.Remove(item);
                    if (item == m_hotItem)
                    {
                        m_hotItem = null;
                    }
                    if (m_listView.Items.Count == 0)
                    {
                        m_isSetting = false;
                    }
                }
            }
//             else if (e.Button == MouseButtons.Right && (!m_isSetting))
//             {
//                 Point pt = this.PointToScreen(e.Location);
//                 m_menuStrip.Show(pt.X, pt.Y);
//             }
        }

        private void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem item = GetItemByLoction(e.X, e.Y);
            m_longPressTimer.Enabled = false;
            if (e.Button == MouseButtons.Right && item == null && m_isSetting == false)
            {
                PjtConfigForm pjtForm = new PjtConfigForm();
                Project pjt = null;
                if (pjtForm.ShowDialog(ref pjt) != DialogResult.OK)
                {
                    return;
                }
                if (IsProjectNameExist(pjt.PjtName))
                    MessageBox.Show(string.Format("当前项目名{0}已存在", pjt.PjtName));
                else
                    m_listView.Items.Add(ProjectToItem(pjt));
            }
            if (e.Button == MouseButtons.Right && item != null && m_isSetting == false)
            {
                PjtConfigForm pjtForm = new PjtConfigForm();
                Project pjt = ItemToProject(item);
                if (pjtForm.ShowDialog(ref pjt) != DialogResult.OK)
                {
                    return;
                }
                item.SubItems[1].Text = pjt.SampleFolder;
                item.SubItems[2].Text = pjt.PointFolder;
            }
            if (e.Button == MouseButtons.Left && item != null && m_isSetting == false)
            {
                m_workconfig.ProjectArray.Clear();
                foreach (ListViewItem subitem in m_listView.Items)
                {
                    m_workconfig.ProjectArray.Add(ItemToProject(subitem));
                }
                m_workconfig.CurrentProject = ItemToProject(item);
                this.Close();
            }
        }
        #endregion

    }

    public class Project
    {
        public string PjtName = "";
        public string SampleFolder = "";
        public string PointFolder = "";

        public Project()
        {        }

        public Project(string _pjtName, string _sampleFolder, string _pointFolder)
        {
            PjtName = _pjtName;
            SampleFolder = _sampleFolder;
            PointFolder = _pointFolder;
        }
    }

    public class WorkConfig
    {

        public List<Project> ProjectArray = new List<Project>();
        Project m_currentProject = new Project();
        public Project CurrentProject
        {
            get { return m_currentProject; }
            set { m_currentProject = value; }
        }

        private bool Contains(string pjtName)
        {
            foreach (Project pjt in ProjectArray)
            {
                if (pjt.PjtName == pjtName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取样本文件夹路径
        /// </summary>
        /// <returns></returns>
        public string GetCurrentSampleFolder()
        {
            /*if (index >=0 && index < SampleFolderList.Count)
            {
                Object o = SampleFolderList[index];
                if (o.GetType() == typeof(string))
                {
                    return o as string;
                }
            }*/
            return @"C:\";
        }
      
        /// <summary>
        /// 获取点文件文件夹路径
        /// </summary>
        /// <returns></returns>
        public string GetCurrentPointFolder()
        {
//             if (index >= 0 && index < PointFolderList.Count)
//             {
//                 Object o = PointFolderList[index];
//                 if (o.GetType() == typeof(string))
//                 {
//                     return o as string;
//                 }
//             }
            return @"C:\";
        }

        public Project GetProjectByName(string pjtName)
        {
            if (pjtName != "")
            {
                foreach (Project pjt in ProjectArray)
                {
                    if (pjtName == pjt.PjtName)
                        return pjt;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取点文件文件夹路径
        /// </summary>
        /// <returns></returns>
        public string GetPjtName()
        {
//             if (index >= 0 && index < PjtNameList.Count)
//             {
//                 Object o = PjtNameList[index];
//                 if (o.GetType() == typeof(string))
//                 {
//                     return o as string;
//                 }
//             }
            return @"default";
        }

        /// <summary>
        /// 增加一个项目配置
        /// </summary>
        /// <param name="pjtName">项目名字</param>
        /// <param name="sampleFolder">项目样本路径</param>
        /// <param name="pointFolder">项目点文件路径</param>
        /// <returns></returns>
        public bool AddConfig(string pjtName, string sampleFolder, string pointFolder)
        {
            if (pjtName.Length > 10 || pjtName.Length <= 0 || this.Contains(pjtName))
            {
                return false;
            }
            ProjectArray.Add(new Project(pjtName, sampleFolder, pointFolder));
            return true;
        }

        /// <summary>
        /// 设置指定项目的,样本路径和点文件路径
        /// </summary>
        /// <param name="pjtName">项目名</param>
        /// <param name="sampleFolder">样本路径</param>
        /// <param name="pointFolder">点文件路径</param>
        /// <returns></returns>
        public bool SetConfig(string pjtName, string sampleFolder, string pointFolder)
        {
            if (!this.Contains(pjtName))
            {
                return AddConfig(pjtName, sampleFolder, pointFolder);
            }
            Project pjt = GetProjectByName(pjtName);
            pjt.PjtName = pjtName;
            pjt.SampleFolder = sampleFolder;
            pjt.PointFolder = pointFolder;
            return true;
        }

        /// <summary>
        /// 删除当前配置
        /// </summary>
        /// <returns></returns>
        public bool DeleteCurrentConfig(string pjtName)
        {
            try
            {
                ProjectArray.Remove(GetProjectByName(pjtName));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 把配置方案保存在默认路径
        /// </summary>
        /// <returns></returns>
        public void SerializeXML()
        {
            if (!Directory.Exists("Config"))
                Directory.CreateDirectory("Config");
            SerializeXML("Config\\workconfig.xml");
        }

        /// <summary>
        /// 把配置方案保存在指定路径
        /// </summary>
        /// <returns></returns>
        public void SerializeXML(string paths)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(WorkConfig));
                Stream stream = new FileStream(paths, FileMode.Create, FileAccess.Write, FileShare.Read);
                xs.Serialize(stream, this);
                stream.Close();
            }
            catch (System.Exception)
            {

            }
        }

        /// <summary>
        /// 从默认路径加载配置方案
        /// </summary>
        /// <returns></returns>
        static public WorkConfig DeserializeXML()
        {
            if (!Directory.Exists("Config"))
                Directory.CreateDirectory("Config");
            return WorkConfig.DeserializeXML("Config\\workconfig.xml");
        }

        /// <summary>
        /// 从指定路径加载配置方案
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static public WorkConfig DeserializeXML(string path)
        {
            WorkConfig config;
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(WorkConfig));
                Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                config = xs.Deserialize(stream) as WorkConfig;
                stream.Close();
                return config;
            }
            catch (System.Exception)
            {
                config = new WorkConfig();
                return config;
            }
        }
    }
}
