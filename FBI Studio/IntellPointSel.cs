using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Focus;

namespace FBI_Studio
{
    /// <summary>
    /// edit by ny @ Focus
    /// 2011 09 28
    /// 智能取点算法</summary>
    static public class IntellPointSel
    {
        static public IntellPointSelConfig IntellPointConfig = new IntellPointSelConfig();
        static private string[] workFiles = null;
        static private string[] distingFiles = null;

        /// <summary>
        /// 后台进行智能取点, 由于速度快过预期, 占不实现后台操作
        /// </summary>
        /// <param name="workSampleFiles">工作样本集</param>
        /// <param name="distingSampleFiles">对比样本集</param>
        /// <returns></returns>
        static public void IntellSelBackground(string[] workSampleFiles, string[] distingSampleFiles)
        {
            if (workSampleFiles != null && distingSampleFiles != null)
            {
                workFiles = workSampleFiles;
                distingFiles = distingSampleFiles;
                IntellSel();
            }
        }

        /// <summary>
        /// 智能取点算法
        /// </summary>
        /// <returns></returns>
        static private void IntellSel()
        {
            Rectangle searchRect = GetProcessRect();
            //map和list都太慢 所以直接用数组
            short[][][] workPointHist = CreatPointHist(workFiles,searchRect);
            short[][][] distingPointHist = CreatPointHist(distingFiles,searchRect);

            //根据区分度对点集排序
            ArrayList whtPoint = new ArrayList();
            ArrayList blcPoint = new ArrayList();
            PointSort(workPointHist, distingPointHist, searchRect, ref whtPoint, ref blcPoint);

            //删除低区分度的点
            whtPoint.RemoveRange(IntellPointSel.IntellPointConfig.whtSelPoint, whtPoint.Count - IntellPointSel.IntellPointConfig.whtSelPoint);
            blcPoint.RemoveRange(IntellPointSel.IntellPointConfig.blcSelPoint, blcPoint.Count - IntellPointSel.IntellPointConfig.blcSelPoint);

            //是否替换现有的点
            if ( !IntellPointSel.IntellPointConfig.AddTo)
            {
                PointTrain.PointSet.BlackFpSet.Clear();
                PointTrain.PointSet.WhiteFpSet.Clear();
            }

            //保存点
            foreach (FocusPoint p in whtPoint)
            {
                bool isExist = false;
                foreach (FocusPoint op in PointTrain.PointSet.WhiteFpSet)
                {
                    if (op.x == p.x && op.y == p.y)
                        isExist = true;
                }
                if ( !isExist )
                    PointTrain.PointSet.WhiteFpSet.AddLast(p);
            }

            foreach (FocusPoint p in blcPoint)
            {
                bool isExist = false;
                foreach (FocusPoint op in PointTrain.PointSet.BlackFpSet)
                {
                    if (op.x == p.x && op.y == p.y)
                        isExist = true;
                }
                if (!isExist)
                    PointTrain.PointSet.BlackFpSet.AddLast(p);
            }
        }

        /// <summary>
        /// 对点集进行排序
        /// </summary>
        /// <param name="workPointHist">工作样本点集</param>
        /// <param name="distingPointHist">对比样本点集</param>
        /// <param name="rect">搜索区域</param>
        /// <param name="whtPoint">输出根据区分度排好序的白点</param>
        /// <param name="blcPoint">输出根据区分度排好序的黑点</param>
        /// <returns></returns>
        static private void PointSort(short[][][] workPointHist, short[][][] distingPointHist, Rectangle rect, ref ArrayList whtPoint, ref ArrayList blcPoint)
        {
            for (int h = rect.Top + IntellPointConfig.SearchSize; h < rect.Bottom - IntellPointConfig.SearchSize; h++)
            {
                for (int w = rect.Left + IntellPointConfig.SearchSize; w < rect.Right - IntellPointConfig.SearchSize; w++)
                {
                    PointDisc whtPointDisc = new PointDisc();
                    PointDisc blcPointDisc = new PointDisc();
                    GetDiscrimination(workPointHist[h][w], distingPointHist[h][w], ref whtPointDisc, ref blcPointDisc);
                    whtPointDisc.x = blcPointDisc.x = (short)w;
                    whtPointDisc.y = blcPointDisc.y = (short)h;
                    whtPoint.Add(whtPointDisc);
                    blcPoint.Add(blcPointDisc);
                }
            }
            RandomArrayList(ref whtPoint);
            RandomArrayList(ref blcPoint);
            whtPoint.Sort();
            blcPoint.Sort();
        }

        /// <summary>
        /// 对一个ArrayList随机化
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        static private void RandomArrayList(ref ArrayList list)
        {
            Random r = new Random();
            for (int i = 0; i < list.Count; i++ )
            {
                int n = r.Next(i, list.Count - 1);
                Object temp = list[n];
                list[n] = list[i];
                list[i] = temp;
            }
        }

        /// <summary>
        /// 两个点之间可区分度的关键函数(算法性能主要改动这个函数)
        /// 本算法为类叶贝斯分类
        /// </summary>
        /// <param name="workhist">工作点的灰度分布情况</param>
        /// <param name="distinghist">区分点的灰度分布情况</param>
        /// <returns></returns>
        static private void GetDiscrimination(short[] workhist, short[] distinghist, ref PointDisc whtPointDisc, ref PointDisc blcPointDisc)
        {
            int whtDisc = 0; int blcDisc = 0;
            //统计
            int workCount = 0, distCount = 0;
            for (int i = 0; i < 256; i++ )
            {
                workCount += workhist[i];
                distCount += distinghist[i];
            }
            if (workCount <=0 || distCount <=0)
            {
                whtDisc = 0;
                blcDisc = 0;
                return;
            }

            short whtThr = 0;
            short blcThr = 255;
            GetThr(workhist, workCount, ref whtThr, ref blcThr);
         
            //计算distPoint的误判数
            int whtErrCount = 0;
            for (int i = whtThr; i >= 0; i-- )
            {
                whtErrCount += distinghist[i];
            }
            int blcErrCount = 0;
            for (int i = blcThr; i < 256; i++ )
            {
                blcErrCount += distinghist[i];
            }

            //计算distPoint和workPoint的灰度差异
            short up = 0; short down = 0;
            GetUpAndDown(distinghist, distCount, ref up, ref down);
            short whtGrayDiff = (short)(whtThr - up);
            short blcGrayDiff = (short)(down - blcThr);

            //distPoint的误判个数
            whtDisc = (int)((double)30000 * (whtErrCount) / (double)distCount);
            blcDisc = (int)((double)30000 * (blcErrCount) / (double)distCount);

            if (IntellPointSel.IntellPointConfig.ClassMode ==  ConfigClassMode.GrayDiff)
            {
                whtDisc = whtDisc * 256 + whtGrayDiff;
                blcDisc = blcDisc * 256 + blcGrayDiff;
            }

            whtPointDisc.g = whtThr;
            blcPointDisc.g = blcThr;
            whtPointDisc.disc = whtDisc;
            blcPointDisc.disc = blcDisc;
        }

        /// <summary>
        /// 根据灰度分布,获取其灰度上下界, 根据根据PointTrain.TrainConfig的一些设置舍弃一些点
        /// </summary>
        /// <param name="hist">灰度分布</param>
        /// <param name="count">总点数</param>
        /// <param name="up">输出上届</param>
        /// <param name="down">输出下界</param>
        /// <returns></returns>
        static private void GetUpAndDown(short[] hist, int count, ref short up, ref short down)
        {
            //摒弃的点个数, 减少孤立点对算法性能的影响
            int blackAband = (int)(PointTrain.TrainConfig.BLACKLEFTPERCENT * count);
            int whiteAband = (int)(PointTrain.TrainConfig.WHITELEFTPERCENT * count);

            //分别求出灰度上下限
            for (up = 255; up >= 0; up--)
            {
                blackAband -= hist[up];
                if (blackAband < 0)
                    break;
            }
            for (down = 0; down < 256; down++)
            {
                whiteAband -= hist[down];
                if (whiteAband < 0)
                    break;
            }

            if (down > 255 || up < 0)
            {
                down = 0;
                up = 255;
            }
        }

        /// <summary>
        /// 根据灰度分布,获取其黑点和白点的阈值 
        /// </summary>
        /// <param name="hist">灰度分布</param>
        /// <param name="count">点总数</param>
        /// <param name="whtThr">输出白点灰度</param>
        /// <param name="blcThr">输出黑点灰度</param>
        /// <returns></returns>
        static private void GetThr(short[] hist, int count, ref short whtThr, ref short blcThr)
        {
            //摒弃的点个数, 减少孤立点对算法性能的影响, 为了配合晋朝的点门阀训练 暂时不提供此功能
            int blackAband = 0; //(int)(PointTrain.TrainConfig.BLACKLEFTPERCENT * count);
            int whiteAband = 0;//(int)(PointTrain.TrainConfig.WHITELEFTPERCENT * count);

            //分别求出灰度上下限
            for (blcThr = 255; blcThr >= 0; blcThr--)
            {
                blackAband -= hist[blcThr];
                if (blackAband < 0)
                    break;
            }
            for (whtThr = 0; whtThr < 256; whtThr++)
            {
                whiteAband -= hist[whtThr];
                if (whiteAband < 0)
                    break;
            }

            if (whtThr>255 || blcThr<0)
            {
                whtThr = 0;
                blcThr = 255;
                return;
            }

            //调用PointTrain里面的点过滤函数得到
            ArrayList value = new ArrayList();
            value.Add(whtThr);
            value.Add(blcThr);
            if ( !PointTrain.FilterGrey(true, value, ref whtThr) )
                whtThr = 0;
            if (!PointTrain.FilterGrey(false, value, ref blcThr))
                blcThr = 255;
        }

        /// <summary>
        /// 获取需要进行智能取点的区域, 受到IntellPointConfig.Area的配置影响
        /// </summary>
        /// <returns></returns>
        static private Rectangle GetProcessRect()
        {
            int minWidth = short.MaxValue, minHeight = short.MaxValue;
            foreach (string file in workFiles)
            {
                Size si = FocusApi.GetFormatFocusSize(file);
                if (si.Width < 0)
                    continue;
                minWidth = Math.Min(si.Width, minWidth);
                minHeight = Math.Min(si.Height, minHeight);
            }

            int xoff = 15+IntellPointConfig.SearchSize;
            int yoff = 10+IntellPointConfig.SearchSize;
            Rectangle rect = new Rectangle(xoff, yoff, minWidth-2*xoff, minHeight-2*yoff);
            if (IntellPointConfig.Area)
            {
                Rectangle temp = PointAnlysePicBox.m_selectRect;
                if (rect.Contains(temp) && temp.Width > 0 && temp.Height > 0)
                    rect = temp;
            }
            return rect;
        }

        /// <summary>
        /// 创建点集
        /// </summary>
        /// <param name="files">样本集合</param>
        /// <param name="searchRect">需要创建点集的区域</param>
        /// <returns></returns>
        static short[][][] CreatPointHist( string[] files, Rectangle searchRect)
        {
            int searchSize = IntellPointConfig.SearchSize;
            short[][][] pointHist = new short[searchRect.Bottom][][];
            for (int j = 0; j < searchRect.Bottom; j++)
            {
                pointHist[j] = new short[searchRect.Right][];
                for (int i = 0; i < searchRect.Right; i++)
                {
                    pointHist[j][i] = new short[256];
                }
            }
           
            foreach (string file in files)
            {
                FocusImg img = FocusApi.OpenFocusDat(file, Global.SuplyPicIndex);
                if (img == null)
                    continue;
                int right = Math.Min(searchRect.Right, img.Width);
                int bottom = Math.Min(searchRect.Bottom, img.Height);
                Rectangle adjustRect = new Rectangle(searchRect.Left, searchRect.Top, right - searchRect.Left, bottom - searchRect.Top);
                byte[] imgData = img.ImgData;
                int stride = img.Stride;
                int witdh = searchRect.Width;

                for (int y = adjustRect.Top + searchSize; y < adjustRect.Bottom - searchSize; y++)
                {
                    for (int x = adjustRect.Left + searchSize; x < adjustRect.Right - searchSize; x++)
                    {
                        int index = y * stride + x;
                        int w = x;
                        int h = y;
                        for (int i = -searchSize; i <= searchSize; i++)
                        {
                            for (int j = -searchSize; j <= searchSize; j++)
                            {
                                byte g = imgData[index + i*stride + j];
                                pointHist[h][w][g]++;
                            }
                        }
                    }
                }
            }
            return pointHist;
        }

    }//end of class IntellPointSel

    public enum ConfigClassMode { ErrorCount, GrayDiff };  //2种格式

    public class IntellPointSelConfig
    {
        public ConfigClassMode ClassMode = ConfigClassMode.GrayDiff;
        public int whtSelPoint = 10;
        public int blcSelPoint = 25;
        public int SearchSize = 1;
        public bool AddTo = true;
        public bool Area = false;
    }

    public class PointDisc : FocusPoint, IComparable 
    {
        public int disc = 0;
        public int CompareTo(object obj)
        {
            //倒序
            return (-1)*this.disc.CompareTo( ((PointDisc)obj).disc );
        }
    }
}
