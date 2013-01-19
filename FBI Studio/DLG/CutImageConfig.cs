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
    public partial class CutImageConfig : Form
    {
        public bool isOK;
        public bool Equalization;
        public int RotateMode;
        public bool Subset;
        public int SubsetWidth;
        public int SubsetHeight;

        public CutImageConfig()
        {
            InitializeComponent();
        }

        private void CutImageConfig_Load(object sender, EventArgs e)
        {
            comboBox_rotate.SelectedIndex = 0;
            isOK = false;
            numericUpDown_width.Maximum = SubsetWidth;
            numericUpDown_height.Maximum = SubsetHeight;
            numericUpDown_width.Value = SubsetWidth;
            numericUpDown_height.Value = SubsetHeight;
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            isOK = true;
            if (checkBox_equalization.Checked==true)
            {
                Equalization = true;
            } 
            else
            {
                Equalization = false;
            }
            RotateMode = comboBox_rotate.SelectedIndex;
            if (checkBox_floatcut.Checked == true)
            {
                Subset = true;
                SubsetWidth = (int)numericUpDown_width.Value;
                SubsetHeight = (int)numericUpDown_height.Value;
            } 
            else
            {
                Subset = false;
            }
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            isOK = false;
            this.Close();
        }

        private void checkBox_floatcut_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_floatcut.Checked==true)
            {
                panel_floatcut.Visible = false;
            }
            else
            {
                panel_floatcut.Visible = true;
            }
        }
    }
}
