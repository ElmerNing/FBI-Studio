namespace FBI_Studio
{
    partial class IntellSelPoitForm
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
            this.m_errorCount = new System.Windows.Forms.RadioButton();
            this.m_grayDiff = new System.Windows.Forms.RadioButton();
            this.m_whtSelPoint = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.m_addto = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.m_searchSize = new System.Windows.Forms.TextBox();
            this.m_area = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.m_blcSelPoint = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_errorCount
            // 
            this.m_errorCount.AutoSize = true;
            this.m_errorCount.Location = new System.Drawing.Point(20, 27);
            this.m_errorCount.Name = "m_errorCount";
            this.m_errorCount.Size = new System.Drawing.Size(59, 16);
            this.m_errorCount.TabIndex = 1;
            this.m_errorCount.TabStop = true;
            this.m_errorCount.Text = "误判数";
            this.m_errorCount.UseVisualStyleBackColor = true;
            // 
            // m_grayDiff
            // 
            this.m_grayDiff.AutoSize = true;
            this.m_grayDiff.Location = new System.Drawing.Point(103, 26);
            this.m_grayDiff.Name = "m_grayDiff";
            this.m_grayDiff.Size = new System.Drawing.Size(71, 16);
            this.m_grayDiff.TabIndex = 0;
            this.m_grayDiff.TabStop = true;
            this.m_grayDiff.Text = "灰度差异";
            this.m_grayDiff.UseVisualStyleBackColor = true;
            // 
            // m_whtSelPoint
            // 
            this.m_whtSelPoint.Location = new System.Drawing.Point(332, 20);
            this.m_whtSelPoint.Name = "m_whtSelPoint";
            this.m_whtSelPoint.Size = new System.Drawing.Size(100, 21);
            this.m_whtSelPoint.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(273, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "白点数";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(193, 165);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 52);
            this.button1.TabIndex = 3;
            this.button1.Text = "开始";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnBegin);
            // 
            // m_addto
            // 
            this.m_addto.AutoSize = true;
            this.m_addto.Location = new System.Drawing.Point(47, 111);
            this.m_addto.Name = "m_addto";
            this.m_addto.Size = new System.Drawing.Size(72, 16);
            this.m_addto.TabIndex = 0;
            this.m_addto.Text = "追加模式";
            this.m_addto.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(273, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "点搜索";
            // 
            // m_searchSize
            // 
            this.m_searchSize.Location = new System.Drawing.Point(332, 108);
            this.m_searchSize.Name = "m_searchSize";
            this.m_searchSize.Size = new System.Drawing.Size(100, 21);
            this.m_searchSize.TabIndex = 4;
            // 
            // m_area
            // 
            this.m_area.AutoSize = true;
            this.m_area.Location = new System.Drawing.Point(132, 111);
            this.m_area.Name = "m_area";
            this.m_area.Size = new System.Drawing.Size(72, 16);
            this.m_area.TabIndex = 5;
            this.m_area.Text = "区域模式";
            this.m_area.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.m_errorCount);
            this.groupBox1.Controls.Add(this.m_grayDiff);
            this.groupBox1.Location = new System.Drawing.Point(27, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(193, 68);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "误判模式";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(273, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "黑点数";
            // 
            // m_blcSelPoint
            // 
            this.m_blcSelPoint.Location = new System.Drawing.Point(332, 61);
            this.m_blcSelPoint.Name = "m_blcSelPoint";
            this.m_blcSelPoint.Size = new System.Drawing.Size(100, 21);
            this.m_blcSelPoint.TabIndex = 7;
            // 
            // IntellSelPoitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 272);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_blcSelPoint);
            this.Controls.Add(this.m_area);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.m_searchSize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.m_addto);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.m_whtSelPoint);
            this.Controls.Add(this.groupBox1);
            this.Name = "IntellSelPoitForm";
            this.Text = "IntellSelPoitForm";
            this.Load += new System.EventHandler(this.OnLoad);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton m_errorCount;
        private System.Windows.Forms.RadioButton m_grayDiff;
        private System.Windows.Forms.TextBox m_whtSelPoint;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox m_addto;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox m_searchSize;
        private System.Windows.Forms.CheckBox m_area;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox m_blcSelPoint;
    }
}