using HslCommunication;
using HslCommunication.Profinet.Toyota;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OnlineMeasurement.IO
{
    public class ToyotaPlc : IHsl
    {
        PlcParam param = new PlcParam();
        Dictionary<string, IoAddress> ioDict = new Dictionary<string, IoAddress>();
        ToyoPuc plc = new ToyoPuc();
        bool _isOpen = false;
        string _errMsg = string.Empty;
        public string ErrMsg => _errMsg;
        public bool IsOpen => _isOpen;
        public PlcParam Param { get => param; set => param = value; }
        public Dictionary<string, IoAddress> IoDict { get => ioDict; set => ioDict = value; }

        public ToyotaPlc()
        {
            if (!HslCommunication.Authorization.SetAuthorizationCode("0293fde5-6e7c-4c76-bacd-e3bdb0ee6187"))
            {
                System.Windows.Forms.MessageBox.Show("active failed");
            }
            param.Port = 6000;
            param.DataFormat = plc.ByteTransform.DataFormat;
            param.IsStringReverseByteWord = plc.ByteTransform.IsStringReverseByteWord;
        }

        public bool Load()
        {
            bool result = true;
            string basePath = AppDomain.CurrentDomain.BaseDirectory + "Data\\";
            try
            {
                string paramPath = basePath + "PlcParam.xml";
                if (File.Exists(paramPath))
                {
                    XmlSerializer xml = new XmlSerializer(param.GetType());
                    using (FileStream stream = new FileStream(paramPath, FileMode.OpenOrCreate))
                    {
                        PlcParam _ = (PlcParam)xml.Deserialize(stream);
                        if (_ != null)
                        {
                            param = _;
                        }
                        else
                        {
                            result = false;
                            _errMsg = $"{paramPath}文件格式异常";
                        }
                    }
                }
                else
                {
                    result = false;
                    _errMsg = $"{paramPath}文件不存在";
                }
            }
            catch (Exception ex)
            {
                result = false;
                _errMsg = ex.ToString();
            }

            try
            {
                string paramPath = basePath + "IoParam.xml";
                if (File.Exists(paramPath))
                {
                    List<IoAddress> ios = new List<IoAddress>();
                    XmlSerializer xml = new XmlSerializer(ios.GetType());
                    using (FileStream stream = new FileStream(paramPath, FileMode.OpenOrCreate))
                    {
                        ios = (List<IoAddress>)xml.Deserialize(stream);
                    }
                    if (ios == null)
                    {
                        result = false;
                        _errMsg = $"{paramPath}文件格式异常";
                    }
                    else
                    {
                        ioDict = ios.ToDictionary(n => { return n.IoName; });
                    }
                }
                else
                {
                    result = false;
                    _errMsg = $"{paramPath}文件不存在";
                }
            }
            catch (Exception ex)
            {
                result = false;
                _errMsg = ex.ToString();
            }
            return result;
        }
        public bool Save()
        {
            bool result = true;
            try
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory + "Data\\";
                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }

                string OpcParamPath = basePath + "PlcParam.xml";
                XmlSerializer xml = new XmlSerializer(param.GetType());
                using (FileStream stream = new FileStream(OpcParamPath, FileMode.Create))
                {
                    xml.Serialize(stream, param);
                }

                List<IoAddress> ios = ioDict.Values.ToList();
                string ioParamPath = basePath + "IoParam.xml";
                XmlSerializer ioXml = new XmlSerializer(ios.GetType());
                using (FileStream stream = new FileStream(ioParamPath, FileMode.Create))
                {
                    ioXml.Serialize(stream, ios);
                }
            }
            catch (Exception ex) { result = false; _errMsg = ex.ToString(); }
            return result;
        }

        public bool Open()
        {
            plc.ConnectTimeOut = 2000;
            plc.ByteTransform.DataFormat = param.DataFormat;
            plc.ByteTransform.IsStringReverseByteWord = param.IsStringReverseByteWord;

            plc.IpAddress = param.IpAddress;
            plc.Port = param.Port;

            var result = plc.ConnectServer();
            if (result.IsSuccess)
            {
                _isOpen = true;
                return true;
            }
            else
            {
                _errMsg = result.Message;
                return false;
            }
        }
        public void Close()
        {
            plc.ConnectClose();
            _isOpen = false;
        }


        public OperateResult<bool> ReadBool(DO eDO)
        {
            return plc.ReadBool(ioDict[eDO.ToString()].Address);
        }
        public OperateResult<bool> ReadBool(DI eDI)
        {
            return plc.ReadBool(ioDict[eDI.ToString()].Address);
        }
        public OperateResult<ushort> ReadUInt16(DI eDI)
        {
            return plc.ReadUInt16(ioDict[eDI.ToString()].Address);
        }
        public OperateResult<short> ReadInt16(DI eDI)
        {
            return plc.ReadInt16(ioDict[eDI.ToString()].Address);
        }
        public OperateResult<uint> ReadUInt32(DI eDI)
        {
            return plc.ReadUInt32(ioDict[eDI.ToString()].Address);
        }
        public OperateResult<int> ReadInt32(DI eDI)
        {
            return plc.ReadInt32(ioDict[eDI.ToString()].Address);
        }
        public OperateResult<string> ReadString(DI eDI, ushort length)
        {
            return plc.ReadString(ioDict[eDI.ToString()].Address, length);
        }


        public OperateResult Write(DO eDO, bool value)
        {
            //D寄存器不支持位写入
            string address = ioDict[eDO.ToString()].Address;
            if (address.Contains('.'))//存在位地址
            {
                if (address.ToUpper().Contains("D"))
                {
                    string[] strings = address.Split('.');
                    if (strings.Length == 2)
                    {
                        string io = strings[0];
                        int index = Convert.ToInt32(strings[1], 16);
                        var read = plc.ReadUInt16(io);
                        if (read.IsSuccess)
                        {
                            ushort num;
                            if (value)
                            {
                                num = (ushort)(read.Content | (1 << index));
                            }
                            else
                            {
                                num = (ushort)(read.Content & (ushort.MaxValue - (1 << index)));
                            }
                            Thread.Sleep(2);
                            return plc.Write(io, num);
                        }
                        else
                        {
                            var re = new OperateResult();
                            re.IsSuccess = false;
                            re.Message = read.Message;
                            return re;
                        }
                    }
                }
            }
            return plc.Write(ioDict[eDO.ToString()].Address, value);
        }
        public OperateResult Write(DO eDO, ushort value)
        {
            return plc.Write(ioDict[eDO.ToString()].Address, value);
        }
        public OperateResult Write(DO eDO, short value)
        {
            return plc.Write(ioDict[eDO.ToString()].Address, value);
        }
        public OperateResult Write(DO eDO, uint value)
        {
            return plc.Write(ioDict[eDO.ToString()].Address, value);
        }
        public OperateResult Write(DO eDO, int value)
        {
            return plc.Write(ioDict[eDO.ToString()].Address, value);
        }
        public OperateResult Write(DO eDO, int[] value)
        {
            return plc.Write(ioDict[eDO.ToString()].Address, value);
        }
        public OperateResult Write(DO eDO, string value)
        {
            return plc.Write(ioDict[eDO.ToString()].Address, value);
        }


        public void ShowForm()
        {
            new HslForm(this, false).ShowDialog();
        }
    }
}
