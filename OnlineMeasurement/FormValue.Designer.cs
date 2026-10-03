namespace OnlineMeasurement
{
    partial class FormValue
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormValue));
            this.label1 = new System.Windows.Forms.Label();
            this.buttonOut = new System.Windows.Forms.Button();
            this.buttonIn = new System.Windows.Forms.Button();
            this.textBox_Value = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(30, 41);
            this.label1.Name = "label1";
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(239, 243, 248);
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "值：";
            // 
            // buttonOut
            // 
            this.buttonOut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonOut.Location = new System.Drawing.Point(200, 100);
            this.buttonOut.Name = "buttonOut";
            this.buttonOut.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonOut.BackColor = System.Drawing.Color.FromArgb(49, 61, 77);
            this.buttonOut.ForeColor = System.Drawing.Color.FromArgb(239, 243, 248);
            this.buttonOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOut.Size = new System.Drawing.Size(75, 23);
            this.buttonOut.TabIndex = 4;
            this.buttonOut.Text = "取消";
            this.buttonOut.UseVisualStyleBackColor = false;
            this.buttonOut.Click += new System.EventHandler(this.buttonOut_Click);
            // 
            // buttonIn
            // 
            this.buttonIn.Location = new System.Drawing.Point(102, 100);
            this.buttonIn.Name = "buttonIn";
            this.buttonIn.Font = new System.Drawing.Font("Microsoft YaHei UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonIn.BackColor = System.Drawing.Color.FromArgb(49, 61, 77);
            this.buttonIn.ForeColor = System.Drawing.Color.FromArgb(239, 243, 248);
            this.buttonIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonIn.Size = new System.Drawing.Size(75, 23);
            this.buttonIn.TabIndex = 5;
            this.buttonIn.Text = "确定";
            this.buttonIn.UseVisualStyleBackColor = false;
            this.buttonIn.Click += new System.EventHandler(this.buttonIn_Click);
            // 
            // textBox_Value
            // 
            this.textBox_Value.Location = new System.Drawing.Point(65, 38);
            this.textBox_Value.Name = "textBox_Value";
            this.textBox_Value.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_Value.BackColor = System.Drawing.Color.FromArgb(54, 66, 83);
            this.textBox_Value.ForeColor = System.Drawing.Color.FromArgb(239, 243, 248);
            this.textBox_Value.Size = new System.Drawing.Size(210, 21);
            this.textBox_Value.TabIndex = 3;
            // 
            // FormValue
            // 
            this.AcceptButton = this.buttonIn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonOut;
            this.ClientSize = new System.Drawing.Size(313, 147);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonOut);
            this.Controls.Add(this.buttonIn);
            this.Controls.Add(this.textBox_Value);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormValue";
            this.BackColor = System.Drawing.Color.FromArgb(31, 37, 47);
            this.ForeColor = System.Drawing.Color.FromArgb(239, 243, 248);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormValue";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FormValue_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonOut;
        private System.Windows.Forms.Button buttonIn;
        private System.Windows.Forms.TextBox textBox_Value;
    }
}
