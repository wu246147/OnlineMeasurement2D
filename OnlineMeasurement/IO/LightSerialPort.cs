using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OnlineMeasurement.IO
{
    public partial class LightSerialPort
    {

        public SerialPort _serialPort = new SerialPort();
        public string _errMsg = string.Empty;
        SerialPortParam param = new SerialPortParam();

        public LightSerialPort()
        {
            
        }

        public bool Load()
        {

            bool result = true;
            string basePath = AppDomain.CurrentDomain.BaseDirectory + "Data\\";
            try
            {
                string paramPath = basePath + "SerialPortParam.xml";
                if (File.Exists(paramPath))
                {
                    XmlSerializer xml = new XmlSerializer(param.GetType());
                    using (FileStream stream = new FileStream(paramPath, FileMode.OpenOrCreate))
                    {
                        SerialPortParam _ = (SerialPortParam)xml.Deserialize(stream);
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

            return result;
        }

        public bool Load2()
        {

            bool result = true;
            string basePath = AppDomain.CurrentDomain.BaseDirectory + "Data\\";
            try
            {
                string paramPath = basePath + "SerialPortParam2.xml";
                if (File.Exists(paramPath))
                {
                    XmlSerializer xml = new XmlSerializer(param.GetType());
                    using (FileStream stream = new FileStream(paramPath, FileMode.OpenOrCreate))
                    {
                        SerialPortParam _ = (SerialPortParam)xml.Deserialize(stream);
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

            return result;
        }

        public bool Open()
        {
            try
            {
                // 设置串口参数
                _serialPort.PortName = param.PortName;       // 串口号
                _serialPort.BaudRate = param.BaudRate;         // 波特率
                _serialPort.Parity = Parity.None;    // 校验位
                _serialPort.DataBits = 8;            // 数据位
                _serialPort.StopBits = StopBits.One; // 停止位
                _serialPort.Handshake = Handshake.None;

                // 设置读取超时
                _serialPort.ReadTimeout = 500;
                _serialPort.WriteTimeout = 500;

                _serialPort.Open();
                return true;

            }
            catch (Exception ex)
            {
                _errMsg = ex.ToString();
                return false;
            }
        }

        public bool Close()
        {
            if (_serialPort.IsOpen)
            {
                // 关闭串口
                _serialPort.Close();
            }
            return true;

        }
        public bool controlLightX(bool isOpen,string lightID)
        {
            if (_serialPort.IsOpen)
            {
                if (isOpen)
                {
                    _serialPort.Write($"S{lightID}0255#\r\n");
                }
                else
                {
                    _serialPort.Write($"S{lightID}0000#\r\n");
                }
                return true;
            }
            return false;
        }
        public bool controlLight(bool isOpen)
        {
            if(_serialPort.IsOpen)
            {
                if (isOpen)
                {
                    _serialPort.Write("SH#\r\n");
                }
                else
                {
                    _serialPort.Write("SL#\r\n");
                }
                return true;
            }
           return false;
        }
    }
}
