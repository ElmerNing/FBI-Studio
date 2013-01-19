namespace FBI_Studio
{
    partial class PjtConfigForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.m_namebox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.m_sampleBox = new System.Windows.Forms.TextBox();
            this.m_pointBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.m_cancel = new System.Windows.Forms.Button();
            this.m_OK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "名称";
            // 
            // m_namebox
            // 
            this.m_namebox.Location = new System.Drawing.Point(84, 31);
            this.m_namebox.Name = "m_namebox";
            this.m_namebox.Size = new System.Drawing.Size(100, 21);
            this.m_namebox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "样本目录";
            this.label2.Click += new System.EventHandler(this.OnSampleBrowse);
            // 
            // m_sampleBox
            // 
            this.m_sampleBox.Location = new System.Drawing.Point(84, 82);
            this.m_sampleBox.Name = "m_sampleBox";
            this.m_sampleBox.Size = new System.Drawing.Size(274, 21);
            this.m_sampleBox.TabIndex = 3;
            // 
            // m_pointBox
            // 
            this.m_pointBox.Location = new System.Drawing.Point(84, 132);
            this.m_pointBox.Name = "m_pointBox";
            this.m_pointBox.Size = new System.Drawing.Size(274, 21);
            this.m_pointBox.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 135);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "点目录";
            this.label3.Click += new System.EventHandler(this.OnPointBrowse);
            // 
            // m_cancel
            // 
            this.m_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_cancel.Location = new System.Drawing.Point(227, 177);
            this.m_cancel.Name = "m_cancel";
            this.m_cancel.Size = new System.Drawing.Size(75, 23);
            this.m_cancel.TabIndex = 6;
            this.m_cancel.Text = "取消";
            this.m_cancel.UseVisualStyleBackColor = true;
            // 
            // m_OK
            // 
            this.m_OK.Location = new System.Drawing.Point(84, 177);
            this.m_OK.Name = "m_OK";
            this.m_OK.Size = new System.Drawing.Size(75, 23);
            this.m_OK.TabIndex = 7;
            this.m_OK.Text = "确定";
            this.m_OK.UseVisualStyleBackColor = true;
            this.m_OK.Click += new System.EventHandler(this.OnOK);
            // 
            // PjtConfigForm
            // 
            this.AcceptButton = this.m_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.m_cancel;
            this.ClientSize = new System.Drawing.Size(384, 212);
            this.Controls.Add(this.m_OK);
            this.Controls.Add(this.m_cancel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.m_pointBox);
            this.Controls.Add(this.m_sampleBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.m_namebox);
            this.Controls.Add(this.label1);
            this.Name = "PjtConfigForm";
            this.Text = "PjtConfigForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox m_namebox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox m_sampleBox;
        private System.Windows.Forms.TextBox m_pointBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button m_cancel;
        private System.Windows.Forms.Button m_OK;
    }
}