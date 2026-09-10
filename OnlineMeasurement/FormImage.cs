using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace OnlineMeasurement
{
    public partial class FormImage : Form
    {
        HTuple hv_WindowHandle = null;
        HObject image = null;
        public FormImage(HObject image,string text = "查看图片")
        {
            InitializeComponent();
            HOperatorSet.SetWindowAttr("background_color", "black");
            HOperatorSet.OpenWindow(-1, -1, hWindowControl1.Width - 4, hWindowControl1.Height - 4, hWindowControl1.HalconWindow, "", "", out hv_WindowHandle);
            this.Text = text;
            this.image = image.Clone();
            DispImage(this.image);
        }

        public void DispObj(HObject hObject)
        {
            if (hObject == null)
            {
                HOperatorSet.ClearWindow(hv_WindowHandle);
                return;
            }
            HOperatorSet.DispObj(hObject, hv_WindowHandle);
        }
        public void DispImage(HObject image)
        {
            if (image == null)
            {
                HOperatorSet.ClearWindow(hv_WindowHandle);
                return;
            }
            if (image is HImage)
            {
                HImage img = (HImage)image;
                int w, h;
                img.GetImageSize(out w, out h);
                HTuple row, column, width, height;
                HOperatorSet.GetWindowExtents(hv_WindowHandle, out row, out column, out width, out height);

                double wndRatio = (double)width / (double)height;
                double imgRatio = (double)w / (double)h;
                int beginRow, beginCol, endRow, endCol;
                if (wndRatio > imgRatio)
                {
                    beginRow = 0;
                    endRow = h;
                    beginCol = (int)((w - h * wndRatio) / 2d);
                    endCol = (int)(w - (w - h * wndRatio) / 2d);
                }
                else
                {
                    beginRow = (int)(-(w / wndRatio - h) / 2d);
                    endRow = (int)(h + (w / wndRatio - h) / 2d);
                    beginCol = 0;
                    endCol = w;
                }

                HOperatorSet.SetPart(hv_WindowHandle, beginRow, beginCol, endRow, endCol);
            }
            else
            {
                HOperatorSet.GetImageSize(image, out HTuple imgW, out HTuple imgH);
                double w = imgW.D, h = imgH.D;
                HOperatorSet.GetWindowExtents(hv_WindowHandle, out HTuple row, out HTuple column, out HTuple width, out HTuple height);

                double wndRatio = width.D / height.D;
                double imgRatio = w / h;
                int beginRow, beginCol, endRow, endCol;
                if (wndRatio > imgRatio)
                {
                    beginRow = 0;
                    endRow = (int)h;
                    beginCol = (int)((w - h * wndRatio) / 2d);
                    endCol = (int)(w - (w - h * wndRatio) / 2d);
                }
                else
                {
                    beginRow = (int)(-(w / wndRatio - h) / 2d);
                    endRow = (int)(h + (w / wndRatio - h) / 2d);
                    beginCol = 0;
                    endCol = (int)w;
                }

                HOperatorSet.SetPart(hv_WindowHandle, beginRow, beginCol, endRow, endCol);
            }
            HOperatorSet.DispObj(image, hv_WindowHandle);
        }

        /// <summary>
        /// 滚动滑轮，缩放图像
        /// </summary>
        private void hWindowControl1_HMouseWheel(object sender, HMouseEventArgs e)
        {
            try
            {
                if (image != null && image.IsInitialized())
                {
                    HOperatorSet.GetMpositionSubPix(hv_WindowHandle, out HTuple r, out HTuple c, out HTuple button);
                    double mposRow = r.D, mposCol = c.D;

                    HOperatorSet.GetPart(hv_WindowHandle, out HTuple r1, out HTuple c1, out HTuple r2, out HTuple c2);
                    int oldBeginRow = r1, oldBeginCol = c1, oldEndRow = r2, oldEndCol = c2;

                    int zoomBeginRow = 0, zoomBeginCol = 0, zoomEndRow = 0, zoomEndCol = 0;
                    if (e.Delta > 0)
                    {
                        zoomBeginRow = (int)(oldBeginRow + (mposRow - oldBeginRow) * 0.2d);
                        zoomBeginCol = (int)(oldBeginCol + (mposCol - oldBeginCol) * 0.2d);
                        zoomEndRow = (int)(oldEndRow - (oldEndRow - mposRow) * 0.2d);
                        zoomEndCol = (int)(oldEndCol - (oldEndCol - mposCol) * 0.2d);
                    }
                    else
                    {
                        zoomBeginRow = (int)(mposRow - (mposRow - oldBeginRow) / 0.8d);
                        zoomBeginCol = (int)(mposCol - (mposCol - oldBeginCol) / 0.8d);
                        zoomEndRow = (int)(mposRow + (oldEndRow - mposRow) / 0.8d);
                        zoomEndCol = (int)(mposCol + (oldEndCol - mposCol) / 0.8d);
                    }

                    HOperatorSet.GetImageSize(image, out HTuple imgW, out HTuple imgH);
                    int imgWidth = imgW, imgHeight = imgH;

                    bool outOfArea = zoomBeginRow >= imgHeight || zoomEndRow <= 0 || zoomBeginCol >= imgWidth || zoomEndCol <= 0;
                    bool outOfSize = (zoomEndRow - zoomBeginRow) > imgWidth * 20 || (zoomEndCol - zoomBeginCol) > imgWidth * 20;
                    bool outOfPixel = (hWindowControl1.Height / (zoomEndRow - zoomBeginRow) > 500)
                        || (hWindowControl1.Width / (zoomEndCol - zoomBeginCol)) > 500;

                    if (outOfArea || outOfSize)
                    {

                    }
                    else if (!outOfPixel)
                    {
                        HOperatorSet.ClearWindow(hv_WindowHandle);
                        HOperatorSet.SetPaint(hv_WindowHandle, new HTuple("default"));
                        HOperatorSet.SetPart(hv_WindowHandle, zoomBeginRow, zoomBeginCol, zoomEndRow, zoomBeginCol + (zoomEndRow - zoomBeginRow) * hWindowControl1.Width / hWindowControl1.Height);
                        DispObj(image);
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void hWindowControl1_SizeChanged(object sender, EventArgs e)
        {
            HOperatorSet.SetWindowExtents(hv_WindowHandle, -1, -1, hWindowControl1.Width - 4, hWindowControl1.Height - 4);
            DispImage(image);
        }

        private void hWindowControl1_DoubleClick(object sender, EventArgs e)
        {
            //上层有halcon窗口，进不来
            Close();
        }
        DateTime now = DateTime.MinValue;
        private void hWindowControl1_HMouseUp(object sender, HMouseEventArgs e)
        {
            //自制双击关闭
            if (e.Button == MouseButtons.Left)
            {
                if ((DateTime.Now - now).TotalSeconds < 0.3)
                {
                    Close();
                }
                else
                {
                    now = DateTime.Now;
                }
            }
        }
    }
}
