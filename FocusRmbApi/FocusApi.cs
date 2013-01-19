using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace Focus
{
    public class FocusApi
    {
        private static object mux = new object();
        private static PreinstallConfig _Config = new PreinstallConfig();
        /// <summary>
        /// 根据配置打开dat文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="config">打开配置</param>
        /// <returns>config为All模式时候: FocusImg[0]为红外; FocusImg[1],绿光. FocusImg[2]为紫外，
        /// config为Ir，Green或Uv模式时，FocusImg[0]返回当前指定的图像。
        /// 出错时返回null</returns>
        public static FocusImg[] OpenFocusDat(string path, OpenConfig config)
        {
            switch (config.format)
            {
                case ConfigFormat.DM642:
                    return OpenFormatDM642(path, config);
                case ConfigFormat.C54XX:
                    return OpenFormatC54XX(path, config);
                case ConfigFormat.FOCUS:
                    return OpenFormatFOCUS(path, config);
                default:
                    return OpenFormatDM642(path, config);
            }
        }

        /// <summary>
        /// 根据配置打开dat文件,并返回指定的一幅图片
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="config">打开配置</param>
        /// <param name="picIndex">图片索引: 一般情况0为绿光,1为红外,2为紫外</param>
        /// <returns>返回一幅图片</returns>
        public static FocusImg OpenFocusDat(string path, OpenConfig config, int picIndex)
        {
            FocusImg[] imgs = OpenFocusDat(path, config);
            if (imgs == null || imgs.Length == 0)
            {
                return null;
            }
            if (picIndex < 0 || picIndex > imgs.Length)
            {
                return imgs[0];
            }
            else
                return imgs[picIndex];
        }

        /// <summary>
        /// 自动识别文件格式 并打开图片
        /// </summary>
        /// <param name="path"></param>
        /// <returns>
        /// config为All模式时候: FocusImg[0]为绿光; FocusImg[1]红外. FocusImg[2]为紫外。
        /// config为Ir，Green或Uv模式时，FocusImg[0]返回当前指定的图像。
        /// 出错时返回null。
        /// </returns>
        public static FocusImg[] OpenFocusDat(string path)
        {
            BinaryReader br = null;
            try
            {
                FileStream stream = File.OpenRead(path);
                br = new BinaryReader(stream);
                int first = br.ReadInt32();
                int second = br.ReadInt32();
                int stride = 0;
                if (first % 4 != 0)
                {
                    stride = (first + 4) & 0x7FFFFFFC;
                }
                else
                    stride = first;

                if (first+4 == stream.Length)
                {
                    return OpenFocusDat(path, _Config.DM642Config);
                }
                else if ( stride * second == stream.Length - 8)
                {
                    return OpenFormatFOCUS(path, null);
                }
                else
                {
                    return OpenFormatC54XX(path, _Config.C54XXConfig);
                }
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 自动识别文件格式 并打开图片,并根据索引返回一幅图片
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="picIndex">图片索引: 一般情况0为绿光, 1为红外, 2为紫外</param>
        /// <returns>返回一幅图片,识别返回null</returns>
        public static FocusImg OpenFocusDat(string path, int picIndex)
        {
            FocusImg[] imgs = OpenFocusDat(path);
            if (imgs == null || imgs.Length == 0)
            {
                return null;
            }
            if (picIndex < 0 || picIndex >= imgs.Length)
            {
                return imgs[0];
            }
            else
                return imgs[picIndex];
        }

        /// <summary>
        /// 打开DM642格式的文件, 多线程调用不安全
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="config">转换配置配置</param>
        /// <returns>
        /// config为All模式时候: FocusImg[0]为绿光; FocusImg[1]红外. FocusImg[2]为紫外。
        /// config为Ir，Green或Uv模式时，FocusImg[0]返回当前指定的图像。
        /// 出错时返回null。
        /// </returns>
        public static FocusImg[] OpenFormatDM642(string path, OpenConfig config)
        {
            string fileName = Path.GetFileName(path);
            string folder = Path.GetDirectoryName(path);
            string folderName = Path.GetFileName(folder);

            string[] inputpath = new string[3];
            inputpath[0] = ""; inputpath[1] = ""; inputpath[2] = "";
            
            short lightNum = 0;

            //单光源
            if (config.lightNum == ConfigLightNum.Single)
            {
                inputpath[0] = path;
                lightNum = 1;
                if (!File.Exists(inputpath[0]))
                    return null;
            }

            //多光源 : 三光源和双光源
            if (config.lightNum == ConfigLightNum.Double || config.lightNum == ConfigLightNum.Triple)
            {
                //绿光路径
                inputpath[0] = folder.Replace(folderName, "green") + @"\" + fileName;
                //红外路径
                inputpath[1] = folder.Replace(folderName, "ir") + @"\" + fileName;
                lightNum = 2;
                if (!File.Exists(inputpath[0]) || !File.Exists(inputpath[1]))
                    return null;
            }
            if (config.lightNum == ConfigLightNum.Triple)
            {
                //紫外路径
                inputpath[2] = folder.Replace(folderName, "uv") + @"\" + fileName;
                lightNum = 3;
                if (!File.Exists(inputpath[2]))
                    return null;
            }

            //设置校正方式
            int isReflect = config.reviseMode != ConfigReviseMode.Reflect ? 0 : 1;

            //设置矫正图
            int reviseIndex = 0;
            if (config.lightNum == ConfigLightNum.Single)
                reviseIndex = 0;
            if (config.basicImg == ConfigBasicImg.Ir)
                reviseIndex = 1;
            else if (config.basicImg == ConfigBasicImg.Green)
                reviseIndex = 0;

            //读取文件并且校正
            try
            {
                FileStream[] fsArray = new FileStream[3];
                byte[][] datArray = new byte[3][];
                int[] lenArray = new int[3];

                fsArray[0] = File.OpenRead(inputpath[0]);
                datArray[0] = new byte[fsArray[0].Length];
                fsArray[0].Read(datArray[0], 0, (int)fsArray[0].Length);
                lenArray[0] = datArray[0].Length-4;
                fsArray[0].Close();

                if (config.lightNum == ConfigLightNum.Double || config.lightNum == ConfigLightNum.Triple)
                {
                    fsArray[1] = File.OpenRead(inputpath[1]);
                    datArray[1] = new byte[fsArray[1].Length];
                    fsArray[1].Read(datArray[1], 0, (int)fsArray[1].Length);
                    lenArray[1] = datArray[1].Length-4;
                    fsArray[1].Close();
                }
                if (config.lightNum == ConfigLightNum.Triple)
                {
                    fsArray[2] = File.OpenRead(inputpath[2]);
                    datArray[2] = new byte[fsArray[2].Length];
                    fsArray[2].Read(datArray[2], 0, (int)fsArray[2].Length);
                    lenArray[2] = datArray[2].Length-4;
                    fsArray[2].Close();
                }

                IntPtr[] pDat = new IntPtr[3];
                for (int i = 0; i < pDat.Length; i++ )
                {
                        unsafe
                        {
                            fixed (byte* p = datArray[i])
                            {
                                pDat[i] = new IntPtr(p);
                            }
                    }
                }

                if (1 != DM642API.WinImageInitPlus(pDat, lenArray, lightNum, reviseIndex, isReflect))
                    return null;

                FocusImg[] imgs = new FocusImg[lightNum];
                for (int picIndex = 0; picIndex < lightNum; picIndex++)
                {
                    int width = DM642API.RMB_getEWidth(picIndex);
                    int height = DM642API.RMB_getEHeight(picIndex);
                    FocusImg img = new FocusImg(width, height);
                    DM642API.RmbGetReviseImgByStride(img.ImgData, 0, 0, width, height, img.Stride, picIndex);
                        
                    bool isEqual = false;
                    bool isNeighbor = false;
                    if (lightNum == 1 || picIndex == 1) //单光源或者红外
                    {
                        isEqual = config.irEqual;
                        isNeighbor = config.irNeighbor;
                    }
                    else
                    {
                        isEqual = config.uvgrEqual;
                        isNeighbor = config.uvgrNeighbor;
                    }

                    if (isEqual)
                    {
                        
                        int[] hist = new int[256];
                        DM642API.GetHist(hist, picIndex);
                        img.Equalization(hist);
                    }
                    if (isNeighbor)
                        img.AverageFilter();

                    imgs[picIndex] = img;
                }
                return imgs;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 打开C54XX格式的dat文件,暂时没有实现
        /// </summary>
        /// <param name="path"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static FocusImg[] OpenFormatC54XX(string path, OpenConfig config)
        {
            FileStream irfs = null;
            try
            {
                irfs = File.OpenRead(path);
                byte[] irDat = new byte[irfs.Length];
                irfs.Read(irDat, 0, (int)irfs.Length);
                C54XXAPI.C5409_Init(irDat, (UInt32)irfs.Length);
                float EW = C54XXAPI.C5409_GetW();
                float EH = C54XXAPI.C5409_GetH();
                int width = (int)Math.Round(EW);
                int height = (int)Math.Round(EH);
                FocusImg[] img = new FocusImg[1];
                img[0] = new FocusImg(width, height);

                if (config.irEqual == true)
                    C54XXAPI.C5409_GethistequalImg(img[0].ImgData, 0, 0, width, height, img[0].Stride);
                else
                    C54XXAPI.C5409_GetImg(img[0].ImgData, 0, 0, width, height, img[0].Stride);

                return img;
            }
            catch (System.Exception ex)
            {
                string m = ex.Message;
                return null;
            }
            finally
            {
                irfs.Close();
            }
        }

        /// <summary>
        /// 打开Focus格式的文件,包含双光源模式
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="config">转换配置配置,本函数配置可以为null</param>
        /// <returns>
        /// config为All模式时候: FocusImg[0]为绿光; FocusImg[1]红外. FocusImg[2]为紫外。
        /// config为Ir，Green或Uv模式时，FocusImg[0]返回当前指定的图像。
        /// 出错时返回null。
        /// </returns>
        public static FocusImg[] OpenFormatFOCUS(string path, OpenConfig config)
        {
            FocusImg img1 = OpenFormatFOCUS(path);
            FocusImg img2 = null;
            string pathgr = path.Replace("ir", "green");
            int imgNum = 1;
            if (File.Exists(pathgr) && pathgr != path)
            {
                imgNum = 2;
                img2 = OpenFormatFOCUS(pathgr);
            }

            FocusImg[] imgs = null;
            if (imgNum == 1)
            {
                if (img1 != null)
                {
                    imgs = new FocusImg[1];
                    imgs[0] = img1;
                    return imgs;
                }
                else
                    return imgs;
            }
            else
            {
                if (img1 != null && img2 != null)
                {
                    imgs = new FocusImg[2];
                    imgs[0] = img1;
                    imgs[1] = img2;
                    return imgs;
                }
                else
                    return imgs;
            }
        }

        /// <summary>
        /// 打开单幅Focus格式的图片
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static FocusImg OpenFormatFOCUS(string path)
        {
            BinaryReader br = null;
            try
            {
                br = new BinaryReader(File.OpenRead(path));
                int width = br.ReadInt32();
                int height = br.ReadInt32();
                FocusImg img = new FocusImg(width, height);
                img.ImgData = br.ReadBytes(img.Stride * height);
                return img;
            }
            catch (System.Exception)
            {
                return null;
            }
            finally
            {
                if (br != null)
                {
                    br.Close();
                }
            }
        }

        /// <summary>
        /// 获得Focus格式图片的长宽
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static Size GetFormatFocusSize(string Path)
        {
            BinaryReader br = null;
            try
            {
                br = new BinaryReader(File.OpenRead(Path));
                UInt64 wh = (UInt64)br.ReadInt64();
                //long length = br.BaseStream.Length;
                int width = (int)(wh & 0xFFFFFFFF);
                int height = (int)(wh >> 32);
//                 if (length != width*height + 4)
//                 {
//                     return new Size(-1, -1);
//                 }
//                 else
                    return new Size(width, height);
            }
            catch (System.Exception)
            {
                return new Size(-1, -1);
            }
            finally
            {
                if (br != null)
                {
                    br.Close();
                }
            }
        }

    }

    partial class PreinstallConfig
    {
        public DM642OpenConfig DM642Config = new DM642OpenConfig();
        public C54XXOpenConfig C54XXConfig = new C54XXOpenConfig();
        public PreinstallConfig()
        {
            DM642Config = (DM642OpenConfig)DM642Config.DeserializeXML();
            DM642Config.SerializeXML();

            C54XXConfig = (C54XXOpenConfig)C54XXConfig.DeserializeXML();
            C54XXConfig.SerializeXML();
        }
    }
}
