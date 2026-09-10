using HalconDotNet;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OnlineMeasurement
{
    public partial class FormShow : Form
    {
        public bool IsShow = false;
        public bool EnableClose = false;
        public FormShow()
        {
            InitializeComponent();
        }

        private void PictureBox_DoubleClick(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            HImage hImage = Bitmap2HImage((Bitmap)pictureBox.Image);
            string text = "查看图片" + pictureBox.Name.Substring(10);
            if (hImage != null)
            {
                Thread th = new Thread(() =>
                {
                    new FormImage(hImage, text).ShowDialog();
                });
                th.IsBackground = true;
                th.Start();
            }
        }

        int _column = 0, _row = 0;
        List<PictureBox> pictureBoxs = new List<PictureBox>();
        void CreatePictureBox(int column, int row)
        {
            if (_column != column || _row != row)
            {
                _column = column;
                _row = row;

                bTableLayoutPanel_Max_Paint = false;

                tableLayoutPanel_CamImage.Controls.Clear();
                tableLayoutPanel_CamImage.RowStyles.Clear();
                tableLayoutPanel_CamImage.ColumnStyles.Clear();
                pictureBoxs.Clear();

                int ColumnCount = column * 2 - 1;
                tableLayoutPanel_CamImage.ColumnCount = ColumnCount;
                for (int c = 0; c < ColumnCount; c++)
                {
                    if (c % 2 == 0)
                    {
                        tableLayoutPanel_CamImage.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / column));
                    }
                    else
                    {
                        tableLayoutPanel_CamImage.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 6F));
                    }
                }
                int RowCount = row * 3 - 1;
                tableLayoutPanel_CamImage.RowCount = RowCount;
                for (int r = 0; r < RowCount; r++)
                {
                    if (r % 3 == 0)
                    {
                        tableLayoutPanel_CamImage.RowStyles.Add(new RowStyle(SizeType.Absolute, 19F));
                    }
                    else if (r % 3 == 1)
                    {
                        tableLayoutPanel_CamImage.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / row));
                    }
                    else
                    {
                        tableLayoutPanel_CamImage.RowStyles.Add(new RowStyle(SizeType.Absolute, 6F));
                    }
                }

                //添加竖边框
                for (int c = 0; c < ColumnCount; c++)
                {
                    if (c % 2 != 0)
                    {
                        Panel panel = new Panel();
                        panel.Dock = DockStyle.Fill;
                        panel.BackColor = Color.Transparent;
                        panel.Margin = new Padding(0);
                        panel.TabStop = false;
                        panel.Paint += panel_bian_Paint;
                        tableLayoutPanel_CamImage.Controls.Add(panel, c, 0);
                        tableLayoutPanel_CamImage.SetRowSpan(panel, RowCount);
                    }
                }

                for (int r = 0; r < RowCount; r++)
                {
                    for (int c = 0; c < ColumnCount; c++)
                    {

                        if (r % 3 == 0)//序号
                        {
                            if (c % 2 == 0)
                            {
                                Label label = new Label();
                                label.AutoSize = true;
                                label.BackColor = Color.Transparent;
                                label.Font = new Font("微软雅黑", 10.5F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
                                label.ForeColor = Color.White;
                                label.Text = (column * r / 3 + c / 2 + 1).ToString();
                                label.TabStop = false;
                                tableLayoutPanel_CamImage.Controls.Add(label, c, r);
                            }
                        }
                        else if (r % 3 == 1)//图片
                        {
                            if (c % 2 == 0)
                            {
                                PictureBox pictureBox = new PictureBox();
                                pictureBox.Dock = DockStyle.Fill;
                                pictureBox.BackColor = Color.Transparent;
                                pictureBox.Name = "pictureBox" + (column * r / 3 + c / 2 + 1);
                                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                                pictureBox.TabStop = false;
                                pictureBox.DoubleClick += PictureBox_DoubleClick;
                                tableLayoutPanel_CamImage.Controls.Add(pictureBox, c, r);
                                pictureBoxs.Add(pictureBox);
                            }
                        }
                        else//边框
                        {
                            if (c % 2 == 0)
                            {
                                Panel panel = new Panel();
                                panel.Dock = DockStyle.Fill;
                                panel.BackColor = Color.Transparent;
                                panel.Margin = new Padding(0);
                                panel.TabStop = false;
                                panel.Paint += panel_bian_Paint;
                                tableLayoutPanel_CamImage.Controls.Add(panel, c, r);
                            }
                        }
                    }
                }

                bTableLayoutPanel_Max_Paint = true;
            }
        }

        bool bTableLayoutPanel_Max_Paint = true;
        private void tableLayoutPanel_Max_Paint(object sender, PaintEventArgs e)
        {
            if (bTableLayoutPanel_Max_Paint)
            {
                try
                {
                    Control control = (Control)sender;
                    Graphics g = e.Graphics;
                    g.SmoothingMode = SmoothingMode.AntiAlias;//抗锯齿
                    using (GraphicsPath graphicsPath = new GraphicsPath())
                    {
                        Rectangle rectangle = new Rectangle(control.ClientRectangle.Left, control.ClientRectangle.Top + 56, control.ClientRectangle.Width, control.ClientRectangle.Height - 56);
                        graphicsPath.AddEllipse(rectangle);
                        using (PathGradientBrush pathGradientBrush = new PathGradientBrush(graphicsPath))
                        {
                            pathGradientBrush.CenterColor = Color.FromArgb(0, 100, 251);
                            pathGradientBrush.CenterPoint = new PointF(control.Width / 2, control.Height / 2);
                            pathGradientBrush.SurroundColors = new Color[] { control.BackColor };
                            g.FillPath(pathGradientBrush, graphicsPath);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
            }
        }

        private void FormShow_SizeChanged(object sender, EventArgs e)
        {
            Refresh();
        }

        private void panel_Title_Paint(object sender, PaintEventArgs e)
        {
            Control control = (Control)sender;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;//抗锯齿
            float f = 3;

            using (GraphicsPath graphicsPath = new GraphicsPath())
            {
                graphicsPath.AddLines(new PointF[] { new PointF(0, 0), new PointF(control.Width * 0.05f, control.Height), new PointF(control.Width * 0.95f, control.Height), new PointF(control.Width, 0) });
                graphicsPath.AddLines(new PointF[] { new PointF(f, 0), new PointF(control.Width * 0.05f + f, control.Height - f), new PointF(control.Width * 0.95f - f, control.Height - f), new PointF(control.Width - f, 0) });
                using (LinearGradientBrush brush = new LinearGradientBrush(new PointF(0, 0), new PointF(f, 0), Color.FromArgb(0, 77, 191), control.BackColor))
                {
                    g.FillPath(brush, graphicsPath);
                }
            }

            using (GraphicsPath graphicsPath2 = new GraphicsPath())
            {
                graphicsPath2.AddLines(new PointF[] { new PointF(control.Width * 0.1f, 0), new PointF(control.Width * 0.05f, control.Height - f), new PointF(control.Width * 0.95f, control.Height - f), new PointF(control.Width * 0.9f, 0) });
                using (LinearGradientBrush brush2 = new LinearGradientBrush(new PointF(0, control.Height * 0.8f), new PointF(0, 0), Color.FromArgb(0, 77, 191), control.BackColor))
                {
                    brush2.WrapMode = WrapMode.TileFlipX;
                    g.FillPath(brush2, graphicsPath2);
                }
            }

            //GC.Collect();
        }
        bool isMove = true;
        private void panel10_MouseDown(object sender, MouseEventArgs e)
        {
            if (isMove)
            {
                FormTool.MouseDown_MoveForm(this);
            }
        }
        private void FormShow_MouseDown(object sender, MouseEventArgs e)
        {
            if (isMove)
            {
                FormTool.MouseDown_ResizeForm(this);
            }
        }
        private void ResizeForm(object sender, MouseEventArgs e)
        {
            if (isMove)
            {
                Control control = (Control)sender;
                //MouseEventArgs mouseEventArgs = new MouseEventArgs(e.Button, e.Clicks, control.Location.X + e.X, control.Location.Y + e.Y, e.Delta);
                //FormTool.MouseMove(this, mouseEventArgs);

                if (int.TryParse(control.Tag.ToString(), out int n))
                {
                    FormTool.formResizeMode = 0xf000 + n;
                }
            }
        }
        private void MinimizeBox_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        public void MaximizeBox_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                SetFormWindowState(FormWindowState.Maximized);
            }
            else if (WindowState == FormWindowState.Maximized)
            {
                SetFormWindowState(FormWindowState.Normal);
            }
        }

        public void SetFormWindowState(FormWindowState state)
        {
            if (state == FormWindowState.Normal)
            {
                label_MaximizeBox.Text = "□";
                isMove = true;
                最大化ToolStripMenuItem.Text = "最大化";
            }
            else if (state == FormWindowState.Maximized)
            {
                label_MaximizeBox.Text = "❐";
                isMove = false;
                最大化ToolStripMenuItem.Text = "还原";
            }
            WindowState = state;
        }

        private void 最小化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MinimizeBox_Click(null, null);
        }

        private void 最大化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MaximizeBox_Click(null, null);
        }
        private void 切换屏幕ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string DeviceName = Screen.FromControl(this).DeviceName;
            int index = 0;
            for (int i = 0; i < Screen.AllScreens.Length; i++)
            {
                if (Screen.AllScreens[i].DeviceName == DeviceName)
                {
                    index = i;
                    break;
                }
            }
            index += 1;
            if (index >= Screen.AllScreens.Length)
            {
                index = 0;
            }
            SetFormWindowState(FormWindowState.Normal);
            Left = Screen.AllScreens[index].Bounds.Left;
            Top = Screen.AllScreens[index].Bounds.Top;
            SetFormWindowState(FormWindowState.Maximized);
        }

        private void panel10_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MaximizeBox_Click(null, null);
            }
        }
        private void CloseBox_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MinimizeBox_MouseEnter(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            Color color = Color.FromArgb(130, 180, 255);
            if (control is Label)
            {
                control.Parent.BackColor = color;
            }
            else
            {
                control.BackColor = color;
            }
        }

        private void MinimizeBox_MouseLeave(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            Color color = Color.Transparent;
            if (control is Label)
            {
                control.Parent.BackColor = color;
            }
            else
            {
                control.BackColor = color;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            //panel1.Refresh();
            BeginInvoke(new Action(() =>
            {
                label_time.Text = DateTime.Now.ToString("G");
            }));
        }
        private void panel_Top_Paint(object sender, PaintEventArgs e)
        {
            Control control = (Control)sender;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;//抗锯齿

            using (GraphicsPath graphicsPath = new GraphicsPath())
            {
                graphicsPath.AddRectangle(control.ClientRectangle);
                using (LinearGradientBrush brush = new LinearGradientBrush(control.ClientRectangle, control.BackColor, Color.FromArgb(0, 50, 125), LinearGradientMode.BackwardDiagonal))
                {
                    g.FillPath(brush, graphicsPath);
                }
            }

            using (GraphicsPath graphicsPath2 = new GraphicsPath())
            {
                float f = 15;
                float x_1 = panel_MinimizeBox.Location.X + control.Height;
                float x0 = panel_MinimizeBox.Location.X;
                float x1 = panel_MinimizeBox.Location.X - control.Height;
                float x2 = panel_MinimizeBox.Location.X - control.Height * 2;
                graphicsPath2.StartFigure();
                graphicsPath2.AddCurve(new PointF[] { new PointF(x2 - f, control.Height), new PointF(x1 - f, control.Height), new PointF(x0 - f, -1), new PointF(x_1 - f, -1) });
                graphicsPath2.AddCurve(new PointF[] { new PointF(x_1, -1), new PointF(x0, -1), new PointF(x1, control.Height), new PointF(x2, control.Height) });
                graphicsPath2.CloseFigure();
                using (LinearGradientBrush brush2 = new LinearGradientBrush(new PointF(0, 0), new PointF(f, 0), Color.FromArgb(0, 50, 125), Color.FromArgb(0, 50, 125)))
                {
                    g.FillPath(brush2, graphicsPath2);
                }
            }

            //g.Dispose();
            //GC.Collect();
        }

        public Dictionary<string, ShowCarXYZ> ShowCarXYZs = new Dictionary<string, ShowCarXYZ>();
        public Dictionary<string, Image> CarImages = new Dictionary<string, Image>();
        private void FormShow_Load(object sender, EventArgs e)
        {
            IsShow = true;
            try
            {
                ShowCarXYZs.Clear();
                string[] cars = Directory.GetFiles("数模图", "*.xml");
                foreach (string car in cars)
                {
                    ShowCarXYZ showCarXYZ = new ShowCarXYZ();
                    if (showCarXYZ.Load(car))
                    {
                        ShowCarXYZs.Add(Path.GetFileNameWithoutExtension(car), showCarXYZ);
                    }
                    //showCarXYZ.Save(car);
                }

                CarImages.Clear();
                string[] imagePaths = Directory.GetFiles("数模图", "*.png");
                foreach (string path in imagePaths)
                {
                    Stream stream = File.OpenRead(path);
                    byte[] bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, bytes.Length);
                    Stream stream1 = new MemoryStream(bytes);
                    CarImages.Add(Path.GetFileNameWithoutExtension(path), (Image)Bitmap.FromStream(stream1));
                    stream.Close();
                    stream.Dispose();
                }

                ////透明处理
                //Bitmap image = new Bitmap("530.png");
                //for (int h = 0; h < image.Height; h++)
                //{
                //    for (int w = 0; w < image.Width; w++)
                //    {
                //        Color color = image.GetPixel(w, h);
                //        if (color.R == 0 && color.G == 0 && color.B == 255)
                //        {
                //            image.SetPixel(w, h, Color.FromArgb(0, 0, 0, 0));
                //        }
                //    }
                //}
                //image.Save("CRD.png", ImageFormat.Png);
                //pictureBox11.Image = image;
            }
            catch (Exception ex)
            {
                MessageBox.Show("数模加载失败：" + ex.Message);
            }
        }

        private void FormShow_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsShow = false;
        }

        private void FormShow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!EnableClose)
            {
                e.Cancel = true;
            }
        }

        string CarNameNow = string.Empty;
        string CarNumNow = string.Empty;
        string TRNumNow = string.Empty;
        public void UpDataCarInform(string carName, string carNum, string TRNum, int count)
        {
            CarNameNow = carName;
            CarNumNow = carNum;
            TRNumNow = TRNum;
            BeginInvoke(new Action(() =>
            {
                try
                {
                    label_车型.Text = "车型：" + CarNameNow;
                    label_连番.Text = "连番：" + CarNumNow;
                    label_托盘.Text = "托盘：" + TRNumNow;
                    if (pictureBox11.Image != null)
                    {
                        pictureBox11.Image.Dispose();
                    }
                    if (CarImages.ContainsKey(CarNameNow))
                    {
                        pictureBox11.Image = (Image)CarImages[CarNameNow].Clone();
                    }
                    else
                    {
                        pictureBox11.Image = null;
                    }

                    int n = 1;
                    while (count > n * (n + 3)) { n++; }
                    CreatePictureBox(n, (int)Math.Ceiling(count / (double)n));
                }
                catch { }
            }));
        }
        public void ResetCarImage()
        {
            BeginInvoke(new Action(() =>
            {
                try
                {
                    if (pictureBox11.Image != null)
                    {
                        pictureBox11.Image.Dispose();
                    }
                    if (CarImages.ContainsKey(CarNameNow))
                    {
                        pictureBox11.Image = (Image)CarImages[CarNameNow].Clone();
                    }
                    else
                    {
                        pictureBox11.Image = null;
                    }
                }
                catch { }
            }));
        }
        public void UpDataCamImage(HImage hImage, string pointName)
        {
            HImage _hImage = hImage != null ? hImage.Clone() : null;
            BeginInvoke(new Action(() =>
            {
                try
                {
                    if (pointName == "Clear")
                    {
                        foreach (var item in pictureBoxs)
                        {
                            item.Image = null;
                        }
                        return;
                    }
                    if (_hImage == null) return;
                    Bitmap image = HImage2Bitmap(_hImage);
                    _hImage?.Dispose();
                    if (int.TryParse(pointName, out int num))
                    {
                        if (num - 1 < pictureBoxs.Count)
                        {
                            pictureBoxs[num - 1].Image = image;
                        }
                    }
                }
                catch { }
            }));
        }
        public void UpDataXYZ(Point3D point3D, Point3DColor point3DColor, string pointKey, string pointName = "")
        {
            BeginInvoke(new Action(() =>
            {
                try
                {
                    //GC.Collect();
                    //GC.WaitForPendingFinalizers();
                    if (pictureBox11.Image == null) return;
                    if (!ShowCarXYZs.ContainsKey(CarNameNow) || !ShowCarXYZs[CarNameNow].Points.ContainsKey(pointKey)) return;
                    Image image = (Image)pictureBox11.Image.Clone();
                    using (Graphics g = Graphics.FromImage(image))
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;//抗锯齿
                        Rectangle rectangle = new Rectangle(ShowCarXYZs[CarNameNow].Points[pointKey].Location.X, ShowCarXYZs[CarNameNow].Points[pointKey].Location.Y, 120, 96);
                        //背景
                        g.FillRectangle(new SolidBrush(Color.LightBlue), rectangle);
                        PointF connection = new PointF();
                        switch (ShowCarXYZs[CarNameNow].Points[pointKey].Connection)
                        {
                            case 1:
                                connection.X = rectangle.Left;
                                connection.Y = rectangle.Top;
                                break;
                            case 2:
                                connection.X = (rectangle.Left + rectangle.Right) / 2;
                                connection.Y = rectangle.Top;
                                break;
                            case 3:
                                connection.X = rectangle.Right;
                                connection.Y = rectangle.Top;
                                break;
                            case 4:
                                connection.X = rectangle.Left;
                                connection.Y = (rectangle.Top + rectangle.Bottom) / 2;
                                break;
                            case 5:
                                connection.X = rectangle.Right;
                                connection.Y = (rectangle.Top + rectangle.Bottom) / 2;
                                break;
                            case 6:
                                connection.X = rectangle.Left;
                                connection.Y = rectangle.Bottom;
                                break;
                            case 7:
                                connection.X = (rectangle.Left + rectangle.Right) / 2;
                                connection.Y = rectangle.Bottom;
                                break;
                            case 8:
                                connection.X = rectangle.Right;
                                connection.Y = rectangle.Bottom;
                                break;
                            default:
                                connection.X = (rectangle.Left + rectangle.Right) / 2;
                                connection.Y = (rectangle.Top + rectangle.Bottom) / 2;
                                break;
                        }
                        //连接线
                        g.DrawLine(new Pen(Color.LightGreen), ShowCarXYZs[CarNameNow].Points[pointKey].Point, connection);
                        //边框
                        g.DrawRectangle(new Pen(Color.Green), rectangle);
                        //文字
                        //g.DrawString($"X:{point3D.X:0.00}\nY:{point3D.Y:0.00}\nZ:{point3D.Z:0.00}", new Font(new FontFamily("Arial"), 16, FontStyle.Italic), new SolidBrush(Color.Black), rectangle, StringFormat.GenericDefault);
                        int 行数 = 4;
                        int 减径 = 2;
                        int WH = rectangle.Height / 行数 - 减径 * 2;
                        int Left = rectangle.Right - rectangle.Height / 行数 - 减径;
                        Rectangle[] rectangles = new Rectangle[行数];
                        for (int i = 0; i < rectangles.Length; i++)
                        {
                            rectangles[i] = new Rectangle(Left, rectangle.Top + rectangle.Height * i / 行数 + 减径, WH, WH);
                        }
                        g.FillEllipse(new SolidBrush(point3DColor.ColorX), rectangles[1]);
                        g.FillEllipse(new SolidBrush(point3DColor.ColorY), rectangles[2]);
                        g.FillEllipse(new SolidBrush(point3DColor.ColorZ), rectangles[3]);

                        g.DrawString($"{pointKey}:{pointName}", new Font(new FontFamily("微软雅黑"), 16, FontStyle.Underline), new SolidBrush(Color.Black), rectangle.Left, rectangles[0].Top - 4, StringFormat.GenericDefault);
                        g.DrawString($"X:{point3D.X:0.00}", new Font(new FontFamily("微软雅黑"), 16, FontStyle.Italic), new SolidBrush(Color.Black), rectangle.Left, rectangles[1].Top - 4, StringFormat.GenericDefault);
                        g.DrawString($"Y:{point3D.Y:0.00}", new Font(new FontFamily("微软雅黑"), 16, FontStyle.Italic), new SolidBrush(Color.Black), rectangle.Left, rectangles[2].Top - 4, StringFormat.GenericDefault);
                        g.DrawString($"Z:{point3D.Z:0.00}", new Font(new FontFamily("微软雅黑"), 16, FontStyle.Italic), new SolidBrush(Color.Black), rectangle.Left, rectangles[3].Top - 4, StringFormat.GenericDefault);
                    }

                    if (pictureBox11.Image != null)
                    {
                        pictureBox11.Image.Dispose();
                    }
                    pictureBox11.Image = image;
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }));
        }
        public void UpDataXYZ新(Point3D point3D, Point3DColor point3DColor, string pointKey)
        {
            BeginInvoke(new Action(() =>
            {
                try
                {
                    if (pictureBox11.Image == null) return;
                    if (!ShowCarXYZs.ContainsKey(CarNameNow) || !ShowCarXYZs[CarNameNow].Points.ContainsKey(pointKey)) return;
                    Image image = (Image)pictureBox11.Image.Clone();
                    using (Graphics g = Graphics.FromImage(image))
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;//抗锯齿
                        Rectangle rectangle = new Rectangle(ShowCarXYZs[CarNameNow].Points[pointKey].Location.X + 120, ShowCarXYZs[CarNameNow].Points[pointKey].Location.Y, 120, 96);
                        //背景
                        g.FillRectangle(new SolidBrush(Color.LightBlue), rectangle);
                        //边框
                        g.DrawRectangle(new Pen(Color.Green), rectangle);

                        int 行数 = 4;
                        int 减径 = 2;
                        int WH = rectangle.Height / 行数 - 减径 * 2;
                        int Left = rectangle.Right - rectangle.Height / 行数 - 减径;
                        Rectangle[] rectangles = new Rectangle[行数];
                        for (int i = 0; i < rectangles.Length; i++)
                        {
                            rectangles[i] = new Rectangle(Left, rectangle.Top + rectangle.Height * i / 行数 + 减径, WH, WH);
                        }
                        g.FillEllipse(new SolidBrush(point3DColor.ColorX), rectangles[1]);
                        g.FillEllipse(new SolidBrush(point3DColor.ColorY), rectangles[2]);
                        g.FillEllipse(new SolidBrush(point3DColor.ColorZ), rectangles[3]);

                        g.DrawString($"X:{point3D.X:0.00}", new Font(new FontFamily("微软雅黑"), 16, FontStyle.Italic), new SolidBrush(Color.Black), rectangle.Left, rectangles[1].Top - 4, StringFormat.GenericDefault);
                        g.DrawString($"Y:{point3D.Y:0.00}", new Font(new FontFamily("微软雅黑"), 16, FontStyle.Italic), new SolidBrush(Color.Black), rectangle.Left, rectangles[2].Top - 4, StringFormat.GenericDefault);
                        g.DrawString($"Z:{point3D.Z:0.00}", new Font(new FontFamily("微软雅黑"), 16, FontStyle.Italic), new SolidBrush(Color.Black), rectangle.Left, rectangles[3].Top - 4, StringFormat.GenericDefault);
                    }

                    if (pictureBox11.Image != null)
                    {
                        pictureBox11.Image.Dispose();
                    }
                    pictureBox11.Image = image;
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }));
        }
        public void UpDataXYZ2(Point3D point3D, Point3DColor point3DColor, Point3D point3D新, Point3DColor point3DColor新, string pointKey, string pointName = "")
        {
            BeginInvoke(new Action(() =>
            {
                try
                {
                    //GC.Collect();
                    //GC.WaitForPendingFinalizers();
                    if (pictureBox11.Image == null) return;
                    if (!ShowCarXYZs.ContainsKey(CarNameNow) || !ShowCarXYZs[CarNameNow].Points.ContainsKey(pointKey)) return;
                    Image image = (Image)pictureBox11.Image.Clone();
                    using (Graphics g = Graphics.FromImage(image))
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;//抗锯齿
                        Rectangle rectangle = new Rectangle(ShowCarXYZs[CarNameNow].Points[pointKey].Location.X, ShowCarXYZs[CarNameNow].Points[pointKey].Location.Y, 192, 96);
                        //背景
                        g.FillRectangle(new SolidBrush(Color.LightBlue), rectangle);
                        PointF connection = new PointF();
                        switch (ShowCarXYZs[CarNameNow].Points[pointKey].Connection)
                        {
                            case 1:
                                connection.X = rectangle.Left;
                                connection.Y = rectangle.Top;
                                break;
                            case 2:
                                connection.X = (rectangle.Left + rectangle.Right) / 2;
                                connection.Y = rectangle.Top;
                                break;
                            case 3:
                                connection.X = rectangle.Right;
                                connection.Y = rectangle.Top;
                                break;
                            case 4:
                                connection.X = rectangle.Left;
                                connection.Y = (rectangle.Top + rectangle.Bottom) / 2;
                                break;
                            case 5:
                                connection.X = rectangle.Right;
                                connection.Y = (rectangle.Top + rectangle.Bottom) / 2;
                                break;
                            case 6:
                                connection.X = rectangle.Left;
                                connection.Y = rectangle.Bottom;
                                break;
                            case 7:
                                connection.X = (rectangle.Left + rectangle.Right) / 2;
                                connection.Y = rectangle.Bottom;
                                break;
                            case 8:
                                connection.X = rectangle.Right;
                                connection.Y = rectangle.Bottom;
                                break;
                            default:
                                connection.X = (rectangle.Left + rectangle.Right) / 2;
                                connection.Y = (rectangle.Top + rectangle.Bottom) / 2;
                                break;
                        }
                        //连接线
                        g.DrawLine(new Pen(Color.LightGreen), ShowCarXYZs[CarNameNow].Points[pointKey].Point, connection);
                        //边框
                        g.DrawRectangle(new Pen(Color.Green), rectangle);

                        int 行数 = 4;
                        int 减径 = 2;
                        int WH = rectangle.Height / 行数 - 减径 * 2;
                        int Left = rectangle.Right - rectangle.Width / 2 - rectangle.Height / 行数 - 减径;
                        int Left新 = rectangle.Right - rectangle.Height / 行数 - 减径;
                        Rectangle[] rectangles = new Rectangle[行数];
                        Rectangle[] rectangles新 = new Rectangle[行数];
                        for (int i = 0; i < rectangles.Length; i++)
                        {
                            rectangles[i] = new Rectangle(Left - 4, rectangle.Top + rectangle.Height * i / 行数 + 减径, WH, WH);
                            rectangles新[i] = new Rectangle(Left新, rectangle.Top + rectangle.Height * i / 行数 + 减径, WH, WH);
                        }
                        //灯
                        g.FillEllipse(new SolidBrush(point3DColor.ColorX), rectangles[1]);
                        g.FillEllipse(new SolidBrush(point3DColor.ColorY), rectangles[2]);
                        g.FillEllipse(new SolidBrush(point3DColor.ColorZ), rectangles[3]);
                        g.FillEllipse(new SolidBrush(point3DColor新.ColorX), rectangles新[1]);
                        g.FillEllipse(new SolidBrush(point3DColor新.ColorY), rectangles新[2]);
                        g.FillEllipse(new SolidBrush(point3DColor新.ColorZ), rectangles新[3]);
                        //文字
                        g.DrawString($"{pointKey}:{pointName}", new Font(new FontFamily("微软雅黑"), 12, FontStyle.Underline), new SolidBrush(Color.Black), rectangle.Left, rectangles[0].Top - 1, StringFormat.GenericDefault);
                        g.DrawString($"X:{point3D.X:0.00}", new Font(new FontFamily("微软雅黑"), 12, FontStyle.Italic), new SolidBrush(Color.Black), rectangle.Left, rectangles[1].Top - 1, StringFormat.GenericDefault);
                        g.DrawString($"Y:{point3D.Y:0.00}", new Font(new FontFamily("微软雅黑"), 12, FontStyle.Italic), new SolidBrush(Color.Black), rectangle.Left, rectangles[2].Top - 1, StringFormat.GenericDefault);
                        g.DrawString($"Z:{point3D.Z:0.00}", new Font(new FontFamily("微软雅黑"), 12, FontStyle.Italic), new SolidBrush(Color.Black), rectangle.Left, rectangles[3].Top - 1, StringFormat.GenericDefault);
                        g.DrawString($"nX:{point3D新.X:0.00}", new Font(new FontFamily("微软雅黑"), 12, FontStyle.Italic), new SolidBrush(Color.Black), rectangle.Left + rectangle.Width / 2, rectangles新[1].Top - 1, StringFormat.GenericDefault);
                        g.DrawString($"nY:{point3D新.Y:0.00}", new Font(new FontFamily("微软雅黑"), 12, FontStyle.Italic), new SolidBrush(Color.Black), rectangle.Left + rectangle.Width / 2, rectangles新[2].Top - 1, StringFormat.GenericDefault);
                        g.DrawString($"nZ:{point3D新.Z:0.00}", new Font(new FontFamily("微软雅黑"), 12, FontStyle.Italic), new SolidBrush(Color.Black), rectangle.Left + rectangle.Width / 2, rectangles新[3].Top - 1, StringFormat.GenericDefault);
                    }

                    if (pictureBox11.Image != null)
                    {
                        pictureBox11.Image.Dispose();
                    }
                    pictureBox11.Image = image;
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }));
        }
        public void UpDataMessage(string Message, string pointKey, string pointName = "", bool bRed = true)
        {
            BeginInvoke(new Action(() =>
            {
                try
                {
                    //GC.Collect();
                    //GC.WaitForPendingFinalizers();
                    if (pictureBox11.Image == null) return;
                    if (!ShowCarXYZs.ContainsKey(CarNameNow) || !ShowCarXYZs[CarNameNow].Points.ContainsKey(pointKey)) return;
                    Image image = (Image)pictureBox11.Image.Clone();
                    using (Graphics g = Graphics.FromImage(image))
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;//抗锯齿
                        Rectangle rectangle = new Rectangle(ShowCarXYZs[CarNameNow].Points[pointKey].Location.X, ShowCarXYZs[CarNameNow].Points[pointKey].Location.Y, 120, 96);
                        //背景
                        g.FillRectangle(new SolidBrush(Color.LightBlue), rectangle);
                        PointF connection = new PointF();
                        switch (ShowCarXYZs[CarNameNow].Points[pointKey].Connection)
                        {
                            case 1:
                                connection.X = rectangle.Left;
                                connection.Y = rectangle.Top;
                                break;
                            case 2:
                                connection.X = (rectangle.Left + rectangle.Right) / 2;
                                connection.Y = rectangle.Top;
                                break;
                            case 3:
                                connection.X = rectangle.Right;
                                connection.Y = rectangle.Top;
                                break;
                            case 4:
                                connection.X = rectangle.Left;
                                connection.Y = (rectangle.Top + rectangle.Bottom) / 2;
                                break;
                            case 5:
                                connection.X = rectangle.Right;
                                connection.Y = (rectangle.Top + rectangle.Bottom) / 2;
                                break;
                            case 6:
                                connection.X = rectangle.Left;
                                connection.Y = rectangle.Bottom;
                                break;
                            case 7:
                                connection.X = (rectangle.Left + rectangle.Right) / 2;
                                connection.Y = rectangle.Bottom;
                                break;
                            case 8:
                                connection.X = rectangle.Right;
                                connection.Y = rectangle.Bottom;
                                break;
                            default:
                                connection.X = (rectangle.Left + rectangle.Right) / 2;
                                connection.Y = (rectangle.Top + rectangle.Bottom) / 2;
                                break;
                        }
                        //连接线
                        g.DrawLine(new Pen(Color.LightGreen), ShowCarXYZs[CarNameNow].Points[pointKey].Point, connection);
                        //边框
                        g.DrawRectangle(new Pen(bRed ? Color.Red : Color.Yellow), rectangle);
                        //文字
                        //g.DrawString($"X:{point3D.X:0.00}\nY:{point3D.Y:0.00}\nZ:{point3D.Z:0.00}", new Font(new FontFamily("Arial"), 16, FontStyle.Italic), new SolidBrush(Color.Black), rectangle, StringFormat.GenericDefault);
                        int 行数 = 4;
                        int 减径 = 2;
                        int WH = rectangle.Height / 行数 - 减径 * 2;
                        int Left = rectangle.Right - rectangle.Height / 行数 - 减径;
                        Rectangle[] rectangles = new Rectangle[行数];
                        for (int i = 0; i < rectangles.Length; i++)
                        {
                            rectangles[i] = new Rectangle(Left, rectangle.Top + rectangle.Height * i / 行数 + 减径, WH, WH);
                        }

                        g.DrawString($"{pointKey}:{pointName}", new Font(new FontFamily("微软雅黑"), 16, FontStyle.Underline), new SolidBrush(Color.Black), rectangle.Left, rectangles[0].Top - 4, StringFormat.GenericDefault);
                        //g.DrawString($"X:{point3D.X:0.00}", new Font(new FontFamily("微软雅黑"), 16, FontStyle.Italic), new SolidBrush(Color.Black), rectangle.Left, rectangles[1].Top - 4, StringFormat.GenericDefault);
                        g.DrawString($" {Message}", new Font(new FontFamily("微软雅黑"), 12, FontStyle.Regular), new SolidBrush(bRed ? Color.Red : Color.Black), rectangle.Left, rectangles[2].Top - 4, StringFormat.GenericDefault);
                        //g.DrawString($"Z:{point3D.Z:0.00}", new Font(new FontFamily("微软雅黑"), 16, FontStyle.Italic), new SolidBrush(Color.Black), rectangle.Left, rectangles[3].Top - 4, StringFormat.GenericDefault);
                    }

                    if (pictureBox11.Image != null)
                    {
                        pictureBox11.Image.Dispose();
                    }
                    pictureBox11.Image = image;
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }));
        }

        private Bitmap HImage2Bitmap(HImage image)
        {
            IntPtr r, g, b;
            HTuple conut = image.CountChannels();
            g = image.GetImagePointer1(out string type, out int w, out int h);
            r = g;
            b = g;
            if (conut.D > 1)
            {
                // 获取存放r，g，b值的指针
                image.GetImagePointer3(out r, out g, out b, out type, out w, out h);
            }

            byte[] red = new byte[w * h];
            byte[] green = new byte[w * h];
            byte[] blue = new byte[w * h];
            // 将指针指向地址的值取出来放到byte数组中
            Marshal.Copy(r, red, 0, w * h);
            Marshal.Copy(g, green, 0, w * h);
            Marshal.Copy(b, blue, 0, w * h);

            Bitmap bitmap2 = new Bitmap(w, h, PixelFormat.Format32bppRgb);
            Rectangle rect2 = new Rectangle(0, 0, w, h);
            BitmapData bitmapData2 = bitmap2.LockBits(rect2, ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);
            unsafe
            {
                byte* bptr2 = (byte*)bitmapData2.Scan0;
                for (int i = 0; i < w * h; i++)
                {
                    bptr2[i * 4] = blue[i];
                    bptr2[i * 4 + 1] = green[i];
                    bptr2[i * 4 + 2] = red[i];
                    bptr2[i * 4 + 3] = 255;
                }
            }
            bitmap2.UnlockBits(bitmapData2);
            return bitmap2;
        }
        private HImage Bitmap2HImage(Bitmap image)
        {
            if (image == null)
            {
                return null;
            }
            try
            {
                Rectangle rectangle = new Rectangle(0, 0, image.Width, image.Height);
                BitmapData bitmapData = image.LockBits(rectangle, ImageLockMode.ReadOnly, image.PixelFormat);
                HImage hImage = new HImage();
                hImage.GenImageInterleaved(bitmapData.Scan0, "bgrx", image.Width, image.Height, 0, "byte", 0, 0, 0, 0, -1, 0);
                image.UnlockBits(bitmapData);
                return hImage;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void panel_bian_Paint(object sender, PaintEventArgs e)
        {
            Control control = (Control)sender;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;//抗锯齿
            using (GraphicsPath graphicsPath = new GraphicsPath())
            {
                graphicsPath.AddRectangle(control.ClientRectangle);
                using (PathGradientBrush pathGradientBrush = new PathGradientBrush(graphicsPath))
                {
                    pathGradientBrush.CenterColor = Color.FromArgb(0, 100, 251);
                    pathGradientBrush.CenterPoint = new PointF(control.Width / 2, control.Height / 2);
                    pathGradientBrush.SurroundColors = new Color[] { control.BackColor };
                    g.FillPath(pathGradientBrush, graphicsPath);
                }
            }
        }

    }
}
