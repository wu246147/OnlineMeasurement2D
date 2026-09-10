using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineMeasurement
{
    public partial class FormCam : Form
    {
        bool isAlter = false;
        Cam cam = new Cam();
        public FormCam()
        {
            InitializeComponent();
        }
        private void FormCam_Load(object sender, EventArgs e)
        {
            cam.sets.Clear();
            cam.Load();

            treeView1.Nodes.Clear();
            foreach (var key1 in cam.sets.Keys.OrderBy(n => { return n.Length; }).ThenBy(n => { return n; }))//多条件排序
            {
                var node2 = treeView1.Nodes.Add(key1);//10-1-3UG、...
            }
        }
        private void EnableUpData()
        {
            checkBox_Disabled.CheckedChanged += UpData;
            comboBox_CameraType.SelectedIndexChanged += UpData;
        }
        private void DisenableUpData()
        {
            checkBox_Disabled.CheckedChanged -= UpData;
            comboBox_CameraType.SelectedIndexChanged -= UpData;
        }

        private void UpData(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Level != 0)
            {
                return;
            }
            isAlter = true;
            var set = cam.sets[treeView1.SelectedNode.Text];

            set.gSet.Disabled = checkBox_Disabled.Checked;
            set.gSet.CameraType = comboBox_CameraType.SelectedIndex == 1 ? CameraType.Hikvision : CameraType.Basler;
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Level == 0)
            {
                DisenableUpData();

                panel1.Visible = true;
                var set = cam.sets[treeView1.SelectedNode.Text];
                label_Node.Text = e.Node.Text;

                checkBox_Disabled.Checked = set.gSet.Disabled;
                comboBox_CameraType.SelectedIndex = set.gSet.CameraType == CameraType.Hikvision ? 1 : 0;
                if (set.camparam == null)
                {
                    label_标定状态.Text = "未标定";
                    label_标定状态.BackColor = Color.Red;
                }
                else
                {
                    label_标定状态.Text = "已标定";
                    label_标定状态.BackColor = Color.Transparent;
                }

                EnableUpData();
            }
            else
            {
                panel1.Visible = false;
            }
        }


        private void buttonAdd_Click(object sender, EventArgs e)
        {
            FormName frm = new FormName();
            List<string> list = new List<string>();
            foreach (TreeNode item in treeView1.Nodes)
            {
                list.Add(item.Text);
            }
            frm.InNames = list.ToArray();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                cam.sets.Add(frm.NameValue, new CamSetting(frm.NameValue));
                treeView1.Nodes.Add(frm.NameValue);
                isAlter = true;
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (cam.sets.Remove(treeView1.SelectedNode.Text))
            {
                treeView1.SelectedNode.Remove();
                isAlter = true;
            }
        }

        private void FormCam_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isAlter)
            {
                DialogResult dialogResult = MessageBox.Show("是否保存参数？", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    if (!cam.Save())
                    {
                        MessageBox.Show($"保存失败：" + cam.ErrMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                        return;
                    }
                    isAlter = false;
                }
                else if (dialogResult == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void FormSet_Paint(object sender, PaintEventArgs e)
        {
            Control control = (Control)sender;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;//抗锯齿

            GraphicsPath graphicsPath = new GraphicsPath();
            if (control.ClientRectangle.Width > 0 && control.ClientRectangle.Height > 0)
            {
                graphicsPath.AddRectangle(control.ClientRectangle);
                LinearGradientBrush brush = new LinearGradientBrush(control.ClientRectangle, Color.FromArgb(100, 212, 225), Color.FromArgb(100, 162, 225), LinearGradientMode.BackwardDiagonal);
                g.FillPath(brush, graphicsPath);
            }
        }
    }
}
