using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FBI_Studio
{
    public partial class IntellSelPoitForm : Form
    {
        public IntellSelPoitForm()
        {
            InitializeComponent();
        }
        public string[] workFiles = null;
        public string[] distingFiles = null;

        public DialogResult ShowDialog(string[] _workFiles, string[] _distingFiles)
        {
            this.workFiles = _workFiles;
            this.distingFiles = _distingFiles;
            return ShowDialog();
        }

        private void OnBegin(object sender, EventArgs e)
        {
            if (m_errorCount.Checked)
                IntellPointSel.IntellPointConfig.ClassMode = ConfigClassMode.ErrorCount;
            if (m_grayDiff.Checked)
                IntellPointSel.IntellPointConfig.ClassMode = ConfigClassMode.GrayDiff;

            int i = int.Parse(m_whtSelPoint.Text);
            if (i>=0 && i<100)
                IntellPointSel.IntellPointConfig.whtSelPoint = i;
            else
            {
                MessageBox.Show("白点数不能大于100或小于0!");
                return;
            }

            i = int.Parse(m_blcSelPoint.Text);
            if (i >= 0 && i < 100)
                IntellPointSel.IntellPointConfig.blcSelPoint = i;
            else
            {
                MessageBox.Show("黑多数不能大于100或小于0!");
                return;
            }

            i = int.Parse(m_searchSize.Text);
            if (i >= 0 && i < 10)
                IntellPointSel.IntellPointConfig.SearchSize = i;
            else
            {
                MessageBox.Show("点搜索范围不能大于9或小于0!");
                return;
            }

            IntellPointSel.IntellPointConfig.AddTo = m_addto.Checked;

            IntellPointSel.IntellPointConfig.Area = m_area.Checked;

            IntellPointSel.IntellSelBackground(this.workFiles, this.distingFiles);
            PointTrain.SharpThreshold(this.workFiles);
            PointAnlysePicBox.SynFresh();
            MessageBox.Show("取点完成");
        }

        private void OnLoad(object sender, EventArgs e)
        {
            if (IntellPointSel.IntellPointConfig.ClassMode == ConfigClassMode.ErrorCount)
                m_errorCount.Checked = true;
            else
                m_grayDiff.Checked = true;

            m_whtSelPoint.Text = IntellPointSel.IntellPointConfig.whtSelPoint.ToString();

            m_blcSelPoint.Text = IntellPointSel.IntellPointConfig.blcSelPoint.ToString();

            m_searchSize.Text = IntellPointSel.IntellPointConfig.SearchSize.ToString();

            m_addto.Checked = IntellPointSel.IntellPointConfig.AddTo;

            m_area.Checked = IntellPointSel.IntellPointConfig.Area;
           
        }
    }
}
