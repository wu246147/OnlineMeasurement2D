namespace OnlineMeasurement
{
    partial class FormRun
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRun));
            this.buttonRun = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.labelCarKind = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label连番 = new System.Windows.Forms.Label();
            this.dataGridViewLog = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSysStatus = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelCarName = new System.Windows.Forms.Label();
            this.dataGridViewShow = new System.Windows.Forms.DataGridView();
            this.pointNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cam = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dx = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.新dx = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.新dy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.新dz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.新dd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBoxTestNum = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonCam = new System.Windows.Forms.Button();
            this.button_Led = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.checkBox相机仿真 = new System.Windows.Forms.CheckBox();
            this.checkBoxPLC仿真 = new System.Windows.Forms.CheckBox();
            this.checkBoxZero = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonPLC = new System.Windows.Forms.Button();
            this.buttonSetting = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonSkipStart = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.labelResult = new System.Windows.Forms.Label();
            this.label托盘 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewShow)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonRun
            // 
            this.buttonRun.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonRun.Location = new System.Drawing.Point(943, 32);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(84, 35);
            this.buttonRun.TabIndex = 0;
            this.buttonRun.Text = "启动流程";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonStop.Location = new System.Drawing.Point(943, 82);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(84, 35);
            this.buttonStop.TabIndex = 0;
            this.buttonStop.Text = "退出流程";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(28, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "车型：";
            // 
            // labelCarKind
            // 
            this.labelCarKind.AutoSize = true;
            this.labelCarKind.BackColor = System.Drawing.Color.Transparent;
            this.labelCarKind.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelCarKind.Location = new System.Drawing.Point(28, 84);
            this.labelCarKind.Name = "labelCarKind";
            this.labelCarKind.Size = new System.Drawing.Size(34, 21);
            this.labelCarKind.TabIndex = 1;
            this.labelCarKind.Text = "--";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(221, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 19);
            this.label3.TabIndex = 1;
            this.label3.Text = "连番：";
            // 
            // label连番
            // 
            this.label连番.AutoSize = true;
            this.label连番.BackColor = System.Drawing.Color.Transparent;
            this.label连番.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label连番.Location = new System.Drawing.Point(221, 84);
            this.label连番.Name = "label连番";
            this.label连番.Size = new System.Drawing.Size(46, 21);
            this.label连番.TabIndex = 1;
            this.label连番.Text = "---";
            // 
            // dataGridViewLog
            // 
            this.dataGridViewLog.AllowUserToAddRows = false;
            this.dataGridViewLog.AllowUserToDeleteRows = false;
            this.dataGridViewLog.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewLog.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridViewLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dataGridViewLog.Location = new System.Drawing.Point(560, 152);
            this.dataGridViewLog.Name = "dataGridViewLog";
            this.dataGridViewLog.ReadOnly = true;
            this.dataGridViewLog.RowHeadersVisible = false;
            this.dataGridViewLog.RowHeadersWidth = 51;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewLog.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewLog.RowTemplate.Height = 23;
            this.dataGridViewLog.Size = new System.Drawing.Size(475, 396);
            this.dataGridViewLog.TabIndex = 2;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "时间";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column1.Width = 145;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "消息";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column2.Width = 310;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(426, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = "状态：";
            // 
            // lblSysStatus
            // 
            this.lblSysStatus.AutoSize = true;
            this.lblSysStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblSysStatus.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSysStatus.Location = new System.Drawing.Point(426, 84);
            this.lblSysStatus.Name = "lblSysStatus";
            this.lblSysStatus.Size = new System.Drawing.Size(58, 21);
            this.lblSysStatus.TabIndex = 1;
            this.lblSysStatus.Text = "----";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(119, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 19);
            this.label4.TabIndex = 1;
            this.label4.Text = "名称：";
            // 
            // labelCarName
            // 
            this.labelCarName.AutoSize = true;
            this.labelCarName.BackColor = System.Drawing.Color.Transparent;
            this.labelCarName.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelCarName.Location = new System.Drawing.Point(119, 84);
            this.labelCarName.Name = "labelCarName";
            this.labelCarName.Size = new System.Drawing.Size(70, 21);
            this.labelCarName.TabIndex = 1;
            this.labelCarName.Text = "-----";
            // 
            // dataGridViewShow
            // 
            this.dataGridViewShow.AllowUserToAddRows = false;
            this.dataGridViewShow.AllowUserToDeleteRows = false;
            this.dataGridViewShow.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewShow.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.pointNum,
            this.cam,
            this.name,
            this.dx,
            this.dy,
            this.dz,
            this.dd,
            this.新dx,
            this.新dy,
            this.新dz,
            this.新dd});
            this.dataGridViewShow.Location = new System.Drawing.Point(16, 152);
            this.dataGridViewShow.Name = "dataGridViewShow";
            this.dataGridViewShow.ReadOnly = true;
            this.dataGridViewShow.RowHeadersVisible = false;
            this.dataGridViewShow.RowTemplate.Height = 23;
            this.dataGridViewShow.Size = new System.Drawing.Size(538, 396);
            this.dataGridViewShow.TabIndex = 4;
            // 
            // pointNum
            // 
            this.pointNum.HeaderText = "点位";
            this.pointNum.MinimumWidth = 6;
            this.pointNum.Name = "pointNum";
            this.pointNum.ReadOnly = true;
            this.pointNum.Width = 52;
            // 
            // cam
            // 
            this.cam.HeaderText = "相机";
            this.cam.Name = "cam";
            this.cam.ReadOnly = true;
            this.cam.Width = 52;
            // 
            // name
            // 
            this.name.HeaderText = "名称";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            this.name.Width = 70;
            // 
            // dx
            // 
            this.dx.HeaderText = "dx";
            this.dx.Name = "dx";
            this.dx.ReadOnly = true;
            this.dx.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dx.Width = 43;
            // 
            // dy
            // 
            this.dy.HeaderText = "dy";
            this.dy.Name = "dy";
            this.dy.ReadOnly = true;
            this.dy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dy.Width = 43;
            // 
            // dz
            // 
            this.dz.HeaderText = "dz";
            this.dz.Name = "dz";
            this.dz.ReadOnly = true;
            this.dz.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dz.Width = 43;
            // 
            // dd
            // 
            this.dd.HeaderText = "dd";
            this.dd.Name = "dd";
            this.dd.ReadOnly = true;
            this.dd.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dd.Width = 43;
            // 
            // 新dx
            // 
            this.新dx.HeaderText = "新dx";
            this.新dx.Name = "新dx";
            this.新dx.ReadOnly = true;
            this.新dx.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.新dx.Width = 43;
            // 
            // 新dy
            // 
            this.新dy.HeaderText = "新dy";
            this.新dy.Name = "新dy";
            this.新dy.ReadOnly = true;
            this.新dy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.新dy.Width = 43;
            // 
            // 新dz
            // 
            this.新dz.HeaderText = "新dz";
            this.新dz.Name = "新dz";
            this.新dz.ReadOnly = true;
            this.新dz.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.新dz.Width = 43;
            // 
            // 新dd
            // 
            this.新dd.HeaderText = "新dd";
            this.新dd.Name = "新dd";
            this.新dd.ReadOnly = true;
            this.新dd.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.新dd.Width = 43;
            // 
            // textBoxTestNum
            // 
            this.textBoxTestNum.Location = new System.Drawing.Point(54, 20);
            this.textBoxTestNum.Name = "textBoxTestNum";
            this.textBoxTestNum.Size = new System.Drawing.Size(66, 21);
            this.textBoxTestNum.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.buttonCam);
            this.groupBox1.Controls.Add(this.button_Led);
            this.groupBox1.Controls.Add(this.buttonSave);
            this.groupBox1.Controls.Add(this.checkBox相机仿真);
            this.groupBox1.Controls.Add(this.checkBoxPLC仿真);
            this.groupBox1.Controls.Add(this.checkBoxZero);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.buttonPLC);
            this.groupBox1.Controls.Add(this.textBoxTestNum);
            this.groupBox1.Location = new System.Drawing.Point(591, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(238, 130);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "工具";
            // 
            // buttonCam
            // 
            this.buttonCam.Location = new System.Drawing.Point(144, 71);
            this.buttonCam.Name = "buttonCam";
            this.buttonCam.Size = new System.Drawing.Size(75, 23);
            this.buttonCam.TabIndex = 15;
            this.buttonCam.Text = "相机设置";
            this.buttonCam.UseVisualStyleBackColor = true;
            this.buttonCam.Click += new System.EventHandler(this.buttonCam_Click);
            // 
            // button_Led
            // 
            this.button_Led.Location = new System.Drawing.Point(144, 42);
            this.button_Led.Name = "button_Led";
            this.button_Led.Size = new System.Drawing.Size(75, 23);
            this.button_Led.TabIndex = 14;
            this.button_Led.Text = "光源";
            this.button_Led.UseVisualStyleBackColor = true;
            this.button_Led.Click += new System.EventHandler(this.button_Led_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(144, 100);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 13;
            this.buttonSave.Text = "保存校准";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // checkBox相机仿真
            // 
            this.checkBox相机仿真.AutoSize = true;
            this.checkBox相机仿真.Location = new System.Drawing.Point(36, 75);
            this.checkBox相机仿真.Name = "checkBox相机仿真";
            this.checkBox相机仿真.Size = new System.Drawing.Size(72, 16);
            this.checkBox相机仿真.TabIndex = 12;
            this.checkBox相机仿真.Text = "相机仿真";
            this.checkBox相机仿真.UseVisualStyleBackColor = true;
            // 
            // checkBoxPLC仿真
            // 
            this.checkBoxPLC仿真.AutoSize = true;
            this.checkBoxPLC仿真.Location = new System.Drawing.Point(36, 50);
            this.checkBoxPLC仿真.Name = "checkBoxPLC仿真";
            this.checkBoxPLC仿真.Size = new System.Drawing.Size(66, 16);
            this.checkBoxPLC仿真.TabIndex = 12;
            this.checkBoxPLC仿真.Text = "plc仿真";
            this.checkBoxPLC仿真.UseVisualStyleBackColor = true;
            // 
            // checkBoxZero
            // 
            this.checkBoxZero.AutoSize = true;
            this.checkBoxZero.Location = new System.Drawing.Point(36, 100);
            this.checkBoxZero.Name = "checkBoxZero";
            this.checkBoxZero.Size = new System.Drawing.Size(72, 16);
            this.checkBoxZero.TabIndex = 12;
            this.checkBoxZero.Text = "补偿校准";
            this.checkBoxZero.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "车型号";
            // 
            // buttonPLC
            // 
            this.buttonPLC.Location = new System.Drawing.Point(144, 13);
            this.buttonPLC.Name = "buttonPLC";
            this.buttonPLC.Size = new System.Drawing.Size(75, 23);
            this.buttonPLC.TabIndex = 11;
            this.buttonPLC.Text = "PLC设置";
            this.buttonPLC.UseVisualStyleBackColor = true;
            this.buttonPLC.Click += new System.EventHandler(this.buttonPLC_Click);
            // 
            // buttonSetting
            // 
            this.buttonSetting.Location = new System.Drawing.Point(844, 64);
            this.buttonSetting.Name = "buttonSetting";
            this.buttonSetting.Size = new System.Drawing.Size(75, 23);
            this.buttonSetting.TabIndex = 11;
            this.buttonSetting.Text = "测点设置";
            this.buttonSetting.UseVisualStyleBackColor = true;
            this.buttonSetting.Click += new System.EventHandler(this.buttonSetting_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.buttonClear);
            this.panel1.Controls.Add(this.buttonSkipStart);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.buttonSetting);
            this.panel1.Controls.Add(this.buttonRun);
            this.panel1.Controls.Add(this.dataGridViewShow);
            this.panel1.Controls.Add(this.buttonStop);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.dataGridViewLog);
            this.panel1.Controls.Add(this.labelResult);
            this.panel1.Controls.Add(this.labelCarKind);
            this.panel1.Controls.Add(this.lblSysStatus);
            this.panel1.Controls.Add(this.labelCarName);
            this.panel1.Controls.Add(this.label托盘);
            this.panel1.Controls.Add(this.label连番);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(-1, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1053, 567);
            this.panel1.TabIndex = 10;
            // 
            // buttonClear
            // 
            this.buttonClear.Enabled = false;
            this.buttonClear.Location = new System.Drawing.Point(844, 98);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 12;
            this.buttonClear.Text = "重载参数";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // buttonSkipStart
            // 
            this.buttonSkipStart.Location = new System.Drawing.Point(844, 30);
            this.buttonSkipStart.Name = "buttonSkipStart";
            this.buttonSkipStart.Size = new System.Drawing.Size(75, 23);
            this.buttonSkipStart.TabIndex = 10;
            this.buttonSkipStart.Text = "强制开始";
            this.buttonSkipStart.UseVisualStyleBackColor = true;
            this.buttonSkipStart.Click += new System.EventHandler(this.buttonSkipStart_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(519, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 19);
            this.label7.TabIndex = 3;
            this.label7.Text = "结果：";
            // 
            // labelResult
            // 
            this.labelResult.AutoSize = true;
            this.labelResult.BackColor = System.Drawing.Color.Transparent;
            this.labelResult.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelResult.Location = new System.Drawing.Point(519, 85);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(58, 21);
            this.labelResult.TabIndex = 1;
            this.labelResult.Text = "----";
            // 
            // label托盘
            // 
            this.label托盘.AutoSize = true;
            this.label托盘.BackColor = System.Drawing.Color.Transparent;
            this.label托盘.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label托盘.Location = new System.Drawing.Point(308, 82);
            this.label托盘.Name = "label托盘";
            this.label托盘.Size = new System.Drawing.Size(46, 21);
            this.label托盘.TabIndex = 1;
            this.label托盘.Text = "---";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(308, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 19);
            this.label5.TabIndex = 1;
            this.label5.Text = "托盘号：";
            // 
            // FormRun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1051, 565);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormRun";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OnlineMeasurement(v1.17.1.0)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormRun_FormClosing);
            this.Load += new System.EventHandler(this.FormRun_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormRun_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewShow)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelCarKind;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label连番;
        private System.Windows.Forms.DataGridView dataGridViewLog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSysStatus;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelCarName;
        private System.Windows.Forms.DataGridView dataGridViewShow;
        private System.Windows.Forms.TextBox textBoxTestNum;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonSetting;
        private System.Windows.Forms.CheckBox checkBoxZero;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonSkipStart;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelResult;
        private System.Windows.Forms.Button button_Led;
        private System.Windows.Forms.Button buttonPLC;
        private System.Windows.Forms.CheckBox checkBoxPLC仿真;
        private System.Windows.Forms.CheckBox checkBox相机仿真;
        private System.Windows.Forms.Label label托盘;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonCam;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn pointNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn cam;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn dx;
        private System.Windows.Forms.DataGridViewTextBoxColumn dy;
        private System.Windows.Forms.DataGridViewTextBoxColumn dz;
        private System.Windows.Forms.DataGridViewTextBoxColumn dd;
        private System.Windows.Forms.DataGridViewTextBoxColumn 新dx;
        private System.Windows.Forms.DataGridViewTextBoxColumn 新dy;
        private System.Windows.Forms.DataGridViewTextBoxColumn 新dz;
        private System.Windows.Forms.DataGridViewTextBoxColumn 新dd;
    }
}

