namespace FBI_Studio
{
    partial class MutiplySetForm
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.m_OK = new System.Windows.Forms.Button();
            this.m_3D = new System.Windows.Forms.CheckBox();
            this.m_onlyChecked = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(27, 42);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(87, 21);
            this.textBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "输入匹配字符串";
            // 
            // m_OK
            // 
            this.m_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.m_OK.Location = new System.Drawing.Point(73, 82);
            this.m_OK.Name = "m_OK";
            this.m_OK.Size = new System.Drawing.Size(87, 23);
            this.m_OK.TabIndex = 2;
            this.m_OK.Text = "确认";
            this.m_OK.UseVisualStyleBackColor = true;
            // 
            // m_3D
            // 
            this.m_3D.AutoSize = true;
            this.m_3D.Location = new System.Drawing.Point(143, 13);
            this.m_3D.Name = "m_3D";
            this.m_3D.Size = new System.Drawing.Size(36, 16);
            this.m_3D.TabIndex = 3;
            this.m_3D.Text = "3D";
            this.m_3D.UseVisualStyleBackColor = true;
            // 
            // m_onlyChecked
            // 
            this.m_onlyChecked.AutoSize = true;
            this.m_onlyChecked.Location = new System.Drawing.Point(143, 47);
            this.m_onlyChecked.Name = "m_onlyChecked";
            this.m_onlyChecked.Size = new System.Drawing.Size(84, 16);
            this.m_onlyChecked.TabIndex = 4;
            this.m_onlyChecked.Text = "选中文件夹";
            this.m_onlyChecked.UseVisualStyleBackColor = true;
            // 
            // MutiplySetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(260, 117);
            this.Controls.Add(this.m_onlyChecked);
            this.Controls.Add(this.m_3D);
            this.Controls.Add(this.m_OK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.KeyPreview = true;
            this.Name = "MutiplySetForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MutiplySetForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button m_OK;
        private System.Windows.Forms.CheckBox m_3D;
        private System.Windows.Forms.CheckBox m_onlyChecked;
    }
}