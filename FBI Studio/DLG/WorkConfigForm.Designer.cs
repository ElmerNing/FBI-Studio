namespace FBI_Studio
{
    partial class WorkConfigForm
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
            this.components = new System.ComponentModel.Container();
            this.m_menuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.m_listView = new FBI_Studio.ListViewNF();
            this.m_menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_menuStrip
            // 
            this.m_menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2});
            this.m_menuStrip.Name = "m_menuStrip";
            this.m_menuStrip.Size = new System.Drawing.Size(93, 26);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(92, 22);
            this.toolStripMenuItem2.Text = "123";
            // 
            // m_listView
            // 
            this.m_listView.BackColor = System.Drawing.Color.White;
            this.m_listView.BackgroundImageTiled = true;
            this.m_listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_listView.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.m_listView.LabelEdit = true;
            this.m_listView.Location = new System.Drawing.Point(0, 0);
            this.m_listView.Name = "m_listView";
            this.m_listView.OwnerDraw = true;
            this.m_listView.Size = new System.Drawing.Size(904, 533);
            this.m_listView.TabIndex = 0;
            this.m_listView.UseCompatibleStateImageBehavior = false;
            this.m_listView.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.OnDrawItem);
            this.m_listView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.OnItemDrag);
            this.m_listView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OnMouseClick);
            this.m_listView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OnMouseDoubleClick);
            this.m_listView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            this.m_listView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
            this.m_listView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            this.m_listView.Resize += new System.EventHandler(this.OnReSize);
            // 
            // WorkConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(904, 533);
            this.ControlBox = false;
            this.Controls.Add(this.m_listView);
            this.Name = "WorkConfigForm";
            this.Text = "WorkConfigForm";
            this.m_menuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ListViewNF m_listView;
        private System.Windows.Forms.ContextMenuStrip m_menuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;

    }
}