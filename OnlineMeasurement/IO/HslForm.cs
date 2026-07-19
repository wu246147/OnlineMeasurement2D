using HslCommunication.Core;
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

namespace OnlineMeasurement.IO
{
    public partial class HslForm : Form
    {
        List<Label> diNameLabels = new List<Label>();
        List<TextBox> diAddressTextBoxs = new List<TextBox>();
        List<TextBox> diValueTextBoxs = new List<TextBox>();

        List<Label> doNameLabels = new List<Label>();
        List<TextBox> doAddressTextBoxs = new List<TextBox>();
        List<TextBox> doValueTextBoxs = new List<TextBox>();

        IHsl signal = null;
        PlcParam param => signal.Param;
        Dictionary<string, IoAddress> ioDict => signal.IoDict;

        bool isAlter = false;

        public HslForm(IHsl signal, bool DA2)
        {
            InitializeComponent();
            numericUpDownDA2.Enabled = DA2;
            comboBox_DataFormat.Items.AddRange(Enum.GetNames(typeof(DataFormat)));

            string[] diNames = Enum.GetNames(typeof(DI));
            for (int i = 0; i < diNames.Length; i++)
            {
                TextBox textBox = new TextBox();
                textBox.Tag = i;
                textBox.Size = new Size(80, 21);
                textBox.Location = new Point(labelInAddress.Location.X, (int)(labelInAddress.Location.Y - 5 + (labelInAddress.Size.Height * 2.5 * (i + 1))));
                diAddressTextBoxs.Add(textBox);
                this.Controls.Add(textBox);

                Label lbl = new Label();
                lbl.Text = diNames[i];
                lbl.Tag = i;
                lbl.Location = new Point(labelInName.Location.X, (int)(labelInName.Location.Y + (labelInName.Size.Height * 2.5 * (i + 1))));
                lbl.BackColor = Color.Transparent;
                lbl.DoubleClick += LblIn_DoubleClick;
                diNameLabels.Add(lbl);
                this.Controls.Add(lbl);

                TextBox textBoxValue = new TextBox();
                textBoxValue.Tag = i;
                textBoxValue.Size = new Size(80, 21);
                textBoxValue.Location = new Point(labelInValue.Location.X, (int)(labelInValue.Location.Y - 5 + (labelInValue.Size.Height * 2.5 * (i + 1))));
                textBoxValue.ReadOnly = true;
                diValueTextBoxs.Add(textBoxValue);
                this.Controls.Add(textBoxValue);
            }

            string[] doNames = Enum.GetNames(typeof(DO));
            for (int i = 0; i < doNames.Length; i++)
            {
                TextBox textBox = new TextBox();
                textBox.Tag = i;
                textBox.Size = new Size(80, 21);
                textBox.Location = new Point(labelOutAddress.Location.X, (int)(labelOutAddress.Location.Y - 5 + (labelOutAddress.Size.Height * 2.5 * (i + 1))));
                doAddressTextBoxs.Add(textBox);
                this.Controls.Add(textBox);

                Label lbl = new Label();
                lbl.Text = doNames[i];
                lbl.Tag = i;
                lbl.Location = new Point(labelOutName.Location.X, (int)(labelOutName.Location.Y + (labelOutName.Size.Height * 2.5 * (i + 1))));
                lbl.BackColor = Color.Transparent;
                lbl.DoubleClick += LblOut_DoubleClick;
                doNameLabels.Add(lbl);
                this.Controls.Add(lbl);

                TextBox textBoxValue = new TextBox();
                textBoxValue.Tag = i;
                textBoxValue.Size = new Size(80, 21);
                textBoxValue.Location = new Point(labelOutValue.Location.X, (int)(labelOutValue.Location.Y - 5 + (labelOutValue.Size.Height * 2.5 * (i + 1))));
                doValueTextBoxs.Add(textBoxValue);
                this.Controls.Add(textBoxValue);
            }

            this.signal = signal;
        }

        private void HslForm_Load(object sender, EventArgs e)
        {
            signal.Load();

            textBoxIpAddress.Text = param.IpAddress;
            numericUpDownPort.Value = param.Port;
            numericUpDownDA2.Value = param.DA2;
            comboBox_DataFormat.Text = param.DataFormat.ToString();
            checkBox_IsStringReverseByteWord.Checked = param.IsStringReverseByteWord;

            textBoxIpAddress.TextChanged += UpData;
            numericUpDownPort.ValueChanged += UpData;
            numericUpDownDA2.ValueChanged += UpData;
            comboBox_DataFormat.TextChanged += UpData;
            checkBox_IsStringReverseByteWord.CheckedChanged += UpData;

            for (int i = 0; i < diNameLabels.Count; i++)
            {
                if (ioDict.ContainsKey(diNameLabels[i].Text))
                {
                    diAddressTextBoxs[i].Text = ioDict[diNameLabels[i].Text].Address;
                }
                diAddressTextBoxs[i].TextChanged += UpDataDi;
            }
            for (int i = 0; i < doNameLabels.Count; i++)
            {
                if (ioDict.ContainsKey(doNameLabels[i].Text))
                {
                    doAddressTextBoxs[i].Text = ioDict[doNameLabels[i].Text].Address;
                }
                doAddressTextBoxs[i].TextChanged += UpDataDo;
            }
        }

        private void UpData(object sender, EventArgs e)
        {
            param.IpAddress = textBoxIpAddress.Text;
            param.Port = (int)numericUpDownPort.Value;
            param.DA2 = (byte)numericUpDownDA2.Value;
            param.DataFormat = (DataFormat)Enum.Parse(typeof(DataFormat), comboBox_DataFormat.Text);
            param.IsStringReverseByteWord = checkBox_IsStringReverseByteWord.Checked;
            isAlter = true;
        }
        private void UpDataDi(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                int i = (int)((TextBox)sender).Tag;
                if (ioDict.ContainsKey(diNameLabels[i].Text))
                {
                    ioDict[diNameLabels[i].Text].Address = diAddressTextBoxs[i].Text;
                }
                else
                {
                    ioDict.Add(diNameLabels[i].Text, new IoAddress() { IoName = diNameLabels[i].Text, Address = diAddressTextBoxs[i].Text });
                }
                isAlter = true;
            }
        }
        private void UpDataDo(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                int i = (int)((TextBox)sender).Tag;
                if (ioDict.ContainsKey(doNameLabels[i].Text))
                {
                    ioDict[doNameLabels[i].Text].Address = doAddressTextBoxs[i].Text;
                }
                else
                {
                    ioDict.Add(doNameLabels[i].Text, new IoAddress() { IoName = doNameLabels[i].Text, Address = doAddressTextBoxs[i].Text });
                }
                isAlter = true;
            }
        }

        private void LblIn_DoubleClick(object sender, EventArgs e)
        {
            if (signal.IsOpen)
            {
                if (sender is Label)
                {
                    Label lbl = (Label)sender;
                    int i = (int)lbl.Tag;
                    DI eIO = (DI)Enum.Parse(typeof(DI), lbl.Text);
                    if ((int)eIO < 256)
                    {
                        var result = signal.ReadBool(eIO);
                        if (result.IsSuccess)
                        {
                            diValueTextBoxs[i].Text = result.Content.ToString();
                        }
                        else
                        {
                            ShowMessage("读取失败:" + result.Message);
                        }
                    }
                    else if ((int)eIO < 512)
                    {
                        var result = signal.ReadUInt16(eIO);
                        if (result.IsSuccess)
                        {
                            diValueTextBoxs[i].Text = result.Content.ToString();
                        }
                        else
                        {
                            ShowMessage("读取失败:" + result.Message);
                        }
                    }
                    else if ((int)eIO < 768)
                    {
                        var result = signal.ReadInt16(eIO);
                        if (result.IsSuccess)
                        {
                            diValueTextBoxs[i].Text = result.Content.ToString();
                        }
                        else
                        {
                            ShowMessage("读取失败:" + result.Message);
                        }
                    }
                    else if ((int)eIO < 1024)
                    {
                        var result = signal.ReadUInt32(eIO);
                        if (result.IsSuccess)
                        {
                            diValueTextBoxs[i].Text = result.Content.ToString();
                        }
                        else
                        {
                            ShowMessage("读取失败:" + result.Message);
                        }
                    }
                    else if ((int)eIO < 1280)
                    {
                        var result = signal.ReadInt32(eIO);
                        if (result.IsSuccess)
                        {
                            diValueTextBoxs[i].Text = result.Content.ToString();
                        }
                        else
                        {
                            ShowMessage("读取失败:" + result.Message);
                        }
                    }
                    else if ((int)eIO < 1536)
                    {
                        var result = signal.ReadString(eIO,19);
                        if (result.IsSuccess)
                        {
                            diValueTextBoxs[i].Text = result.Content.ToString();
                        }
                        else
                        {
                            ShowMessage("读取失败:" + result.Message);
                        }
                    }

                }
            }
        }

        private void LblOut_DoubleClick(object sender, EventArgs e)
        {
            if (signal.IsOpen)
            {
                if (sender is Label)
                {
                    Label lbl = (Label)sender;
                    int i = (int)lbl.Tag;
                    DO eIO = (DO)Enum.Parse(typeof(DO), lbl.Text);
                    if ((int)eIO < 256)
                    {
                        if (bool.TryParse(doValueTextBoxs[i].Text, out bool value))
                        {
                            var result = signal.Write(eIO, value);
                            if (!result.IsSuccess)
                            {
                                ShowMessage("写入失败:" + result.Message);
                            }
                        }
                        else
                        {
                            ShowMessage("输入bool格式有误，转换失败！");
                        }
                    }
                    else if ((int)eIO < 512)
                    {
                        if (ushort.TryParse(doValueTextBoxs[i].Text, out ushort value))
                        {
                            var result = signal.Write(eIO, value);
                            if (!result.IsSuccess)
                            {
                                ShowMessage("写入失败:" + result.Message);
                            }
                        }
                        else
                        {
                            ShowMessage("输入uint16格式有误，转换失败！");
                        }
                    }
                    else if ((int)eIO < 768)
                    {
                        if (short.TryParse(doValueTextBoxs[i].Text, out short value))
                        {
                            var result = signal.Write(eIO, value);
                            if (!result.IsSuccess)
                            {
                                ShowMessage("写入失败:" + result.Message);
                            }
                        }
                        else
                        {
                            ShowMessage("输入int16格式有误，转换失败！");
                        }
                    }
                    else if ((int)eIO < 1024)
                    {
                        if (uint.TryParse(doValueTextBoxs[i].Text, out uint value))
                        {
                            var result = signal.Write(eIO, value);
                            if (!result.IsSuccess)
                            {
                                ShowMessage("写入失败:" + result.Message);
                            }
                        }
                        else
                        {
                            ShowMessage("输入uint32格式有误，转换失败！");
                        }
                    }
                    else if ((int)eIO < 1280)
                    {
                        if (int.TryParse(doValueTextBoxs[i].Text, out int value))
                        {
                            var result = signal.Write(eIO, value);
                            if (!result.IsSuccess)
                            {
                                ShowMessage("写入失败:" + result.Message);
                            }
                        }
                        else
                        {
                            ShowMessage("输入int32格式有误，转换失败！");
                        }
                    }
                    else if ((int)eIO < 1536)
                    {
                        var value = doValueTextBoxs[i].Text;
                        if (value.Length <= 19)
                        {
                            var result = signal.Write(eIO, value);
                            if (!result.IsSuccess)
                            {
                                ShowMessage("写入失败:" + result.Message);
                            }
                        }
                        else
                        {
                            ShowMessage("输入长度过长！");
                        }
                    }
                }
            }
        }

        private void HslForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isAlter)
            {
                DialogResult dialogResult = MessageBox.Show("是否保存参数？", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    if (!signal.Save())
                    {
                        MessageBox.Show("保存失败！！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        isAlter = false;
                    }
                }
                else if (dialogResult == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
            signal.Close();
        }

        private void button_Open_Click(object sender, EventArgs e)
        {
            if (button_Open.Enabled)//没有效果
            {
                button_Open.Enabled = false;
                if (signal.Open())
                {
                    ShowMessage("连接成功");
                    button_Close.Enabled = true;
                }
                else
                {
                    ShowMessage("连接失败");
                    button_Open.Enabled = true;
                }
            }
        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            signal.Close();
            button_Close.Enabled = false;
            button_Open.Enabled = true;
            ShowMessage("关闭成功");
        }
        void ShowMessage(string message)
        {
            textBoxLog.Text += DateTime.Now.TimeOfDay.ToString("hh\\:mm\\:ss") + "  " + message + "\r\n";
        }
        private void HslForm_Paint(object sender, PaintEventArgs e)
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
