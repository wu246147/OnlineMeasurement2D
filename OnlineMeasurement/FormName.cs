using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineMeasurement
{
    public partial class FormName : Form
    {
        public string[] InNames = new string[0];
        public string NameValue;
        public FormName()
        {
            InitializeComponent();
            AppTheme.Apply(this);
        }

        private void buttonIn_Click(object sender, EventArgs e)
        {
            if (textBox_Name.Text == string.Empty)
            {
                MessageBox.Show("未输入名称！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (InNames.Contains(textBox_Name.Text))
            {
                MessageBox.Show("名称已存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_Name.SelectAll();
                return;
            }
            this.NameValue = textBox_Name.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonOut_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FormName_Paint(object sender, PaintEventArgs e)
        {
            Control control = (Control)sender;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;//抗锯齿

            GraphicsPath graphicsPath = new GraphicsPath();
            if (control.ClientRectangle.Width > 0 && control.ClientRectangle.Height > 0)
            {
                graphicsPath.AddRectangle(control.ClientRectangle);
                LinearGradientBrush brush = new LinearGradientBrush(control.ClientRectangle, AppTheme.RaisedSurface, AppTheme.Window, LinearGradientMode.BackwardDiagonal);
                g.FillPath(brush, graphicsPath);
            }
        }
    }
}
