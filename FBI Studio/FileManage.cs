using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;

namespace FBI_Studio
{
    public class FileManage
    {
        public FileManage(string folder)
        {
            CurruntFolder = folder;
        }

        private bool m_isDouble = false;
        public bool IsDouble
        {
            get { return m_isDouble; }
            private set { m_isDouble = value; }
        }
        private string m_currentFolder = "";
        public string CurruntFolder
        {
            get { return m_currentFolder; }
            set
            {
                try
                {
                    m_currentFolder = value;
                    if (Directory.Exists(m_currentFolder + @"\ir") && Directory.Exists(m_currentFolder + @"\green"))
                    {
                        m_filepaths = Directory.GetFiles(m_currentFolder + @"\ir", "*.dat");
                        this.IsDouble = true;
                    }
                    else
                    {
                        m_filepaths = Directory.GetFiles(m_currentFolder, "*.dat");
                        this.IsDouble = false;
                    }
                    m_index = 0;
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
            }
        }

        private int m_index = 0;
        public int Index
        {
            get { return m_index; }
            set { m_index = value;}
        }
        private string[] m_filepaths = null;
        public string[] Filepaths
        {
            get
            {
                return m_filepaths;
            }
        }

        public delegate void ShowMessage(string s, Color c);

        /// <summary>
        /// 处理过程中产生的消息的委托
        /// </summary>
        public static event ShowMessage showMessage = null;

        public string GetNextFilePath()
        {
            if (Filepaths == null || Filepaths.Length <= 0)
            {
                throw new Exception("未找到任何文件");
            }
            m_index++;
            if (m_index >= m_filepaths.Length)
            {
                m_index = m_filepaths.Length - 1;
                throw new Exception("已是最后");
            }
            return m_filepaths[m_index];
        }

        public string GetLastFilePath()
        {
            if (Filepaths == null || Filepaths.Length <= 0)
            {
                throw new Exception("未找到任何文件");
            }
            m_index--;
            if (m_index < 0)
            {
                m_index = 0;
                throw new Exception("已是第一张");
            }
            return m_filepaths[m_index];
        }

        public string GetCurrentFilePath()
        {
            if (Filepaths == null || Filepaths.Length <= 0 )
            {
                throw new Exception("未找到任何文件");
            }
            if (m_index < 0)
                m_index = 0;
            else if (m_index >= Filepaths.Length)
                m_index = Filepaths.Length - 1;

            return Filepaths[m_index];
        }

        public string GetRandFilePath()
        {
            if (Filepaths == null || Filepaths.Length == 0)
            {
                throw new Exception("未找到任何文件");
            }
            Random r = new Random(DateTime.Now.Millisecond * Filepaths.Length);
            m_index = r.Next(Filepaths.Length-1);
            return Filepaths[m_index];
        }

        public void FreshCurrentFolder()
        {
            int oldIndex = this.Index;
            this.CurruntFolder = this.CurruntFolder;
            if (oldIndex < 0)
            {
                this.Index = 0;
            }
            else if (oldIndex >= this.Filepaths.Length)
            {
                this.Index = this.Filepaths.Length - 1;
            }
            else
                this.Index = oldIndex;
        }

        public static bool MoveTo(FileManage src, FileManage dst)
        {
            if ( !(Directory.Exists(src.CurruntFolder) && Directory.Exists(dst.CurruntFolder) ) )
            {
                return false;
            }
            if (src.IsDouble != dst.IsDouble)
            {
                return false;
            }

            string srcDatPath = src.GetCurrentFilePath();
            string srcPngPath = srcDatPath.Replace(".dat", ".png");
            string srcDatName = Path.GetFileName(srcDatPath);
            string srcPngName = Path.GetFileName(srcPngPath);
            string newDatPath = Path.GetDirectoryName(dst.GetCurrentFilePath()) + "\\" + srcDatName;;
            string newPngPath = Path.GetDirectoryName(dst.GetCurrentFilePath()) + "\\" + srcPngName;

            try
            {
                if (File.Exists(srcDatPath))
                {
                    File.Move(srcDatPath, newDatPath);
                    if (showMessage != null)
                        showMessage(srcDatPath + "\r\nmove to\r\n" + newDatPath + "\r\n\r\n", Color.Blue);
                }
            }
            catch (System.Exception ex)
            {
                if (showMessage != null)
                    showMessage(ex.Message + "\r\n", Color.Red);
            }
            try
            {
                if (File.Exists(srcPngPath))
                {
                    File.Move(srcPngPath, newPngPath);
                    if (showMessage != null)
                        showMessage(srcPngPath + "\r\nmove to\r\n" + newPngPath + "\r\n\r\n", Color.Blue);
                }
            }
            catch (System.Exception ex)
            {
                if (showMessage != null)
                    showMessage(ex.Message + "\r\n", Color.Red);
            }

            srcDatPath = srcDatPath.Replace("\\ir\\", "\\green\\");
            srcPngPath = srcPngPath.Replace("\\ir\\", "\\green\\");
            newDatPath = newDatPath.Replace("\\ir\\", "\\green\\");
            newPngPath = newPngPath.Replace("\\ir\\", "\\green\\");

            try
            {
                if (File.Exists(srcDatPath))
                {
                    File.Move(srcDatPath, newDatPath);
                    if (showMessage != null)
                        showMessage(srcDatPath + "\r\nmove to\r\n" + newDatPath + "\r\n\r\n", Color.Blue);
                }
            }
            catch (System.Exception ex)
            {
                if (showMessage != null)
                    showMessage(ex.Message + "\r\n", Color.Red);
            }
            try
            {
                if (File.Exists(srcPngPath))
                {
                    File.Move(srcPngPath, newPngPath);
                    if (showMessage != null)
                        showMessage(srcPngPath + "\r\nmove to\r\n" + newPngPath + "\r\n\r\n", Color.Blue);
                }
            }
            catch (System.Exception ex)
            {
                if (showMessage != null)
                    showMessage(ex.Message + "\r\n", Color.Red);
            }

            //刷新两个目录
            src.FreshCurrentFolder();
            dst.FreshCurrentFolder();
            return true;
        }
    }
}
