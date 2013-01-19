using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using Focus;

namespace ImageFormatConvertor
{
    public partial class MainForm : Form
    {
        ConvertConfig m_config = new ConvertConfig(); //当前配置方案

        public MainForm()
        {
            InitializeComponent();

            //string src = "";
            string dps = "";
            
            //有命令行参数时候, 通过命令行参数赋值
            if (Program.ags != null && Program.ags.Length == 1)
            {
                if (Program.ags[0] != null && Program.ags[0] != "")
                {
                    dps = Program.ags[0];
                }
                string configPath = dps + "\\config.xml";
                if (File.Exists(configPath))
                {
                    m_config = (ConvertConfig)m_config.DeserializeXML(configPath);
                }
                else
                {
                    m_config = (ConvertConfig)m_config.DeserializeXML("config.xml");
                }
                m_config.dps = dps;
            }
            else
            {
                m_config = (ConvertConfig)m_config.DeserializeXML("config.xml");
            }

            SetConvertConfig(m_config);
            Convertor.convertEvent += new Convertor.ConvertEventHandler(OnConvertEvent);
        }

        /// <summary>
        /// 配置界面
        /// </summary>
        /// <param name="config">配置</param>
        /// <returns></returns>
        private void SetConvertConfig(ConvertConfig config)
        {
            if (m_combox_format.Enabled)
            {
                switch (config.format)
                {
                    case ConfigFormat.DM642:
                        m_combox_format.SelectedIndex = 0; break;
                    case ConfigFormat.C54XX:
                        m_combox_format.SelectedIndex = 1; break;
                }
            }

            if (m_combox_lightNum.Enabled)
            {
                switch (config.lightNum)
                {
                    case ConfigLightNum.Single:
                        m_combox_lightNum.SelectedIndex = 0; break;
                    case ConfigLightNum.Double:
                        m_combox_lightNum.SelectedIndex = 1; break;
                    case ConfigLightNum.Triple:
                        m_combox_lightNum.SelectedIndex = 2; break;
                }
            }

            if (m_combox_output.Enabled)
            {
                switch (config.Output)
                {
                    case ConfigOutput.Ir:
                        m_combox_output.SelectedIndex = 0; break;
                    case ConfigOutput.Green:
                        m_combox_output.SelectedIndex = 1; break;
                    case ConfigOutput.Uv:
                        m_combox_output.SelectedIndex = 2; break;
                    case ConfigOutput.All:
                        {
                            config.Output = ConfigOutput.All;
                            m_combox_output.SelectedIndex = 3; break;
                        }
                }
            }

            if (m_combox_mod.Enabled)
            {
                switch (config.reviseMode)
                {
                    case ConfigReviseMode.Penetrate:
                        m_combox_mod.SelectedIndex = 0; break;
                    case ConfigReviseMode.Reflect:
                        m_combox_mod.SelectedIndex = 1; break;
                }
            }

            if (m_combox_basicImg.Enabled)
            {
                switch (config.basicImg)
                {
                    case ConfigBasicImg.Ir:
                        m_combox_basicImg.SelectedIndex = 0; break;
                    case ConfigBasicImg.Green:
                        m_combox_basicImg.SelectedIndex = 1; break;
                }
            }

            if (m_checkBox_irEquel.Enabled)
                 m_checkBox_irEquel.Checked = config.irEqual;

            if (m_checkBox_irNeighbor.Enabled)
                 m_checkBox_irNeighbor.Checked = config.irNeighbor;

            if (m_checkBox_irPrefix.Enabled)
                m_checkBox_irPrefix.Checked = config.irPrefix;

            if (m_checkBox_grEquel.Enabled)
                m_checkBox_grEquel.Checked = config.uvgrEqual;

            if (m_checkBox_grNeighbor.Enabled)
                m_checkBox_grNeighbor.Checked = config.uvgrNeighbor;

            if (m_checkBox_grPrefix.Enabled)
                m_checkBox_grPrefix.Checked = config.grPrefix;

            m_txtbox_src.Text = config.src;
            m_txtbox_out.Text = config.dps;
        }

        /// <summary>
        /// 获取配置方案
        /// </summary>
        /// <returns>返回配置方案</returns>
        private ConvertConfig GetConvertConfig()
        {
            ConvertConfig config = new ConvertConfig();
            if (m_combox_format.Enabled)
            {
                switch (m_combox_format.SelectedIndex)
                {
                    case 0:
                        config.format = ConfigFormat.DM642; break;
                    case 1:
                        config.format = ConfigFormat.C54XX; break;
                }
            }

            if (m_combox_lightNum.Enabled)
            {
                switch (m_combox_lightNum.SelectedIndex)
                {
                    case 0:
                        config.lightNum = ConfigLightNum.Single; break;
                    case 1:
                        config.lightNum = ConfigLightNum.Double; break;
                    case 2:
                        config.lightNum = ConfigLightNum.Triple; break;
                }
            }  

            if (m_combox_output.Enabled)
            {
                switch (m_combox_output.SelectedIndex)
                {
                    case 0:
                        config.Output = ConfigOutput.Ir; break;
                    case 1:
                        config.Output = ConfigOutput.Green; break;
                    case 2:
                        config.Output = ConfigOutput.Uv; break;
                    case 3:
                        config.Output = ConfigOutput.All; break;
                }
            }

            if (m_combox_mod.Enabled)
            {
                switch (m_combox_mod.SelectedIndex)
                {
                    case 0:
                        config.reviseMode = ConfigReviseMode.Penetrate; break;
                    case 1:
                        config.reviseMode = ConfigReviseMode.Reflect; break;
                }
            }

            if (m_combox_basicImg.Enabled)
            {
                switch (m_combox_basicImg.SelectedIndex)
                {
                    case 0:
                        config.basicImg = ConfigBasicImg.Ir; break;
                    case 1:
                        config.basicImg = ConfigBasicImg.Green; break;
                }
            }

            if (m_checkBox_irEquel.Enabled)
                config.irEqual = m_checkBox_irEquel.Checked;

            if (m_checkBox_irNeighbor.Enabled)
                config.irNeighbor = m_checkBox_irNeighbor.Checked;

            if (m_checkBox_irPrefix.Enabled)
                config.irPrefix = m_checkBox_irPrefix.Checked;

            if (m_checkBox_grEquel.Enabled)
                config.uvgrEqual = m_checkBox_grEquel.Checked;

            if (m_checkBox_grNeighbor.Enabled)
                config.uvgrNeighbor = m_checkBox_grNeighbor.Checked;

            if (m_checkBox_grPrefix.Enabled)
                config.grPrefix = m_checkBox_grPrefix.Checked;

            config.src = m_txtbox_src.Text;
            config.dps = m_txtbox_out.Text;
            return config;
        }

        /// <summary>
        /// 单击"来源"时触发
        /// </summary>
        private void OnBtSrcClick(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.ShowNewFolderButton = true;
            folderBrowser.SelectedPath = m_txtbox_src.Text;
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                m_txtbox_src.Text = folderBrowser.SelectedPath;
            }
            string configPath = m_txtbox_src.Text + "config.xml";
            if (File.Exists(configPath))
            {
                m_config = (ConvertConfig)m_config.DeserializeXML();
                m_config.src = m_txtbox_src.Text;
                SetConvertConfig(m_config);
            }
        }

        /// <summary>
        /// 单击"输出"时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnBtOutClick(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.ShowNewFolderButton = true;
            folderBrowser.SelectedPath = m_txtbox_out.Text == "" ? Environment.CurrentDirectory : m_txtbox_out.Text;
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                m_txtbox_out.Text = folderBrowser.SelectedPath;
            }
        }

        /// <summary>
        /// 单击"执行"时触发
        /// </summary>
        private void OnBtExeClick(object sender, EventArgs e)
        {
            string srcFolder = m_config.src;
            string outFolder = m_config.dps;
            if (!Directory.Exists(srcFolder) || !Directory.Exists(outFolder))
            {
                MessageBox.Show("请设置正确路径!");
                return;
            }
            DialogResult r = MessageBox.Show("是否重新全部生成!", "提示", MessageBoxButtons.YesNo);
            if (r == DialogResult.Cancel)
                return;
            if (r == DialogResult.Yes)
                m_config.isTotal = true;
            else
                m_config.isTotal = false;


            r = MessageBox.Show("是否保存转换配置到输出目录", "提示", MessageBoxButtons.YesNo);
            if (r == DialogResult.Yes)
            {
                string fileName = m_config.dps + "\\" + "config.xml";
                m_config.SerializeXML(fileName);
            }

            if (!Convertor.ConvertBackGround(m_config))
            {
                MessageBox.Show("稍后再执行");
                return;
            }
        }

        /// <summary>
        /// 配置方案改变时候触发
        /// </summary>
        private void OnConfigChanged(object sender, EventArgs e)
        {
            m_config = GetConvertConfig();
        }

        /// <summary>
        /// 转换文件的某些过程中触发
        /// </summary>
        /// <param name="e"></param>
        private void OnConvertEvent(ConvertEventArgs e)
        {
            if (m_processBar.InvokeRequired)
            {
                Convertor.ConvertEventHandler invokeProcess = new Convertor.ConvertEventHandler(OnConvertEvent);
                this.Invoke(invokeProcess, new object[] { e });
            }
            else
            {
                switch (e.ConvertedNum)
                {
                    case 0:
                        m_processBar.Visible = true;
                        m_processBar.Minimum = 0;
                        m_processBar.Maximum = e.TotalFileNum;
                        m_processBar.Value = 0;
                        m_processBar.Step = 1;
                        break;
                    case -1:
                        m_processBar.Step = 0;
                        m_processBar.Visible = false;
                        break;
                    default:
                        m_processBar.PerformStep();
                        break;
                }
            }
        }

        /// <summary>
        /// 引发窗体即将关闭的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            m_config.SerializeXML();
            if (Convertor.IsBackGroundAlive)
            {
                DialogResult result = MessageBox.Show("当前正在转换,是否强制退出!", "提示", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Convertor.BackGroundAbort();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// 引发浏览事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private void OnExplore(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", m_config.dps);
            System.Diagnostics.Process.Start("explorer.exe", m_config.src);
        }

        //end of class
    }
}
