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
    public partial class PjtConfigForm : Form
    {
        public PjtConfigForm()
        {
            InitializeComponent();
        }

        Project m_project = null;

        public DialogResult ShowDialog(ref Project project)
        {
            if (project == null)
            {
                project = new Project();
                m_project = project;
            }
            else
            {
                m_project = project;
                m_namebox.ReadOnly = true;
            }
            m_namebox.Text = project.PjtName;
            m_sampleBox.Text = project.SampleFolder;
            m_pointBox.Text = project.PointFolder;
            return ShowDialog();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_namebox.Text == "" || m_sampleBox.Text == "" || m_sampleBox.Text == "")
            {
                if (this.DialogResult == DialogResult.OK)
                {
                    MessageBox.Show("信息没有填写完整！");
                    e.Cancel = true;
                    return;
                }
            }

            m_project.PjtName = m_namebox.Text;
            m_project.SampleFolder = m_sampleBox.Text;
            m_project.PointFolder = m_pointBox.Text;
        }

        private void OnOK(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void OnPointBrowse(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = m_pointBox.Text;
            dlg.ShowDialog();
            m_pointBox.Text = dlg.SelectedPath;
        }

        private void OnSampleBrowse(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = m_sampleBox.Text;
            dlg.ShowDialog();
            m_sampleBox.Text = dlg.SelectedPath;
        }

    }
}
