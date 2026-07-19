using HslCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMeasurement.IO
{
    public interface IHsl
    {
        string ErrMsg { get; }
        PlcParam Param { get; set; }
        Dictionary<string, IoAddress> IoDict { get; set; }
        bool IsOpen { get; }
        bool Load();
        bool Save();
        bool Open();
        void Close();
        void ShowForm();

        OperateResult<bool> ReadBool(DO eDO);
        OperateResult<bool> ReadBool(DI eDI);
        OperateResult<ushort> ReadUInt16(DI eDI);
        OperateResult<short> ReadInt16(DI eDI);
        OperateResult<uint> ReadUInt32(DI eDI);
        OperateResult<int> ReadInt32(DI eDI);
        OperateResult<string> ReadString(DI eDI,ushort length);

        OperateResult Write(DO eDO, bool value);
        OperateResult Write(DO eDO, ushort value);
        OperateResult Write(DO eDO, short value);
        OperateResult Write(DO eDO, uint value);
        OperateResult Write(DO eDO, int value);
        OperateResult Write(DO eDO, int[] value);
        OperateResult Write(DO eDO, string value);
    }
    [Serializable]
    public class PlcParam
    {
        public string IpAddress = "127.0.0.1";
        public int Port = 2000;
        public byte DA2 = 0;
        public HslCommunication.Core.DataFormat DataFormat = new HslCommunication.Core.DataFormat();
        public bool IsStringReverseByteWord = false;
    }
    [Serializable]
    public class IoAddress
    {
        public string IoName;
        public string Address;
    }



    [Serializable]
    public class SerialPortParam
    {
        public string PortName = "COM1";
        public int BaudRate = 19200;
    }
}
