using HalconDotNet;
using MvCameraControl;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OnlineMeasurement
{
    /// <summary>
    /// 海康相机适配器。
    /// 相机名称使用 MvCameraControl 的 IDeviceInfo.UserDefinedName，
    /// 与 Basler 的 UserDefinedName 保持一致，调用端无需区分厂商。
    /// </summary>
    public sealed class HikCam : ICam
    {
        private IDevice _device;
        private bool _isGrabbing;
        private string _errMsg = string.Empty;
        private string _name = string.Empty;

        public string Name => _name;
        public string ErrMsg => _errMsg;
        public bool IsOpen => _device != null && _device.IsConnected;

        public bool Open()
        {
            return OpenInternal(null);
        }

        public bool Open(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                _errMsg = "海康相机名称不能为空";
                return false;
            }

            return OpenInternal(name);
        }

        private bool OpenInternal(string name)
        {
            try
            {
                Close();

                List<IDeviceInfo> devices = new List<IDeviceInfo>();
                int result = DeviceEnumerator.EnumDevices(
                    DeviceTLayerType.MvGigEDevice | DeviceTLayerType.MvUsbDevice,
                    out devices);
                if (result != MvError.MV_OK)
                {
                    SetSdkError("枚举海康相机失败", result);
                    return false;
                }

                IDeviceInfo selected = null;
                foreach (IDeviceInfo deviceInfo in devices)
                {
                    // 用户要求使用 chUserDefinedName；托管 SDK 对应 UserDefinedName。
                    if (name == null || string.Equals(deviceInfo.UserDefinedName, name, StringComparison.Ordinal))
                    {
                        selected = deviceInfo;
                        break;
                    }
                }

                if (selected == null)
                {
                    _errMsg = name == null ? "未找到海康相机" : "未找到海康相机：" + name;
                    return false;
                }

                _device = DeviceFactory.CreateDevice(selected);
                if (_device == null)
                {
                    _errMsg = "海康相机创建设备对象失败：" + (selected.UserDefinedName ?? string.Empty);
                    return false;
                }

                result = _device.Open();
                if (result != MvError.MV_OK)
                {
                    SetSdkError("打开海康相机失败", result);
                    _device = null;
                    return false;
                }

                _name = selected.UserDefinedName ?? string.Empty;
                _errMsg = string.Empty;

                // 与 Basler 的 AcquireContinuous 配置保持一致。部分型号不支持其中某个节点，
                // 不影响设备打开，实际错误仍会在采集或参数设置时返回。
                TrySetEnum("AcquisitionMode", "Continuous");
                TrySetEnum("TriggerMode", "Off");
                TrySetEnum("ExposureAuto", "Off");
                return true;
            }
            catch (Exception ex)
            {
                _errMsg = "打开海康相机异常：" + ex;
                _device = null;
                _isGrabbing = false;
                return false;
            }
        }

        public bool GetExposure(out double exposure)
        {
            exposure = 0;
            if (!IsOpen)
            {
                _errMsg = "海康相机未打开";
                return false;
            }

            try
            {
                IFloatValue value;
                int result = _device.Parameters.GetFloatValue("ExposureTime", out value);
                if (result != MvError.MV_OK)
                {
                    SetSdkError("获取海康相机曝光失败", result);
                    return false;
                }

                exposure = value.CurValue;
                return true;
            }
            catch (Exception ex)
            {
                _errMsg = "获取海康相机曝光异常：" + ex;
                return false;
            }
        }

        public bool SetExposure(double exposure)
        {
            if (!IsOpen)
            {
                _errMsg = "海康相机未打开";
                return false;
            }

            try
            {
                TrySetEnum("ExposureAuto", "Off");
                int result = _device.Parameters.SetFloatValue("ExposureTime", (float)exposure);
                if (result != MvError.MV_OK)
                {
                    SetSdkError("设置海康相机曝光失败", result);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _errMsg = "设置海康相机曝光异常：" + ex;
                return false;
            }
        }

        public HImage OneShot()
        {
            if (!IsOpen)
            {
                _errMsg = "海康相机未打开";
                return null;
            }

            IFrameOut frame = null;
            bool startedHere = false;
            try
            {
                if (!_isGrabbing)
                {
                    int startResult = _device.StreamGrabber.StartGrabbing(StreamGrabStrategy.OneByOne);
                    if (startResult != MvError.MV_OK)
                    {
                        SetSdkError("开始海康相机采集失败", startResult);
                        return null;
                    }

                    _isGrabbing = true;
                    startedHere = true;
                }

                int result = _device.StreamGrabber.GetImageBuffer(1000, out frame);
                if (result != MvError.MV_OK || frame == null || frame.Image == null)
                {
                    SetSdkError("获取海康相机图像失败", result);
                    return null;
                }

                return ToHImage(frame.Image);
            }
            catch (Exception ex)
            {
                _errMsg = "采集海康相机图像异常：" + ex;
                return null;
            }
            finally
            {
                if (frame != null && _device != null)
                {
                    try
                    {
                        _device.StreamGrabber.FreeImageBuffer(frame);
                    }
                    catch (Exception ex)
                    {
                        _errMsg = "释放海康相机图像缓存异常：" + ex;
                    }
                }

                if (startedHere)
                {
                    Stop();
                }
            }
        }

        private HImage ToHImage(IImage sourceImage)
        {
            IImage image = sourceImage;
            MvGvspPixelType pixelType = image.PixelType;

            bool isMono8 = pixelType == MvGvspPixelType.PixelType_Gvsp_Mono8;
            bool isRgb8 = pixelType == MvGvspPixelType.PixelType_Gvsp_RGB8_Packed;
            bool isBgr8 = pixelType == MvGvspPixelType.PixelType_Gvsp_BGR8_Packed;

            if (!isMono8 && !isRgb8 && !isBgr8)
            {
                IImage convertedImage;
                MvGvspPixelType targetType = IsColorPixelType(pixelType)
                    ? MvGvspPixelType.PixelType_Gvsp_BGR8_Packed
                    : MvGvspPixelType.PixelType_Gvsp_Mono8;
                int result = _device.PixelTypeConverter.ConvertPixelType(sourceImage, out convertedImage, targetType);
                if (result != MvError.MV_OK || convertedImage == null)
                {
                    SetSdkError("转换海康图像像素格式失败", result);
                    return null;
                }

                image = convertedImage;
                pixelType = targetType;
                isMono8 = pixelType == MvGvspPixelType.PixelType_Gvsp_Mono8;
                isRgb8 = pixelType == MvGvspPixelType.PixelType_Gvsp_RGB8_Packed;
                isBgr8 = pixelType == MvGvspPixelType.PixelType_Gvsp_BGR8_Packed;
            }

            byte[] pixelData = image.PixelData;
            if (pixelData == null || pixelData.Length == 0)
            {
                _errMsg = "海康相机图像数据为空";
                return null;
            }

            GCHandle handle = GCHandle.Alloc(pixelData, GCHandleType.Pinned);
            try
            {
                int width = checked((int)image.Width);
                int height = checked((int)image.Height);
                IntPtr dataPtr = handle.AddrOfPinnedObject();

                if (isMono8)
                {
                    return new HImage("byte", width, height, dataPtr);
                }

                HImage colorImage = new HImage();
                colorImage.GenImageInterleaved(
                    dataPtr,
                    isRgb8 ? "rgb" : "bgr",
                    width,
                    height,
                    -1,
                    "byte",
                    width,
                    height,
                    0,
                    0,
                    -1,
                    0);
                return colorImage;
            }
            finally
            {
                handle.Free();
            }
        }

        private static bool IsColorPixelType(MvGvspPixelType pixelType)
        {
            return pixelType == MvGvspPixelType.PixelType_Gvsp_RGB8_Packed
                || pixelType == MvGvspPixelType.PixelType_Gvsp_BGR8_Packed
                || pixelType == MvGvspPixelType.PixelType_Gvsp_RGBA8_Packed
                || pixelType == MvGvspPixelType.PixelType_Gvsp_BGRA8_Packed
                || pixelType == MvGvspPixelType.PixelType_Gvsp_RGB8_Planar
                || pixelType.ToString().IndexOf("Bayer", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public void Stop()
        {
            if (_device == null || !_isGrabbing)
            {
                return;
            }

            try
            {
                int result = _device.StreamGrabber.StopGrabbing();
                if (result != MvError.MV_OK)
                {
                    SetSdkError("停止海康相机采集失败", result);
                }
            }
            catch (Exception ex)
            {
                _errMsg = "停止海康相机采集异常：" + ex;
            }
            finally
            {
                _isGrabbing = false;
            }
        }

        public void Close()
        {
            if (_device == null)
            {
                _isGrabbing = false;
                return;
            }

            try
            {
                Stop();
                int result = _device.Close();
                if (result != MvError.MV_OK)
                {
                    SetSdkError("关闭海康相机失败", result);
                }
            }
            catch (Exception ex)
            {
                _errMsg = "关闭海康相机异常：" + ex;
            }
            finally
            {
                _device = null;
                _isGrabbing = false;
            }
        }

        private void TrySetEnum(string key, string value)
        {
            if (_device == null)
            {
                return;
            }

            try
            {
                _device.Parameters.SetEnumValueByString(key, value);
            }
            catch
            {
                // 不同海康型号的节点集合可能不同，初始化设置采用尽力而为。
            }
        }

        private void SetSdkError(string action, int result)
        {
            _errMsg = action + "，错误码：0x" + result.ToString("X8");
        }
    }
}
