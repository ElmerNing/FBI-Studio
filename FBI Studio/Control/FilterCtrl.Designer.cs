namespace FBI_Studio
{
    partial class FilterCtrl
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilterCtrl));
            this.m_folderTreeView = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // m_folderTreeView
            // 
            this.m_folderTreeView.BackColor = System.Drawing.SystemColors.Window;
            this.m_folderTreeView.CheckBoxes = true;
            this.m_folderTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_folderTreeView.ImageIndex = 1;
            this.m_folderTreeView.ImageList = this.imageList1;
            this.m_folderTreeView.Location = new System.Drawing.Point(0, 0);
            this.m_folderTreeView.Name = "m_folderTreeView";
            this.m_folderTreeView.SelectedImageIndex = 2;
            this.m_folderTreeView.Size = new System.Drawing.Size(167, 531);
            this.m_folderTreeView.TabIndex = 2;
            this.m_folderTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnFolderTreeSelect);
            this.m_folderTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnSetWorkFolder);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Tree3.bmp");
            this.imageList1.Images.SetKeyName(1, "Tree1.bmp");
            this.imageList1.Images.SetKeyName(2, "Tree2.bmp");
            this.imageList1.Images.SetKeyName(3, "Tree2.bmp");
            // 
            // FilterCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_folderTreeView);
            this.Name = "FilterCtrl";
            this.Size = new System.Drawing.Size(167, 531);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView m_folderTreeView;
        private System.Windows.Forms.ImageList imageList1;
    }
}
