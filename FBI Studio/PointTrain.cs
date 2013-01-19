using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Focus;
using System.Collections;
using System.Drawing;
using System.Xml.Serialization;

namespace FBI_Studio
{
    /// <summary>
    /// edit by ny @ Focus
    /// 2011 09 28
    /// 取点算法类(静态)
    /// </summary>
    static class PointTrain
    {
        
        /// <summary>
        /// 特征点集
        /// </summary>
        public static PointSet PointSet
        {
            get { return m_pointSet; }
            set { m_pointSet = value; }
        }
        private static PointSet m_pointSet = new PointSet();

        
        /// <summary>
        /// 特征点训练配置参数
        /// </summary>
        public static PointTrainConfig TrainConfig
        {
            get { return m_trainConfig; }
            set { m_trainConfig = value; }
        }
        private static PointTrainConfig m_trainConfig = new PointTrainConfig();

        public delegate void ShowMessage(string s, Color c);

        /// <summary>
        /// 处理过程中产生的消息的委托
        /// </summary>
        public static event ShowMessage showMessage = null;

        /// <summary>
        /// 训练点门阀
        /// </summary>
        /// <param name="sampleFiles">样本集的路径</param>
        /// <returns></returns>
        public static void SharpThreshold(string[] sampleFiles)
        {
            PointSet ps = m_pointSet;
            FocusPoint f = new FocusPoint();
            Dictionary<FocusPoint, ArrayList> PtGs = new Dictionary<FocusPoint, ArrayList>(f);     //点集对应灰度集合

            foreach (string file in sampleFiles)
            {
                FocusImg focusImg = FocusApi.OpenFocusDat(file, Global.WorkPicIndex);
                if (focusImg == null)
                {
                    if (showMessage != null)
                    {
                        showMessage(file + " Open Failed", Color.Red);
                    }
                    continue;
                }
                //focusImg.GetHistEqualBmp(focusImg.Hist);
                //使用0表示WHITEPOINT,1表示BLACKPOINT
                foreach (FocusPoint fp in ps.WhiteFpSet)
                {
                    FocusPoint _ = new FocusPoint(fp.x, fp.y, 0);
                    if (!PtGs.ContainsKey(_)) PtGs.Add(_, new ArrayList());
                    short g = focusImg.GetPixel(fp.x, fp.y);
                    if (g >= 0 || g < 256)
                    {
                        PtGs[_].Add(g);
                    }                  
                }
                foreach (FocusPoint fp in ps.BlackFpSet)
                {
                    FocusPoint _ = new FocusPoint(fp.x, fp.y, 1);
                    if (!PtGs.ContainsKey(_)) PtGs.Add(_, new ArrayList());
                    short g = focusImg.GetPixel(fp.x, fp.y);
                    if (g >= 0 || g < 256)
                    {
                        PtGs[_].Add(g);
                    }  
                }
            }


            foreach (KeyValuePair<FocusPoint, ArrayList> gs in PtGs)
            {
                if (gs.Value.Count <= 0)
                {
                    return;
                }
                gs.Value.Sort();
                short newg = 0;
                bool keep = false;
                if (FilterGrey(gs.Key.g == 0 ? true : false, gs.Value, ref newg))
                {
                    keep = true;
                }
                FocusPoint rfp = null;
                if (gs.Key.g == 0)
                {
                    foreach (FocusPoint fp in ps.WhiteFpSet)
                    {
                        if (gs.Key.Equals(gs.Key, fp))
                        {
                            if (keep) fp.g = newg;
                            else rfp = fp;
                        }
                    }

                    if (!keep)
                    {
                        ps.WhiteFpSet.Remove(rfp);
                    }
                }
                else
                {
                    foreach (FocusPoint fp in ps.BlackFpSet)
                    {
                        if (gs.Key.Equals(gs.Key, fp))
                        {
                            if (keep) fp.g = newg;
                            else rfp = fp;
                        }
                    }

                    if (!keep)
                    {
                        ps.BlackFpSet.Remove(rfp);
                    }
                }
            }

        }

        /// <summary>
        /// 用排好序的灰度集来过滤，得到是否保留此点，如果保留，给出最终灰度
        /// </summary>
        /// <param name="isWhitePoint">是否是白点</param>
        /// <param name="Value">排好序的灰度集</param>
        /// <param name="g">引用传出灰度值</param>
        /// <returns>是否继续保留这个点</returns>
        public static bool FilterGrey(bool isWhitePoint, ArrayList Value, ref short g)
        {
            if (isWhitePoint)
            {
                int abandanNum = TrainConfig.enable[2] ? (int)(TrainConfig.WHITELEFTPERCENT * Value.Count) : 0;
                g = (short)((short)Value[abandanNum] - TrainConfig.WHITEMINUSTHR);
                if (g < TrainConfig.WHITEBOTTOMTHR)
                    g = TrainConfig.WHITEBOTTOMTHR;
                if (TrainConfig.enable[1] && g < TrainConfig.WHITEACCEPTTHR)
                    return false;
                if (TrainConfig.enable[3] && g > TrainConfig.WHITETOPTHR)
                    g = TrainConfig.WHITETOPTHR;
            }
            else
            {
                int abandanNum = TrainConfig.enable[2] ? (int)(TrainConfig.BLACKLEFTPERCENT * Value.Count) : 0;
                g = (short)((short)Value[Value.Count - abandanNum - 1] + TrainConfig.BLACKPLUSTHR);
                if (g > TrainConfig.BLACKTOPTHR)
                    g = TrainConfig.BLACKTOPTHR;
                if (TrainConfig.enable[1] && g > TrainConfig.BLACKACCEPTTHR)
                    return false;
                if (TrainConfig.enable[3] && g < TrainConfig.BLACKBOTTOMTHR)
                    g = TrainConfig.BLACKBOTTOMTHR;
            }

            return true;
        }

        /// <summary>
        /// 根据点集去识别某一幅图
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns>相关图像处理信息</returns>
        public static ImageInfo ImageCheck(FocusImg focusImg)
        {
            PointSet ps = m_pointSet;
            if (focusImg == null)
            {
                return new ImageInfo(0, 0xFF, 0, 0);
            };
            int mismatchPtA = 0;
            foreach (FocusPoint fp in ps.WhiteFpSet)
            {
                short g = focusImg.GetPixel(fp.x, fp.y);
                if (g >= 256 || g < fp.g)
                    ++mismatchPtA;
            }

            foreach (FocusPoint fp in ps.BlackFpSet)
            {
                if (focusImg.GetPixel(fp.x, fp.y) > fp.g)
                    ++mismatchPtA;
            }
            return new ImageInfo(0, mismatchPtA, focusImg.Width, focusImg.Height);
        }

        /// <summary>
        /// 根据点集去识别指定路径的文件是否匹配
        /// </summary>
        /// <param name="sampleFile">文件路径</param>
        /// <returns>相关图像处理信息</returns>
        public static ImageInfo ImageCheck(string sampleFile)
        {
            PointSet ps = m_pointSet;
            FocusImg focusImg = FocusApi.OpenFocusDat(sampleFile, Global.SuplyPicIndex);
            if (focusImg == null)
            {
                if (showMessage != null)
                {
                    showMessage(sampleFile + " Open Failed\r\n", Color.Red);
                }
                return new ImageInfo(0, 0xFF, 0, 0);
            };
            int mismatchPtA = 0;
            foreach (FocusPoint fp in ps.WhiteFpSet)
            {
                short g = focusImg.GetPixel(fp.x, fp.y);
                if (g >= 256 || g < fp.g)
                    ++mismatchPtA;
            }

            foreach (FocusPoint fp in ps.BlackFpSet)
            {
                if (focusImg.GetPixel(fp.x, fp.y) > fp.g)
                    ++mismatchPtA;
            }
            return new ImageInfo(0, mismatchPtA, focusImg.Width, focusImg.Height);
        }

        /// <summary>
        /// 根据点集去识别指定路径的目录下的文件是否匹配
        /// </summary>
        /// <param name="folderpath">目录名</param>
        /// <returns>匹配信息</returns>
        public static Dictionary<string, ImageInfo> FolderImageCheck(string folderpath)
        {
            PointSet ps = m_pointSet;
            FileManage manage = new FileManage(folderpath);
            Dictionary<string, ImageInfo> d = new Dictionary<string, ImageInfo>();
            foreach (string path in manage.Filepaths)
            {
                d.Add(Path.GetFileName(path),  ImageCheck( path));
            }
            return d;
        }
    }//end of class PointTrain

    /// <summary>
    /// edit by ny @ Focus
    /// 2011 09 28
    /// 图像信息
    /// </summary>
    public class ImageInfo
    {
        /// <summary>
        /// 是否作过旋转
        /// </summary>
        public int Turn;

        /// <summary>
        /// 不匹配点数
        /// </summary>
        public int MissPt;

        /// <summary>
        /// 宽度
        /// </summary>
        public int Width;

        /// <summary>
        /// 高度
        /// </summary>
        public int Height;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="turn"></param>
        /// <param name="misspt"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public ImageInfo(int turn, int misspt, int width, int height)
        {
            Turn = turn;
            MissPt = misspt;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// 重写toString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("E:{0},    W:{1},H:{2}",MissPt,Width,Height);
        }
    }//end of class ImageInfo

    /// <summary>
    /// edit by ny @ Focus
    /// 2011 09 28
    /// 点训练信息
    /// </summary>
    public class PointTrainConfig
    {
        /// <summary>
        /// 使能标志位
        /// </summary>
        public bool[] enable = {false,true,false,true};

        /// <summary>
        /// 白点最低门限值
        /// </summary>
        public short WHITEBOTTOMTHR = 25;

        /// <summary>
        /// 黑点最高门限值
        /// </summary>
        public short BLACKTOPTHR = 190;

        /// <summary>
        /// 白点最低可接受门限值，否则丢弃
        /// </summary>
        public short WHITEACCEPTTHR = 30;

        /// <summary>
        /// 黑点最低可接受门限值，否则丢弃
        /// </summary>
        public short BLACKACCEPTTHR = 150;

        /// <summary>
        /// 白点训练门限再次减少值
        /// </summary>
        public short WHITEMINUSTHR = 8;

        /// <summary>
        /// 黑点训练门限再次增加值
        /// </summary>
        public short BLACKPLUSTHR = 8;

        /// <summary>
        /// 当白点门限高于此值时，考虑增大容错率，可以将门限改为此值
        /// </summary>
        public short WHITETOPTHR = 80;

        /// <summary>
        /// 当黑点门限低于此值时，考虑增大容错率，可以将门限改为此值
        /// </summary>
        public short BLACKBOTTOMTHR = 60;

        /// <summary>
        /// 白点总数限制
        /// </summary>
        public short WHITENUMBER = 60;

        /// <summary>
        /// 黑点总数限制
        /// </summary>
        public short BLACKNUMBER = 80;

        /// <summary>
        /// 白点不计入门限计算的百分比
        /// </summary>
        public float WHITELEFTPERCENT = 0.005f;

        /// <summary>
        /// 黑点不计入门限计算的百分比
        /// </summary>
        public float BLACKLEFTPERCENT = 0.005f;

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="savePath">保存路径</param>
        /// <returns></returns>
        public bool SerializeXML(string savePath)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(PointTrainConfig));
                Stream stream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.Read);
                xs.Serialize(stream, this);
                stream.Close();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <returns></returns>
        public static PointTrainConfig DeserializeXML(string path)
        {
            PointTrainConfig config;
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(PointTrainConfig));
                Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                config = xs.Deserialize(stream) as PointTrainConfig;
                stream.Close();
                return config;
            }
            catch (System.Exception)
            {
                return new PointTrainConfig();
            }
        }
    }//end of class PointTrainConfig
}
