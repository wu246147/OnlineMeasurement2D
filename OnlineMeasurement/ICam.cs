using HalconDotNet;

namespace OnlineMeasurement
{
    /// <summary>
    /// 相机类别。Basler 使用 0，保证旧的 XML 配置在新增该字段后仍按 Basler 处理。
    /// </summary>
    public enum CameraType
    {
        Basler = 0,
        Hikvision = 1
    }

    /// <summary>
    /// 业务层使用的统一相机接口。
    /// 具体厂商 SDK 的差异应封装在 BaslerCam/HikCam 内部。
    /// </summary>
    public interface ICam
    {
        string Name { get; }
        string ErrMsg { get; }

        bool Open();
        bool Open(string name);
        bool GetExposure(out double exposure);
        bool SetExposure(double exposure);
        HImage OneShot();
        void Stop();
        void Close();
    }

    /// <summary>
    /// 根据配置创建具体厂商相机，避免业务代码散落厂商判断。
    /// </summary>
    public static class CameraFactory
    {
        public static ICam Create(CameraType cameraType)
        {
            switch (cameraType)
            {
                case CameraType.Hikvision:
                    return new HikCam();
                case CameraType.Basler:
                default:
                    return new BaslerCam();
            }
        }
    }
}
