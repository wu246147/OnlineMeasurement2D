namespace OnlineMeasurement.IO
{
    partial class HslForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HslForm));
            this.button_Close = new System.Windows.Forms.Button();
            this.button_Open = new System.Windows.Forms.Button();
            this.numericUpDownPort = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownDA2 = new System.Windows.Forms.NumericUpDown();
            this.textBoxIpAddress = new System.Windows.Forms.TextBox();
            this.labelInName = new System.Windows.Forms.Label();
            this.labelOutName = new System.Windows.Forms.Label();
            this.labelInAddress = new System.Windows.Forms.Label();
            this.labelInValue = new System.Windows.Forms.Label();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.labelOutAddress = new System.Windows.Forms.Label();
            this.labelOutValue = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox_IsStringReverseByteWord = new System.Windows.Forms.CheckBox();
            this.comboBox_DataFormat = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDA2)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_Close
            // 
            this.button_Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Close.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(61)))), ((int)(((byte)(77)))));
            this.button_Close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Close.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Close.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
            this.button_Close.Location = new System.Drawing.Point(731, 62);
            this.button_Close.Name = "button_Close";
            this.button_Close.Size = new System.Drawing.Size(75, 23);
            this.button_Close.TabIndex = 25;
            this.button_Close.Text = "断开";
            this.button_Close.UseVisualStyleBackColor = false;
            this.button_Close.Click += new System.EventHandler(this.button_Close_Click);
            // 
            // button_Open
            // 
            this.button_Open.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Open.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(61)))), ((int)(((byte)(77)))));
            this.button_Open.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Open.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Open.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
            this.button_Open.Location = new System.Drawing.Point(731, 27);
            this.button_Open.Name = "button_Open";
            this.button_Open.Size = new System.Drawing.Size(75, 23);
            this.button_Open.TabIndex = 26;
            this.button_Open.Text = "连接";
            this.button_Open.UseVisualStyleBackColor = false;
            this.button_Open.Click += new System.EventHandler(this.button_Open_Click);
            // 
            // numericUpDownPort
            // 
            this.numericUpDownPort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(66)))), ((int)(((byte)(83)))));
            this.numericUpDownPort.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numericUpDownPort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
            this.numericUpDownPort.Location = new System.Drawing.Point(70, 51);
            this.numericUpDownPort.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.numericUpDownPort.Name = "numericUpDownPort";
            this.numericUpDownPort.Size = new System.Drawing.Size(88, 23);
            this.numericUpDownPort.TabIndex = 24;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
            this.label2.Location = new System.Drawing.Point(23, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 17);
            this.label2.TabIndex = 22;
            this.label2.Text = "Port：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
            this.label1.Location = new System.Drawing.Point(23, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 17);
            this.label1.TabIndex = 23;
            this.label1.Text = "Ip：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
            this.label3.Location = new System.Drawing.Point(184, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 17);
            this.label3.TabIndex = 22;
            this.label3.Text = "DA2：";
            // 
            // numericUpDownDA2
            // 
            this.numericUpDownDA2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(66)))), ((int)(((byte)(83)))));
            this.numericUpDownDA2.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numericUpDownDA2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
            this.numericUpDownDA2.Location = new System.Drawing.Point(225, 21);
            this.numericUpDownDA2.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.numericUpDownDA2.Name = "numericUpDownDA2";
            this.numericUpDownDA2.Size = new System.Drawing.Size(77, 23);
            this.numericUpDownDA2.TabIndex = 24;
            // 
            // textBoxIpAddress
            // 
            this.textBoxIpAddress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(66)))), ((int)(((byte)(83)))));
            this.textBoxIpAddress.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxIpAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
            this.textBoxIpAddress.Location = new System.Drawing.Point(58, 21);
            this.textBoxIpAddress.Name = "textBoxIpAddress";
            this.textBoxIpAddress.Size = new System.Drawing.Size(100, 23);
            this.textBoxIpAddress.TabIndex = 27;
            // 
            // labelInName
            // 
            this.labelInName.AutoSize = true;
            this.labelInName.BackColor = System.Drawing.Color.Transparent;
            this.labelInName.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelInName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
            this.labelInName.Location = new System.Drawing.Point(14, 110);
            this.labelInName.Name = "labelInName";
            this.labelInName.Size = new System.Drawing.Size(56, 17);
            this.labelInName.TabIndex = 28;
            this.labelInName.Text = "信号名称";
            // 
            // labelOutName
            // 
            this.labelOutName.AutoSize = true;
            this.labelOutName.BackColor = System.Drawing.Color.Transparent;
            this.labelOutName.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelOutName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
            this.labelOutName.Location = new System.Drawing.Point(315, 110);
            this.labelOutName.Name = "labelOutName";
            this.labelOutName.Size = new System.Drawing.Size(56, 17);
            this.labelOutName.TabIndex = 29;
            this.labelOutName.Text = "信号名称";
            // 
            // labelInAddress
            // 
            this.labelInAddress.AutoSize = true;
            this.labelInAddress.BackColor = System.Drawing.Color.Transparent;
            this.labelInAddress.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelInAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
            this.labelInAddress.Location = new System.Drawing.Point(129, 110);
            this.labelInAddress.Name = "labelInAddress";
            this.labelInAddress.Size = new System.Drawing.Size(32, 17);
            this.labelInAddress.TabIndex = 30;
            this.labelInAddress.Text = "地址";
            // 
            // labelInValue
            // 
            this.labelInValue.AutoSize = true;
            this.labelInValue.BackColor = System.Drawing.Color.Transparent;
            this.labelInValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelInValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
            this.labelInValue.Location = new System.Drawing.Point(224, 110);
            this.labelInValue.Name = "labelInValue";
            this.labelInValue.Size = new System.Drawing.Size(20, 17);
            this.labelInValue.TabIndex = 30;
            this.labelInValue.Text = "值";
            // 
            // textBoxLog
            // 
            this.textBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(66)))), ((int)(((byte)(83)))));
            this.textBoxLog.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
            this.textBoxLog.Location = new System.Drawing.Point(646, 134);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(212, 605);
            this.textBoxLog.TabIndex = 27;
            // 
            // labelOutAddress
            // 
            this.labelOutAddress.AutoSize = true;
            this.labelOutAddress.BackColor = System.Drawing.Color.Transparent;
            this.labelOutAddress.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelOutAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
            this.labelOutAddress.Location = new System.Drawing.Point(414, 110);
            this.labelOutAddress.Name = "labelOutAddress";
            this.labelOutAddress.Size = new System.Drawing.Size(32, 17);
            this.labelOutAddress.TabIndex = 30;
            this.labelOutAddress.Text = "地址";
            // 
            // labelOutValue
            // 
            this.labelOutValue.AutoSize = true;
            this.labelOutValue.BackColor = System.Drawing.Color.Transparent;
            this.labelOutValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelOutValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
            this.labelOutValue.Location = new System.Drawing.Point(508, 110);
            this.labelOutValue.Name = "labelOutValue";
            this.labelOutValue.Size = new System.Drawing.Size(20, 17);
            this.labelOutValue.TabIndex = 30;
            this.labelOutValue.Text = "值";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(49)))), ((int)(((byte)(63)))));
            this.groupBox1.Controls.Add(this.checkBox_IsStringReverseByteWord);
            this.groupBox1.Controls.Add(this.comboBox_DataFormat);
            this.groupBox1.Controls.Add(this.textBoxIpAddress);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.numericUpDownPort);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numericUpDownDA2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
            this.groupBox1.Location = new System.Drawing.Point(16, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(573, 88);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "连接参数";
            // 
            // checkBox_IsStringReverseByteWord
            // 
            this.checkBox_IsStringReverseByteWord.AutoSize = true;
            this.checkBox_IsStringReverseByteWord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(49)))), ((int)(((byte)(63)))));
            this.checkBox_IsStringReverseByteWord.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox_IsStringReverseByteWord.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
            this.checkBox_IsStringReverseByteWord.Location = new System.Drawing.Point(341, 53);
            this.checkBox_IsStringReverseByteWord.Name = "checkBox_IsStringReverseByteWord";
            this.checkBox_IsStringReverseByteWord.Size = new System.Drawing.Size(99, 21);
            this.checkBox_IsStringReverseByteWord.TabIndex = 29;
            this.checkBox_IsStringReverseByteWord.Text = "字符按字反转";
            this.checkBox_IsStringReverseByteWord.UseVisualStyleBackColor = false;
            // 
            // comboBox_DataFormat
            // 
            this.comboBox_DataFormat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(66)))), ((int)(((byte)(83)))));
            this.comboBox_DataFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_DataFormat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox_DataFormat.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_DataFormat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
            this.comboBox_DataFormat.FormattingEnabled = true;
            this.comboBox_DataFormat.Location = new System.Drawing.Point(410, 21);
            this.comboBox_DataFormat.Name = "comboBox_DataFormat";
            this.comboBox_DataFormat.Size = new System.Drawing.Size(72, 25);
            this.comboBox_DataFormat.TabIndex = 28;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
            this.label5.Location = new System.Drawing.Point(339, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 17);
            this.label5.TabIndex = 22;
            this.label5.Text = "数据格式：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
            this.label4.Location = new System.Drawing.Point(649, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 17);
            this.label4.TabIndex = 32;
            this.label4.Text = "日志";
            // 
            // HslForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(37)))), ((int)(((byte)(47)))));
            this.ClientSize = new System.Drawing.Size(879, 761);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labelOutValue);
            this.Controls.Add(this.labelOutAddress);
            this.Controls.Add(this.labelInValue);
            this.Controls.Add(this.labelInAddress);
            this.Controls.Add(this.labelOutName);
            this.Controls.Add(this.labelInName);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.button_Close);
            this.Controls.Add(this.button_Open);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(248)))));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HslForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "通信设置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HslForm_FormClosing);
            this.Load += new System.EventHandler(this.HslForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.HslForm_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDA2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Close;
        private System.Windows.Forms.Button button_Open;
        private System.Windows.Forms.NumericUpDown numericUpDownPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownDA2;
        private System.Windows.Forms.TextBox textBoxIpAddress;
        private System.Windows.Forms.Label labelInName;
        private System.Windows.Forms.Label labelOutName;
        private System.Windows.Forms.Label labelInAddress;
        private System.Windows.Forms.Label labelInValue;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.Label labelOutAddress;
        private System.Windows.Forms.Label labelOutValue;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBox_IsStringReverseByteWord;
        private System.Windows.Forms.ComboBox comboBox_DataFormat;
        private System.Windows.Forms.Label label5;
    }
}
