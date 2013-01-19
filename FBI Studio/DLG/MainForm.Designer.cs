namespace FBI_Studio
{
    partial class MainForm
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
            this.m_menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.配置管理toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.合并点文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开样本目录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.浏览点目录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.工具ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.验证ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重置样本目录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.样本转换ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.自定义ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.抠图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.单个抠图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_workLast = new System.Windows.Forms.Button();
            this.m_workNext = new System.Windows.Forms.Button();
            this.m_suplyLast = new System.Windows.Forms.Button();
            this.m_suplyNext = new System.Windows.Forms.Button();
            this.m_workRandom = new System.Windows.Forms.Button();
            this.m_suplyRandom = new System.Windows.Forms.Button();
            this.m_statusStrip = new System.Windows.Forms.StatusStrip();
            this.m_pointGreyLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.m_pointFolder = new System.Windows.Forms.Label();
            this.m_pointTran = new System.Windows.Forms.Button();
            this.m_tips = new System.Windows.Forms.RichTextBox();
            this.m_tipsMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.清空ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_workBath = new System.Windows.Forms.Button();
            this.m_suplyBatch = new System.Windows.Forms.Button();
            this.m_mutiply = new System.Windows.Forms.Button();
            this.m_3D = new System.Windows.Forms.Button();
            this.m_intell = new System.Windows.Forms.Button();
            this.m_config = new System.Windows.Forms.Button();
            this.m_workDel = new System.Windows.Forms.Button();
            this.m_suplyDel = new System.Windows.Forms.Button();
            this.m_deletePt = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.m_workIrRadio = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.m_workGrRadio = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.m_suplyGrRadio = new System.Windows.Forms.RadioButton();
            this.m_suplyIrRadio = new System.Windows.Forms.RadioButton();
            this.m_sampleFolder = new System.Windows.Forms.Label();
            this.m_workUvRadio = new System.Windows.Forms.RadioButton();
            this.m_picBoxSuply = new FBI_Studio.PointAnlysePicBox();
            this.m_filterCtrl = new FBI_Studio.FilterCtrl();
            this.m_picBoxWork = new FBI_Studio.PointAnlysePicBox();
            this.m_suplyUvRadio = new System.Windows.Forms.RadioButton();
            this.m_menuStrip.SuspendLayout();
            this.m_statusStrip.SuspendLayout();
            this.m_tipsMenu.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_picBoxSuply)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_picBoxWork)).BeginInit();
            this.SuspendLayout();
            // 
            // m_menuStrip
            // 
            this.m_menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.工具ToolStripMenuItem,
            this.自定义ToolStripMenuItem});
            this.m_menuStrip.Location = new System.Drawing.Point(0, 0);
            this.m_menuStrip.Name = "m_menuStrip";
            this.m_menuStrip.Size = new System.Drawing.Size(1156, 24);
            this.m_menuStrip.TabIndex = 5;
            this.m_menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.保存配置ToolStripMenuItem,
            this.配置管理toolStripMenuItem,
            this.合并点文件ToolStripMenuItem,
            this.打开样本目录ToolStripMenuItem,
            this.浏览点目录ToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.fileToolStripMenuItem.Text = "文件";
            // 
            // 保存配置ToolStripMenuItem
            // 
            this.保存配置ToolStripMenuItem.Name = "保存配置ToolStripMenuItem";
            this.保存配置ToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.保存配置ToolStripMenuItem.Text = "保存配置";
            this.保存配置ToolStripMenuItem.Click += new System.EventHandler(this.OnSaveConfig);
            // 
            // 配置管理toolStripMenuItem
            // 
            this.配置管理toolStripMenuItem.Name = "配置管理toolStripMenuItem";
            this.配置管理toolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.配置管理toolStripMenuItem.Text = "配置管理";
            this.配置管理toolStripMenuItem.Click += new System.EventHandler(this.OnConfigManage);
            // 
            // 合并点文件ToolStripMenuItem
            // 
            this.合并点文件ToolStripMenuItem.Name = "合并点文件ToolStripMenuItem";
            this.合并点文件ToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.合并点文件ToolStripMenuItem.Text = "合并点文件";
            this.合并点文件ToolStripMenuItem.Click += new System.EventHandler(this.OnMergePoint);
            // 
            // 打开样本目录ToolStripMenuItem
            // 
            this.打开样本目录ToolStripMenuItem.Name = "打开样本目录ToolStripMenuItem";
            this.打开样本目录ToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.打开样本目录ToolStripMenuItem.Text = "浏览样本目录";
            this.打开样本目录ToolStripMenuItem.Click += new System.EventHandler(this.OnExploreSamplePath);
            // 
            // 浏览点目录ToolStripMenuItem
            // 
            this.浏览点目录ToolStripMenuItem.Name = "浏览点目录ToolStripMenuItem";
            this.浏览点目录ToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.浏览点目录ToolStripMenuItem.Text = "浏览点目录";
            this.浏览点目录ToolStripMenuItem.Click += new System.EventHandler(this.OnExplorePointPath);
            // 
            // 工具ToolStripMenuItem
            // 
            this.工具ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.验证ToolStripMenuItem,
            this.重置样本目录ToolStripMenuItem,
            this.样本转换ToolStripMenuItem});
            this.工具ToolStripMenuItem.Name = "工具ToolStripMenuItem";
            this.工具ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.工具ToolStripMenuItem.Text = "工具";
            // 
            // 验证ToolStripMenuItem
            // 
            this.验证ToolStripMenuItem.Name = "验证ToolStripMenuItem";
            this.验证ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.验证ToolStripMenuItem.Text = "重识检测";
            this.验证ToolStripMenuItem.ToolTipText = "用checkBox选中的特征点来识别辅助样本";
            this.验证ToolStripMenuItem.Click += new System.EventHandler(this.OnCheckOverReg);
            // 
            // 重置样本目录ToolStripMenuItem
            // 
            this.重置样本目录ToolStripMenuItem.Name = "重置样本目录ToolStripMenuItem";
            this.重置样本目录ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.重置样本目录ToolStripMenuItem.Text = "重定位";
            this.重置样本目录ToolStripMenuItem.ToolTipText = "把当前辅助样本,移至工作目录下";
            this.重置样本目录ToolStripMenuItem.Click += new System.EventHandler(this.OnReplace);
            // 
            // 样本转换ToolStripMenuItem
            // 
            this.样本转换ToolStripMenuItem.Name = "样本转换ToolStripMenuItem";
            this.样本转换ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.样本转换ToolStripMenuItem.Text = "样本转换";
            this.样本转换ToolStripMenuItem.Click += new System.EventHandler(this.OnConvertSample);
            // 
            // 自定义ToolStripMenuItem
            // 
            this.自定义ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.抠图ToolStripMenuItem,
            this.单个抠图ToolStripMenuItem});
            this.自定义ToolStripMenuItem.Name = "自定义ToolStripMenuItem";
            this.自定义ToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.自定义ToolStripMenuItem.Text = "自定义";
            // 
            // 抠图ToolStripMenuItem
            // 
            this.抠图ToolStripMenuItem.Name = "抠图ToolStripMenuItem";
            this.抠图ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.抠图ToolStripMenuItem.Text = "批量抠图";
            this.抠图ToolStripMenuItem.Click += new System.EventHandler(this.OnBatchDigROI);
            // 
            // 单个抠图ToolStripMenuItem
            // 
            this.单个抠图ToolStripMenuItem.Name = "单个抠图ToolStripMenuItem";
            this.单个抠图ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.单个抠图ToolStripMenuItem.Text = "单个抠图";
            this.单个抠图ToolStripMenuItem.Click += new System.EventHandler(this.OnDigRoi);
            // 
            // m_workLast
            // 
            this.m_workLast.Location = new System.Drawing.Point(209, 53);
            this.m_workLast.Name = "m_workLast";
            this.m_workLast.Size = new System.Drawing.Size(75, 23);
            this.m_workLast.TabIndex = 6;
            this.m_workLast.Text = "上一张";
            this.m_workLast.UseVisualStyleBackColor = true;
            this.m_workLast.Click += new System.EventHandler(this.OnBtClick);
            // 
            // m_workNext
            // 
            this.m_workNext.Location = new System.Drawing.Point(209, 82);
            this.m_workNext.Name = "m_workNext";
            this.m_workNext.Size = new System.Drawing.Size(75, 23);
            this.m_workNext.TabIndex = 7;
            this.m_workNext.Text = "下一张";
            this.m_workNext.UseVisualStyleBackColor = true;
            this.m_workNext.Click += new System.EventHandler(this.OnBtClick);
            // 
            // m_suplyLast
            // 
            this.m_suplyLast.Location = new System.Drawing.Point(209, 426);
            this.m_suplyLast.Name = "m_suplyLast";
            this.m_suplyLast.Size = new System.Drawing.Size(75, 23);
            this.m_suplyLast.TabIndex = 8;
            this.m_suplyLast.Text = "上一张";
            this.m_suplyLast.UseVisualStyleBackColor = true;
            this.m_suplyLast.Click += new System.EventHandler(this.OnBtClick);
            // 
            // m_suplyNext
            // 
            this.m_suplyNext.Location = new System.Drawing.Point(209, 455);
            this.m_suplyNext.Name = "m_suplyNext";
            this.m_suplyNext.Size = new System.Drawing.Size(75, 23);
            this.m_suplyNext.TabIndex = 9;
            this.m_suplyNext.Text = "下一张";
            this.m_suplyNext.UseVisualStyleBackColor = true;
            this.m_suplyNext.Click += new System.EventHandler(this.OnBtClick);
            // 
            // m_workRandom
            // 
            this.m_workRandom.Location = new System.Drawing.Point(209, 111);
            this.m_workRandom.Name = "m_workRandom";
            this.m_workRandom.Size = new System.Drawing.Size(75, 23);
            this.m_workRandom.TabIndex = 10;
            this.m_workRandom.Text = "随机";
            this.m_workRandom.UseVisualStyleBackColor = true;
            this.m_workRandom.Click += new System.EventHandler(this.OnBtClick);
            // 
            // m_suplyRandom
            // 
            this.m_suplyRandom.Location = new System.Drawing.Point(209, 484);
            this.m_suplyRandom.Name = "m_suplyRandom";
            this.m_suplyRandom.Size = new System.Drawing.Size(75, 23);
            this.m_suplyRandom.TabIndex = 11;
            this.m_suplyRandom.Text = "随机";
            this.m_suplyRandom.UseVisualStyleBackColor = true;
            this.m_suplyRandom.Click += new System.EventHandler(this.OnBtClick);
            // 
            // m_statusStrip
            // 
            this.m_statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_pointGreyLabel});
            this.m_statusStrip.Location = new System.Drawing.Point(0, 794);
            this.m_statusStrip.Name = "m_statusStrip";
            this.m_statusStrip.Size = new System.Drawing.Size(1156, 22);
            this.m_statusStrip.TabIndex = 12;
            this.m_statusStrip.Text = "m_statusStrip";
            // 
            // m_pointGreyLabel
            // 
            this.m_pointGreyLabel.Name = "m_pointGreyLabel";
            this.m_pointGreyLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 15;
            this.label1.Text = "样本目录";
            this.label1.Click += new System.EventHandler(this.OnSetSampleFolder);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(441, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 16;
            this.label2.Text = "点文件目录";
            this.label2.Click += new System.EventHandler(this.OnSetPointFolder);
            // 
            // m_pointFolder
            // 
            this.m_pointFolder.AutoSize = true;
            this.m_pointFolder.Location = new System.Drawing.Point(524, 32);
            this.m_pointFolder.Name = "m_pointFolder";
            this.m_pointFolder.Size = new System.Drawing.Size(101, 12);
            this.m_pointFolder.TabIndex = 17;
            this.m_pointFolder.Text = "未设置点文件目录";
            this.m_pointFolder.Click += new System.EventHandler(this.OnSetPointFolder);
            // 
            // m_pointTran
            // 
            this.m_pointTran.Location = new System.Drawing.Point(209, 263);
            this.m_pointTran.Name = "m_pointTran";
            this.m_pointTran.Size = new System.Drawing.Size(75, 23);
            this.m_pointTran.TabIndex = 18;
            this.m_pointTran.Text = "大神・训练";
            this.m_pointTran.UseVisualStyleBackColor = true;
            this.m_pointTran.Click += new System.EventHandler(this.OnPointTran);
            // 
            // m_tips
            // 
            this.m_tips.ContextMenuStrip = this.m_tipsMenu;
            this.m_tips.Location = new System.Drawing.Point(936, 38);
            this.m_tips.Name = "m_tips";
            this.m_tips.ReadOnly = true;
            this.m_tips.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.m_tips.Size = new System.Drawing.Size(208, 667);
            this.m_tips.TabIndex = 19;
            this.m_tips.Text = "";
            // 
            // m_tipsMenu
            // 
            this.m_tipsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.清空ToolStripMenuItem});
            this.m_tipsMenu.Name = "m_tipsMenu";
            this.m_tipsMenu.Size = new System.Drawing.Size(99, 26);
            // 
            // 清空ToolStripMenuItem
            // 
            this.清空ToolStripMenuItem.Name = "清空ToolStripMenuItem";
            this.清空ToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.清空ToolStripMenuItem.Text = "清空";
            this.清空ToolStripMenuItem.Click += new System.EventHandler(this.OnClearTip);
            // 
            // m_workBath
            // 
            this.m_workBath.Location = new System.Drawing.Point(209, 140);
            this.m_workBath.Name = "m_workBath";
            this.m_workBath.Size = new System.Drawing.Size(75, 23);
            this.m_workBath.TabIndex = 20;
            this.m_workBath.Text = "批量";
            this.m_workBath.UseVisualStyleBackColor = true;
            this.m_workBath.Click += new System.EventHandler(this.OnBtBatch);
            // 
            // m_suplyBatch
            // 
            this.m_suplyBatch.Location = new System.Drawing.Point(209, 513);
            this.m_suplyBatch.Name = "m_suplyBatch";
            this.m_suplyBatch.Size = new System.Drawing.Size(75, 23);
            this.m_suplyBatch.TabIndex = 21;
            this.m_suplyBatch.Text = "批量";
            this.m_suplyBatch.UseVisualStyleBackColor = true;
            this.m_suplyBatch.Click += new System.EventHandler(this.OnBtBatch);
            // 
            // m_mutiply
            // 
            this.m_mutiply.Location = new System.Drawing.Point(209, 292);
            this.m_mutiply.Name = "m_mutiply";
            this.m_mutiply.Size = new System.Drawing.Size(75, 23);
            this.m_mutiply.TabIndex = 22;
            this.m_mutiply.Text = "奥义・批量";
            this.m_mutiply.UseVisualStyleBackColor = true;
            this.m_mutiply.Click += new System.EventHandler(this.OnBtMultiply);
            // 
            // m_3D
            // 
            this.m_3D.Location = new System.Drawing.Point(209, 321);
            this.m_3D.Name = "m_3D";
            this.m_3D.Size = new System.Drawing.Size(75, 23);
            this.m_3D.TabIndex = 23;
            this.m_3D.Text = "无双・3 D";
            this.m_3D.UseVisualStyleBackColor = true;
            this.m_3D.Click += new System.EventHandler(this.OnBt3D);
            // 
            // m_intell
            // 
            this.m_intell.Location = new System.Drawing.Point(209, 350);
            this.m_intell.Name = "m_intell";
            this.m_intell.Size = new System.Drawing.Size(75, 23);
            this.m_intell.TabIndex = 24;
            this.m_intell.Text = "必杀・取点";
            this.m_intell.UseVisualStyleBackColor = true;
            this.m_intell.Click += new System.EventHandler(this.OnIntellSelPoint);
            // 
            // m_config
            // 
            this.m_config.Location = new System.Drawing.Point(209, 234);
            this.m_config.Name = "m_config";
            this.m_config.Size = new System.Drawing.Size(75, 23);
            this.m_config.TabIndex = 25;
            this.m_config.Text = "真妹・配置";
            this.m_config.UseVisualStyleBackColor = true;
            this.m_config.Click += new System.EventHandler(this.OnBtConfig);
            // 
            // m_workDel
            // 
            this.m_workDel.Location = new System.Drawing.Point(209, 170);
            this.m_workDel.Name = "m_workDel";
            this.m_workDel.Size = new System.Drawing.Size(75, 23);
            this.m_workDel.TabIndex = 26;
            this.m_workDel.Text = "删除";
            this.m_workDel.UseVisualStyleBackColor = true;
            this.m_workDel.Click += new System.EventHandler(this.OnDeleteClick);
            // 
            // m_suplyDel
            // 
            this.m_suplyDel.Location = new System.Drawing.Point(209, 543);
            this.m_suplyDel.Name = "m_suplyDel";
            this.m_suplyDel.Size = new System.Drawing.Size(75, 23);
            this.m_suplyDel.TabIndex = 27;
            this.m_suplyDel.Text = "删除";
            this.m_suplyDel.UseVisualStyleBackColor = true;
            this.m_suplyDel.Click += new System.EventHandler(this.OnDeleteClick);
            // 
            // m_deletePt
            // 
            this.m_deletePt.Location = new System.Drawing.Point(209, 379);
            this.m_deletePt.Name = "m_deletePt";
            this.m_deletePt.Size = new System.Drawing.Size(75, 23);
            this.m_deletePt.TabIndex = 28;
            this.m_deletePt.Text = "疾风・删除";
            this.m_deletePt.UseVisualStyleBackColor = true;
            this.m_deletePt.Click += new System.EventHandler(this.OnDeletePt);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(300, 426);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 29;
            this.label3.Text = "辅助区";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(309, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 30;
            this.label4.Text = "工作区";
            // 
            // m_workIrRadio
            // 
            this.m_workIrRadio.AutoSize = true;
            this.m_workIrRadio.Location = new System.Drawing.Point(83, 2);
            this.m_workIrRadio.Name = "m_workIrRadio";
            this.m_workIrRadio.Size = new System.Drawing.Size(35, 16);
            this.m_workIrRadio.TabIndex = 31;
            this.m_workIrRadio.Text = "ir";
            this.m_workIrRadio.UseVisualStyleBackColor = true;
            this.m_workIrRadio.CheckedChanged += new System.EventHandler(this.OnPicIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.m_workUvRadio);
            this.panel1.Controls.Add(this.m_workGrRadio);
            this.panel1.Controls.Add(this.m_workIrRadio);
            this.panel1.Location = new System.Drawing.Point(360, 47);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(228, 18);
            this.panel1.TabIndex = 32;
            // 
            // m_workGrRadio
            // 
            this.m_workGrRadio.AutoSize = true;
            this.m_workGrRadio.Checked = true;
            this.m_workGrRadio.Location = new System.Drawing.Point(16, -1);
            this.m_workGrRadio.Name = "m_workGrRadio";
            this.m_workGrRadio.Size = new System.Drawing.Size(53, 16);
            this.m_workGrRadio.TabIndex = 32;
            this.m_workGrRadio.TabStop = true;
            this.m_workGrRadio.Text = "green";
            this.m_workGrRadio.UseVisualStyleBackColor = true;
            this.m_workGrRadio.CheckedChanged += new System.EventHandler(this.OnPicIndexChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.m_suplyUvRadio);
            this.panel2.Controls.Add(this.m_suplyGrRadio);
            this.panel2.Controls.Add(this.m_suplyIrRadio);
            this.panel2.Location = new System.Drawing.Point(360, 420);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 18);
            this.panel2.TabIndex = 33;
            // 
            // m_suplyGrRadio
            // 
            this.m_suplyGrRadio.AutoSize = true;
            this.m_suplyGrRadio.Checked = true;
            this.m_suplyGrRadio.Location = new System.Drawing.Point(16, 2);
            this.m_suplyGrRadio.Name = "m_suplyGrRadio";
            this.m_suplyGrRadio.Size = new System.Drawing.Size(53, 16);
            this.m_suplyGrRadio.TabIndex = 32;
            this.m_suplyGrRadio.TabStop = true;
            this.m_suplyGrRadio.Text = "green";
            this.m_suplyGrRadio.UseVisualStyleBackColor = true;
            this.m_suplyGrRadio.CheckedChanged += new System.EventHandler(this.OnPicIndexChanged);
            // 
            // m_suplyIrRadio
            // 
            this.m_suplyIrRadio.AutoSize = true;
            this.m_suplyIrRadio.Location = new System.Drawing.Point(83, 3);
            this.m_suplyIrRadio.Name = "m_suplyIrRadio";
            this.m_suplyIrRadio.Size = new System.Drawing.Size(35, 16);
            this.m_suplyIrRadio.TabIndex = 31;
            this.m_suplyIrRadio.Text = "ir";
            this.m_suplyIrRadio.UseVisualStyleBackColor = true;
            this.m_suplyIrRadio.CheckedChanged += new System.EventHandler(this.OnPicIndexChanged);
            // 
            // m_sampleFolder
            // 
            this.m_sampleFolder.AutoSize = true;
            this.m_sampleFolder.Location = new System.Drawing.Point(81, 32);
            this.m_sampleFolder.Name = "m_sampleFolder";
            this.m_sampleFolder.Size = new System.Drawing.Size(89, 12);
            this.m_sampleFolder.TabIndex = 34;
            this.m_sampleFolder.Text = "未设置样本目录";
            // 
            // m_workUvRadio
            // 
            this.m_workUvRadio.AutoSize = true;
            this.m_workUvRadio.Location = new System.Drawing.Point(139, 1);
            this.m_workUvRadio.Name = "m_workUvRadio";
            this.m_workUvRadio.Size = new System.Drawing.Size(35, 16);
            this.m_workUvRadio.TabIndex = 33;
            this.m_workUvRadio.TabStop = true;
            this.m_workUvRadio.Text = "uv";
            this.m_workUvRadio.UseVisualStyleBackColor = true;
            this.m_workUvRadio.CheckedChanged += new System.EventHandler(this.OnPicIndexChanged);
            // 
            // m_picBoxSuply
            // 
            this.m_picBoxSuply.Location = new System.Drawing.Point(302, 441);
            this.m_picBoxSuply.Name = "m_picBoxSuply";
            this.m_picBoxSuply.Size = new System.Drawing.Size(0, 0);
            this.m_picBoxSuply.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.m_picBoxSuply.TabIndex = 2;
            this.m_picBoxSuply.TabStop = false;
            this.m_picBoxSuply.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnPicBoxMouseMove);
            // 
            // m_filterCtrl
            // 
            this.m_filterCtrl.Location = new System.Drawing.Point(13, 53);
            this.m_filterCtrl.Name = "m_filterCtrl";
            this.m_filterCtrl.Size = new System.Drawing.Size(180, 738);
            this.m_filterCtrl.TabIndex = 0;
            this.m_filterCtrl.OnWorkFolderChanged += new FBI_Studio.FilterCtrl.FilterEventHandle(this.OnWorkFolderChanged);
            this.m_filterCtrl.OnSuplyFolderChanged += new FBI_Studio.FilterCtrl.FilterEventHandle(this.OnSuplyFolderChanged);
            // 
            // m_picBoxWork
            // 
            this.m_picBoxWork.Location = new System.Drawing.Point(311, 68);
            this.m_picBoxWork.Name = "m_picBoxWork";
            this.m_picBoxWork.Size = new System.Drawing.Size(0, 0);
            this.m_picBoxWork.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.m_picBoxWork.TabIndex = 1;
            this.m_picBoxWork.TabStop = false;
            this.m_picBoxWork.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnPicBoxMouseMove);
            // 
            // m_suplyUvRadio
            // 
            this.m_suplyUvRadio.AutoSize = true;
            this.m_suplyUvRadio.Location = new System.Drawing.Point(139, 4);
            this.m_suplyUvRadio.Name = "m_suplyUvRadio";
            this.m_suplyUvRadio.Size = new System.Drawing.Size(35, 16);
            this.m_suplyUvRadio.TabIndex = 34;
            this.m_suplyUvRadio.TabStop = true;
            this.m_suplyUvRadio.Text = "uv";
            this.m_suplyUvRadio.UseVisualStyleBackColor = true;
            this.m_suplyUvRadio.CheckedChanged += new System.EventHandler(this.OnPicIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1156, 816);
            this.Controls.Add(this.m_sampleFolder);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.m_deletePt);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.m_suplyDel);
            this.Controls.Add(this.m_workDel);
            this.Controls.Add(this.m_config);
            this.Controls.Add(this.m_intell);
            this.Controls.Add(this.m_3D);
            this.Controls.Add(this.m_mutiply);
            this.Controls.Add(this.m_pointTran);
            this.Controls.Add(this.m_suplyBatch);
            this.Controls.Add(this.m_pointFolder);
            this.Controls.Add(this.m_workBath);
            this.Controls.Add(this.m_tips);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_statusStrip);
            this.Controls.Add(this.m_suplyRandom);
            this.Controls.Add(this.m_workRandom);
            this.Controls.Add(this.m_suplyNext);
            this.Controls.Add(this.m_suplyLast);
            this.Controls.Add(this.m_workLast);
            this.Controls.Add(this.m_workNext);
            this.Controls.Add(this.m_picBoxSuply);
            this.Controls.Add(this.m_filterCtrl);
            this.Controls.Add(this.m_menuStrip);
            this.Controls.Add(this.m_picBoxWork);
            this.KeyPreview = true;
            this.MainMenuStrip = this.m_menuStrip;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FBI Studio";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Click += new System.EventHandler(this.OnBtClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.m_menuStrip.ResumeLayout(false);
            this.m_menuStrip.PerformLayout();
            this.m_statusStrip.ResumeLayout(false);
            this.m_statusStrip.PerformLayout();
            this.m_tipsMenu.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_picBoxSuply)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_picBoxWork)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FBI_Studio.FilterCtrl m_filterCtrl;
        private PointAnlysePicBox m_picBoxWork;
        private PointAnlysePicBox m_picBoxSuply;
        private System.Windows.Forms.MenuStrip m_menuStrip;
        private System.Windows.Forms.Button m_workLast;
        private System.Windows.Forms.Button m_workNext;
        private System.Windows.Forms.Button m_suplyLast;
        private System.Windows.Forms.Button m_suplyNext;
        private System.Windows.Forms.Button m_workRandom;
        private System.Windows.Forms.Button m_suplyRandom;
        private System.Windows.Forms.StatusStrip m_statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel m_pointGreyLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label m_pointFolder;
        private System.Windows.Forms.Button m_pointTran;
        private System.Windows.Forms.RichTextBox m_tips;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存配置ToolStripMenuItem;
        private System.Windows.Forms.Button m_workBath;
        private System.Windows.Forms.Button m_suplyBatch;
        private System.Windows.Forms.Button m_mutiply;
        private System.Windows.Forms.ContextMenuStrip m_tipsMenu;
        private System.Windows.Forms.ToolStripMenuItem 清空ToolStripMenuItem;
        private System.Windows.Forms.Button m_3D;
        private System.Windows.Forms.Button m_intell;
        private System.Windows.Forms.Button m_config;
        private System.Windows.Forms.ToolStripMenuItem 合并点文件ToolStripMenuItem;
        private System.Windows.Forms.Button m_workDel;
        private System.Windows.Forms.Button m_suplyDel;
        private System.Windows.Forms.Button m_deletePt;
        private System.Windows.Forms.ToolStripMenuItem 工具ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 验证ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开样本目录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 重置样本目录ToolStripMenuItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripMenuItem 配置管理toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 样本转换ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 浏览点目录ToolStripMenuItem;
        private System.Windows.Forms.RadioButton m_workIrRadio;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton m_workGrRadio;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton m_suplyGrRadio;
        private System.Windows.Forms.RadioButton m_suplyIrRadio;
        private System.Windows.Forms.Label m_sampleFolder;
        private System.Windows.Forms.ToolStripMenuItem 自定义ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 抠图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 单个抠图ToolStripMenuItem;
        private System.Windows.Forms.RadioButton m_workUvRadio;
        private System.Windows.Forms.RadioButton m_suplyUvRadio;
    }
}