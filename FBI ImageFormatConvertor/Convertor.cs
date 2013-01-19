using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;
using Focus;

namespace ImageFormatConvertor
{
    public class ConvertEventArgs
    {
        public ConvertEventArgs(int totalFileNum, int convertedNum)
        {
            TotalFileNum = totalFileNum;
            ConvertedNum = convertedNum;
        }
        public int TotalFileNum { get; private set; } // readonly
        public int ConvertedNum { get; private set; } // readonly
    }

    static class Convertor
    {

        public delegate void ConvertEventHandler(ConvertEventArgs args);

        public static event ConvertEventHandler convertEvent = null;

        private static Thread convertThread = new Thread(Convert);

        public static bool IsBackGroundAlive
        {
            get { return convertThread.IsAlive; }
        }

        private static object mux = new object();

        /// <summary>
        /// 后台执行转换操作
        /// </summary>
        /// <param name="config">转换参数</param>
        /// <returns>成功:true, 失败:false</returns>
        public static bool ConvertBackGround(ConvertConfig config)
        {
            if (convertThread.IsAlive)
            {
                return false;
            }
            convertThread = new Thread(Convert);
            convertThread.Start((object)config);
            return true;
        }

        /// <summary>
        /// 退出后台线程
        /// </summary>
        /// <returns></returns>
        public static void BackGroundAbort()
        {
            if (convertThread.IsAlive)
            {
                convertThread.Abort();
            }
        }

        /// <summary>
        /// 执行转换操作
        /// </summary>
        /// <param name="config">转换参数, ConvertConfig类型， 为了配合能线程调用, 声明为Object</param>
        public static void Convert(object config) 
        {
            ConvertConfig convertConfig = (ConvertConfig)config;
            switch (convertConfig.format)
            {
                case ConfigFormat.DM642:
                    ConvertFormatDM642(convertConfig);
                    return ;
                case ConfigFormat.C54XX:
                    return ;
                case ConfigFormat.FOCUS:
                    return ;
                default:
                    return ;
            }
        }

        #region  DM642图片格式转换
        
        class ConvertThreadPara
        {
            public string[] files; 
            public string saveRootFolder;
            public string denoName;
            public ConvertConfig config;
            public int totalNum;
            public ConvertThreadPara(string[] _files, string _saveRootFolder, string _denoName, int _totalNum, ConvertConfig _config)
            {
                files = _files;
                saveRootFolder = _saveRootFolder;
                denoName = _denoName;
                totalNum = _totalNum;
                config = _config;
            }
        }

        /// <summary>
        /// 转换一个目录下的DM642格式文件
        /// </summary>
        /// <param name="folder">目录路径</param>
        /// <param name="config">转换参数</param>
        private static void ConvertFormatDM642(ConvertConfig config)
        {
            string[] folders = Directory.GetDirectories(config.src);
            string saveRootFolder = config.dps;// +"\\" + Path.GetFileName(config.src);

            //统计总文件个数
            int totalFileNum = 0;
            int convertedNum = 0;
            for (int i = 0; i < folders.Length; i++ )
            {
                totalFileNum += GetFiles(folders[i], config).Length;
            }

            //开始转换,触发事件
            if (convertEvent != null)
            {
                ConvertEventArgs args = new ConvertEventArgs(totalFileNum, convertedNum);
                convertEvent(args);
            }  

            Parallel.ForEach(folders, denofolders =>
            //foreach(string denofolders in folders)
            {
                string denoName = Path.GetFileName(denofolders);
                string[] files = GetFiles(denofolders, config);
                foreach (string filepath in files)
                {
                    ConvertOnefile(filepath, saveRootFolder, denoName, config);
                    //完成一个文件的转换,触发事件
                    convertedNum++;
                    if (convertEvent != null)
                    {
                        ConvertEventArgs args = new ConvertEventArgs(totalFileNum, convertedNum);
                        convertEvent(args);
                    }                  
                }
            }
                    );

            //转换完成, 触发事件
            if (convertEvent != null)
            {
                ConvertEventArgs args = new ConvertEventArgs(totalFileNum, -1);
                convertEvent(args);
            }
        }

        /// <summary>
        /// 转换一个文件
        /// </summary>
        /// <param name="srcFilePath">文件路径</param>
        /// <param name="saveRootFolder">保存根目录</param>
        /// <param name="denoName">图片的面额名字</param>
        /// <param name="config">转换参数</param>
        private static void ConvertOnefile(string srcFilePath, string saveRootFolder, string denoName, ConvertConfig config)
        {
            string fileName = Path.GetFileName(srcFilePath);

            if (config.lightNum == ConfigLightNum.Triple)
            {
                string irPrefix = "";
                string grPrefix = "";
                string uvPrefix = "";
                if (config.irPrefix)
                    irPrefix = "IR_";
                if (config.grPrefix)
                    grPrefix = "GR_";
                if (config.uvPrefix)
                    uvPrefix = "UV_";

                string irSaveFolder = saveRootFolder + "\\" + denoName + "\\ir";
                string grSaveFolder = saveRootFolder + "\\" + denoName + "\\green";
                string uvSaveFolder = saveRootFolder + "\\" + denoName + "\\uv";
                if (config.Output != ConfigOutput.All)
                {
                    irSaveFolder = saveRootFolder + "\\" + irPrefix + denoName;
                    grSaveFolder = saveRootFolder + "\\" + grPrefix + denoName;
                    uvSaveFolder = saveRootFolder + "\\" + uvPrefix + denoName;
                }
                string irSavePath = irSaveFolder + "\\" + fileName;
                string grSavePath = grSaveFolder + "\\" + fileName;
                string uvSavePath = uvSaveFolder + "\\" + fileName;
                bool isConverted = true;

                //检查并创建目录
                if (config.Output == ConfigOutput.Ir || config.Output == ConfigOutput.All)
                {
                    if (!Directory.Exists(irSaveFolder))
                        Directory.CreateDirectory(irSaveFolder);
                    if (!File.Exists(irSavePath))
                        isConverted = false;
                }
                else if (config.Output == ConfigOutput.Green || config.Output == ConfigOutput.All)
                {
                    if (!Directory.Exists(grSaveFolder))
                        Directory.CreateDirectory(grSaveFolder);
                    if (!File.Exists(grSavePath))
                        isConverted = false;
                }
                else if (config.Output == ConfigOutput.Uv || config.Output == ConfigOutput.All)
                {
                    if (!Directory.Exists(uvSaveFolder))
                        Directory.CreateDirectory(uvSaveFolder);
                    if (!File.Exists(uvSavePath))
                        isConverted = false;
                }
                else
                    return;

                //判断是否转换
                if (isConverted && (!config.isTotal))
                    return;

                //格式转换
                FocusImg[] img;
                lock (mux)
                {
                    img = FocusApi.OpenFocusDat(srcFilePath, (OpenConfig)config);
                }
                if (img == null || img.Length != 3 || img[0] == null || img[1] == null || img[2] == null)
                    return;
                if (config.Output == ConfigOutput.Green || config.Output == ConfigOutput.All)
                {
                    img[0].Save(grSavePath);
                    img[0].ConvertTo8bppBitmap().Save(grSavePath.Replace(".dat", ".png"), ImageFormat.Png);
                }
                if (config.Output == ConfigOutput.Ir || config.Output == ConfigOutput.All)
                {
                    img[1].Save(irSavePath);
                    img[1].ConvertTo8bppBitmap().Save(irSavePath.Replace(".dat", ".png"), ImageFormat.Png);
                }
                if (config.Output == ConfigOutput.Uv || config.Output == ConfigOutput.All)
                {
                    img[2].Save(uvSavePath);
                    img[2].ConvertTo8bppBitmap().Save(uvSavePath.Replace(".dat", ".png"), ImageFormat.Png);
                }

            }
            else if (config.lightNum == ConfigLightNum.Double)
            {
                string irPrefix = "";
                string grPrefix = "";
                if (config.irPrefix)
                    irPrefix = "IR_";
                if (config.grPrefix)
                    grPrefix = "GR_";

                string irSaveFolder = saveRootFolder + "\\" + denoName + "\\ir";
                string grSaveFolder = saveRootFolder + "\\" + denoName + "\\green";
                if (config.Output != ConfigOutput.All)
                {
                    irSaveFolder = saveRootFolder + "\\" + irPrefix + denoName;
                    grSaveFolder = saveRootFolder + "\\" + grPrefix + denoName;
                }
                string irSavePath = irSaveFolder + "\\" + fileName;
                string grSavePath = grSaveFolder + "\\" + fileName;
                bool isConverted = true;

                //检查并创建目录
                if (config.Output == ConfigOutput.Ir || config.Output == ConfigOutput.All)
                {
                    if (!Directory.Exists(irSaveFolder))
                        Directory.CreateDirectory(irSaveFolder);
                    if (!File.Exists(irSavePath))
                        isConverted = false;
                }
                else if (config.Output == ConfigOutput.Green || config.Output == ConfigOutput.All)
                {
                    if (!Directory.Exists(grSaveFolder))
                        Directory.CreateDirectory(grSaveFolder);
                    if (!File.Exists(grSavePath))
                        isConverted = false;
                }
                else
                    return;

                //判断是否转换
                if (isConverted && (!config.isTotal))
                    return;

                //格式转换
                FocusImg[] img;
                lock (mux)
                {
                    img = FocusApi.OpenFocusDat(srcFilePath, (OpenConfig)config);
                }
                if (img == null || img.Length != 2 || img[0] == null || img[1] == null)
                    return;
                if (config.Output == ConfigOutput.Green || config.Output == ConfigOutput.All)
                {
                    img[0].Save(grSavePath);
                    img[0].ConvertTo8bppBitmap().Save(grSavePath.Replace(".dat", ".png"), ImageFormat.Png);
                }
                if (config.Output == ConfigOutput.Ir || config.Output == ConfigOutput.All)
                {
                    img[1].Save(irSavePath);
                    img[1].ConvertTo8bppBitmap().Save(irSavePath.Replace(".dat", ".png"), ImageFormat.Png);
                }
                    
            }
            else
            {
                string saveFolder = saveRootFolder + "\\" + denoName;
                string savePath = saveFolder + "\\" + fileName;
                bool isConverted = true;

                //检查并创建目录
                if (!Directory.Exists(saveFolder))
                    Directory.CreateDirectory(saveFolder);
                if (!File.Exists(savePath))
                    isConverted = false;

                //判断是否转换
                if (isConverted && (!config.isTotal))
                    return;

                //格式转换
                FocusImg[] img;
                lock (mux)
                {
                    img = FocusApi.OpenFocusDat(srcFilePath, (OpenConfig)config);
                }
                if (img == null || img.Length != 1 || img[0] == null )
                    return;
                img[0].Save(savePath);
                img[0].ConvertTo8bppBitmap().Save(savePath.Replace(".dat", ".png"), ImageFormat.Png);
            }
        }

        /// <summary>
        /// 获取一个目录下的文件: 有双光源和单光源两种形式
        /// </summary>
        /// <param name="folder">目录名</param>
        /// <param name="config">转换参数</param>
        /// <returns></returns>
        private static string[] GetFiles(string folder, ConvertConfig config)
        {
            string[] files = new string[0];
            try
            {
                if (config.lightNum == ConfigLightNum.Single)
                {
                    files = Directory.GetFiles(folder, "*.dat");
                }
                else
                {
                    folder += "\\ir";
                    files = Directory.GetFiles(folder, "*.dat");
                }
            }
            catch (System.Exception)
            {
            }
            return files;
        }

        #endregion
    }

    [Serializable]
    public class ConvertConfig : OpenConfig
    {
        public string src = "";
        public string dps = "";
        public bool irPrefix = true;
        public bool grPrefix = false;
        public bool uvPrefix = false;
        [XmlIgnore]
        public bool isTotal = true;
        
        public override void SerializeXML()
        {
            SerializeXML("config.xml");
        }

        /// <summary>
        /// 从默认路径加载配置方案
        /// </summary>
        /// <returns></returns>
        public override Object DeserializeXML()
        {
            return DeserializeXML("config.xml");
        }
    }
}
