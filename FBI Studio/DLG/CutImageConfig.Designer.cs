namespace FBI_Studio
{
    partial class CutImageConfig
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkBox_equalization = new System.Windows.Forms.CheckBox();
            this.comboBox_rotate = new System.Windows.Forms.ComboBox();
            this.label_rotate = new System.Windows.Forms.Label();
            this.checkBox_floatcut = new System.Windows.Forms.CheckBox();
            this.label_width = new System.Windows.Forms.Label();
            this.label_height = new System.Windows.Forms.Label();
            this.numericUpDown_width = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_height = new System.Windows.Forms.NumericUpDown();
            this.panel_floatcut = new System.Windows.Forms.Panel();
            this.button_ok = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_width)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_height)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBox_equalization
            // 
            this.checkBox_equalization.AutoSize = true;
            this.checkBox_equalization.Location = new System.Drawing.Point(13, 13);
            this.checkBox_equalization.Name = "checkBox_equalization";
            this.checkBox_equalization.Size = new System.Drawing.Size(60, 16);
            this.checkBox_equalization.TabIndex = 0;
            this.checkBox_equalization.Text = "均衡化";
            this.checkBox_equalization.UseVisualStyleBackColor = true;
            // 
            // comboBox_rotate
            // 
            this.comboBox_rotate.FormattingEnabled = true;
            this.comboBox_rotate.Items.AddRange(new object[] {
            "不旋转",
            "绕X轴旋转180",
            "绕Y轴旋转180",
            "绕Z轴旋转180"});
            this.comboBox_rotate.Location = new System.Drawing.Point(159, 9);
            this.comboBox_rotate.Name = "comboBox_rotate";
            this.comboBox_rotate.Size = new System.Drawing.Size(121, 20);
            this.comboBox_rotate.TabIndex = 1;
            // 
            // label_rotate
            // 
            this.label_rotate.AutoSize = true;
            this.label_rotate.Location = new System.Drawing.Point(94, 13);
            this.label_rotate.Name = "label_rotate";
            this.label_rotate.Size = new System.Drawing.Size(59, 12);
            this.label_rotate.TabIndex = 2;
            this.label_rotate.Text = "旋转方式:";
            // 
            // checkBox_floatcut
            // 
            this.checkBox_floatcut.AutoSize = true;
            this.checkBox_floatcut.Location = new System.Drawing.Point(13, 49);
            this.checkBox_floatcut.Name = "checkBox_floatcut";
            this.checkBox_floatcut.Size = new System.Drawing.Size(60, 16);
            this.checkBox_floatcut.TabIndex = 3;
            this.checkBox_floatcut.Text = "抠子图";
            this.checkBox_floatcut.UseVisualStyleBackColor = true;
            this.checkBox_floatcut.CheckedChanged += new System.EventHandler(this.checkBox_floatcut_CheckedChanged);
            // 
            // label_width
            // 
            this.label_width.AutoSize = true;
            this.label_width.Location = new System.Drawing.Point(94, 49);
            this.label_width.Name = "label_width";
            this.label_width.Size = new System.Drawing.Size(35, 12);
            this.label_width.TabIndex = 5;
            this.label_width.Text = "宽度:";
            // 
            // label_height
            // 
            this.label_height.AutoSize = true;
            this.label_height.Location = new System.Drawing.Point(187, 49);
            this.label_height.Name = "label_height";
            this.label_height.Size = new System.Drawing.Size(35, 12);
            this.label_height.TabIndex = 6;
            this.label_height.Text = "高度:";
            // 
            // numericUpDown_width
            // 
            this.numericUpDown_width.Location = new System.Drawing.Point(125, 44);
            this.numericUpDown_width.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown_width.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_width.Name = "numericUpDown_width";
            this.numericUpDown_width.Size = new System.Drawing.Size(56, 21);
            this.numericUpDown_width.TabIndex = 7;
            this.numericUpDown_width.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // numericUpDown_height
            // 
            this.numericUpDown_height.Location = new System.Drawing.Point(220, 44);
            this.numericUpDown_height.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown_height.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_height.Name = "numericUpDown_height";
            this.numericUpDown_height.Size = new System.Drawing.Size(60, 21);
            this.numericUpDown_height.TabIndex = 8;
            this.numericUpDown_height.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // panel_floatcut
            // 
            this.panel_floatcut.Location = new System.Drawing.Point(96, 44);
            this.panel_floatcut.Name = "panel_floatcut";
            this.panel_floatcut.Size = new System.Drawing.Size(184, 21);
            this.panel_floatcut.TabIndex = 9;
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(96, 84);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 10;
            this.button_ok.Text = "确定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(204, 84);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 11;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // CutImageConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 123);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.panel_floatcut);
            this.Controls.Add(this.numericUpDown_height);
            this.Controls.Add(this.numericUpDown_width);
            this.Controls.Add(this.label_height);
            this.Controls.Add(this.label_width);
            this.Controls.Add(this.checkBox_floatcut);
            this.Controls.Add(this.label_rotate);
            this.Controls.Add(this.comboBox_rotate);
            this.Controls.Add(this.checkBox_equalization);
            this.Name = "CutImageConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CutImage";
            this.Load += new System.EventHandler(this.CutImageConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_width)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_height)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_equalization;
        private System.Windows.Forms.ComboBox comboBox_rotate;
        private System.Windows.Forms.Label label_rotate;
        private System.Windows.Forms.CheckBox checkBox_floatcut;
        private System.Windows.Forms.Label label_width;
        private System.Windows.Forms.Label label_height;
        private System.Windows.Forms.NumericUpDown numericUpDown_width;
        private System.Windows.Forms.NumericUpDown numericUpDown_height;
        private System.Windows.Forms.Panel panel_floatcut;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_cancel;
    }
}