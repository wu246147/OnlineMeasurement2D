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
    public partial class FormSet : Form
    {
        bool isAlter = false;
        Car 车型参数 = new Car();
        OtherSet otherSet = new OtherSet();
        public bool IsSave = false;

        public FormSet()
        {
            InitializeComponent();
            AppTheme.Apply(this);

            groupBox绿色范围值.ContextMenuStrip = new ContextMenuStrip();
            groupBox绿色范围值.ContextMenuStrip.Opening += (s, e) =>
            {
                构建右键菜单((ContextMenuStrip)s, Copy绿色范围值);
            };
            groupBox新绿色范围值.ContextMenuStrip = new ContextMenuStrip();
            groupBox新绿色范围值.ContextMenuStrip.Opening += (s, e) =>
            {
                构建右键菜单((ContextMenuStrip)s, Copy新绿色范围值);
            };

            groupBox黄色范围值.ContextMenuStrip = new ContextMenuStrip();
            groupBox黄色范围值.ContextMenuStrip.Opening += (s, e) =>
            {
                构建右键菜单((ContextMenuStrip)s, Copy黄色范围值);
            };
            groupBox新黄色范围值.ContextMenuStrip = new ContextMenuStrip();
            groupBox新黄色范围值.ContextMenuStrip.Opening += (s, e) =>
            {
                构建右键菜单((ContextMenuStrip)s, Copy新黄色范围值);
            };
            groupBoxPointsGreenThreshold.ContextMenuStrip = new ContextMenuStrip();
            groupBoxPointsGreenThreshold.ContextMenuStrip.Opening += (s, e) =>
            {
                构建右键菜单((ContextMenuStrip)s, CopyPoints绿色范围值);
            };
            groupBoxPointsYellowThreshold.ContextMenuStrip = new ContextMenuStrip();
            groupBoxPointsYellowThreshold.ContextMenuStrip.Opening += (s, e) =>
            {
                构建右键菜单((ContextMenuStrip)s, CopyPoints黄色范围值);
            };

            checkBoxKeepOK.ContextMenuStrip = new ContextMenuStrip();
            checkBoxKeepOK.ContextMenuStrip.Opening += (s, e) =>
            {
                构建右键菜单((ContextMenuStrip)s, Copy屏蔽);
            };
            AppTheme.ApplyToolStrip(groupBox绿色范围值.ContextMenuStrip);
            AppTheme.ApplyToolStrip(groupBox新绿色范围值.ContextMenuStrip);
            AppTheme.ApplyToolStrip(groupBox黄色范围值.ContextMenuStrip);
            AppTheme.ApplyToolStrip(groupBox新黄色范围值.ContextMenuStrip);
            AppTheme.ApplyToolStrip(groupBoxPointsGreenThreshold.ContextMenuStrip);
            AppTheme.ApplyToolStrip(groupBoxPointsYellowThreshold.ContextMenuStrip);
            AppTheme.ApplyToolStrip(checkBoxKeepOK.ContextMenuStrip);
        }

        /// <summary>
        /// 为两孔相对距离控件统一绑定或解除参数更新事件。
        /// </summary>
        private void SetPointsControlsEvent(EventHandler handler, bool add)
        {
            if (add)
            {
                checkBoxEnablePoints.CheckedChanged += handler;
                numericUpDown_minPointsDX_1.ValueChanged += handler;
                numericUpDown_minPointsDY_1.ValueChanged += handler;
                numericUpDown_minPointsDZ_1.ValueChanged += handler;
                numericUpDown_minPointsDD_1.ValueChanged += handler;
                numericUpDown_maxPointsDX_1.ValueChanged += handler;
                numericUpDown_maxPointsDY_1.ValueChanged += handler;
                numericUpDown_maxPointsDZ_1.ValueChanged += handler;
                numericUpDown_maxPointsDD_1.ValueChanged += handler;
                numericUpDown_minPointsDX_2.ValueChanged += handler;
                numericUpDown_minPointsDY_2.ValueChanged += handler;
                numericUpDown_minPointsDZ_2.ValueChanged += handler;
                numericUpDown_minPointsDD_2.ValueChanged += handler;
                numericUpDown_maxPointsDX_2.ValueChanged += handler;
                numericUpDown_maxPointsDY_2.ValueChanged += handler;
                numericUpDown_maxPointsDZ_2.ValueChanged += handler;
                numericUpDown_maxPointsDD_2.ValueChanged += handler;
            }
            else
            {
                checkBoxEnablePoints.CheckedChanged -= handler;
                numericUpDown_minPointsDX_1.ValueChanged -= handler;
                numericUpDown_minPointsDY_1.ValueChanged -= handler;
                numericUpDown_minPointsDZ_1.ValueChanged -= handler;
                numericUpDown_minPointsDD_1.ValueChanged -= handler;
                numericUpDown_maxPointsDX_1.ValueChanged -= handler;
                numericUpDown_maxPointsDY_1.ValueChanged -= handler;
                numericUpDown_maxPointsDZ_1.ValueChanged -= handler;
                numericUpDown_maxPointsDD_1.ValueChanged -= handler;
                numericUpDown_minPointsDX_2.ValueChanged -= handler;
                numericUpDown_minPointsDY_2.ValueChanged -= handler;
                numericUpDown_minPointsDZ_2.ValueChanged -= handler;
                numericUpDown_minPointsDD_2.ValueChanged -= handler;
                numericUpDown_maxPointsDX_2.ValueChanged -= handler;
                numericUpDown_maxPointsDY_2.ValueChanged -= handler;
                numericUpDown_maxPointsDZ_2.ValueChanged -= handler;
                numericUpDown_maxPointsDD_2.ValueChanged -= handler;
            }
        }

        private void 构建右键菜单(ContextMenuStrip contextMenuStrip, Action<GeneralSet> action)
        {
            if (contextMenuStrip.Items.Count > 0)
            {
                contextMenuStrip.Items.Clear();
            }
            ToolStripMenuItem tool0 = new ToolStripMenuItem();
            tool0.Text = "复制至全部车型";
            tool0.Click += (s0, e0) =>
            {
                foreach (var carSet in 车型参数.sets.Values)
                {
                    foreach (var set in carSet.gSets.Values)
                    {
                        action.Invoke(set);
                    }
                }
                contextMenuStrip.Close();
            };
            foreach (var carkey in 车型参数.sets.Keys)
            {
                ToolStripMenuItem tool1 = new ToolStripMenuItem();
                tool1.Text = carkey;
                tool1.Click += (s1, e1) =>
                {
                    ToolStripMenuItem tool = (ToolStripMenuItem)s1;
                    foreach (var set in 车型参数.sets[carkey].gSets.Values)
                    {
                        action.Invoke(set);
                    }
                    contextMenuStrip.Close();
                };
                foreach (var set in 车型参数.sets[carkey].gSets.Values)
                {
                    ToolStripMenuItem tool2 = new ToolStripMenuItem();
                    tool2.Text = set.key.ToString();
                    tool2.Click += (s2, e2) =>
                    {
                        ToolStripMenuItem tool = (ToolStripMenuItem)s2;
                        action.Invoke(set);
                        contextMenuStrip.Close();
                    };
                    tool1.DropDownItems.Add(tool2);
                }
                tool0.DropDownItems.Add(tool1);
            }
            contextMenuStrip.Items.Add(tool0);
            AppTheme.ApplyToolStrip(contextMenuStrip);
        }

        #region Copy
        private void Copy绿色范围值(GeneralSet set)
        {
            set.minDX_1 = (float)numericUpDown_minDX_1.Value;
            set.minDY_1 = (float)numericUpDown_minDY_1.Value;
            set.minDZ_1 = (float)numericUpDown_minDZ_1.Value;
            set.maxDX_1 = (float)numericUpDown_maxDX_1.Value;
            set.maxDY_1 = (float)numericUpDown_maxDY_1.Value;
            set.maxDZ_1 = (float)numericUpDown_maxDZ_1.Value;
            isAlter = true;
        }
        private void Copy新绿色范围值(GeneralSet set)
        {
            set.minNewDX_1 = (float)numericUpDown_minNewDX_1.Value;
            set.minNewDY_1 = (float)numericUpDown_minNewDY_1.Value;
            set.minNewDZ_1 = (float)numericUpDown_minNewDZ_1.Value;
            set.maxNewDX_1 = (float)numericUpDown_maxNewDX_1.Value;
            set.maxNewDY_1 = (float)numericUpDown_maxNewDY_1.Value;
            set.maxNewDZ_1 = (float)numericUpDown_maxNewDZ_1.Value;
            isAlter = true;
        }
        private void Copy黄色范围值(GeneralSet set)
        {
            set.minDX_2 = (float)numericUpDown_minDX_2.Value;
            set.minDY_2 = (float)numericUpDown_minDY_2.Value;
            set.minDZ_2 = (float)numericUpDown_minDZ_2.Value;
            set.maxDX_2 = (float)numericUpDown_maxDX_2.Value;
            set.maxDY_2 = (float)numericUpDown_maxDY_2.Value;
            set.maxDZ_2 = (float)numericUpDown_maxDZ_2.Value;
            isAlter = true;
        }
        private void Copy新黄色范围值(GeneralSet set)
        {
            set.minNewDX_2 = (float)numericUpDown_minNewDX_2.Value;
            set.minNewDY_2 = (float)numericUpDown_minNewDY_2.Value;
            set.minNewDZ_2 = (float)numericUpDown_minNewDZ_2.Value;
            set.maxNewDX_2 = (float)numericUpDown_maxNewDX_2.Value;
            set.maxNewDY_2 = (float)numericUpDown_maxNewDY_2.Value;
            set.maxNewDZ_2 = (float)numericUpDown_maxNewDZ_2.Value;
            isAlter = true;
        }
        private void Copy屏蔽(GeneralSet set)
        {
            set.keepOK = checkBoxKeepOK.Checked;
            isAlter = true;
        }
        /// <summary>
        /// 将当前测点的两孔相对距离绿色阈值复制到选定测点。
        /// </summary>
        private void CopyPoints绿色范围值(GeneralSet set)
        {
            set.minPointsDX_1 = (float)numericUpDown_minPointsDX_1.Value;
            set.minPointsDY_1 = (float)numericUpDown_minPointsDY_1.Value;
            set.minPointsDZ_1 = (float)numericUpDown_minPointsDZ_1.Value;
            set.minPointsDD_1 = (float)numericUpDown_minPointsDD_1.Value;
            set.maxPointsDX_1 = (float)numericUpDown_maxPointsDX_1.Value;
            set.maxPointsDY_1 = (float)numericUpDown_maxPointsDY_1.Value;
            set.maxPointsDZ_1 = (float)numericUpDown_maxPointsDZ_1.Value;
            set.maxPointsDD_1 = (float)numericUpDown_maxPointsDD_1.Value;
            isAlter = true;
        }

        /// <summary>
        /// 将当前测点的两孔相对距离黄色阈值复制到选定测点。
        /// </summary>
        private void CopyPoints黄色范围值(GeneralSet set)
        {
            set.minPointsDX_2 = (float)numericUpDown_minPointsDX_2.Value;
            set.minPointsDY_2 = (float)numericUpDown_minPointsDY_2.Value;
            set.minPointsDZ_2 = (float)numericUpDown_minPointsDZ_2.Value;
            set.minPointsDD_2 = (float)numericUpDown_minPointsDD_2.Value;
            set.maxPointsDX_2 = (float)numericUpDown_maxPointsDX_2.Value;
            set.maxPointsDY_2 = (float)numericUpDown_maxPointsDY_2.Value;
            set.maxPointsDZ_2 = (float)numericUpDown_maxPointsDZ_2.Value;
            set.maxPointsDD_2 = (float)numericUpDown_maxPointsDD_2.Value;
            isAlter = true;
        }
        #endregion

        private void FormSet_Load(object sender, EventArgs e)
        {
            车型参数.LoadGeneralSet();

            treeView1.Nodes.Clear();
            var node1 = treeView1.Nodes.Add("根节点");
            foreach (var key1 in 车型参数.sets.Keys)
            {
                var node2 = node1.Nodes.Add(key1);//10-1-3UG、...

                var keys2 = 车型参数.sets[key1].gSets.Keys.OrderBy(n => { return n; });//排序
                foreach (var key3 in keys2)
                {
                    var node4 = node2.Nodes.Add(key3.ToString());//1、2、3、...
                }
            }

            // 默认选中第一个测点，使测点参数面板（包括两孔相对距离设置）打开后直接可见。
            TreeNode firstPointNode = null;
            foreach (TreeNode carNode in node1.Nodes)
            {
                if (carNode.Nodes.Count > 0)
                {
                    firstPointNode = carNode.Nodes[0];
                    break;
                }
            }
            if (firstPointNode != null)
            {
                treeView1.SelectedNode = firstPointNode;
            }

            Cam cam = new Cam();
            cam.Load();
            foreach (var key in cam.sets.Keys.OrderBy(n => { return n.Length; }).ThenBy(n => { return n; }))//多条件排序
            {
                comboBox_CamName.Items.Add(key.ToString());
            }

            otherSet.Load();
            checkBox_isSaveImage.Checked = otherSet.isSaveImage;
            textBox_imagePath.Text = otherSet.imagePath;
            checkBox_TRnum.Checked = otherSet.bTRnum;
            checkBox_isSaveImage.CheckedChanged += checkBox_isSaveImage_CheckedChanged;
            textBox_imagePath.TextChanged += textBox_imagePath_TextChanged;
            checkBox_TRnum.CheckedChanged += CheckBox_TRnum_CheckedChanged;
        }

        private void EnableUpData()
        {
            textBox_name.TextChanged += UpData;
            comboBox_CamName.TextChanged += UpData;
            comboBox_X.SelectedIndexChanged += UpData;
            comboBox_Y.SelectedIndexChanged += UpData;
            checkBoxKeepOK.CheckedChanged += UpData;
            SetPointsControlsEvent(UpData, true);
            checkBox_相对基准点.CheckedChanged += UpData;
            checkBox_增强对比度.CheckedChanged += UpData;
            checkBox_sharpenProcess.CheckedChanged += UpData;
            numericUpDown_ledExposure.ValueChanged += UpData;
            numericUpDown_score0.ValueChanged += UpData;
            numericUpDown_minDX_1.ValueChanged += UpData;
            numericUpDown_minDY_1.ValueChanged += UpData;
            numericUpDown_minDZ_1.ValueChanged += UpData;
            numericUpDown_maxDX_1.ValueChanged += UpData;
            numericUpDown_maxDY_1.ValueChanged += UpData;
            numericUpDown_maxDZ_1.ValueChanged += UpData;
            numericUpDown_minDX_2.ValueChanged += UpData;
            numericUpDown_minDY_2.ValueChanged += UpData;
            numericUpDown_minDZ_2.ValueChanged += UpData;
            numericUpDown_maxDX_2.ValueChanged += UpData;
            numericUpDown_maxDY_2.ValueChanged += UpData;
            numericUpDown_maxDZ_2.ValueChanged += UpData;
            numericUpDown_offsetX.ValueChanged += UpData;
            numericUpDown_offsetY.ValueChanged += UpData;
            numericUpDown_offsetZ.ValueChanged += UpData;
            numericUpDown_minNewDX_1.ValueChanged += UpData;
            numericUpDown_minNewDY_1.ValueChanged += UpData;
            numericUpDown_minNewDZ_1.ValueChanged += UpData;
            numericUpDown_maxNewDX_1.ValueChanged += UpData;
            numericUpDown_maxNewDY_1.ValueChanged += UpData;
            numericUpDown_maxNewDZ_1.ValueChanged += UpData;
            numericUpDown_minNewDX_2.ValueChanged += UpData;
            numericUpDown_minNewDY_2.ValueChanged += UpData;
            numericUpDown_minNewDZ_2.ValueChanged += UpData;
            numericUpDown_maxNewDX_2.ValueChanged += UpData;
            numericUpDown_maxNewDY_2.ValueChanged += UpData;
            numericUpDown_maxNewDZ_2.ValueChanged += UpData;
        }
        private void DisenableUpData()
        {
            textBox_name.TextChanged -= UpData;
            comboBox_CamName.TextChanged -= UpData;
            comboBox_X.SelectedIndexChanged -= UpData;
            comboBox_Y.SelectedIndexChanged -= UpData;
            checkBoxKeepOK.CheckedChanged -= UpData;
            SetPointsControlsEvent(UpData, false);
            checkBox_相对基准点.CheckedChanged -= UpData;
            checkBox_增强对比度.CheckedChanged -= UpData;
            numericUpDown_ledExposure.ValueChanged -= UpData;
            numericUpDown_score0.ValueChanged -= UpData;

            numericUpDown_minDX_1.ValueChanged -= UpData;
            numericUpDown_minDY_1.ValueChanged -= UpData;
            numericUpDown_minDZ_1.ValueChanged -= UpData;
            numericUpDown_maxDX_1.ValueChanged -= UpData;
            numericUpDown_maxDY_1.ValueChanged -= UpData;
            numericUpDown_maxDZ_1.ValueChanged -= UpData;
            numericUpDown_minDX_2.ValueChanged -= UpData;
            numericUpDown_minDY_2.ValueChanged -= UpData;
            numericUpDown_minDZ_2.ValueChanged -= UpData;
            numericUpDown_maxDX_2.ValueChanged -= UpData;
            numericUpDown_maxDY_2.ValueChanged -= UpData;
            numericUpDown_maxDZ_2.ValueChanged -= UpData;
            numericUpDown_offsetX.ValueChanged -= UpData;
            numericUpDown_offsetY.ValueChanged -= UpData;
            numericUpDown_offsetZ.ValueChanged -= UpData;
            numericUpDown_minNewDX_1.ValueChanged -= UpData;
            numericUpDown_minNewDY_1.ValueChanged -= UpData;
            numericUpDown_minNewDZ_1.ValueChanged -= UpData;
            numericUpDown_maxNewDX_1.ValueChanged -= UpData;
            numericUpDown_maxNewDY_1.ValueChanged -= UpData;
            numericUpDown_maxNewDZ_1.ValueChanged -= UpData;
            numericUpDown_minNewDX_2.ValueChanged -= UpData;
            numericUpDown_minNewDY_2.ValueChanged -= UpData;
            numericUpDown_minNewDZ_2.ValueChanged -= UpData;
            numericUpDown_maxNewDX_2.ValueChanged -= UpData;
            numericUpDown_maxNewDY_2.ValueChanged -= UpData;
            numericUpDown_maxNewDZ_2.ValueChanged -= UpData;
        }

        private void UpData(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Level != 2)
            {
                return;
            }
            isAlter = true;
            var gSet = 车型参数.sets[treeView1.SelectedNode.Parent.Text].gSets[int.Parse(treeView1.SelectedNode.Text)];

            gSet.name = textBox_name.Text;
            gSet.CamName = comboBox_CamName.Text;
            gSet.convertX = comboBox_X.Text;
            gSet.convertY = comboBox_Y.Text;
            gSet.keepOK = checkBoxKeepOK.Checked;
            gSet.相对基准点 = checkBox_相对基准点.Checked;
            gSet.增强对比度 = checkBox_增强对比度.Checked;
            gSet.sharpenProcess = checkBox_sharpenProcess.Checked; 
            gSet.ledExposure = (int)numericUpDown_ledExposure.Value;
            gSet.score0 = (double)numericUpDown_score0.Value;
            gSet.enablePoints = checkBoxEnablePoints.Checked;
            gSet.minPointsDX_1 = (float)numericUpDown_minPointsDX_1.Value;
            gSet.minPointsDY_1 = (float)numericUpDown_minPointsDY_1.Value;
            gSet.minPointsDZ_1 = (float)numericUpDown_minPointsDZ_1.Value;
            gSet.minPointsDD_1 = (float)numericUpDown_minPointsDD_1.Value;
            gSet.maxPointsDX_1 = (float)numericUpDown_maxPointsDX_1.Value;
            gSet.maxPointsDY_1 = (float)numericUpDown_maxPointsDY_1.Value;
            gSet.maxPointsDZ_1 = (float)numericUpDown_maxPointsDZ_1.Value;
            gSet.maxPointsDD_1 = (float)numericUpDown_maxPointsDD_1.Value;
            gSet.minPointsDX_2 = (float)numericUpDown_minPointsDX_2.Value;
            gSet.minPointsDY_2 = (float)numericUpDown_minPointsDY_2.Value;
            gSet.minPointsDZ_2 = (float)numericUpDown_minPointsDZ_2.Value;
            gSet.minPointsDD_2 = (float)numericUpDown_minPointsDD_2.Value;
            gSet.maxPointsDX_2 = (float)numericUpDown_maxPointsDX_2.Value;
            gSet.maxPointsDY_2 = (float)numericUpDown_maxPointsDY_2.Value;
            gSet.maxPointsDZ_2 = (float)numericUpDown_maxPointsDZ_2.Value;
            gSet.maxPointsDD_2 = (float)numericUpDown_maxPointsDD_2.Value;
            gSet.minDX_1 = (float)numericUpDown_minDX_1.Value;
            gSet.minDY_1 = (float)numericUpDown_minDY_1.Value;
            gSet.minDZ_1 = (float)numericUpDown_minDZ_1.Value;
            gSet.maxDX_1 = (float)numericUpDown_maxDX_1.Value;
            gSet.maxDY_1 = (float)numericUpDown_maxDY_1.Value;
            gSet.maxDZ_1 = (float)numericUpDown_maxDZ_1.Value;
            gSet.minDX_2 = (float)numericUpDown_minDX_2.Value;
            gSet.minDY_2 = (float)numericUpDown_minDY_2.Value;
            gSet.minDZ_2 = (float)numericUpDown_minDZ_2.Value;
            gSet.maxDX_2 = (float)numericUpDown_maxDX_2.Value;
            gSet.maxDY_2 = (float)numericUpDown_maxDY_2.Value;
            gSet.maxDZ_2 = (float)numericUpDown_maxDZ_2.Value;
            gSet.offsetX = (float)numericUpDown_offsetX.Value;
            gSet.offsetY = (float)numericUpDown_offsetY.Value;
            gSet.offsetZ = (float)numericUpDown_offsetZ.Value;
            gSet.minNewDX_1 = (float)numericUpDown_minNewDX_1.Value;
            gSet.minNewDY_1 = (float)numericUpDown_minNewDY_1.Value;
            gSet.minNewDZ_1 = (float)numericUpDown_minNewDZ_1.Value;
            gSet.maxNewDX_1 = (float)numericUpDown_maxNewDX_1.Value;
            gSet.maxNewDY_1 = (float)numericUpDown_maxNewDY_1.Value;
            gSet.maxNewDZ_1 = (float)numericUpDown_maxNewDZ_1.Value;
            gSet.minNewDX_2 = (float)numericUpDown_minNewDX_2.Value;
            gSet.minNewDY_2 = (float)numericUpDown_minNewDY_2.Value;
            gSet.minNewDZ_2 = (float)numericUpDown_minNewDZ_2.Value;
            gSet.maxNewDX_2 = (float)numericUpDown_maxNewDX_2.Value;
            gSet.maxNewDY_2 = (float)numericUpDown_maxNewDY_2.Value;
            gSet.maxNewDZ_2 = (float)numericUpDown_maxNewDZ_2.Value;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Level == 2)
            {
                DisenableUpData();

                panel1.Visible = true;
                var gSet = 车型参数.sets[e.Node.Parent.Text].gSets[int.Parse(e.Node.Text)];
                label_Node.Text = e.Node.Parent.Text + "/" + e.Node.Text;

                textBox_name.Text = gSet.name;
                comboBox_CamName.Text = gSet.CamName;
                comboBox_X.Text = gSet.convertX;
                comboBox_Y.Text = gSet.convertY;
                checkBoxKeepOK.Checked = gSet.keepOK;
                checkBox_相对基准点.Checked = gSet.相对基准点;
                checkBox_增强对比度.Checked = gSet.增强对比度;
                checkBox_sharpenProcess.Checked = gSet.sharpenProcess;
                numericUpDown_ledExposure.Value = gSet.ledExposure;
                numericUpDown_score0.Value = (decimal)gSet.score0;
                checkBoxEnablePoints.Checked = gSet.enablePoints;
                numericUpDown_minPointsDX_1.Value = (decimal)gSet.minPointsDX_1;
                numericUpDown_minPointsDY_1.Value = (decimal)gSet.minPointsDY_1;
                numericUpDown_minPointsDZ_1.Value = (decimal)gSet.minPointsDZ_1;
                numericUpDown_minPointsDD_1.Value = (decimal)gSet.minPointsDD_1;
                numericUpDown_maxPointsDX_1.Value = (decimal)gSet.maxPointsDX_1;
                numericUpDown_maxPointsDY_1.Value = (decimal)gSet.maxPointsDY_1;
                numericUpDown_maxPointsDZ_1.Value = (decimal)gSet.maxPointsDZ_1;
                numericUpDown_maxPointsDD_1.Value = (decimal)gSet.maxPointsDD_1;
                numericUpDown_minPointsDX_2.Value = (decimal)gSet.minPointsDX_2;
                numericUpDown_minPointsDY_2.Value = (decimal)gSet.minPointsDY_2;
                numericUpDown_minPointsDZ_2.Value = (decimal)gSet.minPointsDZ_2;
                numericUpDown_minPointsDD_2.Value = (decimal)gSet.minPointsDD_2;
                numericUpDown_maxPointsDX_2.Value = (decimal)gSet.maxPointsDX_2;
                numericUpDown_maxPointsDY_2.Value = (decimal)gSet.maxPointsDY_2;
                numericUpDown_maxPointsDZ_2.Value = (decimal)gSet.maxPointsDZ_2;
                numericUpDown_maxPointsDD_2.Value = (decimal)gSet.maxPointsDD_2;
                numericUpDown_minDX_1.Value = (decimal)gSet.minDX_1;
                numericUpDown_minDY_1.Value = (decimal)gSet.minDY_1;
                numericUpDown_minDZ_1.Value = (decimal)gSet.minDZ_1;
                numericUpDown_maxDX_1.Value = (decimal)gSet.maxDX_1;
                numericUpDown_maxDY_1.Value = (decimal)gSet.maxDY_1;
                numericUpDown_maxDZ_1.Value = (decimal)gSet.maxDZ_1;
                numericUpDown_minDX_2.Value = (decimal)gSet.minDX_2;
                numericUpDown_minDY_2.Value = (decimal)gSet.minDY_2;
                numericUpDown_minDZ_2.Value = (decimal)gSet.minDZ_2;
                numericUpDown_maxDX_2.Value = (decimal)gSet.maxDX_2;
                numericUpDown_maxDY_2.Value = (decimal)gSet.maxDY_2;
                numericUpDown_maxDZ_2.Value = (decimal)gSet.maxDZ_2;
                numericUpDown_offsetX.Value = (decimal)gSet.offsetX;
                numericUpDown_offsetY.Value = (decimal)gSet.offsetY;
                numericUpDown_offsetZ.Value = (decimal)gSet.offsetZ;
                numericUpDown_minNewDX_1.Value = (decimal)gSet.minNewDX_1;
                numericUpDown_minNewDY_1.Value = (decimal)gSet.minNewDY_1;
                numericUpDown_minNewDZ_1.Value = (decimal)gSet.minNewDZ_1;
                numericUpDown_maxNewDX_1.Value = (decimal)gSet.maxNewDX_1;
                numericUpDown_maxNewDY_1.Value = (decimal)gSet.maxNewDY_1;
                numericUpDown_maxNewDZ_1.Value = (decimal)gSet.maxNewDZ_1;
                numericUpDown_minNewDX_2.Value = (decimal)gSet.minNewDX_2;
                numericUpDown_minNewDY_2.Value = (decimal)gSet.minNewDY_2;
                numericUpDown_minNewDZ_2.Value = (decimal)gSet.minNewDZ_2;
                numericUpDown_maxNewDX_2.Value = (decimal)gSet.maxNewDX_2;
                numericUpDown_maxNewDY_2.Value = (decimal)gSet.maxNewDY_2;
                numericUpDown_maxNewDZ_2.Value = (decimal)gSet.maxNewDZ_2;

                if (!车型参数.sets[e.Node.Parent.Text].camPose.ContainsKey(int.Parse(e.Node.Text)))
                {
                    label_位姿状态.Text = "未标定";
                    label_位姿状态.BackColor = Color.Red;
                }
                else
                {
                    label_位姿状态.Text = "已标定";
                    label_位姿状态.BackColor = Color.Transparent;
                }

                EnableUpData();
            }
            else
            {
                panel1.Visible = false;
            }
        }

        private void FormSet_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isAlter)
            {
                DialogResult dialogResult = MessageBox.Show("是否保存参数？", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    if (!车型参数.SaveGeneralSet())
                    {
                        MessageBox.Show($"保存失败：" + 车型参数.ErrMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                        return;
                    }
                    otherSet.Save();
                    isAlter = false;
                    IsSave = true;
                }
                else if (dialogResult == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Level == 0)//根节点
            {
                FormName frm = new FormName();
                List<string> list = new List<string>();
                foreach (TreeNode item in treeView1.SelectedNode.Nodes)
                {
                    list.Add(item.Text);
                }
                frm.InNames = list.ToArray();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    车型参数.sets.Add(frm.NameValue, new CarSetting(frm.NameValue));//10-1-3UG、...
                    treeView1.SelectedNode.Nodes.Add(frm.NameValue);
                    isAlter = true;
                }
            }
            if (treeView1.SelectedNode.Level == 1)//10-1-3UG、...
            {
                FormName frm = new FormName();
                List<string> list = new List<string>();
                foreach (TreeNode item in treeView1.SelectedNode.Nodes)
                {
                    list.Add(item.Text);
                }
                frm.InNames = list.ToArray();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    if (int.TryParse(frm.NameValue, out int i))
                    {
                        GeneralSet set = new GeneralSet();
                        set.key = i;
                        车型参数.sets[treeView1.SelectedNode.Text].gSets.Add(i, set);
                        treeView1.SelectedNode.Nodes.Add(frm.NameValue);
                        isAlter = true;
                    }
                    else
                    {
                        MessageBox.Show("需要输入数字名称！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Level == 0)//根节点
            {
                //车型参数.Clear();
            }
            if (treeView1.SelectedNode.Level == 1)//10-1-3UG、...
            {
                if (车型参数.sets.Remove(treeView1.SelectedNode.Text))
                {
                    treeView1.SelectedNode.Remove();
                    isAlter = true;
                }
            }
            if (treeView1.SelectedNode.Level == 2)//1、2、3、...
            {
                if (车型参数.sets[treeView1.SelectedNode.Parent.Text].gSets.Remove(int.Parse(treeView1.SelectedNode.Text)))
                {
                    treeView1.SelectedNode.Remove();
                    isAlter = true;
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
                LinearGradientBrush brush = new LinearGradientBrush(control.ClientRectangle, AppTheme.RaisedSurface, AppTheme.Window, LinearGradientMode.BackwardDiagonal);
                g.FillPath(brush, graphicsPath);
            }
        }

        private void checkBox_isSaveImage_CheckedChanged(object sender, EventArgs e)
        {
            isAlter = true;
            otherSet.isSaveImage = checkBox_isSaveImage.Checked;
        }
        private void textBox_imagePath_TextChanged(object sender, EventArgs e)
        {
            var chars = Path.GetInvalidPathChars();
            foreach (char c in chars)
            {
                if (textBox_imagePath.Text.Contains(c))
                {
                    MessageBox.Show("路径存在非法字符" + c);
                    textBox_imagePath.Text = otherSet.imagePath;
                    return;
                }
            }
            isAlter = true;
            otherSet.imagePath = textBox_imagePath.Text;
        }
        private void CheckBox_TRnum_CheckedChanged(object sender, EventArgs e)
        {
            isAlter = true;
            otherSet.bTRnum = checkBox_TRnum.Checked;
        }

        private void button_offsetXAdd_Click(object sender, EventArgs e)
        {
            FormValue formValue = new FormValue();
            if (formValue.ShowDialog() == DialogResult.OK)
            {
                numericUpDown_offsetX.Value += (decimal)formValue.Value;
            }
        }

        private void button_offsetYAdd_Click(object sender, EventArgs e)
        {
            FormValue formValue = new FormValue();
            if (formValue.ShowDialog() == DialogResult.OK)
            {
                numericUpDown_offsetY.Value += (decimal)formValue.Value;
            }
        }

        private void button_offsetZAdd_Click(object sender, EventArgs e)
        {
            FormValue formValue = new FormValue();
            if (formValue.ShowDialog() == DialogResult.OK)
            {
                numericUpDown_offsetZ.Value += (decimal)formValue.Value;
            }
        }

    }
}
