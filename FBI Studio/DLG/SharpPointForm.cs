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
    public partial class SharpPointForm : Form
    {
        public SharpPointForm()
        {
            InitializeComponent();
        }

        private void OnOk(object sender, EventArgs e)
        {
            PointTrain.TrainConfig.enable[0] = this.NumberCheckBox.Checked;
            PointTrain.TrainConfig.WHITENUMBER = short.Parse(this.tb_w_num.Text);
            PointTrain.TrainConfig.BLACKNUMBER = short.Parse(this.tb_b_num.Text);

            PointTrain.TrainConfig.enable[1] = this.ThrCheckBox.Checked;
            PointTrain.TrainConfig.WHITEACCEPTTHR = short.Parse(this.tb_w_bottom.Text);
            PointTrain.TrainConfig.BLACKACCEPTTHR = short.Parse(this.tb_b_top.Text);

            PointTrain.TrainConfig.enable[2] = this.PercentCheckBox.Checked;
            PointTrain.TrainConfig.WHITELEFTPERCENT = float.Parse(this.tb_w_per.Text);
            PointTrain.TrainConfig.BLACKLEFTPERCENT = float.Parse(this.tb_b_per.Text);

            PointTrain.TrainConfig.enable[3] = this.AcceptCheckBox.Checked;
            PointTrain.TrainConfig.WHITETOPTHR = short.Parse(this.tb_w_top.Text);
            PointTrain.TrainConfig.BLACKBOTTOMTHR = short.Parse(this.tb_b_bottom.Text);
        }

        private void SharpPointForm_Load(object sender, EventArgs e)
        {
            this.NumberCheckBox.Checked = PointTrain.TrainConfig.enable[0];
            this.ThrCheckBox.Checked = PointTrain.TrainConfig.enable[1];
            this.PercentCheckBox.Checked = PointTrain.TrainConfig.enable[2];
            this.AcceptCheckBox.Checked = PointTrain.TrainConfig.enable[3];

            this.tb_w_num.Text = PointTrain.TrainConfig.WHITENUMBER.ToString();
            this.tb_b_num.Text = PointTrain.TrainConfig.BLACKNUMBER.ToString();

            this.tb_w_bottom.Text = PointTrain.TrainConfig.WHITEACCEPTTHR.ToString();
            this.tb_b_top.Text = PointTrain.TrainConfig.BLACKACCEPTTHR.ToString();

            this.tb_w_per.Text = PointTrain.TrainConfig.WHITELEFTPERCENT.ToString();
            this.tb_b_per.Text = PointTrain.TrainConfig.BLACKLEFTPERCENT.ToString();

            this.tb_w_top.Text = PointTrain.TrainConfig.WHITETOPTHR.ToString();
            this.tb_b_bottom.Text = PointTrain.TrainConfig.BLACKBOTTOMTHR.ToString();
        }

        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                //enter键 确认并退出对话框
                case '\r':
                    this.DialogResult = DialogResult.OK;
                    this.Close();
            	    break;

                //esc键 取消并退出对话框
                case (char)27:
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    break;
            }
        }
    }
}
