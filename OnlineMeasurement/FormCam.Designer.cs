namespace OnlineMeasurement
{
    partial class FormCam
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCam));
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_标定设置 = new System.Windows.Forms.Button();
            this.label_标定状态 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox_Disabled = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label_Node = new System.Windows.Forms.Label();
            this.comboBox_CameraType = new System.Windows.Forms.ComboBox();
            this.label_CameraType = new System.Windows.Forms.Label();
            this.groupBox7.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonAdd
            // 
            this.buttonAdd.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonAdd.Location = new System.Drawing.Point(152, 12);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.BackColor = System.Drawing.Color.FromArgb(49, 61, 77);
            this.buttonAdd.ForeColor = System.Drawing.Color.FromArgb(239, 243, 248);
            this.buttonAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAdd.Size = new System.Drawing.Size(75, 30);
            this.buttonAdd.TabIndex = 4;
            this.buttonAdd.Text = "添加";
            this.buttonAdd.UseVisualStyleBackColor = false;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonRemove.Location = new System.Drawing.Point(12, 12);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.BackColor = System.Drawing.Color.FromArgb(49, 61, 77);
            this.buttonRemove.ForeColor = System.Drawing.Color.FromArgb(239, 243, 248);
            this.buttonRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRemove.Size = new System.Drawing.Size(75, 30);
            this.buttonRemove.TabIndex = 3;
            this.buttonRemove.Text = "删除";
            this.buttonRemove.UseVisualStyleBackColor = false;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // treeView1
            // 
            this.treeView1.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeView1.Location = new System.Drawing.Point(12, 58);
            this.treeView1.Name = "treeView1";
            this.treeView1.BackColor = System.Drawing.Color.FromArgb(54, 66, 83);
            this.treeView1.ForeColor = System.Drawing.Color.FromArgb(239, 243, 248);
            this.treeView1.Size = new System.Drawing.Size(215, 380);
            this.treeView1.TabIndex = 2;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // groupBox7
            // 
            this.groupBox7.BackColor = System.Drawing.Color.FromArgb(39, 49, 63);
            this.groupBox7.Controls.Add(this.panel1);
            this.groupBox7.Location = new System.Drawing.Point(233, 58);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox7.ForeColor = System.Drawing.Color.FromArgb(239, 243, 248);
            this.groupBox7.Size = new System.Drawing.Size(555, 380);
            this.groupBox7.TabIndex = 14;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "相机设定";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(39, 49, 63);
            this.panel1.Controls.Add(this.button_标定设置);
            this.panel1.Controls.Add(this.label_标定状态);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.checkBox_Disabled);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label_Node);
            this.panel1.Controls.Add(this.comboBox_CameraType);
            this.panel1.Controls.Add(this.label_CameraType);
            this.panel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.Location = new System.Drawing.Point(6, 20);
            this.panel1.Name = "panel1";
            this.panel1.ForeColor = System.Drawing.Color.FromArgb(239, 243, 248);
            this.panel1.Size = new System.Drawing.Size(543, 354);
            this.panel1.TabIndex = 8;
            this.panel1.Visible = false;
            // 
            // button_标定设置
            // 
            this.button_标定设置.Location = new System.Drawing.Point(403, 36);
            this.button_标定设置.Name = "button_标定设置";
            this.button_标定设置.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_标定设置.BackColor = System.Drawing.Color.FromArgb(49, 61, 77);
            this.button_标定设置.ForeColor = System.Drawing.Color.FromArgb(239, 243, 248);
            this.button_标定设置.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_标定设置.Size = new System.Drawing.Size(88, 35);
            this.button_标定设置.TabIndex = 16;
            this.button_标定设置.Text = "标定设置";
            this.button_标定设置.UseVisualStyleBackColor = false;
            this.button_标定设置.Visible = false;
            // 
            // label_标定状态
            // 
            this.label_标定状态.AutoSize = true;
            this.label_标定状态.Location = new System.Drawing.Point(316, 46);
            this.label_标定状态.Name = "label_标定状态";
            this.label_标定状态.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_标定状态.BackColor = System.Drawing.Color.Transparent;
            this.label_标定状态.ForeColor = System.Drawing.Color.FromArgb(239, 243, 248);
            this.label_标定状态.Size = new System.Drawing.Size(21, 14);
            this.label_标定状态.TabIndex = 15;
            this.label_标定状态.Text = "--";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(240, 46);
            this.label2.Name = "label2";
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(239, 243, 248);
            this.label2.Size = new System.Drawing.Size(70, 14);
            this.label2.TabIndex = 14;
            this.label2.Text = "标定状态;";
            // 
            // checkBox_Disabled
            // 
            this.checkBox_Disabled.AutoSize = true;
            this.checkBox_Disabled.Location = new System.Drawing.Point(35, 42);
            this.checkBox_Disabled.Name = "checkBox_Disabled";
            this.checkBox_Disabled.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox_Disabled.BackColor = System.Drawing.Color.FromArgb(39, 49, 63);
            this.checkBox_Disabled.ForeColor = System.Drawing.Color.FromArgb(239, 243, 248);
            this.checkBox_Disabled.Size = new System.Drawing.Size(110, 18);
            this.checkBox_Disabled.TabIndex = 13;
            this.checkBox_Disabled.Text = "屏蔽当前相机";
            this.checkBox_Disabled.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 9);
            this.label1.Name = "label1";
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(239, 243, 248);
            this.label1.Size = new System.Drawing.Size(105, 14);
            this.label1.TabIndex = 2;
            this.label1.Text = "当前选中相机：";
            // 
            // label_Node
            // 
            this.label_Node.AutoSize = true;
            this.label_Node.Location = new System.Drawing.Point(121, 9);
            this.label_Node.Name = "label_Node";
            this.label_Node.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Node.BackColor = System.Drawing.Color.Transparent;
            this.label_Node.ForeColor = System.Drawing.Color.FromArgb(239, 243, 248);
            this.label_Node.Size = new System.Drawing.Size(21, 14);
            this.label_Node.TabIndex = 2;
            this.label_Node.Text = "--";
            // 
            // comboBox_CameraType
            // 
            this.comboBox_CameraType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_CameraType.FormattingEnabled = true;
            this.comboBox_CameraType.Items.AddRange(new object[] {
            "Basler",
            "海康"});
            this.comboBox_CameraType.Location = new System.Drawing.Point(121, 82);
            this.comboBox_CameraType.Name = "comboBox_CameraType";
            this.comboBox_CameraType.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_CameraType.BackColor = System.Drawing.Color.FromArgb(54, 66, 83);
            this.comboBox_CameraType.ForeColor = System.Drawing.Color.FromArgb(239, 243, 248);
            this.comboBox_CameraType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox_CameraType.Size = new System.Drawing.Size(105, 22);
            this.comboBox_CameraType.TabIndex = 18;
            // 
            // label_CameraType
            // 
            this.label_CameraType.AutoSize = true;
            this.label_CameraType.Location = new System.Drawing.Point(35, 86);
            this.label_CameraType.Name = "label_CameraType";
            this.label_CameraType.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_CameraType.BackColor = System.Drawing.Color.Transparent;
            this.label_CameraType.ForeColor = System.Drawing.Color.FromArgb(239, 243, 248);
            this.label_CameraType.Size = new System.Drawing.Size(63, 14);
            this.label_CameraType.TabIndex = 17;
            this.label_CameraType.Text = "相机类型";
            // 
            // FormCam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.treeView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormCam";
            this.BackColor = System.Drawing.Color.FromArgb(31, 37, 47);
            this.ForeColor = System.Drawing.Color.FromArgb(239, 243, 248);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "相机设置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCam_FormClosing);
            this.Load += new System.EventHandler(this.FormCam_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormSet_Paint);
            this.groupBox7.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox checkBox_Disabled;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_Node;
        private System.Windows.Forms.Label label_CameraType;
        private System.Windows.Forms.ComboBox comboBox_CameraType;
        private System.Windows.Forms.Label label_标定状态;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_标定设置;
    }
}
