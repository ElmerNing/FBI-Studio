using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FBI_Studio
{
    /// <summary>
    /// edit by nicky @ Focus
    /// 2011 04 14
    /// 点集类
    /// </summary>
    public class PointSet
    {
        /// <summary>
        /// 黑点集合
        /// </summary>
        public LinkedList<FocusPoint> BlackFpSet = new LinkedList<FocusPoint>();

        /// <summary>
        /// 白点集合
        /// </summary>
        public LinkedList<FocusPoint> WhiteFpSet = new LinkedList<FocusPoint>();


        public void Save(string filepath, string faceType)
        {
            switch (Global.CurrentPointFormat)
            {
                case PointFormat.DM642:
                    SaveDm642Point(filepath, faceType);
                    return;
                case PointFormat.C54XX:
                    SaveC54XXPoint(filepath, faceType);
                    return;
                default:
                    throw new Exception(string.Format("未知的点格式{0}！", Global.CurrentPointFormat));
            }
        }





        /// <summary>
        /// 从文件中载入点集
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns>是否载入成功，载入失败的原因为点文件不存在</returns>
        public bool Load(string filepath)
        {
            switch (Global.CurrentPointFormat)
            {
                case PointFormat.DM642:
                    return LoadDm642Point(filepath);
                case PointFormat.C54XX:
                    return LoadC54XXPoint(filepath);
                default :
                    throw new Exception(string.Format("未知的点格式{0}！", Global.CurrentPointFormat));
            }
        }

        /// <summary>
        /// 从DM642格式的文件中载入点集
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns>是否载入成功，载入失败的原因为点文件不存在</returns>
        private bool LoadDm642Point(string filepath)
        {
            try
            {
                WhiteFpSet.Clear();
                BlackFpSet.Clear();
                if (!File.Exists(filepath)) return false;
                StreamReader sr = new StreamReader(filepath);
                char[] separator = { '\t', ' ', ',' };

                string getString;
                while ((getString = sr.ReadLine()) != null)
                {
                    string[] fpstr = getString.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    if (fpstr.Length == 3 && fpstr[0].Length <= 3 && fpstr[1].Length <= 3 && fpstr[2].Length <= 3)
                    {
                        FocusPoint fp = new FocusPoint(short.Parse(fpstr[0]), short.Parse(fpstr[1]), short.Parse(fpstr[2]));
                        if (fp.g < 256)
                        {
                            WhiteFpSet.AddLast(fp);
                        }
                        else
                        {
                            BlackFpSet.AddLast(new FocusPoint(fp.x, fp.y, (short)(fp.g - 256)));
                        }
                    }
                }
                sr.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return true;
        }

        /// <summary>
        /// 从C54XX格式的文件中载入点集
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns>是否载入成功，载入失败的原因为点文件不存在</returns>
        private bool LoadC54XXPoint(string filepath)
        {
            try
            {
                WhiteFpSet.Clear();
                BlackFpSet.Clear();

                if (!File.Exists(filepath)) return false;
                StreamReader sr = new StreamReader(filepath);

                string getstring = null;

                //读取whitePoint
                List<FocusPoint> whitePoint = new List<FocusPoint>();
                while ((getstring = sr.ReadLine()) != null && getstring.IndexOf('{') < 0) ;
                while ((getstring = sr.ReadLine()) != null && getstring.IndexOf('}') < 0)
                {
                    string s = getstring.Substring(getstring.IndexOf("0x")+2,4);
                    int v = Convert.ToInt32(s, 16);
                    short v1 = (short)(v >> 8);
                    short v2 = (short)(v & 0xFF);
                    whitePoint.Add(new FocusPoint(v1, v2, 0));
                }

                int index = 0;
                while ((getstring = sr.ReadLine()) != null && getstring.IndexOf('{') < 0) ;
                while ((getstring = sr.ReadLine()) != null && getstring.IndexOf('}') < 0)
                {
                    string s = getstring.Substring(getstring.IndexOf("0x") + 2, 4);
                    int v = Convert.ToInt32(s, 16);
                    short v1 = (short)(v >> 8);
                    short v2 = (short)(v & 0xFF);
                    whitePoint[index++].g = v1;
                    if (index < whitePoint.Count)
                        whitePoint[index++].g = v2;
                }

                foreach (FocusPoint pt in whitePoint)
                {
                    WhiteFpSet.AddLast(pt);
                }

                //读取blackPoint
                List<FocusPoint> blackPoint = new List<FocusPoint>();
                while ((getstring = sr.ReadLine()) != null && getstring.IndexOf('{') < 0) ;
                while ((getstring = sr.ReadLine()) != null && getstring.IndexOf('}') < 0)
                {
                    string s = getstring.Substring(getstring.IndexOf("0x") + 2, 4);
                    int v = Convert.ToInt32(s, 16);
                    short v1 = (short)(v >> 8);
                    short v2 = (short)(v & 0xFF);
                    blackPoint.Add(new FocusPoint(v1, v2, 0));
                }

                index = 0;
                while ((getstring = sr.ReadLine()) != null && getstring.IndexOf('{') < 0) ;
                while ((getstring = sr.ReadLine()) != null && getstring.IndexOf('}') < 0)
                {
                    string s = getstring.Substring(getstring.IndexOf("0x") + 2, 4);
                    int v = Convert.ToInt32(s, 16);
                    short v1 = (short)(v >> 8);
                    short v2 = (short)(v & 0xFF);
                    blackPoint[index++].g = v1;
                    if (index < blackPoint.Count)
                        blackPoint[index++].g = v2;
                }
                foreach (FocusPoint pt in blackPoint)
                {
                    BlackFpSet.AddLast(pt);
                }

                sr.Close();
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("点文件格式有误！");
            }
            return true;
        }

        /// <summary>
        /// 将点集保存到Dm642格式的点文件中
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <param name="faceType">面向信息，形如10_1A</param>
        private void SaveDm642Point(string filepath, string faceType)
        {
            try
            {
                if (WhiteFpSet.Count + BlackFpSet.Count == 0) return;
                StreamWriter sw = new StreamWriter(filepath);

                sw.WriteLine(string.Format("// - - - - - - - - - - - - - {0} - - - - - - - - - - - - - //", faceType));
                // Write Points
                sw.WriteLine(string.Format("#define  POINT_SIZE{0}\t{1}", faceType, WhiteFpSet.Count + BlackFpSet.Count));
                sw.WriteLine(string.Format("static const short Point{0}[POINT_SIZE{1}*3] = ", faceType, faceType));

                sw.WriteLine("{");
                foreach (FocusPoint fp in WhiteFpSet)
                {
                    sw.WriteLine(string.Format("\t{0},{1},{2},", fp.x, fp.y, fp.g));
                }
                //黑点门限+256
                foreach (FocusPoint fp in BlackFpSet)
                {
                    sw.WriteLine(string.Format("\t{0},{1},{2},", fp.x, fp.y, fp.g + 256));
                }
                sw.WriteLine("};");
                sw.WriteLine(string.Format("// - - - - - - - - - - - - - {0} - - - - - - - - - - - - - //", faceType));
                sw.Flush();
                sw.Close();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void SaveC54XXPoint(string filepath, string faceType)
        {
            FocusPoint[] whitePoint = new FocusPoint[WhiteFpSet.Count];
            FocusPoint[] blackPoint = new FocusPoint[BlackFpSet.Count];
            WhiteFpSet.CopyTo(whitePoint,0);
            BlackFpSet.CopyTo(blackPoint,0);

            StreamWriter sw = new StreamWriter(new FileStream(filepath, FileMode.OpenOrCreate), Encoding.ASCII);
            //保存白点
            sw.WriteLine(string.Format("// * * * * * * * * * * * * * {0} * * * * * * * * * * * * * //", faceType));
            sw.WriteLine(string.Format("#define  WHITE_POINT_SIZE{0}	{1}", faceType, WhiteFpSet.Count));
            sw.WriteLine(string.Format("const WORD WhitePoint{0}[WHITE_POINT_SIZE{0}] = ", faceType, faceType));
            sw.WriteLine("{");
            for (int i = 0; i < whitePoint.Length;i++ )
            {
                int x = whitePoint[i].x;
                int y = whitePoint[i].y;
                sw.WriteLine(GenOneLine(x, y));
                //sw.WriteLine(string.Format("\t\t0x{0:X},\t\t//{1},{2}", xy, x, y));
            }
            sw.WriteLine("};");
            sw.WriteLine("");

            sw.WriteLine(string.Format("const WORD WhiteThreshold{0}[(WHITE_POINT_SIZE{0}+1)/2] = ", faceType, faceType));
            sw.WriteLine("{");
            for (int i=0; i<(whitePoint.Length+1)/2; i++)
            {
                int thr1 = (whitePoint[i*2].g)&0xff;
                int thr2 = i * 2 + 1 < whitePoint.Length ? whitePoint[i * 2 + 1].g : 0;
                sw.WriteLine(GenOneLine(thr1, thr2));
                //sw.WriteLine(string.Format("\t\t0x{0:X},\t\t//{1},{2}", thr, thr1, thr2));
            }
            sw.WriteLine("};");
            sw.WriteLine("");

            //保存黑点
            sw.WriteLine(string.Format("#define  BLACK_POINT_SIZE{0}	{1}", faceType, BlackFpSet.Count));
            sw.WriteLine(string.Format("const WORD BlackPoint{0}[BLACK_POINT_SIZE{0}] = ", faceType, faceType));
            sw.WriteLine("{");
            for (int i = 0; i < blackPoint.Length; i++ )
            {
                int x = (blackPoint[i].x)&0xff;
                int y = (blackPoint[i].y)&0xff;
                sw.WriteLine(GenOneLine(x, y));
                //sw.WriteLine(string.Format("\t\t0x{0:X},\t\t//{1},{2}", xy, x, y));
            }
            sw.WriteLine("};");
            sw.WriteLine("");

            sw.WriteLine(string.Format("const WORD BlackThreshold{0}[(BLACK_POINT_SIZE{0}+1)/2] = ", faceType, faceType));
            sw.WriteLine("{");
            for (int i = 0; i < (blackPoint.Length + 1) / 2; i++)
            {
                int thr1 = (blackPoint[i * 2].g)&0xff;
                int thr2 = i * 2 + 1 < blackPoint.Length ? blackPoint[i * 2 + 1].g : 0;
                sw.WriteLine(GenOneLine(thr1, thr2));
                //sw.WriteLine(string.Format("\t\t0x{0:X},\t\t//{1},{2}", thr, thr1, thr2));
            }
            sw.WriteLine("};");
            sw.WriteLine(string.Format("// * * * * * * * * * * * * * {0} * * * * * * * * * * * * * //", faceType));

            sw.Flush();
            sw.Close();
        }

        private string GenOneLine(int data1, int data2)
        {
            data1 &= 0xff;
            data2 &= 0xff;
            int data = (data1 << 8) + data2;
            string sd = string.Format("{0:X}",data);
            int dl = 4 - sd.Length;
            for (int i = 0; i < dl; i++ )
            {
                sd = '0' + sd;
            }
            return string.Format("\t\t0x{0},\t\t//{1},{2}", sd, data1, data2);
        }
    
    }
}
