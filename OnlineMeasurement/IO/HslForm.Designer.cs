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
            this.button_Close.Location = new System.Drawing.Point(652, 62);
            this.button_Close.Name = "button_Close";
            this.button_Close.Size = new System.Drawing.Size(75, 23);
            this.button_Close.TabIndex = 25;
            this.button_Close.Text = "断开";
            this.button_Close.UseVisualStyleBackColor = true;
            this.button_Close.Click += new System.EventHandler(this.button_Close_Click);
            // 
            // button_Open
            // 
            this.button_Open.Location = new System.Drawing.Point(652, 27);
            this.button_Open.Name = "button_Open";
            this.button_Open.Size = new System.Drawing.Size(75, 23);
            this.button_Open.TabIndex = 26;
            this.button_Open.Text = "连接";
            this.button_Open.UseVisualStyleBackColor = true;
            this.button_Open.Click += new System.EventHandler(this.button_Open_Click);
            // 
            // numericUpDownPort
            // 
            this.numericUpDownPort.Location = new System.Drawing.Point(70, 51);
            this.numericUpDownPort.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.numericUpDownPort.Name = "numericUpDownPort";
            this.numericUpDownPort.Size = new System.Drawing.Size(88, 21);
            this.numericUpDownPort.TabIndex = 24;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 22;
            this.label2.Text = "Port：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 23;
            this.label1.Text = "Ip：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(184, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 22;
            this.label3.Text = "DA2：";
            // 
            // numericUpDownDA2
            // 
            this.numericUpDownDA2.Location = new System.Drawing.Point(225, 21);
            this.numericUpDownDA2.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.numericUpDownDA2.Name = "numericUpDownDA2";
            this.numericUpDownDA2.Size = new System.Drawing.Size(77, 21);
            this.numericUpDownDA2.TabIndex = 24;
            // 
            // textBoxIpAddress
            // 
            this.textBoxIpAddress.Location = new System.Drawing.Point(58, 21);
            this.textBoxIpAddress.Name = "textBoxIpAddress";
            this.textBoxIpAddress.Size = new System.Drawing.Size(100, 21);
            this.textBoxIpAddress.TabIndex = 27;
            // 
            // labelInName
            // 
            this.labelInName.AutoSize = true;
            this.labelInName.BackColor = System.Drawing.Color.Transparent;
            this.labelInName.Location = new System.Drawing.Point(14, 110);
            this.labelInName.Name = "labelInName";
            this.labelInName.Size = new System.Drawing.Size(53, 12);
            this.labelInName.TabIndex = 28;
            this.labelInName.Text = "信号名称";
            // 
            // labelOutName
            // 
            this.labelOutName.AutoSize = true;
            this.labelOutName.BackColor = System.Drawing.Color.Transparent;
            this.labelOutName.Location = new System.Drawing.Point(288, 110);
            this.labelOutName.Name = "labelOutName";
            this.labelOutName.Size = new System.Drawing.Size(53, 12);
            this.labelOutName.TabIndex = 29;
            this.labelOutName.Text = "信号名称";
            // 
            // labelInAddress
            // 
            this.labelInAddress.AutoSize = true;
            this.labelInAddress.BackColor = System.Drawing.Color.Transparent;
            this.labelInAddress.Location = new System.Drawing.Point(91, 110);
            this.labelInAddress.Name = "labelInAddress";
            this.labelInAddress.Size = new System.Drawing.Size(29, 12);
            this.labelInAddress.TabIndex = 30;
            this.labelInAddress.Text = "地址";
            // 
            // labelInValue
            // 
            this.labelInValue.AutoSize = true;
            this.labelInValue.BackColor = System.Drawing.Color.Transparent;
            this.labelInValue.Location = new System.Drawing.Point(186, 110);
            this.labelInValue.Name = "labelInValue";
            this.labelInValue.Size = new System.Drawing.Size(17, 12);
            this.labelInValue.TabIndex = 30;
            this.labelInValue.Text = "值";
            // 
            // textBoxLog
            // 
            this.textBoxLog.Location = new System.Drawing.Point(567, 134);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(212, 269);
            this.textBoxLog.TabIndex = 27;
            // 
            // labelOutAddress
            // 
            this.labelOutAddress.AutoSize = true;
            this.labelOutAddress.BackColor = System.Drawing.Color.Transparent;
            this.labelOutAddress.Location = new System.Drawing.Point(365, 110);
            this.labelOutAddress.Name = "labelOutAddress";
            this.labelOutAddress.Size = new System.Drawing.Size(29, 12);
            this.labelOutAddress.TabIndex = 30;
            this.labelOutAddress.Text = "地址";
            // 
            // labelOutValue
            // 
            this.labelOutValue.AutoSize = true;
            this.labelOutValue.BackColor = System.Drawing.Color.Transparent;
            this.labelOutValue.Location = new System.Drawing.Point(460, 110);
            this.labelOutValue.Name = "labelOutValue";
            this.labelOutValue.Size = new System.Drawing.Size(17, 12);
            this.labelOutValue.TabIndex = 30;
            this.labelOutValue.Text = "值";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.checkBox_IsStringReverseByteWord);
            this.groupBox1.Controls.Add(this.comboBox_DataFormat);
            this.groupBox1.Controls.Add(this.textBoxIpAddress);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.numericUpDownPort);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numericUpDownDA2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
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
            this.checkBox_IsStringReverseByteWord.Location = new System.Drawing.Point(341, 53);
            this.checkBox_IsStringReverseByteWord.Name = "checkBox_IsStringReverseByteWord";
            this.checkBox_IsStringReverseByteWord.Size = new System.Drawing.Size(96, 16);
            this.checkBox_IsStringReverseByteWord.TabIndex = 29;
            this.checkBox_IsStringReverseByteWord.Text = "字符按字反转";
            this.checkBox_IsStringReverseByteWord.UseVisualStyleBackColor = true;
            // 
            // comboBox_DataFormat
            // 
            this.comboBox_DataFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_DataFormat.FormattingEnabled = true;
            this.comboBox_DataFormat.Location = new System.Drawing.Point(410, 21);
            this.comboBox_DataFormat.Name = "comboBox_DataFormat";
            this.comboBox_DataFormat.Size = new System.Drawing.Size(72, 20);
            this.comboBox_DataFormat.TabIndex = 28;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(339, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 22;
            this.label5.Text = "数据格式：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(565, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 32;
            this.label4.Text = "日志";
            // 
            // HslForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 425);
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