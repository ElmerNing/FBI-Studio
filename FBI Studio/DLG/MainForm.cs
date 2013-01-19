using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using Focus;

namespace FBI_Studio
{
    /// <summary>
    /// edit by ny @ Focus
    /// 2011 09 28
    /// FBI Studio</summary>
    public partial class MainForm : Form
    {

        #region 属性
        //工作配置
        private WorkConfig m_workConfig = new WorkConfig();
        #endregion

        #region 方法

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <returns></returns>
        public MainForm()
        {
            InitializeComponent();
            FileManage.showMessage += this.ShowMessage;
            PointTrain.showMessage += this.ShowMessage;

            //读取工作配置
            m_workConfig = WorkConfig.DeserializeXML();
            OnConfigManage(null, null);
            string sampleFolder = m_workConfig.CurrentProject.SampleFolder;
            string pointFolder = m_workConfig.CurrentProject.PointFolder;
            SetFolder(sampleFolder, pointFolder);
        }

        /// <summary>
        /// 在文本控件上显示信息, 支持跨线程调用
        /// </summary>
        /// <param name="msg">待显示信息</param>
        /// <param name="color">显示颜色</param>
        /// <returns></returns>
        private void ShowMessage(string msg, Color color)
        {
            if (m_tips.InvokeRequired)
            {
                PointTrain.ShowMessage invokeProcess = new PointTrain.ShowMessage(ShowMessage);
                this.Invoke(invokeProcess, new object[] { msg, color });
            }
            else
            {
                m_tips.SelectionStart = m_tips.TextLength;
                m_tips.Focus();
                m_tips.SelectionColor = color;
                m_tips.AppendText(msg);
            }
        }

        /// <summary>
        /// 在文本控件显示, 单幅图像处理结果, 支持跨线程调用
        /// </summary>
        /// <param name="info">处理结果</param>
        /// <param name="srcFile">源文件</param>
        /// <returns></returns>
        private void ShowImageInfo(ImageInfo info, string srcFile)
        {
            string fileName = Path.GetFileName(srcFile);
            Color c = Color.Blue;
            if (info.MissPt >= 9 && info.MissPt < 19)
                c = Color.Purple;
            else if (info.MissPt >= 19)
                c = Color.Red;
            else if (info.MissPt >= 5)
                c = Color.Blue;
            else
                c = Color.Green;

            string toShow = Path.GetFileName(srcFile) + "\r\n";
            toShow += "\t" + info.ToString() + "\r\n";
            ShowMessage(toShow, c);
        }

        /// <summary>
        /// 显示批量图像的处理结果,支持跨线程调用
        /// </summary>
        /// <param name=""></param>
        /// <param name="infos"></param>
        /// <param name="foldername"></param>
        /// <param name="is3D"></param>
        /// <returns></returns>
        private void ShowImageInfo(Dictionary<string, ImageInfo> infos, string foldername, bool is3D = false)
        {
            int minMis = short.MaxValue, maxMis = 0;
            int minWidth = short.MaxValue, maxWidth = 0, minHeight = short.MaxValue, maxHeight = 0;
            foreach (KeyValuePair<string, ImageInfo> si in infos)
            {
                minMis = Math.Min(si.Value.MissPt, minMis);
                maxMis = Math.Max(si.Value.MissPt, maxMis);
                minWidth = Math.Min(si.Value.Width, minWidth);
                maxWidth = Math.Max(si.Value.Width, maxWidth);
                minHeight = Math.Min(si.Value.Height, minHeight);
                maxHeight = Math.Max(si.Value.Height, maxHeight);
            }
            
            ShowMessage(string.Format("{0}: Min:{1} Max:{2}\r\n", foldername, minMis, maxMis), Color.Blue);
            if (is3D)
            {
                ShowMessage(string.Format("\tWidth Min:{0} Max:{1}\r\n", minWidth, maxWidth), Color.Blue);
                ShowMessage(string.Format("\tHeight Min:{0} Max:{1}\r\n", minHeight, maxHeight), Color.Blue);
            }
            ShowMessage("\r\n", Color.Blue);
        }

        /// <summary>
        /// 设置样本目录和点文件目录
        /// </summary>
        /// <param name="sampleFolder">样本目录</param>
        /// <param name="pointFolder">点文件目录</param>
        /// <returns></returns>
        private void SetFolder(string sampleFolder, string pointFolder)
        {        
            try
            {
                if (pointFolder != null)
                {
                    if (!Directory.Exists(pointFolder))
                    {
                        Directory.CreateDirectory(pointFolder);
                    }
                    m_pointFolder.Text = pointFolder;
                    m_workConfig.CurrentProject.PointFolder = pointFolder;
                } 
                if (sampleFolder != null)
                {
                    m_sampleFolder.Text = sampleFolder;
                    if (!m_filterCtrl.SetRootfolder(sampleFolder))
                        throw new Exception();//此函数会顺带设置工作目录, 设置工作目录的时候，得使用到m_sampleFolder的值， 所以得放置在最后面                 
                    m_workConfig.CurrentProject.SampleFolder = sampleFolder;
                }
            }
            catch (System.Exception )
            {
                ShowMessage("样本目录设置失败!请重新设置！", Color.Red);
            }
        }

        /// <summary>
        /// 保存当前工作目录的样本的点文件
        /// </summary>
        /// <returns></returns>
        private void SavePointSet()
        {
            string denoName = Path.GetFileName(Global.WorkFileManage.CurruntFolder);
            if (Global.WorkFileManage.IsDouble && Global.WorkPicIndex == 0)
            {
                denoName = "IR_" + denoName;
            }
            string savePath = GetPointSetPath();
            PointTrain.PointSet.Save(savePath, denoName);
            string s = string.Format("Point save at {0}!\r\n", savePath);
            ShowMessage(s, Color.Green);
            ShowMessage(string.Format("black: {0}\twhite: {1}\r\n\r\n", PointTrain.PointSet.BlackFpSet.Count, PointTrain.PointSet.WhiteFpSet.Count), Color.Green);
        }

        /// <summary>
        /// 加载当前工作目录对应点文件
        /// </summary>
        /// <returns></returns>
        private void LoadPointSet()
        {
            //打开特征点文件
            string LoadPath = GetPointSetPath();
            PointTrain.PointSet.Load(LoadPath);
            //打开训练配置
            string configPath = LoadPath.Replace(".txt", ".xml");
            PointTrain.TrainConfig = PointTrainConfig.DeserializeXML(configPath);
            //刷新显示的点
            PointAnlysePicBox.SynFresh();
        }

        /// <summary>
        /// 获取点文件的路径，默认情况下获取工作目录对应的点文件路径
        /// </summary>
        /// <param name="sampleFolder">null：获取工作目录的点文件路径；不为null：获取sampleFolder对应的点文件路径</param>
        /// <returns></returns>
        private string GetPointSetPath(string sampleFolder = null)
        {
            sampleFolder = sampleFolder == null ? Global.WorkFileManage.CurruntFolder : sampleFolder;
            string denoName = Path.GetFileName(sampleFolder);
            if (Global.WorkFileManage.IsDouble)
            {
                string LoadPathIr = m_pointFolder.Text + "\\" + "ir";
                string LoadPathGr = m_pointFolder.Text + "\\" + "green";
                if(!Directory.Exists(LoadPathIr))
                {
                    Directory.CreateDirectory(LoadPathIr);
                }
                if (!Directory.Exists(LoadPathGr))
                {
                    Directory.CreateDirectory(LoadPathGr);
                }
                if (Global.WorkPicIndex == 0)
                {
                    return LoadPathIr + @"\IR_" + denoName + @".txt";
                }
                else
                    return LoadPathGr + @"\" + denoName + @".txt";
            }
            else
            {
                string fileName = Global.CurrentPointFormat == PointFormat.C54XX ? (denoName + " Points") : denoName;
                string LoadPath = m_pointFolder.Text + "\\" + fileName + ".txt";
                return LoadPath;
            }
            
        }

        /// <summary>
        /// 合并一个指定目录下的所有点文件
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        private void MergePointInFolder(string folder)
        {
            string[] AllPointFiles = Directory.GetFiles(folder, "*.txt");
            string savePath = folder + "\\" + "TotalPoints.h";
            StreamWriter sw = new StreamWriter(savePath);
            if (AllPointFiles != null && AllPointFiles.Length != 0)
            {
                foreach (string path in AllPointFiles)
                {
                    StreamReader sr = new StreamReader(path);
                    string s = sr.ReadToEnd();
                    sw.WriteLine(s);
                    sr.Close();
                }
                sw.Flush();
                sw.Close();
                ShowMessage(string.Format("merge suc!/r/nsave at {0}", savePath), Color.Green);
            }
        }

        /// <summary>
        /// 打开一个图像文件，并显示在工作区。
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <param name="isShow">是否显示，默认为是</param>
        /// <returns>返回图像信息</returns>
        private ImageInfo OpenWorkFile(string filepath, bool isShow = true)
        {
            FocusImg img = FocusApi.OpenFocusDat(filepath, Global.WorkPicIndex);
            if (img == null)
                return new ImageInfo(0,255,0,0);

            if (isShow)
                m_picBoxWork.ShowImg(img); //使用ShowImg(img)来显示， 避免直接对Image属性直接赋值

            return PointTrain.ImageCheck(img);
        }

        /// <summary>
        /// 打开一个图像文件，并显示在辅助区域
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <param name="isShow">是否显示，默认为是</param>
        /// <returns>返回图像信息</returns>
        private ImageInfo OpenSuplyFile(string filepath, bool isShow = true)
        {
            FocusImg img = FocusApi.OpenFocusDat(filepath, Global.SuplyPicIndex);
            if (img == null)
                return new ImageInfo(0,255,0,0);

            if (isShow)
                m_picBoxSuply.ShowImg(img); //一定得调用showImg来显示

            return PointTrain.ImageCheck(img);
        }

        #endregion

        #region 引发事件

        /// <summary>
        /// 引发工作目录变更
        /// </summary>
        /// <returns></returns>
        private void OnWorkFolderChanged()
        {
            string folder = m_filterCtrl.Workfolder;
            if (Directory.Exists(folder))
            {
                try
                {
                    Global.WorkFileManage = new FileManage(folder);
                    ImageInfo info = OpenWorkFile(Global.WorkFileManage.GetCurrentFilePath());
                    if (info.Width < 200)
                        Global.CurrentPointFormat = PointFormat.C54XX;
                    else
                        Global.CurrentPointFormat = PointFormat.DM642;

                    ShowMessage(string.Format("当前点格式为{0}！\r\n", Global.CurrentPointFormat), Color.Blue);

                    //设置了Global.CurrentPointFormat，加载点文件
                    LoadPointSet();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// 引发辅助目录变更
        /// </summary>
        /// <returns></returns>
        private void OnSuplyFolderChanged()
        {
            string folder = m_filterCtrl.Suplyfolder;
            if (Directory.Exists(folder))
            {
                try
                {
                    Global.SuplyFileManage = new FileManage(folder);
                    OpenSuplyFile(Global.SuplyFileManage.GetCurrentFilePath());
                    ShowImageInfo(PointTrain.FolderImageCheck(Global.SuplyFileManage.CurruntFolder), Path.GetFileName(Global.SuplyFileManage.CurruntFolder), true);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// 引发上一张，下一张，随机事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnBtClick(object sender, EventArgs e)
        {         
            try
            {
            if (Global.WorkFileManage != null)
                {
                    string filepath = "";
                    if (sender.Equals(m_workLast))
                        filepath = Global.WorkFileManage.GetLastFilePath();
                    else if (sender.Equals(m_workNext))
                        filepath = Global.WorkFileManage.GetNextFilePath();
                    else if (sender.Equals(m_workRandom))
                        filepath = Global.WorkFileManage.GetRandFilePath();
                    if (filepath != "")
                        ShowImageInfo(OpenWorkFile(filepath), filepath);   
                }
                if (Global.SuplyFileManage != null)
                {
                    string filepath = "";
                    if (sender.Equals(m_suplyLast))
                        filepath = Global.SuplyFileManage.GetLastFilePath();
                    else if (sender.Equals(m_suplyNext))
                        filepath = Global.SuplyFileManage.GetNextFilePath();
                    else if (sender.Equals(m_suplyRandom))
                        filepath = Global.SuplyFileManage.GetRandFilePath();
                    if (filepath != "")
                        ShowImageInfo(OpenSuplyFile(filepath), filepath); 
                }
            }
            catch (System.Exception ex)
            {
            	MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 引发批量事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnBtBatch(object sender, EventArgs e)
        {
            Dictionary<string, ImageInfo> infos = new Dictionary<string, ImageInfo>();
            if (sender == m_workBath)
            {
                if (Global.WorkFileManage == null)
                    return;
                foreach (string file in Global.WorkFileManage.Filepaths)
                {
                    ImageInfo info = OpenWorkFile(file, false);
                    ShowImageInfo(info, file);
                    infos.Add(Path.GetFileName(file), info);
                }
                if (infos.Count > 0)
                    ShowImageInfo(infos, Path.GetFileName(Global.WorkFileManage.CurruntFolder), true);
            }
            else if (sender == m_suplyBatch)
            {
                if (Global.SuplyFileManage == null)
                    return;
                foreach (string file in Global.SuplyFileManage.Filepaths)
                {
                    ImageInfo info = OpenSuplyFile(file, false);
                    ShowImageInfo(info, file);
                    infos.Add(Path.GetFileName(file), info);
                }
                if (infos.Count > 0)
                    ShowImageInfo(infos, Path.GetFileName(Global.SuplyFileManage.CurruntFolder), true);
            }
        }

        /// <summary>
        /// 引发鼠标在图片区域移动的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnPicBoxMouseMove(object sender, MouseEventArgs e)
        {
            FocusPoint point =  ((PointAnlysePicBox)sender).GetOrigPoint(e.X, e.Y);
            //判定鼠标是否在特征点上
            LinkedListNode<FocusPoint> ptNode = PointTrain.PointSet.BlackFpSet.Find(point);
            string thrInfo = "";
            if (ptNode != null)
                thrInfo = string.Format("thr:{0}", ptNode.Value.g);
            ptNode = PointTrain.PointSet.WhiteFpSet.Find(point);
            if (ptNode != null)
                thrInfo = string.Format("thr:{0}", ptNode.Value.g);
            m_pointGreyLabel.Text = point.ToString() + thrInfo;
        }

        /// <summary>
        /// 引发大神・取点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnPointTran(object sender, EventArgs e)
        {
            //默认每次训练都先进行配置
            if (sender != null)
                OnBtConfig(m_config, new EventArgs());

            try
            {
                PointTrain.SharpThreshold(Global.WorkFileManage.Filepaths);
                PointAnlysePicBox.SynFresh();
                SavePointSet();
            }
            catch (System.Exception )
            {
                string s = string.Format("点训练与保存失败\r\n");
                ShowMessage(s, Color.Red);         	
            }
        }

        /// <summary>
        /// 引发设置样本目录事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnSetSampleFolder(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = m_sampleFolder.Text;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                SetFolder(dlg.SelectedPath, null);
            }
        }

        /// <summary>
        /// 引发是指特征点目录事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnSetPointFolder(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = m_pointFolder.Text;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                SetFolder(null, dlg.SelectedPath);
            }
        }

        /// <summary>
        /// 引发保存当前配置的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnSaveConfig(object sender, EventArgs e)
        {
            PjtNameSetForm form = new PjtNameSetForm();
            form.PrjName = Path.GetFileName(m_sampleFolder.Text);
            if (form.ShowDialog() != DialogResult.OK)
                return;
            
            if (!m_workConfig.AddConfig(form.PrjName, m_sampleFolder.Text, m_pointFolder.Text))
            {
                MessageBox.Show("项目名已存在或者不合法");
            }
            else
            {
                m_workConfig.SerializeXML();
                MessageBox.Show("保存成功");
            }
        }

        /// <summary>
        /// 引发奥义・批量事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnBtMultiply(object sender, EventArgs e)
        {
            MutiplySetForm form = new MutiplySetForm();
            string matchString = form.ShowDialog();
            if (matchString == null)
                return;
            string[] folders;
            if (MutiplySetForm.IsOnlyCheck)
                folders = m_filterCtrl.CheckedFolders;
            else
                folders = m_filterCtrl.TotalFolders;
            
            if (folders == null)
                return;
            
            foreach (string folder in folders)
            {
                string foldername = Path.GetFileName(folder);
                if (foldername.IndexOf(matchString) >= 0)
                {
                    Dictionary<string, ImageInfo> imagesInfo = PointTrain.FolderImageCheck(folder);
                    ShowImageInfo(imagesInfo, foldername, MutiplySetForm.IsShow3D);
                }          
            }

            //多核心处理器的并行优化, 注意互斥
        /*    Parallel.ForEach(folders, folder =>
            //foreach(string denofolders in folders)
            {
                string foldername = Path.GetFileName(folder);
                if (foldername.IndexOf(matchString) >= 0)
                {
                    Dictionary<string, ImageInfo> imagesInfo = PointTrain.FolderImageCheck(folder);
                    showImageInfo(imagesInfo, foldername);
                }
                
            }
                    );*/
        }

        /// <summary>
        /// 引发无双・3D事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnBt3D(object sender, EventArgs e)
        {
            string[] folders = m_filterCtrl.TotalFolders;
            if (folders == null && folders.Length != 0)
                return;

            ArrayList imageSizes = new ArrayList();
            foreach (string file in Global.WorkFileManage.Filepaths)
            {
                imageSizes.Add( FocusApi.GetFormatFocusSize(file) );
            }
            int workminWidth = short.MaxValue, workmaxWidth = 0, workminHeight = short.MaxValue, workmaxHeight = 0;
            foreach (Size si in imageSizes)
            {
                if (si.Width < 0)
                    continue;
                workminWidth = Math.Min(si.Width, workminWidth);
                workmaxWidth = Math.Max(si.Width, workmaxWidth);
                workminHeight = Math.Min(si.Height, workminHeight);
                workmaxHeight = Math.Max(si.Height, workmaxHeight);
            }

            foreach (string folder in folders)
            {
                int minWidth = short.MaxValue, maxWidth = 0, minHeight = short.MaxValue, maxHeight = 0;
                string[] files = Directory.GetFiles(folder, "*.dat");
                foreach (string file in files) 
                {
                    Size si = FocusApi.GetFormatFocusSize(file);
                    if (si.Width < 0)
                        continue;
                    minWidth = Math.Min(si.Width, minWidth);
                    maxWidth = Math.Max(si.Width, maxWidth);
                    minHeight = Math.Min(si.Height, minHeight);
                    maxHeight = Math.Max(si.Height, maxHeight);
                }

                if (minWidth - workmaxWidth > 12
                    || workminWidth - maxWidth > 12
                    || minHeight - workmaxHeight > 8
                    || workminHeight - workmaxHeight > 8)
                {
                    m_filterCtrl.SetTreeCheckBox(Path.GetFileName(folder), false );
                }
                else
                    m_filterCtrl.SetTreeCheckBox(Path.GetFileName(folder), true );
            }
        }

        /// <summary>
        /// 引发FormClosing事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            m_workConfig.SerializeXML();
        }

        /// <summary>
        /// 引发清理文本控件的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnClearTip(object sender, EventArgs e)
        {
            m_tips.Text = "";
        }

        /// <summary>
        /// 引发必杀・取点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnIntellSelPoint(object sender, EventArgs e)
        {   
            ArrayList distingFileList = new ArrayList();
            string[] distingfolders = m_filterCtrl.CheckedFolders;
            if (distingfolders == null || distingfolders.Length == 0)
                return;
            foreach (string folder in distingfolders)
            {
                if (folder == Global.WorkFileManage.CurruntFolder)
                    continue;
                distingFileList.AddRange(Directory.GetFiles(folder, "*.dat"));
            }

            string[] distingFiles = new string[distingFileList.Count];
            distingFileList.CopyTo(distingFiles);
            IntellSelPoitForm form = new IntellSelPoitForm();
            form.ShowDialog(Global.WorkFileManage.Filepaths, distingFiles);
            SavePointSet();
        }

        /// <summary>
        /// 引发真妹・配置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnBtConfig(object sender, EventArgs e)
        {
            SharpPointForm form = new SharpPointForm();
            if (DialogResult.OK != form.ShowDialog())
                return;
            string savePath = GetPointSetPath();
            savePath = savePath.Replace("txt", "xml");
            string denoName = Path.GetFileName(Global.WorkFileManage.CurruntFolder);
            if (PointTrain.TrainConfig.SerializeXML(savePath))
                ShowMessage(string.Format("Config {0}.xml save suc!!!\r\n", denoName), Color.Blue);
            else
                ShowMessage(string.Format("Config {0}.xml save fail!!!\r\n", denoName), Color.Red);
        }

        /// <summary>
        /// 引发合并点文件事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnMergePoint(object sender, EventArgs e)
        {
            string folder;

            folder = m_pointFolder.Text;
            if(Directory.Exists(folder))
                MergePointInFolder(folder);

            folder = m_pointFolder.Text + @"\ir";
            if (Directory.Exists(folder))
                MergePointInFolder(folder);

            folder = m_pointFolder.Text + @"\green";
            if (Directory.Exists(folder))
                MergePointInFolder(folder);
        }

        /// <summary>
        /// 引发删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnDeleteClick(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除此幅图片?", "提示!", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            string delfile = "";
            try
            {
                if (sender == m_suplyDel)
                    delfile = Global.SuplyFileManage.GetCurrentFilePath();
                if (sender == m_workDel)
                    delfile = Global.WorkFileManage.GetCurrentFilePath();
                File.Delete(delfile);
                ShowMessage("dat delete suc!\r\n", Color.Blue);
                File.Delete(delfile.Replace(".dat", ".png"));
                ShowMessage("png delete suc!\r\n", Color.Blue);

                Global.SuplyFileManage.CurruntFolder = Global.SuplyFileManage.CurruntFolder;
                Global.WorkFileManage.CurruntFolder = Global.WorkFileManage.CurruntFolder;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        /// <summary>
        /// 引发疾风・删除事件,删除框框选中的点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnDeletePt(object sender, EventArgs e)
        {
            ArrayList whtForDel = new ArrayList();
            ArrayList blcForDel = new ArrayList();
            if (PointAnlysePicBox.m_selectRect != null
                && PointAnlysePicBox.m_selectRect.Width > 0
                && PointAnlysePicBox.m_selectRect.Height > 0
                && PointAnlysePicBox.m_selectRect.Top >= 0
                && PointAnlysePicBox.m_selectRect.Left >= 0)
            {
                foreach (FocusPoint p in PointTrain.PointSet.WhiteFpSet)
                {
                    Point tp = new Point(p.x, p.y);
                    if (PointAnlysePicBox.m_selectRect.Contains(tp))
                    {
                        whtForDel.Add(p);
                    }
                }
                foreach (FocusPoint p in PointTrain.PointSet.BlackFpSet)
                {
                    Point tp = new Point(p.x, p.y);
                    if (PointAnlysePicBox.m_selectRect.Contains(tp))
                    {
                        blcForDel.Add(p);
                    }
                }
            }

            foreach (FocusPoint p in whtForDel)
            {
                PointTrain.PointSet.WhiteFpSet.Remove(p);
            }
            foreach (FocusPoint p in blcForDel)
            {
                PointTrain.PointSet.BlackFpSet.Remove(p);
            }
            OnPointTran(null, null);
            PointAnlysePicBox.SynFresh();
        }

        /// <summary>
        /// 检测工作区的图片,是否会被checkbox选中的目录的点文件识别到
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnCheckOverReg(object sender, EventArgs e)
        {
            FocusImg img = FocusApi.OpenFocusDat(Global.SuplyFileManage.GetCurrentFilePath(), Global.SuplyPicIndex);
            if (img == null)
            {
                return;
            }

            string[] checkFolders = m_filterCtrl.CheckedFolders;
            foreach (string folder in checkFolders)
            {
                string denoName = Path.GetFileName(folder);
                string LoadPath = this.GetPointSetPath(folder);
                PointTrain.PointSet.Load(LoadPath);
                ImageInfo info = PointTrain.ImageCheck(img);
                this.ShowImageInfo(info, denoName);
            }

            //重新加载当前工作目录点文件
            PointTrain.PointSet.Load(this.GetPointSetPath());

        }

        /// <summary>
        /// 引发浏览样本目录事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnExploreSamplePath(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", m_sampleFolder.Text);
        }

        /// <summary>
        /// 引发浏览点目录事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnExplorePointPath(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", m_pointFolder.Text);
        }

        /// <summary>
        /// 引发对话框上KeyDown事件, 设置一些功能的快捷键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                //delete键 -> 疾风删除
                case Keys.Delete:
                    this.OnDeletePt(m_deletePt, new EventArgs());
                    break;

                //alt + 1键 -> 大神训练
                case Keys.D1 | Keys.Alt:
                    this.OnPointTran(m_pointTran, new EventArgs());
                    break;

                //alt + 2键 -> 奥义批量
                case Keys.D2 | Keys.Alt:
                    this.OnBtMultiply(m_mutiply, new EventArgs());
                    break;

                //上下左右键,移动边框
                case Keys.Up:
                    if (PointAnlysePicBox.m_selectRect.Width == 0 || PointAnlysePicBox.m_selectRect.Height == 0)
                        break;
                    Point tempPoint = PointAnlysePicBox.m_selectRect.Location;
                    tempPoint.Y -= 1;
                    PointAnlysePicBox.m_selectRect.Location = tempPoint;
                    PointAnlysePicBox.SynFresh();
                    break;
                case Keys.Down:
                    if (PointAnlysePicBox.m_selectRect.Width == 0 || PointAnlysePicBox.m_selectRect.Height == 0)
                        break;
                    tempPoint = PointAnlysePicBox.m_selectRect.Location;
                    tempPoint.Y += 1;
                    PointAnlysePicBox.m_selectRect.Location = tempPoint;
                    PointAnlysePicBox.SynFresh();
                    break;
                case Keys.Left:
                    if (PointAnlysePicBox.m_selectRect.Width == 0 || PointAnlysePicBox.m_selectRect.Height == 0)
                        break;
                    tempPoint = PointAnlysePicBox.m_selectRect.Location;
                    tempPoint.X -= 1;
                    PointAnlysePicBox.m_selectRect.Location = tempPoint;
                    PointAnlysePicBox.SynFresh();
                    break;
                case Keys.Right:
                    if (PointAnlysePicBox.m_selectRect.Width == 0 || PointAnlysePicBox.m_selectRect.Height == 0)
                        break;
                    tempPoint = PointAnlysePicBox.m_selectRect.Location;
                    tempPoint.X += 1;
                    PointAnlysePicBox.m_selectRect.Location = tempPoint;
                    PointAnlysePicBox.SynFresh();
                    break;
            }
        }

        /// <summary>
        /// 把当前辅助样本,移至工作目录下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnReplace(object sender, EventArgs e)
        {
            if (Global.SuplyFileManage == null || Global.WorkFileManage == null)
                return;

            //suply的当前文件移动到work的目录下
            FileManage.MoveTo(Global.SuplyFileManage, Global.WorkFileManage);
            //刷新FileManage里面的文件
            OnSuplyFolderChanged();
            OnWorkFolderChanged();
        }

        /// <summary>
        /// 引发转换样本事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnConvertSample(object sender, EventArgs e)
        {
            string sampleArg = "\"" + m_sampleFolder.Text + "\"";
            System.Diagnostics.Process.Start("ImageFormatConvertor.exe", sampleArg);
        }

        /// <summary>
        /// 引发配置管理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnConfigManage(object sender, EventArgs e)
        {
            WorkConfigForm form = new WorkConfigForm();
            form.ShowDialog(m_workConfig);
            SetFolder(m_workConfig.CurrentProject.SampleFolder, m_workConfig.CurrentProject.PointFolder);
        }

        /// <summary>
        /// 引发图片索引更改事件, 改变显示红外图或是绿光图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnPicIndexChanged(object sender, EventArgs e)
        {
            int workPicIndex = Global.WorkPicIndex, suplyPicIndex = Global.SuplyPicIndex;
            if (m_workGrRadio.Checked)
                Global.WorkPicIndex = 0;
            if (m_workIrRadio.Checked)
                Global.WorkPicIndex = 1;
            if (m_workUvRadio.Checked)
                Global.WorkPicIndex = 2;

            if (m_suplyGrRadio.Checked)
                Global.SuplyPicIndex = 0;
            if (m_suplyIrRadio.Checked)
                Global.SuplyPicIndex = 1;
            if (m_suplyUvRadio.Checked)
                Global.SuplyPicIndex = 2;

            if (workPicIndex != Global.WorkPicIndex && Global.WorkFileManage.IsDouble)
            {
                OnWorkFolderChanged();
            }
            if (suplyPicIndex != Global.SuplyPicIndex && Global.WorkFileManage.IsDouble)
            {
                OnSuplyFolderChanged();
            }
        }

        /// <summary>
        /// 引发批量抠图事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnBatchDigROI(object sender, EventArgs e)
        {
            Rectangle roiRect = PointAnlysePicBox.m_selectRect;
            if (roiRect.Width == 0 || roiRect.Height == 0)
            {
                MessageBox.Show("未选择相应感兴趣区域！");
                return;
            }
            CutImageConfig CIC = new CutImageConfig();
            CIC.SubsetWidth = roiRect.Width;
            CIC.SubsetHeight = roiRect.Height;
            CIC.ShowDialog();
            if (CIC.isOK==false)
            {
                return;
            }

            string saveFolder = "ROI_" + Path.GetFileName(Global.WorkFileManage.CurruntFolder);
            if (!Directory.Exists(saveFolder))
                Directory.CreateDirectory(saveFolder);
/* //保存PNG
            foreach (string filePath in Global.WorkFileManage.Filepaths)
            {
                FocusImg img = FocusApi.OpenFocusDat(filePath, Global.WorkPicIndex);
                string savePath = saveFolder + @"\" + Path.GetFileNameWithoutExtension(filePath) + ".png";
                img.GetImgByRect(roiRect).Save(savePath, ImageFormat.Png);
            }
*/
            foreach (string filePath in Global.WorkFileManage.Filepaths)
            {
                FocusImg img = FocusApi.OpenFocusDat(filePath, Global.WorkPicIndex);
                if (img == null)
                    continue;
                string savePath = saveFolder + @"\" + Path.GetFileNameWithoutExtension(filePath);

                switch(CIC.RotateMode)
                {
                    case 0:
                        break;
                    case 1:
                        img = img.Flip(FocusImg.RotateType.FlipX);
                        break;
                    case 2:
                        img = img.Flip(FocusImg.RotateType.FlipY);
                        break;
                    case 3:
                        img = img.Flip(FocusImg.RotateType.FlipXY);
                        break;
                }


                FocusImg roi = img.GetImgByRect(roiRect);
                if (CIC.Equalization==true)
                {
                    roi.Equalization();
                }



                //保存roi区域内,所有大小为[subwidth,subheight]的图片,主要用于生成adaboost训练的负样本

                int subwidth = CIC.SubsetWidth;
                int subheight = CIC.SubsetHeight;
                if (CIC.Subset == true )
                {
                    Rectangle subroiRect = new Rectangle(0, 0, subwidth, subheight);
                    for (int w = 0; w < roi.Width - subwidth+1; w++)
                    {
                        for (int h = 0; h < roi.Height - subheight+1; h++)
                        {
                            subroiRect.Location = new Point(w, h);
                            string saveName = string.Format("{0}{1}{2}", savePath, w, h);
                            roi.GetImgByRect(subroiRect).Save(saveName + ".dat", null);
                            roi.GetImgByRect(subroiRect).Save(saveName + ".png", ImageFormat.Png);
                        }
                    }	
                }
                else
                {
                    //save
                    roi.Save(savePath + ".dat", null);
                    roi.Save(savePath + ".png", ImageFormat.Png);
                }
            }
        }

        private void OnDigRoi(object sender, EventArgs e)
        {
            Rectangle roiRect = PointAnlysePicBox.m_selectRect;
            if (roiRect.Width == 0 || roiRect.Height == 0)
            {
                MessageBox.Show("未选择相应感兴趣区域！");
                return;
            }

            string saveFolder = "ROI_" + Path.GetFileName(Global.WorkFileManage.CurruntFolder);
            if (!Directory.Exists(saveFolder))
                Directory.CreateDirectory(saveFolder);

            string filePath = Global.WorkFileManage.GetCurrentFilePath();
            FocusImg img = FocusApi.OpenFocusDat(filePath, Global.WorkPicIndex);
            string savePath = saveFolder + @"\" + Path.GetFileNameWithoutExtension(filePath);
            
            FocusImg roi = img.GetImgByRect(roiRect);
            
            //save
            roi.Save(savePath + ".dat");
            roi.Save(savePath + ".png", ImageFormat.Png);
        }
        #endregion

    }//end of MainForm

}
