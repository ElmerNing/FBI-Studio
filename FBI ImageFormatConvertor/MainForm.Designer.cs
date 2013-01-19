namespace ImageFormatConvertor
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.m_txtbox_src = new System.Windows.Forms.TextBox();
            this.m_bt_src = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.m_combox_mod = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.m_bt_execute = new System.Windows.Forms.Button();
            this.m_combox_lightNum = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.m_checkBox_irEquel = new System.Windows.Forms.CheckBox();
            this.m_checkBox_irNeighbor = new System.Windows.Forms.CheckBox();
            this.m_combox_format = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.m_combox_basicImg = new System.Windows.Forms.ComboBox();
            this.m_combox_output = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.m_checkBox_grNeighbor = new System.Windows.Forms.CheckBox();
            this.m_checkBox_grEquel = new System.Windows.Forms.CheckBox();
            this.m_processBar = new System.Windows.Forms.ProgressBar();
            this.m_bt_out = new System.Windows.Forms.Button();
            this.m_txtbox_out = new System.Windows.Forms.TextBox();
            this.m_checkBox_irPrefix = new System.Windows.Forms.CheckBox();
            this.m_checkBox_grPrefix = new System.Windows.Forms.CheckBox();
            this.m_explore = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_txtbox_src
            // 
            this.m_txtbox_src.Location = new System.Drawing.Point(92, 24);
            this.m_txtbox_src.Name = "m_txtbox_src";
            this.m_txtbox_src.Size = new System.Drawing.Size(312, 21);
            this.m_txtbox_src.TabIndex = 1;
            this.m_txtbox_src.TextChanged += new System.EventHandler(this.OnConfigChanged);
            // 
            // m_bt_src
            // 
            this.m_bt_src.Location = new System.Drawing.Point(23, 24);
            this.m_bt_src.Name = "m_bt_src";
            this.m_bt_src.Size = new System.Drawing.Size(45, 21);
            this.m_bt_src.TabIndex = 2;
            this.m_bt_src.Text = "来源";
            this.m_bt_src.UseVisualStyleBackColor = true;
            this.m_bt_src.Click += new System.EventHandler(this.OnBtSrcClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(257, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "输出:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 205);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "校准图:";
            // 
            // m_combox_mod
            // 
            this.m_combox_mod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_combox_mod.FormattingEnabled = true;
            this.m_combox_mod.Items.AddRange(new object[] {
            "透射",
            "反射"});
            this.m_combox_mod.Location = new System.Drawing.Point(329, 157);
            this.m_combox_mod.Name = "m_combox_mod";
            this.m_combox_mod.Size = new System.Drawing.Size(119, 20);
            this.m_combox_mod.TabIndex = 9;
            this.m_combox_mod.SelectedIndexChanged += new System.EventHandler(this.OnConfigChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(248, 160);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "校正模式:";
            // 
            // m_bt_execute
            // 
            this.m_bt_execute.Location = new System.Drawing.Point(422, 24);
            this.m_bt_execute.Name = "m_bt_execute";
            this.m_bt_execute.Size = new System.Drawing.Size(52, 21);
            this.m_bt_execute.TabIndex = 11;
            this.m_bt_execute.Text = "执行";
            this.m_bt_execute.UseVisualStyleBackColor = true;
            this.m_bt_execute.Click += new System.EventHandler(this.OnBtExeClick);
            // 
            // m_combox_lightNum
            // 
            this.m_combox_lightNum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_combox_lightNum.FormattingEnabled = true;
            this.m_combox_lightNum.Items.AddRange(new object[] {
            "单光源",
            "双光源","三光源"});
            this.m_combox_lightNum.Location = new System.Drawing.Point(92, 153);
            this.m_combox_lightNum.Name = "m_combox_lightNum";
            this.m_combox_lightNum.Size = new System.Drawing.Size(121, 20);
            this.m_combox_lightNum.TabIndex = 12;
            this.m_combox_lightNum.SelectedIndexChanged += new System.EventHandler(this.OnConfigChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 161);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "光源数:";
            // 
            // m_checkBox_irEquel
            // 
            this.m_checkBox_irEquel.AutoSize = true;
            this.m_checkBox_irEquel.Checked = true;
            this.m_checkBox_irEquel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_checkBox_irEquel.Location = new System.Drawing.Point(91, 258);
            this.m_checkBox_irEquel.Name = "m_checkBox_irEquel";
            this.m_checkBox_irEquel.Size = new System.Drawing.Size(60, 16);
            this.m_checkBox_irEquel.TabIndex = 14;
            this.m_checkBox_irEquel.Text = "均衡化";
            this.m_checkBox_irEquel.UseVisualStyleBackColor = true;
            this.m_checkBox_irEquel.CheckedChanged += new System.EventHandler(this.OnConfigChanged);
            // 
            // m_checkBox_irNeighbor
            // 
            this.m_checkBox_irNeighbor.AutoSize = true;
            this.m_checkBox_irNeighbor.Checked = true;
            this.m_checkBox_irNeighbor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_checkBox_irNeighbor.Location = new System.Drawing.Point(162, 258);
            this.m_checkBox_irNeighbor.Name = "m_checkBox_irNeighbor";
            this.m_checkBox_irNeighbor.Size = new System.Drawing.Size(72, 16);
            this.m_checkBox_irNeighbor.TabIndex = 15;
            this.m_checkBox_irNeighbor.Text = "领域取点";
            this.m_checkBox_irNeighbor.UseVisualStyleBackColor = true;
            this.m_checkBox_irNeighbor.CheckedChanged += new System.EventHandler(this.OnConfigChanged);
            // 
            // m_combox_format
            // 
            this.m_combox_format.FormattingEnabled = true;
            this.m_combox_format.Items.AddRange(new object[] {
            "DM642",
            "C54XX"});
            this.m_combox_format.Location = new System.Drawing.Point(92, 114);
            this.m_combox_format.Name = "m_combox_format";
            this.m_combox_format.Size = new System.Drawing.Size(121, 20);
            this.m_combox_format.TabIndex = 16;
            this.m_combox_format.SelectedIndexChanged += new System.EventHandler(this.OnConfigChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "源格式:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 258);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 18;
            this.label6.Text = "红外模式:";
            // 
            // m_combox_basicImg
            // 
            this.m_combox_basicImg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_combox_basicImg.Items.AddRange(new object[] {
            "红外",
            "绿光"});
            this.m_combox_basicImg.Location = new System.Drawing.Point(92, 196);
            this.m_combox_basicImg.Name = "m_combox_basicImg";
            this.m_combox_basicImg.Size = new System.Drawing.Size(121, 20);
            this.m_combox_basicImg.TabIndex = 0;
            this.m_combox_basicImg.SelectedIndexChanged += new System.EventHandler(this.OnConfigChanged);
            // 
            // m_combox_output
            // 
            this.m_combox_output.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_combox_output.FormattingEnabled = true;
            this.m_combox_output.Items.AddRange(new object[] {
            "红外",
            "绿光",
            "紫外",
            "全部"});
            this.m_combox_output.Location = new System.Drawing.Point(328, 119);
            this.m_combox_output.Name = "m_combox_output";
            this.m_combox_output.Size = new System.Drawing.Size(121, 20);
            this.m_combox_output.TabIndex = 19;
            this.m_combox_output.SelectedIndexChanged += new System.EventHandler(this.OnConfigChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 291);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 20;
            this.label7.Text = "绿光模式:";
            // 
            // m_checkBox_grNeighbor
            // 
            this.m_checkBox_grNeighbor.AutoSize = true;
            this.m_checkBox_grNeighbor.Checked = true;
            this.m_checkBox_grNeighbor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_checkBox_grNeighbor.Location = new System.Drawing.Point(164, 290);
            this.m_checkBox_grNeighbor.Name = "m_checkBox_grNeighbor";
            this.m_checkBox_grNeighbor.Size = new System.Drawing.Size(72, 16);
            this.m_checkBox_grNeighbor.TabIndex = 22;
            this.m_checkBox_grNeighbor.Text = "领域取点";
            this.m_checkBox_grNeighbor.UseVisualStyleBackColor = true;
            this.m_checkBox_grNeighbor.CheckedChanged += new System.EventHandler(this.OnConfigChanged);
            // 
            // m_checkBox_grEquel
            // 
            this.m_checkBox_grEquel.AutoSize = true;
            this.m_checkBox_grEquel.Checked = true;
            this.m_checkBox_grEquel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_checkBox_grEquel.Location = new System.Drawing.Point(93, 290);
            this.m_checkBox_grEquel.Name = "m_checkBox_grEquel";
            this.m_checkBox_grEquel.Size = new System.Drawing.Size(60, 16);
            this.m_checkBox_grEquel.TabIndex = 21;
            this.m_checkBox_grEquel.Text = "均衡化";
            this.m_checkBox_grEquel.UseVisualStyleBackColor = true;
            this.m_checkBox_grEquel.CheckedChanged += new System.EventHandler(this.OnConfigChanged);
            // 
            // m_processBar
            // 
            this.m_processBar.Enabled = false;
            this.m_processBar.Location = new System.Drawing.Point(22, 350);
            this.m_processBar.Name = "m_processBar";
            this.m_processBar.Size = new System.Drawing.Size(451, 23);
            this.m_processBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.m_processBar.TabIndex = 23;
            // 
            // m_bt_out
            // 
            this.m_bt_out.Location = new System.Drawing.Point(23, 52);
            this.m_bt_out.Name = "m_bt_out";
            this.m_bt_out.Size = new System.Drawing.Size(45, 23);
            this.m_bt_out.TabIndex = 24;
            this.m_bt_out.Text = "输出";
            this.m_bt_out.UseVisualStyleBackColor = true;
            this.m_bt_out.Click += new System.EventHandler(this.OnBtOutClick);
            // 
            // m_txtbox_out
            // 
            this.m_txtbox_out.Location = new System.Drawing.Point(92, 52);
            this.m_txtbox_out.Name = "m_txtbox_out";
            this.m_txtbox_out.Size = new System.Drawing.Size(312, 21);
            this.m_txtbox_out.TabIndex = 25;
            this.m_txtbox_out.TextChanged += new System.EventHandler(this.OnConfigChanged);
            // 
            // m_checkBox_irPrefix
            // 
            this.m_checkBox_irPrefix.AutoSize = true;
            this.m_checkBox_irPrefix.Checked = true;
            this.m_checkBox_irPrefix.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_checkBox_irPrefix.Location = new System.Drawing.Point(240, 258);
            this.m_checkBox_irPrefix.Name = "m_checkBox_irPrefix";
            this.m_checkBox_irPrefix.Size = new System.Drawing.Size(48, 16);
            this.m_checkBox_irPrefix.TabIndex = 26;
            this.m_checkBox_irPrefix.Text = "前缀";
            this.m_checkBox_irPrefix.UseVisualStyleBackColor = true;
            this.m_checkBox_irPrefix.CheckedChanged += new System.EventHandler(this.OnConfigChanged);
            // 
            // m_checkBox_grPrefix
            // 
            this.m_checkBox_grPrefix.AutoSize = true;
            this.m_checkBox_grPrefix.Location = new System.Drawing.Point(240, 290);
            this.m_checkBox_grPrefix.Name = "m_checkBox_grPrefix";
            this.m_checkBox_grPrefix.Size = new System.Drawing.Size(48, 16);
            this.m_checkBox_grPrefix.TabIndex = 27;
            this.m_checkBox_grPrefix.Text = "前缀";
            this.m_checkBox_grPrefix.UseVisualStyleBackColor = true;
            this.m_checkBox_grPrefix.CheckedChanged += new System.EventHandler(this.OnConfigChanged);
            // 
            // m_explore
            // 
            this.m_explore.Location = new System.Drawing.Point(422, 51);
            this.m_explore.Name = "m_explore";
            this.m_explore.Size = new System.Drawing.Size(52, 23);
            this.m_explore.TabIndex = 28;
            this.m_explore.Text = "浏览";
            this.m_explore.UseVisualStyleBackColor = true;
            this.m_explore.Click += new System.EventHandler(this.OnExplore);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 419);
            this.Controls.Add(this.m_explore);
            this.Controls.Add(this.m_checkBox_grPrefix);
            this.Controls.Add(this.m_checkBox_irPrefix);
            this.Controls.Add(this.m_txtbox_out);
            this.Controls.Add(this.m_bt_out);
            this.Controls.Add(this.m_processBar);
            this.Controls.Add(this.m_checkBox_grNeighbor);
            this.Controls.Add(this.m_checkBox_grEquel);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.m_combox_output);
            this.Controls.Add(this.m_combox_basicImg);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.m_combox_format);
            this.Controls.Add(this.m_checkBox_irNeighbor);
            this.Controls.Add(this.m_checkBox_irEquel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.m_combox_lightNum);
            this.Controls.Add(this.m_bt_execute);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.m_combox_mod);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_bt_src);
            this.Controls.Add(this.m_txtbox_src);
            this.Name = "MainForm";
            this.Text = "Convertor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox m_txtbox_src;
        private System.Windows.Forms.Button m_bt_src;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox m_combox_mod;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button m_bt_execute;
        private System.Windows.Forms.ComboBox m_combox_lightNum;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox m_checkBox_irEquel;
        private System.Windows.Forms.CheckBox m_checkBox_irNeighbor;
        private System.Windows.Forms.ComboBox m_combox_format;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox m_combox_basicImg;
        private System.Windows.Forms.ComboBox m_combox_output;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox m_checkBox_grNeighbor;
        private System.Windows.Forms.CheckBox m_checkBox_grEquel;
        private System.Windows.Forms.ProgressBar m_processBar;
        private System.Windows.Forms.Button m_bt_out;
        private System.Windows.Forms.TextBox m_txtbox_out;
        private System.Windows.Forms.CheckBox m_checkBox_irPrefix;
        private System.Windows.Forms.CheckBox m_checkBox_grPrefix;
        private System.Windows.Forms.Button m_explore;
    }
}

