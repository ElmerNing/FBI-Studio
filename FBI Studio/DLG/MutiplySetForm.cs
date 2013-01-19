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
    public partial class MutiplySetForm : Form
    {
        public static bool IsShow3D = false;
        public static bool IsOnlyCheck = false;
        
        string m_s = "";
        public MutiplySetForm()
        {
            InitializeComponent();
        }

        private void OnBtClick(object sender, EventArgs e)
        {
            Close();
        }

        new public string ShowDialog()
        {
            m_onlyChecked.Checked = IsOnlyCheck;
            m_3D.Checked = IsShow3D;
            if (base.ShowDialog() == DialogResult.Cancel)
            {
                return null;
            }        
            return m_s;
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            IsShow3D = m_3D.Checked;
            IsOnlyCheck = m_onlyChecked.Checked;
            m_s = textBox1.Text;
        }

        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            switch (e.KeyChar)
            {
                case '\r':          //enter
                    this.DialogResult = DialogResult.OK;
                    this.Close();
            	    break;
                case (char)27:  //esc
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    break;
            }
        }
    }
}
