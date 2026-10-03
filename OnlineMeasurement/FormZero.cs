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
    public partial class FormZero : Form
    {
        public Dictionary<int, Point3D> dataPoint3D = new Dictionary<int, Point3D>();
        public Dictionary<int, string> dataName = new Dictionary<int, string>();
        bool isNeedAll = false;
        public FormZero(bool isNeedAll)
        {
            this.isNeedAll = isNeedAll;
            InitializeComponent();
            AppTheme.Apply(this);
        }
        private void FormZero_Load(object sender, EventArgs e)
        {
            dataGridView.Rows.Clear();
            foreach (int key in dataPoint3D.Keys)
            {
                string name = dataName.ContainsKey(key) ? dataName[key] : null;
                if (dataPoint3D[key] == null)
                {
                    dataGridView.Rows.Add(key, name, null, null, null);
                }
                else
                {
                    dataGridView.Rows.Add(key, name, dataPoint3D[key].X, dataPoint3D[key].Y, dataPoint3D[key].Z);
                }
            }
        }
        private void buttonOK_Click(object sender, EventArgs e)
        {
            dataPoint3D.Clear();
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                try
                {
                    int key = int.Parse(dataGridView.Rows[i].Cells["pointNum"].Value.ToString());
                    float dx = float.Parse(dataGridView.Rows[i].Cells["dx"].Value.ToString());
                    float dy = float.Parse(dataGridView.Rows[i].Cells["dy"].Value.ToString());
                    float dz = float.Parse(dataGridView.Rows[i].Cells["dz"].Value.ToString());
                    dataPoint3D.Add(key, new Point3D(dx, dy, dz));
                }
                catch (Exception ex)
                {
                    if (isNeedAll)
                    {
                        MessageBox.Show("格式有误，坐标校准需要输入全部数据：" + ex.Message);
                        return;
                    }
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void dataGridViewL_KeyPress(object sender, KeyPressEventArgs e)
        {
            //在选定的单元格，如果按下Ctrl+V
            if (e.KeyChar == 22)
            {
                PasteData(dataGridView);
            }
        }

        private void PasteData(DataGridView dataGridView)
        {
            try
            {
                string clipboardText = Clipboard.GetText(); //获取剪贴板中的内容
                if (string.IsNullOrEmpty(clipboardText))
                {
                    return;
                }
                int colnum = 0;
                int rownum = 0;
                for (int i = 0; i < clipboardText.Length; i++)
                {
                    if (clipboardText.Substring(i, 1) == "\t")
                    {
                        colnum++;
                    }
                    if (clipboardText.Substring(i, 1) == "\n")
                    {
                        rownum++;
                    }
                }
                colnum = colnum / rownum + 1;
                int selectedRowIndex, selectedColIndex;
                selectedRowIndex = dataGridView.CurrentRow.Index;
                selectedColIndex = dataGridView.CurrentCell.ColumnIndex;
                if (selectedRowIndex + rownum > dataGridView.RowCount || selectedColIndex + colnum > dataGridView.ColumnCount)
                {
                    MessageBox.Show("粘贴区域大小不一致");
                    return;
                }
                String[][] temp = new String[rownum][];
                for (int i = 0; i < rownum; i++)
                {
                    temp[i] = new String[colnum];
                }
                int m = 0, n = 0, len = 0;
                while (len != clipboardText.Length)
                {
                    String str = clipboardText.Substring(len, 1);
                    if (str == "\t")
                    {
                        n++;
                    }
                    else if (str == "\n")
                    {
                        m++;
                        n = 0;
                    }
                    else
                    {
                        temp[m][n] += str;
                    }
                    len++;
                }
                for (int i = selectedRowIndex; i < selectedRowIndex + rownum; i++)
                {
                    for (int j = selectedColIndex; j < selectedColIndex + colnum; j++)
                    {
                        dataGridView.Rows[i].Cells[j].Value = temp[i - selectedRowIndex][j - selectedColIndex];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonNG_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void FormZero_Paint(object sender, PaintEventArgs e)
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

        private void button_0_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                dataGridView.Rows[i].Cells["dx"].Value = 0;
                dataGridView.Rows[i].Cells["dy"].Value = 0;
                dataGridView.Rows[i].Cells["dz"].Value = 0;
            }
        }
    }
}
