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
    public partial class PjtNameSetForm : Form
    {
        string m_prjName = "";
        public string PrjName
        {
            get { return m_prjName; }
            set { m_prjName = value;
            textBox1.Text = value;
            }
        }
        public PjtNameSetForm()
        {
            InitializeComponent();
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            PrjName = textBox1.Text;
        }

        private void OnSaveClick(object sender, EventArgs e)
        {

        }
    }
}
