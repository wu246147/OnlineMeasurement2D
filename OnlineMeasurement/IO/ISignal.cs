using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMeasurement.IO
{
    //public interface ISignal
    //{
    //    bool IsOpen { get; }
    //    void ShowForm();
    //    bool Load();
    //    bool Open();
    //    void Close();
    //    bool Read(DI eDI, out string value);
    //    bool Read(DO eDO, out string value);
    //    bool Read(DI eDI, out bool value);
    //    bool Read(DO eDO, out bool value);
    //    bool Read(DI eDI, out ushort value);
    //    bool Read(DO eDO, out ushort value);
    //    bool Write(DO eDO, object value);
    //    bool Save();
    //}


    /// <summary>
    /// bool(0~255),ushort(256~511),short(512~767),uint(768~1023),int(1024~1279),string(1280~1535)
    /// </summary>
    public enum DO
    {
        心跳 = 0,
        准备好,
        运行中,
        光源,
        拍照完成,
        测量可读,
        测量结果,
        单次拍照完成1,
        单次拍照完成2,
        单次拍照完成3,



        车型反馈 = 256,
        连番反馈,
        测点结果,

        测量数据 = 1024,
    }
    /// <summary>
    /// bool(0~255),ushort(256~511),short(512~767),uint(768~1023),int(1024~1279),string(1280~1535)
    /// </summary>
    public enum DI
    {
        开始 = 0,
        已读,
        重置,
        空跑,
        车型连番OK,
        拍照触发信号1,
        拍照触发信号2,
        拍照触发信号3,

        检测完成,

        车型号 = 256,
        连番号,
        托盘号,
        控制光源信号,
        触发拍照点位ID1,
        触发拍照点位ID2,
        触发拍照点位ID3,

        车架号 = 1280,
    }

}
