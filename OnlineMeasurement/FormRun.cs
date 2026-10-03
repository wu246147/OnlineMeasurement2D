using HalconDotNet;
using HslCommunication;
using HslCommunication.Profinet.Omron;
using MySqlX.XDevAPI.Common;
using OnlineMeasurement.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineMeasurement
{
    public partial class FormRun : Form
    {
        Thread mainThread = null;
        bool stop = true;
        System.Timers.Timer alive = new System.Timers.Timer();
        IHsl plc = new ToyotaPlc();

        System.Timers.Timer plcinfo = new System.Timers.Timer();

        //光源控制,两个光源控制器
        LightSerialPort lightSerialPort = new LightSerialPort();
        LightSerialPort lightSerialPort2 = new LightSerialPort();

        Dictionary<string, CarSetting> 车型参数 = new Dictionary<string, CarSetting>();
        Dictionary<int, string> 车型号转名称 = new Dictionary<int, string>();
        List<string> 车型参数排序key = new List<string>();

        Cam 相机参数 = new Cam();
        // Camera instances are created through the factory according to each camera setting.
        Dictionary<string, ICam> 相机 = new Dictionary<string, ICam>();
        OtherSet otherSet = new OtherSet();

        wSql sql = null;
        bool sqlEnable = false;
        bool 预载车型参数 = true;

        Color 原色 = Color.White;

        FormShow formShow = null;
        public FormRun()
        {
            InitializeComponent();
            AppTheme.Apply(this);

            原色 = button_Led.BackColor;

            alive.Interval = 500;
            alive.AutoReset = true;
            alive.Elapsed += alive_Tick;

            plcinfo.Interval = 500;
            plcinfo.AutoReset = true;
            plcinfo.Elapsed += plcinfo_Tick;
            plcinfo.Enabled = true;

            //buttonRun_Click(null, null);
        }
        private void FormRun_Load(object sender, EventArgs e)
        {
            formShow = new FormShow();

            Thread thread = new Thread(() =>
            {
                formShow.ShowDialog();
            });
            thread.Start();
            while (!formShow.IsShow) Thread.Sleep(10);
            formShow.BeginInvoke(new Action(() =>
            {
                int index = 0;
                for (int i = 1; i < Screen.AllScreens.Length; i++)
                {
                    if (Screen.AllScreens[i].Bounds.Width >= Screen.AllScreens[i - 1].Bounds.Width)
                    {
                        index = i;
                    }
                }
                formShow.SetFormWindowState(FormWindowState.Normal);
                formShow.Left = Screen.AllScreens[index].Bounds.Left;
                formShow.Top = Screen.AllScreens[index].Bounds.Top;
                formShow.SetFormWindowState(FormWindowState.Maximized);
            }));


            //plc连接
            plcInit();

            //光源控制连接

            if (lightSerialPort.Load())
            {
                ShowMessage($"光源控制参数加载成功");
                lightSerialPort.Open();
            }
            else
            {
                ShowMessage($"光源控制参数加载失败:{lightSerialPort._errMsg}", Color.Red);
                this.BeginInvoke(new Action(() => { this.Activate(); }));
                return;
            }

            if (lightSerialPort2.Load2())
            {
                ShowMessage($"光源控制2参数加载成功");
                lightSerialPort2.Open();
            }
            else
            {
                ShowMessage($"光源控制2参数加载失败:{lightSerialPort2._errMsg}", Color.Red);
                this.BeginInvoke(new Action(() => { this.Activate(); }));
                return;
            }

        }

        ushort lightState = 0; //信号1开灯，信号2关灯
        //测试参数
        OperateResult<ushort> lightState_test = new OperateResult<ushort>();

        void plcinfo_Tick(object sender, EventArgs e)
        {
            if (plc.IsOpen)
            {
                var _lightControlID = ReadUInt16(DI.控制光源信号);
                if (_lightControlID.IsSuccess)
                {
                    if (lightSerialPort._serialPort.IsOpen && lightSerialPort2._serialPort.IsOpen)
                    {
                        if (_lightControlID.Content == 1 && lightState != 1)
                        {
                            lightState = _lightControlID.Content;
                            lightSerialPort.controlLight(true);
                            lightSerialPort2.controlLight(true);

                        }
                        else if (_lightControlID.Content == 2 && lightState != 2)
                        {
                            lightState = _lightControlID.Content;
                            lightSerialPort.controlLight(false);
                            lightSerialPort2.controlLight(false);

                        }
                    }
                }
               
            }
        }

        bool b = true;
        void alive_Tick(object sender, EventArgs e)
        {
            //非UI线程执行,且会并行执行
            if (b)//防止触发间隔小于执行时间造成信号紊乱与线程积累
            {
                b = false;
                if (mainThread != null && mainThread.IsAlive && !stop)
                {
                    var result = plc.ReadBool(DO.心跳);
                    if (result.IsSuccess)
                    {
                        bool b2;
                        lock (iolock)
                        {
                            b2 = plc.Write(DO.心跳, !result.Content).IsSuccess;
                        }
                        if (!b2)
                        {
                            //ShowMessage("心跳写入失败", Color.Red);
                        }
                    }
                    else
                    {
                        //ShowMessage("心跳读取失败", Color.Red);
                    }
                }
                b = true;
            }
        }
        private void buttonRun_Click(object sender, EventArgs e)
        {
            LicenseKey key = new LicenseKey();
            string productID = textBoxTestNum.Text;
            if (key.IsTrue(productID))
            {
                RunState();
                //开启主流程
                if (mainThread == null || mainThread.ThreadState == ThreadState.Stopped)
                {
                    mainThread = new Thread(MainThread);
                    mainThread.Start();
                    b = true;
                }
            }
            else
            {
                MessageBox.Show("许可错误！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        void plcInit()
        {
            if (plc.Load())
            {
                ShowMessage($"plc参数加载成功");
            }
            else
            {
                ShowMessage($"plc参数加载失败:{plc.ErrMsg}", Color.Red);
                this.BeginInvoke(new Action(() => { this.Activate(); }));
                return;
            }
            if (plc.Open())
            {
                ShowMessage($"PLC连接成功");
            }
            else
            {
                ShowMessage($"PLC连接失败:{plc.ErrMsg}", Color.Red);
                this.BeginInvoke(new Action(() => { this.Activate(); }));
                return;
            }
        }

        void plcClose()
        {
            if (plc.IsOpen)
            {
                plc?.Close();
            }
        }

        DateTime dateTimeNow = DateTime.Now;
        Dictionary<string, Task> tasks = new Dictionary<string, Task>();
        bool bAbort = false;
        bool bDry_mode = false;
        void MainThread()
        {
            try
            {
                //加载参数
                车型号转名称.Clear();
                车型参数.Clear();
                车型参数排序key.Clear();
                if (预载车型参数)
                {
                    string[] carPaths = Directory.GetDirectories(Car.BaseDirectory);
                    foreach (var item in carPaths)
                    {
                        string dirName = Path.GetFileNameWithoutExtension(item);
                        string[] strings = dirName.Split('-');
                        if (strings.Length == 3 && int.TryParse(strings[0], out int 车型号) && int.TryParse(strings[1], out int 托盘号))
                        {
                            if (!车型号转名称.ContainsKey(车型号))
                            {
                                车型号转名称.Add(车型号, strings[2]);
                            }

                            CarSetting carSet = new CarSetting(dirName);
                            if (carSet.Load(item))
                            {
                                车型参数.Add($"{车型号}-{托盘号}", carSet);
                                车型参数排序key.Add($"{车型号}-{托盘号}");
                                ShowMessage($"{dirName}参数加载成功");
                            }
                            else
                            {
                                ShowMessage($"{dirName}参数加载失败", Color.Red);
                                this.BeginInvoke(new Action(() => { this.Activate(); }));
                            }
                        }
                    }
                }

                相机.Clear();
                相机参数.sets.Clear();
                string[] camPaths = Directory.GetDirectories(Cam.BaseDirectory);
                if (camPaths.Length > 0)
                {
                    foreach (var item in camPaths)
                    {
                        string name = Path.GetFileNameWithoutExtension(item);
                        CamSetting camSetting = new CamSetting(name);
                        if (camSetting.Load(item))
                        {
                            相机参数.sets.Add(name, camSetting);
                            ShowMessage($"{name}相机参数加载成功");
                        }
                        else
                        {
                            ShowMessage($"{name}相机参数加载失败:" + camSetting.ErrMsg, Color.Red);
                            this.BeginInvoke(new Action(() => { this.Activate(); }));
                        }
                        if (!checkBox相机仿真.Checked)
                        {
                            相机.Add(name, CameraFactory.Create(相机参数.sets[name].gSet.CameraType));
                        }
                        else
                        {
                            相机.Add(name, null);
                        }
                    }
                }
                else
                {
                    ShowMessage($"路径“{Cam.BaseDirectory}”下未分配任何相机", Color.Red);
                    this.BeginInvoke(new Action(() => { this.Activate(); }));
                    return;
                }

                if (!checkBoxPLC仿真.Checked)
                {
                    
                }
                //连接设备
                if (!checkBoxPLC仿真.Checked)
                {
                    if (plc.IsOpen)
                    {

                    }
                    else
                    {
                        ShowMessage($"PLC连接失败:{plc.ErrMsg}", Color.Red);
                        return;
                    }
                    alive.Enabled = true;
                }
                else
                {
                    ShowMessage($"PLC仿真连接成功");
                }

                otherSet.Load();

                if (!checkBox相机仿真.Checked)
                {
                    foreach (var item in 相机.Keys)
                    {
                        if (!相机参数.sets[item].gSet.Disabled && 相机[item].Open(item))
                        {
                            ShowMessage(item + "相机打开成功");
                        }
                        else
                        {
                            ShowMessage(item + "相机打开失败:" + 相机[item].ErrMsg, Color.Red);
                            this.BeginInvoke(new Action(() => { this.Activate(); }));
                            return;
                        }
                    }
                }

                if (!sqlEnable)
                {
                    try
                    {
                        sql = new wSql("127.0.0.1", "admin", "admin", "raivas");
                        sql.Open();
                        ShowMessage("数据库连接测试成功！");
                        sql.Close();
                        sqlEnable = true;
                    }
                    catch (Exception ex)
                    {
                        ShowMessage("数据库连接测试失败！" + ex.Message, Color.Red);
                        this.BeginInvoke(new Action(() => { this.Activate(); }));
                        sqlEnable = false;
                    }
                }

                threadSaveImage = new Thread(SaveImage);
                threadSaveImage.Start();

                while (!stop)
                {
                    if (loadAgain)//热重载参数
                    {
                        车型参数.Clear();
                        车型参数排序key.Clear();
                        otherSet.Load();
                        loadAgain = false;
                    }

                    bAbort = false;
                    ushort carKind;
                    ushort TRnum;
                    ushort carNum;
                    string carVIN;
                    System.Diagnostics.Stopwatch sp = new System.Diagnostics.Stopwatch();

                    //信号复位
                    if (!checkBoxPLC仿真.Checked)
                    {
                        if (!Write(DO.准备好, false).IsSuccess)
                        {
                            ShowMessage("准备好写入失败", Color.Red);
                        }
                        if (!Write(DO.运行中, false).IsSuccess)
                        {
                            ShowMessage("运行中写入失败", Color.Red);
                        }
                        if (!Write(DO.测量可读, false).IsSuccess)
                        {
                            ShowMessage("测量结果可读写入失败", Color.Red);
                        }
                        if (!Write(DO.测量结果, false).IsSuccess)
                        {
                            ShowMessage("测量结果写入失败", Color.Red);
                        }
                        if (!Write(DO.测点结果, 0).IsSuccess)
                        {
                            ShowMessage("测点结果写入失败", Color.Red);
                        }
                        if (!Write(DO.车型反馈, 0).IsSuccess)
                        {
                            ShowMessage("车型反馈写入失败", Color.Red);
                        }
                        if (!Write(DO.连番反馈, 0).IsSuccess)
                        {
                            ShowMessage("连番反馈写入失败", Color.Red);
                        }
                        if (!Write(DO.拍照完成, false).IsSuccess)
                        {
                            ShowMessage("L拍照完成写入失败", Color.Red);
                        }
                        Thread.Sleep(100);
                        var lastStartResult = ReadBool(DI.开始);
                        if (!lastStartResult.IsSuccess)
                        {
                            ShowMessage("开始信号读取失败", Color.Red);
                            return;
                        }

                        if (!Write(DO.准备好, true).IsSuccess)
                        {
                            ShowMessage("准备好写入失败", Color.Red);
                        }
                        ShowMessage("输出准备好信号");

                        lblSysStatus.BeginInvoke(new Action(() =>
                        {
                            lblSysStatus.Text = "等待";
                            lblSysStatus.BackColor = Color.Yellow;
                            lblSysStatus.ForeColor = AppTheme.Window;
                        }));

                        ShowMessage("等待开始信号");
                        while (true)
                        {
                            if (stop) return;
                            var val = ReadBool(DI.开始);
                            if (val.IsSuccess)
                            {
                                if ((val.Content == true && lastStartResult.Content == false) || bSkipStart)
                                {
                                    bSkipStart = false;
                                    sp.Start();
                                    ShowMessage("收到开始信号");
                                    break;
                                }
                                lastStartResult = val;
                            }
                            else
                            {
                                ShowMessage("开始信号读取失败", Color.Red);
                                //return;
                            }
                            Thread.Sleep(50);
                        }

                        //是否空运行
                        var dry_mode = ReadBool(DI.空跑);
                        if (!dry_mode.IsSuccess)
                        {
                            ShowMessage("空运行信号读取失败", Color.Red);
                            return;
                        }
                        bDry_mode = dry_mode.Content;
                        ShowMessage("空运行信号" + bDry_mode);

                        //读车型连番
                        var _carKind = ReadUInt16(DI.车型号);
                        if (!_carKind.IsSuccess)
                        {
                            ShowMessage("车型代码读取失败", Color.Red);
                            return;
                        }
                        carKind = _carKind.Content;
                        if (!ushort.TryParse(Convert.ToString(_carKind.Content, 16), out carKind))
                        {
                            ShowMessage($"车型代码{Convert.ToString(_carKind.Content, 16)}转换BCD失败", Color.Red);
                            return;
                        }

                        var _carNum = ReadUInt16(DI.连番号);
                        if (!_carNum.IsSuccess)
                        {
                            ShowMessage("连番信息读取失败", Color.Red);
                            return;
                        }
                        carNum = _carNum.Content;
                        if (!ushort.TryParse(Convert.ToString(_carNum.Content, 16), out carNum))
                        {
                            ShowMessage($"连番信息{Convert.ToString(_carNum.Content, 16)}转换BCD失败", Color.Red);
                            return;
                        }

                        var _TRnum = ReadUInt16(DI.托盘号);
                        if (!_TRnum.IsSuccess)
                        {
                            ShowMessage("托盘号读取失败", Color.Red);
                            return;
                        }
                        TRnum = _TRnum.Content;
                        if (!ushort.TryParse(Convert.ToString(_TRnum.Content, 16), out TRnum))
                        {
                            ShowMessage($"托盘号{Convert.ToString(_TRnum.Content, 16)}转换BCD失败", Color.Red);
                            return;
                        }
#if true
                        var _carVIN = ReadString(DI.车架号, 10);
                        if (!_carVIN.IsSuccess)
                        {
                            ShowMessage("车架号读取失败", Color.Red);
                            return;
                        }
                        carVIN = _carVIN.Content.Substring(0, 19);
#else
                    OperateResult<string> carNumString = new OperateResult<string>();
                    carNumString.Content = "";
#endif
                    }
                    else
                    {
                        sp.Start();
                        if (textBoxTestNum.Text.Trim().Length == 0)
                        {
                            ShowMessage($"仿真车型未填写", Color.Red);
                            return;
                        }
                        string[] strings = textBoxTestNum.Text.Split('-');
                        if (strings.Length >= 1 && ushort.TryParse(strings[0], out carKind))
                        {
                            TRnum = 0;
                            if (strings.Length >= 2 && !ushort.TryParse(strings[1], out TRnum))
                            {
                                ShowMessage($"{textBoxTestNum.Text}转化托盘号失败", Color.Red);
                                return;
                            }
                            carNum = 0;
                            carVIN = "0000000000000000000";
                        }
                        else
                        {
                            ShowMessage($"{textBoxTestNum.Text}转化车型号失败", Color.Red);
                            return;
                        }
                    }

                    ShowMessage("车型号" + carKind);
                    ShowMessage("托盘号" + TRnum);
                    ShowMessage($"连番号{carNum:000}");
                    ShowMessage("车架号" + carVIN);
                    dateTimeNow = DateTime.Now;

                    string pathVIN = carVIN.Trim();
                    var chars = Path.GetInvalidFileNameChars();
                    foreach (char c in chars)
                    {
                        pathVIN = pathVIN.Replace(c, '_');//替换非法字符
                    }
                    if (pathVIN.Length > 0)
                    {
                        pathVIN = " " + pathVIN;
                    }
                    string path = $"{otherSet.imagePath}\\{carKind}\\{TRnum}\\{dateTimeNow:yyyy-MM-dd}\\{dateTimeNow:yyyy-MM-dd HH_mm_ss} {carNum:000}{pathVIN}";


                    if (!车型参数.ContainsKey($"{carKind}-{(otherSet.bTRnum ? TRnum : 0)}"))
                    {
                        string[] carPaths = Directory.GetDirectories(Car.BaseDirectory, $"{carKind}-{(otherSet.bTRnum ? TRnum : 0)}-*");
                        if (carPaths.Length > 0)
                        {
                            string dirName = Path.GetFileNameWithoutExtension(carPaths[0]);
                            string[] strings = dirName.Split('-');
                            if (strings.Length == 3 && int.TryParse(strings[0], out int 车型号) && int.TryParse(strings[1], out int 托盘号))
                            {
                                if (!车型号转名称.ContainsKey(车型号))
                                {
                                    车型号转名称.Add(车型号, strings[2]);
                                }
                                if (!车型参数.ContainsKey($"{车型号}-{托盘号}"))
                                {
                                    CarSetting carSet = new CarSetting(dirName);
                                    if (carSet.Load(carPaths[0]))
                                    {
                                        if (车型参数.Count >= 8)
                                        {
                                            车型参数.Remove(车型参数排序key[0]);
                                            车型参数排序key.Remove(车型参数排序key[0]);
                                        }
                                        车型参数.Add($"{车型号}-{托盘号}", carSet);
                                        车型参数排序key.Add($"{车型号}-{托盘号}");
                                        ShowMessage($"{dirName}参数加载成功");
                                    }
                                    else
                                    {
                                        ShowMessage($"{dirName}参数加载失败", Color.Red);
                                        this.BeginInvoke(new Action(() => { this.Activate(); }));
                                    }
                                }
                                else
                                {
                                    ShowMessage($"{dirName}参数已加载");
                                }
                            }
                        }
                    }
                    else
                    {
                        if (车型号转名称.ContainsKey(carKind))
                        {
                            ShowMessage($"{carKind}-{(otherSet.bTRnum ? TRnum : 0)}-{车型号转名称[carKind]}参数已加载");
                        }
                        else
                        {
                            ShowMessage($"{carKind}-{(otherSet.bTRnum ? TRnum : 0)}参数已加载");
                        }
                    }

                    string carName = carKind.ToString();
                    if (车型号转名称.ContainsKey(carKind))
                    {
                        carName = 车型号转名称[carKind];
                    }

                    int pointNumMax = 0;//最大点位号
                    if (车型参数.ContainsKey($"{carKind}-{(otherSet.bTRnum ? TRnum : 0)}"))
                    {
                        CarSetting carSet = 车型参数[$"{carKind}-{(otherSet.bTRnum ? TRnum : 0)}"];
                        pointNumMax = carSet.gSets.Keys.Max();
                    }
                    //显示
                    formShow?.UpDataCamImage(null, "Clear");///////////////////////////
                    formShow?.UpDataCarInform(carName, carNum.ToString(), TRnum.ToString(), pointNumMax);/////////////////////////
                    lblSysStatus.BeginInvoke(new Action(() =>
                    {
                        lblSysStatus.Text = "运行";
                        lblSysStatus.BackColor = Color.Green;
                        lblSysStatus.ForeColor = Color.White;
                        labelResult.Text = "--";
                        labelResult.BackColor = Color.Transparent;
                        labelResult.ForeColor = AppTheme.Text;

                        labelCarKind.Text = carKind.ToString();
                        labelCarName.Text = 车型号转名称.ContainsKey(carKind) ? 车型号转名称[carKind] : "不存在";
                        label连番.Text = carNum.ToString("000");
                        label托盘.Text = TRnum.ToString();

                        dataGridViewShow.Rows.Clear();
                    }));

                    if (!checkBoxPLC仿真.Checked)
                    {
                        ////信号复位
                        //if (!Write(DO.测量可读, false).IsSuccess)
                        //{
                        //    ShowMessage("测量结果可读写入失败", Color.Red);
                        //}

                        if (!Write(DO.车型反馈, Convert.ToUInt16(carKind.ToString(), 10)).IsSuccess)
                        {
                            ShowMessage("运车型反馈写入失败", Color.Red);
                        }
                        if (!Write(DO.连番反馈, Convert.ToUInt16(carNum.ToString(), 10)).IsSuccess)
                        {
                            ShowMessage("连番反馈写入失败", Color.Red);
                        }

                        if (!Write(DO.运行中, true).IsSuccess)
                        {
                            ShowMessage("运行中写入失败", Color.Red);
                        }
                        if (!Write(DO.准备好, false).IsSuccess)
                        {
                            ShowMessage("准备好写入失败", Color.Red);
                        }

                        ShowMessage("等待车型连番OK信号");
                        while (true)
                        {
                            if (stop) return;
                            var val = ReadBool(DI.车型连番OK);
                            if (val.IsSuccess)
                            {
                                if (val.Content == true)
                                {
                                    ShowMessage("收到车型连番OK信号");
                                    break;
                                }
                            }
                            else
                            {
                                ShowMessage("车型连番OK信号读取失败", Color.Red);
                                return;
                            }
                            var abort = ReadBool(DI.重置);
                            if (!abort.IsSuccess)
                            {
                                ShowMessage("重置流程信号读取失败", Color.Red);
                                return;
                            }
                            if (bAbort || abort.Content)
                            {
                                bAbort = true;
                                ShowMessage("收到重置流程信号", Color.Yellow);
                                break;
                            }
                            Thread.Sleep(50);
                        }
                        if (bAbort) continue;

                        //开灯
                        if (!Write(DO.光源, true).IsSuccess)
                        {
                            ShowMessage("光源写入失败", Color.Red);
                        }
                        Thread.Sleep(100);
                    }

                    tasks.Clear();
                    bool 无异常 = true;
                    bool 无超差 = true;
                    object olock_Dict = new object();
                    Dictionary<int, bool> 测点结果 = new Dictionary<int, bool>();
                    Dictionary<int, Point3D> point3d给PLC = new Dictionary<int, Point3D>();//测点数据
                    Dictionary<int, Point3D> point3d原数据 = new Dictionary<int, Point3D>();//测点数据
                    Dictionary<int, Point3DColor> point3d原颜色 = new Dictionary<int, Point3DColor>();//测点数据颜色
                    // 保存两孔间距结果，供数模界面首次显示、重建显示和调零显示复用。
                    Dictionary<int, Point3D> points3dData = new Dictionary<int, Point3D>();
                    Dictionary<int, Point3DColor> points3dColors = new Dictionary<int, Point3DColor>();
                    Dictionary<int, Dictionary<string, string>> sqlValuePairs = new Dictionary<int, Dictionary<string, string>>();//存数据库的数据
                    List<float> 重建L = new List<float>();
                    List<float> 重建W = new List<float>();
                    List<float> 重建H = new List<float>();
                    //拍照
                    if (车型参数.ContainsKey($"{carKind}-{(otherSet.bTRnum ? TRnum : 0)}"))
                    {
                        CarSetting carSet = 车型参数[$"{carKind}-{(otherSet.bTRnum ? TRnum : 0)}"];
                        //点位主键转相机主键
                        Dictionary<string, List<int>> 相机名_点位号 = new Dictionary<string, List<int>>();
                        foreach (var item in carSet.gSets.Values)
                        {
                            if (相机名_点位号.ContainsKey(item.CamName))
                            {
                                相机名_点位号[item.CamName].Add(item.key);
                            }
                            else
                            {
                                相机名_点位号.Add(item.CamName, new List<int>() { item.key });
                            }
                        }

                        // 改成等待拍照位
                        while (true)
                        {
                            //等待拍照位信息
                            ShowMessage("等待拍照位触发信号或检测完成信号");
                            while (true)
                            {
                                if (stop) return;
                                var val = ReadBool(DI.拍照触发信号);
                                if (val.IsSuccess)
                                {
                                    if (val.Content == true)
                                    {
                                        ShowMessage("收到拍照触发信号");
                                        break;
                                    }
                                }
                                else
                                {
                                    ShowMessage("拍照触发信号读取失败", Color.Red);
                                    return;
                                }

                                if (stop) return;

                                var finish = ReadBool(DI.检测完成);
                                if (finish.IsSuccess)
                                {
                                    if (finish.Content == true)
                                    {
                                        ShowMessage("收到检测完成信号");
                                        break;
                                    }
                                }
                                else
                                {
                                    ShowMessage("检测完成信号读取失败", Color.Red);
                                    return;
                                }

                                var abort = ReadBool(DI.重置);
                                if (!abort.IsSuccess)
                                {
                                    ShowMessage("重置流程信号读取失败", Color.Red);
                                    return;
                                }
                                if (bAbort || abort.Content)
                                {
                                    bAbort = true;
                                    ShowMessage("收到重置流程信号", Color.Yellow);
                                    break;
                                }
                                Thread.Sleep(50);
                            }

                            ShowMessage("读取拍照点位号信号");

                            int 点位号 = -1;
                            string camName = "";
                            var pointID = ReadUInt16(DI.触发拍照点位ID);
                            if (pointID.IsSuccess)
                            {
                                点位号 = pointID.Content;
                            }
                            else
                            {
                                ShowMessage("拍照点位号信号读取失败", Color.Red);
                                return;
                            }


                            //执行拍照
                            var set = carSet.gSets[点位号];
                            camName = set.CamName;
                            int pointNum = set.key;

                            if (相机参数.sets.ContainsKey(camName))
                            {
                                if (!相机参数.sets[camName].gSet.Disabled && !set.keepOK)
                                {
                                    float Px0 = 0, Py0 = 0, Px1 = 0, Py1 = 0;
                                    bool 匹配成功 = false;
                                    // 第二孔未启用时视为不需要进行第二次定位。
                                    bool Points匹配成功 = !set.enablePoints;
                                    HImage hImage2D = null;
                                    HImage hImage2D匹配 = null;

                                    if (!checkBox相机仿真.Checked)
                                    {
                                        if (相机.ContainsKey(camName))
                                        {
                                            if (相机[camName].SetExposure(set.ledExposure))
                                            {
                                                Thread.Sleep(20);
                                                hImage2D = 相机[camName].OneShot();
                                                if (hImage2D == null)
                                                {
                                                    ShowMessage(camName + "拍照失败:" + 相机[camName].ErrMsg, Color.Yellow);
                                                    Thread.Sleep(50);
                                                    hImage2D = 相机[camName].OneShot();
                                                    if (hImage2D == null)
                                                    {
                                                        ShowMessage(camName + "再次拍照失败:" + 相机[camName].ErrMsg, Color.Red);
                                                        formShow?.UpDataMessage(camName + "拍照失败", pointNum.ToString(), set.name);
                                                        无异常 = false;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                ShowMessage(camName + "曝光设置失败:" + 相机[camName].ErrMsg, Color.Red);
                                                formShow?.UpDataMessage(camName + "曝光设置失败", pointNum.ToString(), set.name);
                                                无异常 = false;
                                            }
                                        }
                                        else
                                        {
                                            ShowMessage($"{camName}相机不存在", Color.Red);
                                            formShow?.UpDataMessage($"{camName}相机不存在", pointNum.ToString(), set.name);
                                            无异常 = false;
                                        }
                                    }
                                    else
                                    {
                                        string imagePath = $"test-Image\\{camName}_{pointNum:00}.bmp";
                                        if (!File.Exists(imagePath))
                                        {
                                            imagePath = $"test-Image\\{camName}_{pointNum:00}.png";
                                        }
                                        if (!File.Exists(imagePath))
                                        {
                                            imagePath = $"test-Image\\{camName}_{pointNum:00}.hobj";
                                        }
                                        if (File.Exists(imagePath))
                                        {
                                            try
                                            {
                                                hImage2D = new HImage(imagePath);
                                            }
                                            catch (Exception ex)
                                            {
                                                ShowMessage($"{imagePath}图片加载异常：{ex}", Color.Red);
                                                formShow?.UpDataMessage("图片加载异常", pointNum.ToString(), set.name);
                                            }
                                        }
                                        else
                                        {
                                            ShowMessage($"{imagePath}图片不存在", Color.Red);
                                            formShow?.UpDataMessage("图片不存在", pointNum.ToString(), set.name);
                                        }
                                    }

                                    if (hImage2D != null)
                                    {
                                        var carSetting = carSet;
                                        formShow?.UpDataCamImage(hImage2D, pointNum.ToString());//////////////////////////
                                        try
                                        {
                                            //匹配计算
                                            if (相机参数.sets[camName].camparam != null)
                                            {
                                                if (carSetting.rectangle0.ContainsKey(pointNum) && carSetting.ShapeModels0.ContainsKey(pointNum))
                                                {
                                                    HImage hImage;
                                                    if (carSetting.gSets[pointNum].增强对比度)
                                                    {
                                                        carSetting.rectangle0[pointNum].MinMaxGray(hImage2D, 0, out double min, out double max, out double range);
                                                        if (range > 0)
                                                        {
                                                            double mult = 255d / range;
                                                            double add = -mult * min;
                                                            hImage = hImage2D.ScaleImage(mult, add);
                                                        }
                                                        else
                                                        {
                                                            hImage = hImage2D.Clone();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        hImage = hImage2D.Clone();
                                                    }
                                                    if (carSetting.FindPxPy(hImage, carSetting.rectangle0[pointNum], carSetting.ShapeModels0[pointNum], carSetting.gSets[pointNum].sharpenProcess, carSetting.gSets[pointNum].score0, carSetting.modeCenter0[pointNum].X, carSetting.modeCenter0[pointNum].Y, ref Px0, ref Py0, out double Pdx, out double Pdy, out double score))
                                                    {
                                                        匹配成功 = true;
                                                        ShowMessage($"{camName}_{pointNum}_0 dx = {Pdx:0.000}像素, dy = {Pdy:0.000}像素，分数{score:0.000}");
                                                    }
                                                    else
                                                    {
                                                        ShowMessage($"{camName}_{pointNum} 匹配失败", Color.Red);
                                                        formShow?.UpDataMessage("匹配失败", pointNum.ToString(), set.name);
                                                        无异常 = false;
                                                    }
                                                    // 第二孔使用同一张图片和同一标定，但使用独立的模型和搜索区域。
                                                    if (set.enablePoints && 匹配成功)
                                                    {
                                                        if (!carSetting.HasDetectionData(pointNum, 1))
                                                        {
                                                            ShowMessage($"{camName}_{pointNum}_1 未做参数", Color.Red);
                                                            formShow?.UpDataMessage("第二孔未做参数", pointNum.ToString(), set.name);
                                                            无异常 = false;
                                                        }
                                                        else if (carSetting.FindPxPy(hImage, pointNum, 1, carSetting.gSets[pointNum].sharpenProcess,
                                                            carSetting.gSets[pointNum].score0, ref Px1, ref Py1, out double Pdx1, out double Pdy1, out double score1))
                                                        {
                                                            Points匹配成功 = true;
                                                            ShowMessage($"{camName}_{pointNum}_1 dx = {Pdx1:0.000}像素, dy = {Pdy1:0.000}像素，分数{score1:0.000}");
                                                        }
                                                        else
                                                        {
                                                            ShowMessage($"{camName}_{pointNum}_1 匹配失败", Color.Red);
                                                            formShow?.UpDataMessage("第二孔匹配失败", pointNum.ToString(), set.name);
                                                            无异常 = false;
                                                        }
                                                    }
                                                    if (set.enablePoints && Points匹配成功)
                                                    {
                                                        hImage2D匹配 = carSetting.GetShowHIamge(hImage,
                                                            carSetting.rectangle0[pointNum], carSetting.modeCenter0[pointNum].X, carSetting.modeCenter0[pointNum].Y, Px0, Py0,
                                                            carSetting.rectangle1[pointNum], carSetting.modeCenter1[pointNum].X, carSetting.modeCenter1[pointNum].Y, Px1, Py1);
                                                    }
                                                    else
                                                    {
                                                        hImage2D匹配 = carSetting.GetShowHIamge(hImage, carSetting.rectangle0[pointNum], carSetting.modeCenter0[pointNum].X, carSetting.modeCenter0[pointNum].Y, Px0, Py0, carSetting.ShapeModels0[pointNum]);
                                                    }
                                                    formShow?.UpDataCamImage(hImage2D匹配, pointNum.ToString());/////////////////////////
                                                    hImage.Dispose();
                                                }
                                                else if (carSetting.rectangle0.ContainsKey(pointNum) && carSetting.Models0.ContainsKey(pointNum))
                                                {
                                                    HImage hImage;
                                                    if (carSetting.gSets[pointNum].增强对比度)
                                                    {
                                                        carSetting.rectangle0[pointNum].MinMaxGray(hImage2D, 0, out double min, out double max, out double range);
                                                        if (range > 0)
                                                        {
                                                            double mult = 255d / range;
                                                            double add = -mult * min;
                                                            hImage = hImage2D.ScaleImage(mult, add);
                                                        }
                                                        else
                                                        {
                                                            hImage = hImage2D.Clone();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        hImage = hImage2D.Clone();
                                                    }
                                                    if (carSetting.FindPxPy(hImage, carSetting.rectangle0[pointNum], carSetting.Models0[pointNum], carSetting.gSets[pointNum].score0, carSetting.modeCenter0[pointNum].X, carSetting.modeCenter0[pointNum].Y, ref Px0, ref Py0, out double Pdx, out double Pdy, out double score))
                                                    {
                                                        匹配成功 = true;
                                                        ShowMessage($"{camName}_{pointNum}_0 dx = {Pdx:0.000}像素, dy = {Pdy:0.000}像素，分数{score:0.000}");
                                                    }
                                                    else
                                                    {
                                                        ShowMessage($"{camName}_{pointNum} 匹配失败", Color.Red);
                                                        formShow?.UpDataMessage("匹配失败", pointNum.ToString(), set.name);
                                                        无异常 = false;
                                                    }
                                                    // 第二孔使用同一张图片和同一标定，但使用独立的模型和搜索区域。
                                                    if (set.enablePoints && 匹配成功)
                                                    {
                                                        if (!carSetting.HasDetectionData(pointNum, 1))
                                                        {
                                                            ShowMessage($"{camName}_{pointNum}_1 未做参数", Color.Red);
                                                            formShow?.UpDataMessage("第二孔未做参数", pointNum.ToString(), set.name);
                                                            无异常 = false;
                                                        }
                                                        else if (carSetting.FindPxPy(hImage, pointNum, 1, carSetting.gSets[pointNum].sharpenProcess,
                                                            carSetting.gSets[pointNum].score0, ref Px1, ref Py1, out double Pdx1, out double Pdy1, out double score1))
                                                        {
                                                            Points匹配成功 = true;
                                                            ShowMessage($"{camName}_{pointNum}_1 dx = {Pdx1:0.000}像素, dy = {Pdy1:0.000}像素，分数{score1:0.000}");
                                                        }
                                                        else
                                                        {
                                                            ShowMessage($"{camName}_{pointNum}_1 匹配失败", Color.Red);
                                                            formShow?.UpDataMessage("第二孔匹配失败", pointNum.ToString(), set.name);
                                                            无异常 = false;
                                                        }
                                                    }
                                                    if (set.enablePoints && Points匹配成功)
                                                    {
                                                        hImage2D匹配 = carSetting.GetShowHIamge(hImage,
                                                            carSetting.rectangle0[pointNum], carSetting.modeCenter0[pointNum].X, carSetting.modeCenter0[pointNum].Y, Px0, Py0,
                                                            carSetting.rectangle1[pointNum], carSetting.modeCenter1[pointNum].X, carSetting.modeCenter1[pointNum].Y, Px1, Py1);
                                                    }
                                                    else
                                                    {
                                                        hImage2D匹配 = carSetting.GetShowHIamge(hImage, carSetting.rectangle0[pointNum], carSetting.modeCenter0[pointNum].X, carSetting.modeCenter0[pointNum].Y, Px0, Py0);
                                                    }
                                                    formShow?.UpDataCamImage(hImage2D匹配, pointNum.ToString());/////////////////////////
                                                }
                                                else
                                                {
                                                    ShowMessage($"{camName}_{pointNum} 未做参数", Color.Yellow);
                                                    formShow?.UpDataMessage($"未做参数", pointNum.ToString(), set.name);
                                                    无异常 = false;
                                                }

                                                if (匹配成功)
                                                {
                                                    if (carSetting.camPose.ContainsKey(pointNum))
                                                    {
                                                        相机参数.sets[camName].GetXY(carSetting.camPose[pointNum], Px0, Py0, out float nx, out float ny);
                                                        相机参数.sets[camName].GetXY(carSetting.camPose[pointNum], carSetting.modeCenter0[pointNum].X, carSetting.modeCenter0[pointNum].Y, out float ox, out float oy);
                                                        float dx = (float)Math.Round(nx - ox, 2);
                                                        float dy = (float)Math.Round(ny - oy, 2);

                                                        // 两孔距离默认显示为红色，只有第二孔定位、坐标转换和阈值判断全部完成后才更新颜色。
                                                        float PointsDX = 0, PointsDY = 0, PointsDZ = 0, PointsDD = 0;
                                                        Color PointsColorX = set.enablePoints ? Color.Red : Color.White;
                                                        Color PointsColorY = set.enablePoints ? Color.Red : Color.White;
                                                        Color PointsColorZ = set.enablePoints ? Color.Red : Color.White;
                                                        Color PointsColorD = set.enablePoints ? Color.Red : Color.White;
                                                        bool pointsDistanceOK = !set.enablePoints;
                                                        if (set.enablePoints && Points匹配成功)
                                                        {
                                                            float actual0X = 0, actual0Y = 0, actual1X = 0, actual1Y = 0;
                                                            float nominal0X = 0, nominal0Y = 0, nominal1X = 0, nominal1Y = 0;
                                                            bool pointsCoordinateOK = 相机参数.sets[camName].GetXY(carSetting.camPose[pointNum], Px0, Py0, out actual0X, out actual0Y);
                                                            pointsCoordinateOK = 相机参数.sets[camName].GetXY(carSetting.camPose[pointNum], Px1, Py1, out actual1X, out actual1Y) && pointsCoordinateOK;
                                                            pointsCoordinateOK = 相机参数.sets[camName].GetXY(carSetting.camPose[pointNum], carSetting.modeCenter0[pointNum].X, carSetting.modeCenter0[pointNum].Y, out nominal0X, out nominal0Y) && pointsCoordinateOK;
                                                            pointsCoordinateOK = 相机参数.sets[camName].GetXY(carSetting.camPose[pointNum], carSetting.modeCenter1[pointNum].X, carSetting.modeCenter1[pointNum].Y, out nominal1X, out nominal1Y) && pointsCoordinateOK;
                                                            if (pointsCoordinateOK)
                                                            {
                                                                Point3D actualPoint0 = ConvertXYToAxes(set, actual0X, actual0Y);
                                                                Point3D actualPoint1 = ConvertXYToAxes(set, actual1X, actual1Y);
                                                                Point3D nominalPoint0 = ConvertXYToAxes(set, nominal0X, nominal0Y);
                                                                Point3D nominalPoint1 = ConvertXYToAxes(set, nominal1X, nominal1Y);
                                                                // 先按定义计算误差向量，再对显示值和阈值值保留两位小数。
                                                                double pointsErrorX = Math.Abs(actualPoint1.X - actualPoint0.X) - Math.Abs(nominalPoint1.X - nominalPoint0.X);
                                                                double pointsErrorY = Math.Abs(actualPoint1.Y - actualPoint0.Y) - Math.Abs(nominalPoint1.Y - nominalPoint0.Y);
                                                                double pointsErrorZ = Math.Abs(actualPoint1.Z - actualPoint0.Z) - Math.Abs(nominalPoint1.Z - nominalPoint0.Z);
                                                                PointsDX = (float)Math.Round(pointsErrorX, 2);
                                                                PointsDY = (float)Math.Round(pointsErrorY, 2);
                                                                PointsDZ = (float)Math.Round(pointsErrorZ, 2);
                                                                PointsDD = (float)Math.Round(Math.Sqrt(pointsErrorX * pointsErrorX + pointsErrorY * pointsErrorY + pointsErrorZ * pointsErrorZ), 2);
                                                                pointsDistanceOK = EvaluatePointsDistance(set, PointsDX, PointsDY, PointsDZ, PointsDD,
                                                                    out PointsColorX, out PointsColorY, out PointsColorZ, out PointsColorD);
                                                                if (!pointsDistanceOK)
                                                                {
                                                                    无超差 = false;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                ShowMessage($"{camName}_{pointNum} 两孔坐标转换失败", Color.Red);
                                                                formShow?.UpDataMessage("两孔坐标转换失败", pointNum.ToString(), set.name);
                                                                无异常 = false;
                                                            }
                                                        }

                                                        float DX = float.NaN, DY = float.NaN, DZ = float.NaN;//没测的方向显示白色
                                                        switch (carSetting.gSets[pointNum].convertX)
                                                        {
                                                            case "X": DX = dx; break;
                                                            case "-X": DX = -dx; break;
                                                            case "Y": DY = dx; break;
                                                            case "-Y": DY = -dx; break;
                                                            case "Z": DZ = dx; break;
                                                            case "-Z": DZ = -dx; break;
                                                            default: DX = dx; break;
                                                        }
                                                        switch (carSetting.gSets[pointNum].convertY)
                                                        {
                                                            case "X": DX = dy; break;
                                                            case "-X": DX = -dy; break;
                                                            case "Y": DY = dy; break;
                                                            case "-Y": DY = -dy; break;
                                                            case "Z": DZ = dy; break;
                                                            case "-Z": DZ = -dy; break;
                                                            default: DX = dy; break;
                                                        }

                                                        //匹配成功先赋OK，超差再赋NG
                                                        lock (olock_Dict)
                                                        {
                                                            if (测点结果.ContainsKey(pointNum))
                                                            {
                                                                测点结果[pointNum] = true;
                                                            }
                                                            else
                                                            {
                                                                测点结果.Add(pointNum, true);
                                                            }
                                                        }
                                                        if (set.enablePoints && !pointsDistanceOK)
                                                        {
                                                            测点结果[pointNum] = false;
                                                        }
                                                        float L = 0, W = 0, H = 0;
                                                        Color colorX = Color.White, colorY = Color.White, colorZ = Color.White;
                                                        //判断X
                                                        if (!DX.Equals(float.NaN))
                                                        {
                                                            DX += carSetting.gSets[pointNum].offsetX;//补偿
                                                            if (DX < carSetting.gSets[pointNum].minDX_2 || DX > carSetting.gSets[pointNum].maxDX_2)
                                                            {
                                                                colorX = Color.Red;
                                                                无超差 = false;
                                                                测点结果[pointNum] = false;
                                                            }
                                                            else if (DX < carSetting.gSets[pointNum].minDX_1 || DX > carSetting.gSets[pointNum].maxDX_1)
                                                            {
                                                                colorX = Color.Yellow;
                                                            }
                                                            else
                                                            {
                                                                colorX = Color.Green;
                                                            }
                                                            if (carSetting.gSets[pointNum].相对基准点)
                                                            {
                                                                重建L.Add(DX);
                                                            }
                                                            L = DX;
                                                        }
                                                        //判断Y
                                                        if (!DY.Equals(float.NaN))
                                                        {
                                                            DY += carSetting.gSets[pointNum].offsetY;//补偿
                                                            if (DY < carSetting.gSets[pointNum].minDY_2 || DY > carSetting.gSets[pointNum].maxDY_2)
                                                            {
                                                                colorY = Color.Red;
                                                                无超差 = false;
                                                                测点结果[pointNum] = false;
                                                            }
                                                            else if (DY < carSetting.gSets[pointNum].minDY_1 || DY > carSetting.gSets[pointNum].maxDY_1)
                                                            {
                                                                colorY = Color.Yellow;
                                                            }
                                                            else
                                                            {
                                                                colorY = Color.Green;
                                                            }
                                                            if (carSetting.gSets[pointNum].相对基准点)
                                                            {
                                                                重建W.Add(DY);
                                                            }
                                                            W = DY;
                                                        }
                                                        //判断Z
                                                        if (!DZ.Equals(float.NaN))
                                                        {
                                                            DZ += carSetting.gSets[pointNum].offsetZ;//补偿
                                                            if (DZ < carSetting.gSets[pointNum].minDZ_2 || DZ > carSetting.gSets[pointNum].maxDZ_2)
                                                            {
                                                                colorZ = Color.Red;
                                                                无超差 = false;
                                                                测点结果[pointNum] = false;
                                                            }
                                                            else if (DZ < carSetting.gSets[pointNum].minDZ_1 || DZ > carSetting.gSets[pointNum].maxDZ_1)
                                                            {
                                                                colorZ = Color.Yellow;
                                                            }
                                                            else
                                                            {
                                                                colorZ = Color.Green;
                                                            }
                                                            if (carSetting.gSets[pointNum].相对基准点)
                                                            {
                                                                重建H.Add(DZ);
                                                            }
                                                            H = DZ;
                                                        }
                                                        float dd = (float)Math.Round(Math.Sqrt(L * L + W * W + H * H), 2);

                                                        Dictionary<string, string> 测点数据表 = new Dictionary<string, string>();
                                                        测点数据表.Add("时间", dateTimeNow.ToString("yyyy-MM-dd HH:mm:ss"));
                                                        测点数据表.Add("相机名", camName);
                                                        测点数据表.Add("点位号", pointNum.ToString());
                                                        测点数据表.Add("DX", L.ToString("0.00"));
                                                        测点数据表.Add("DY", W.ToString("0.00"));
                                                        测点数据表.Add("DZ", H.ToString("0.00"));
                                                        测点数据表.Add("DD", dd.ToString("0.00"));
                                                        if (set.enablePoints)
                                                        {
                                                            // 两孔相对距离误差写入数据库；PLC仍只接收原有单孔测量数据。
                                                            测点数据表.Add("PointsDX", PointsDX.ToString("0.00"));
                                                            测点数据表.Add("PointsDY", PointsDY.ToString("0.00"));
                                                            测点数据表.Add("PointsDZ", PointsDZ.ToString("0.00"));
                                                            测点数据表.Add("PointsDD", PointsDD.ToString("0.00"));
                                                        }

                                                        lock (olock_Dict)
                                                        {
                                                            if (!point3d给PLC.ContainsKey(pointNum))
                                                            {
                                                                point3d给PLC.Add(pointNum, new Point3D(L, W, H));
                                                                point3d原数据.Add(pointNum, new Point3D(DX, DY, DZ));
                                                                point3d原颜色.Add(pointNum, new Point3DColor(colorX, colorY, colorZ));
                                                                if (set.enablePoints)
                                                                {
                                                                    points3dData.Add(pointNum, new Point3D(PointsDX, PointsDY, PointsDZ));
                                                                    points3dColors.Add(pointNum, new Point3DColor(PointsColorX, PointsColorY, PointsColorZ));
                                                                }
                                                                sqlValuePairs.Add(pointNum, 测点数据表);
                                                            }
                                                            else
                                                            {
                                                                ShowMessage($"点位号{pointNum}重复", Color.Red);
                                                                无异常 = false;
                                                            }
                                                        }

                                                        //显示数据
                                                        dataGridViewShow.BeginInvoke(new Action(() =>
                                                        {
                                                            int index = dataGridViewShow.Rows.Add();
                                                            dataGridViewShow.Rows[index].Cells["pointNum"].Value = pointNum;
                                                            dataGridViewShow.Rows[index].Cells["cam"].Value = carSetting.gSets[pointNum].CamName;
                                                            dataGridViewShow.Rows[index].Cells["name"].Value = carSetting.gSets[pointNum].name;
                                                            dataGridViewShow.Rows[index].Cells["dx"].Value = L.ToString("0.00");
                                                            dataGridViewShow.Rows[index].Cells["dy"].Value = W.ToString("0.00");
                                                            dataGridViewShow.Rows[index].Cells["dz"].Value = H.ToString("0.00");
                                                            dataGridViewShow.Rows[index].Cells["dd"].Value = dd.ToString("0.00");

                                                            dataGridViewShow.Rows[index].Cells["dx"].Style.BackColor = colorX;
                                                            dataGridViewShow.Rows[index].Cells["dy"].Style.BackColor = colorY;
                                                            dataGridViewShow.Rows[index].Cells["dz"].Style.BackColor = colorZ;
                                                            if (set.enablePoints)
                                                            {
                                                                dataGridViewShow.Rows[index].Cells["PointsDX"].Value = PointsDX.ToString("0.00");
                                                                dataGridViewShow.Rows[index].Cells["PointsDY"].Value = PointsDY.ToString("0.00");
                                                                dataGridViewShow.Rows[index].Cells["PointsDZ"].Value = PointsDZ.ToString("0.00");
                                                                dataGridViewShow.Rows[index].Cells["PointsDD"].Value = PointsDD.ToString("0.00");
                                                                dataGridViewShow.Rows[index].Cells["PointsDX"].Style.BackColor = PointsColorX;
                                                                dataGridViewShow.Rows[index].Cells["PointsDY"].Style.BackColor = PointsColorY;
                                                                dataGridViewShow.Rows[index].Cells["PointsDZ"].Style.BackColor = PointsColorZ;
                                                                dataGridViewShow.Rows[index].Cells["PointsDD"].Style.BackColor = PointsColorD;
                                                            }
                                                        }));

                                                        formShow?.UpDataXYZ(
                                                            point3d给PLC[pointNum],
                                                            point3d原颜色[pointNum],
                                                            pointNum.ToString(),
                                                            carSetting.gSets[pointNum].name,
                                                            set.enablePoints ? new Point3D(PointsDX, PointsDY, PointsDZ) : null,
                                                            set.enablePoints ? new Point3DColor(PointsColorX, PointsColorY, PointsColorZ) : null);////////////////////////////////////
                                                    }
                                                    else
                                                    {
                                                        ShowMessage($"{camName}_{pointNum} 未做位姿标定", Color.Red);
                                                        formShow?.UpDataMessage($"未做位姿标定", pointNum.ToString(), set.name);
                                                        无异常 = false;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                ShowMessage($"{camName}相机未做标定", Color.Red);
                                                formShow?.UpDataMessage($"{camName}相机未标定", pointNum.ToString(), set.name);
                                                无异常 = false;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ShowMessage($"{camName}_{pointNum} 匹配异常:{ex}", Color.Red);
                                            formShow?.UpDataMessage("匹配异常", pointNum.ToString(), set.name);
                                            无异常 = false;
                                        }

                                        //存图
                                        if (!checkBox相机仿真.Checked)
                                        {
                                            if (otherSet.isSaveImage && !bDry_mode)
                                            {
                                                lock (lockSaveImage)
                                                {
                                                    if (ListImageKeys.Count < 120)
                                                    {
                                                        {
                                                            string key = $"{path}\\{camName}_{pointNum:00}.png";
                                                            ListImageKeys.Add(key);
                                                            DicImages.Add(key, hImage2D);
                                                        }
                                                        {
                                                            string key = $"{path}\\{camName}_{pointNum:00}p.jpg";
                                                            ListImageKeys.Add(key);
                                                            DicImages.Add(key, hImage2D匹配);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ShowMessage($"{camName}_{pointNum:00} 内存中有{ListImageKeys.Count}张图未存进硬盘，跳过本次存图", Color.Yellow);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            try
                                            {
                                                hImage2D匹配?.WriteImage("jpeg 80", 0, $"test-Image\\{camName}_{pointNum:00}p.jpg");
                                            }
                                            catch (Exception ex)
                                            {
                                                ShowMessage("仿真存图异常" + ex.ToString(), Color.Red);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    lock (olock_Dict)
                                    {
                                        if (测点结果.ContainsKey(pointNum))
                                        {
                                            测点结果[pointNum] = true;
                                        }
                                        else
                                        {
                                            测点结果.Add(pointNum, true);
                                        }
                                    }

                                    if (相机参数.sets[camName].gSet.Disabled)
                                    {
                                        ShowMessage($"车型{carKind}-{TRnum} 相机{camName} 已屏蔽", Color.Yellow);
                                        formShow?.UpDataMessage("相机已屏蔽", pointNum.ToString(), set.name, false);
                                    }
                                    else if (set.keepOK)
                                    {
                                        ShowMessage($"车型{carKind}-{TRnum} 点位{pointNum} 已屏蔽", Color.Yellow);
                                        formShow?.UpDataMessage("测点已屏蔽", pointNum.ToString(), set.name, false);
                                    }
                                }
                            }
                            else
                            {
                                ShowMessage($"车型{carKind}-{TRnum} 相机{camName} 不存在", Color.Red);
                                formShow?.UpDataMessage("相机不存在", pointNum.ToString(), set.name);
                            }

                            //数据排序写入，必须要有
                            lock (olock_Dict)
                            {
                                if (!point3d给PLC.ContainsKey(pointNum))
                                {
                                    point3d给PLC.Add(pointNum, new Point3D(0, 0, 0));
                                }
                            }

                            //输出检测完成信号


                            if (!Write(DO.单次拍照完成, true).IsSuccess)
                            {
                                ShowMessage("单次拍照完成写入失败", Color.Red);
                            }
                            ShowMessage("输出单次拍照完成信号");


                            ////判断是否所有都拍完了

                            //if (point3d给PLC.Count == carSet.gSets.Count)
                            //{
                            //    //所有点都检测完
                            //    ShowMessage("所有点都检测完");
                            //    break;
                            //}

                        }

                        ////遍历使用到的相机
                        //foreach (var camName in 相机名_点位号.Keys)
                        //{
                        //    tasks.Add(camName, Task.Run(() =>
                        //    {
                        //        //遍历点位
                        //        foreach (var 点位号 in 相机名_点位号[camName])
                        //        {
                        //            var set = carSet.gSets[点位号];
                        //            int pointNum = set.key;

                        //            if (相机参数.sets.ContainsKey(camName))
                        //            {
                        //                if (!相机参数.sets[camName].gSet.Disabled && !set.keepOK)
                        //                {
                        //                    float Px0 = 0, Py0 = 0, Px1 = 0, Py1 = 0;
                        //                    bool 匹配成功 = false;
                        //                    // 第二孔未启用时视为不需要进行第二次定位。
                        //                    bool Points匹配成功 = !set.enablePoints;
                        //                    HImage hImage2D = null;
                        //                    HImage hImage2D匹配 = null;

                        //                    if (!checkBox相机仿真.Checked)
                        //                    {
                        //                        if (相机.ContainsKey(camName))
                        //                        {
                        //                            if (相机[camName].SetExposure(set.ledExposure))
                        //                            {
                        //                                Thread.Sleep(20);
                        //                                hImage2D = 相机[camName].OneShot();
                        //                                if (hImage2D == null)
                        //                                {
                        //                                    ShowMessage(camName + "拍照失败:" + 相机[camName].ErrMsg, Color.Yellow);
                        //                                    Thread.Sleep(50);
                        //                                    hImage2D = 相机[camName].OneShot();
                        //                                    if (hImage2D == null)
                        //                                    {
                        //                                        ShowMessage(camName + "再次拍照失败:" + 相机[camName].ErrMsg, Color.Red);
                        //                                        formShow?.UpDataMessage(camName + "拍照失败", pointNum.ToString(), set.name);
                        //                                        无异常 = false;
                        //                                    }
                        //                                }
                        //                            }
                        //                            else
                        //                            {
                        //                                ShowMessage(camName + "曝光设置失败:" + 相机[camName].ErrMsg, Color.Red);
                        //                                formShow?.UpDataMessage(camName + "曝光设置失败", pointNum.ToString(), set.name);
                        //                                无异常 = false;
                        //                            }
                        //                        }
                        //                        else
                        //                        {
                        //                            ShowMessage($"{camName}相机不存在", Color.Red);
                        //                            formShow?.UpDataMessage($"{camName}相机不存在", pointNum.ToString(), set.name);
                        //                            无异常 = false;
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        string imagePath = $"test-Image\\{camName}_{pointNum:00}.bmp";
                        //                        if (!File.Exists(imagePath))
                        //                        {
                        //                            imagePath = $"test-Image\\{camName}_{pointNum:00}.png";
                        //                        }
                        //                        if (!File.Exists(imagePath))
                        //                        {
                        //                            imagePath = $"test-Image\\{camName}_{pointNum:00}.hobj";
                        //                        }
                        //                        if (File.Exists(imagePath))
                        //                        {
                        //                            try
                        //                            {
                        //                                hImage2D = new HImage(imagePath);
                        //                            }
                        //                            catch (Exception ex)
                        //                            {
                        //                                ShowMessage($"{imagePath}图片加载异常：{ex}", Color.Red);
                        //                                formShow?.UpDataMessage("图片加载异常", pointNum.ToString(), set.name);
                        //                            }
                        //                        }
                        //                        else
                        //                        {
                        //                            ShowMessage($"{imagePath}图片不存在", Color.Red);
                        //                            formShow?.UpDataMessage("图片不存在", pointNum.ToString(), set.name);
                        //                        }
                        //                    }

                        //                    if (hImage2D != null)
                        //                    {
                        //                        var carSetting = carSet;
                        //                        formShow?.UpDataCamImage(hImage2D, pointNum.ToString());//////////////////////////
                        //                        try
                        //                        {
                        //                            //匹配计算
                        //                            if (相机参数.sets[camName].camparam != null)
                        //                            {
                        //                                if (carSetting.rectangle0.ContainsKey(pointNum) && carSetting.ShapeModels0.ContainsKey(pointNum))
                        //                                {
                        //                                    HImage hImage;
                        //                                    if (carSetting.gSets[pointNum].增强对比度)
                        //                                    {
                        //                                        carSetting.rectangle0[pointNum].MinMaxGray(hImage2D, 0, out double min, out double max, out double range);
                        //                                        if (range > 0)
                        //                                        {
                        //                                            double mult = 255d / range;
                        //                                            double add = -mult * min;
                        //                                            hImage = hImage2D.ScaleImage(mult, add);
                        //                                        }
                        //                                        else
                        //                                        {
                        //                                            hImage = hImage2D.Clone();
                        //                                        }
                        //                                    }
                        //                                    else
                        //                                    {
                        //                                        hImage = hImage2D.Clone();
                        //                                    }
                        //                                    if (carSetting.FindPxPy(hImage, carSetting.rectangle0[pointNum], carSetting.ShapeModels0[pointNum], carSetting.gSets[pointNum].sharpenProcess,carSetting.gSets[pointNum].score0, carSetting.modeCenter0[pointNum].X, carSetting.modeCenter0[pointNum].Y, ref Px0, ref Py0, out double Pdx, out double Pdy, out double score))
                        //                                    {
                        //                                        匹配成功 = true;
                        //                                        ShowMessage($"{camName}_{pointNum}_0 dx = {Pdx:0.000}像素, dy = {Pdy:0.000}像素，分数{score:0.000}");
                        //                                    }
                        //                                    else
                        //                                    {
                        //                                        ShowMessage($"{camName}_{pointNum} 匹配失败", Color.Red);
                        //                                        formShow?.UpDataMessage("匹配失败", pointNum.ToString(), set.name);
                        //                                        无异常 = false;
                        //                                    }
                        //                                    // 第二孔使用同一张图片和同一标定，但使用独立的模型和搜索区域。
                        //                                    if (set.enablePoints && 匹配成功)
                        //                                    {
                        //                                        if (!carSetting.HasDetectionData(pointNum, 1))
                        //                                        {
                        //                                            ShowMessage($"{camName}_{pointNum}_1 未做参数", Color.Red);
                        //                                            formShow?.UpDataMessage("第二孔未做参数", pointNum.ToString(), set.name);
                        //                                            无异常 = false;
                        //                                        }
                        //                                        else if (carSetting.FindPxPy(hImage, pointNum, 1, carSetting.gSets[pointNum].sharpenProcess,
                        //                                            carSetting.gSets[pointNum].score0, ref Px1, ref Py1, out double Pdx1, out double Pdy1, out double score1))
                        //                                        {
                        //                                            Points匹配成功 = true;
                        //                                            ShowMessage($"{camName}_{pointNum}_1 dx = {Pdx1:0.000}像素, dy = {Pdy1:0.000}像素，分数{score1:0.000}");
                        //                                        }
                        //                                        else
                        //                                        {
                        //                                            ShowMessage($"{camName}_{pointNum}_1 匹配失败", Color.Red);
                        //                                            formShow?.UpDataMessage("第二孔匹配失败", pointNum.ToString(), set.name);
                        //                                            无异常 = false;
                        //                                        }
                        //                                    }
                        //                                    if (set.enablePoints && Points匹配成功)
                        //                                    {
                        //                                        hImage2D匹配 = carSetting.GetShowHIamge(hImage,
                        //                                            carSetting.rectangle0[pointNum], carSetting.modeCenter0[pointNum].X, carSetting.modeCenter0[pointNum].Y, Px0, Py0,
                        //                                            carSetting.rectangle1[pointNum], carSetting.modeCenter1[pointNum].X, carSetting.modeCenter1[pointNum].Y, Px1, Py1);
                        //                                    }
                        //                                    else
                        //                                    {
                        //                                        hImage2D匹配 = carSetting.GetShowHIamge(hImage, carSetting.rectangle0[pointNum], carSetting.modeCenter0[pointNum].X, carSetting.modeCenter0[pointNum].Y, Px0, Py0, carSetting.ShapeModels0[pointNum]);
                        //                                    }
                        //                                    formShow?.UpDataCamImage(hImage2D匹配, pointNum.ToString());/////////////////////////
                        //                                    hImage.Dispose();
                        //                                }
                        //                                else if (carSetting.rectangle0.ContainsKey(pointNum) && carSetting.Models0.ContainsKey(pointNum))
                        //                                {
                        //                                    HImage hImage;
                        //                                    if (carSetting.gSets[pointNum].增强对比度)
                        //                                    {
                        //                                        carSetting.rectangle0[pointNum].MinMaxGray(hImage2D, 0, out double min, out double max, out double range);
                        //                                        if (range > 0)
                        //                                        {
                        //                                            double mult = 255d / range;
                        //                                            double add = -mult * min;
                        //                                            hImage = hImage2D.ScaleImage(mult, add);
                        //                                        }
                        //                                        else
                        //                                        {
                        //                                            hImage = hImage2D.Clone();
                        //                                        }
                        //                                    }
                        //                                    else
                        //                                    {
                        //                                        hImage = hImage2D.Clone();
                        //                                    }
                        //                                    if (carSetting.FindPxPy(hImage, carSetting.rectangle0[pointNum], carSetting.Models0[pointNum], carSetting.gSets[pointNum].score0, carSetting.modeCenter0[pointNum].X, carSetting.modeCenter0[pointNum].Y, ref Px0, ref Py0, out double Pdx, out double Pdy, out double score))
                        //                                    {
                        //                                        匹配成功 = true;
                        //                                        ShowMessage($"{camName}_{pointNum}_0 dx = {Pdx:0.000}像素, dy = {Pdy:0.000}像素，分数{score:0.000}");
                        //                                    }
                        //                                    else
                        //                                    {
                        //                                        ShowMessage($"{camName}_{pointNum} 匹配失败", Color.Red);
                        //                                        formShow?.UpDataMessage("匹配失败", pointNum.ToString(), set.name);
                        //                                        无异常 = false;
                        //                                    }
                        //                                    // 第二孔使用同一张图片和同一标定，但使用独立的模型和搜索区域。
                        //                                    if (set.enablePoints && 匹配成功)
                        //                                    {
                        //                                        if (!carSetting.HasDetectionData(pointNum, 1))
                        //                                        {
                        //                                            ShowMessage($"{camName}_{pointNum}_1 未做参数", Color.Red);
                        //                                            formShow?.UpDataMessage("第二孔未做参数", pointNum.ToString(), set.name);
                        //                                            无异常 = false;
                        //                                        }
                        //                                        else if (carSetting.FindPxPy(hImage, pointNum, 1, carSetting.gSets[pointNum].sharpenProcess,
                        //                                            carSetting.gSets[pointNum].score0, ref Px1, ref Py1, out double Pdx1, out double Pdy1, out double score1))
                        //                                        {
                        //                                            Points匹配成功 = true;
                        //                                            ShowMessage($"{camName}_{pointNum}_1 dx = {Pdx1:0.000}像素, dy = {Pdy1:0.000}像素，分数{score1:0.000}");
                        //                                        }
                        //                                        else
                        //                                        {
                        //                                            ShowMessage($"{camName}_{pointNum}_1 匹配失败", Color.Red);
                        //                                            formShow?.UpDataMessage("第二孔匹配失败", pointNum.ToString(), set.name);
                        //                                            无异常 = false;
                        //                                        }
                        //                                    }
                        //                                    if (set.enablePoints && Points匹配成功)
                        //                                    {
                        //                                        hImage2D匹配 = carSetting.GetShowHIamge(hImage,
                        //                                            carSetting.rectangle0[pointNum], carSetting.modeCenter0[pointNum].X, carSetting.modeCenter0[pointNum].Y, Px0, Py0,
                        //                                            carSetting.rectangle1[pointNum], carSetting.modeCenter1[pointNum].X, carSetting.modeCenter1[pointNum].Y, Px1, Py1);
                        //                                    }
                        //                                    else
                        //                                    {
                        //                                        hImage2D匹配 = carSetting.GetShowHIamge(hImage, carSetting.rectangle0[pointNum], carSetting.modeCenter0[pointNum].X, carSetting.modeCenter0[pointNum].Y, Px0, Py0);
                        //                                    }
                        //                                    formShow?.UpDataCamImage(hImage2D匹配, pointNum.ToString());/////////////////////////
                        //                                }
                        //                                else
                        //                                {
                        //                                    ShowMessage($"{camName}_{pointNum} 未做参数", Color.Yellow);
                        //                                    formShow?.UpDataMessage($"未做参数", pointNum.ToString(), set.name);
                        //                                    无异常 = false;
                        //                                }

                        //                                if (匹配成功)
                        //                                {
                        //                                    if (carSetting.camPose.ContainsKey(pointNum))
                        //                                    {
                        //                                        相机参数.sets[camName].GetXY(carSetting.camPose[pointNum], Px0, Py0, out float nx, out float ny);
                        //                                        相机参数.sets[camName].GetXY(carSetting.camPose[pointNum], carSetting.modeCenter0[pointNum].X, carSetting.modeCenter0[pointNum].Y, out float ox, out float oy);
                        //                                        float dx = (float)Math.Round(nx - ox, 2);
                        //                                        float dy = (float)Math.Round(ny - oy, 2);

                        //                                        // 两孔距离默认显示为红色，只有第二孔定位、坐标转换和阈值判断全部完成后才更新颜色。
                        //                                        float PointsDX = 0, PointsDY = 0, PointsDZ = 0, PointsDD = 0;
                        //                                        Color PointsColorX = set.enablePoints ? Color.Red : Color.White;
                        //                                        Color PointsColorY = set.enablePoints ? Color.Red : Color.White;
                        //                                        Color PointsColorZ = set.enablePoints ? Color.Red : Color.White;
                        //                                        Color PointsColorD = set.enablePoints ? Color.Red : Color.White;
                        //                                        bool pointsDistanceOK = !set.enablePoints;
                        //                                        if (set.enablePoints && Points匹配成功)
                        //                                        {
                        //                                            float actual0X = 0, actual0Y = 0, actual1X = 0, actual1Y = 0;
                        //                                            float nominal0X = 0, nominal0Y = 0, nominal1X = 0, nominal1Y = 0;
                        //                                            bool pointsCoordinateOK = 相机参数.sets[camName].GetXY(carSetting.camPose[pointNum], Px0, Py0, out actual0X, out actual0Y);
                        //                                            pointsCoordinateOK = 相机参数.sets[camName].GetXY(carSetting.camPose[pointNum], Px1, Py1, out actual1X, out actual1Y) && pointsCoordinateOK;
                        //                                            pointsCoordinateOK = 相机参数.sets[camName].GetXY(carSetting.camPose[pointNum], carSetting.modeCenter0[pointNum].X, carSetting.modeCenter0[pointNum].Y, out nominal0X, out nominal0Y) && pointsCoordinateOK;
                        //                                            pointsCoordinateOK = 相机参数.sets[camName].GetXY(carSetting.camPose[pointNum], carSetting.modeCenter1[pointNum].X, carSetting.modeCenter1[pointNum].Y, out nominal1X, out nominal1Y) && pointsCoordinateOK;
                        //                                            if (pointsCoordinateOK)
                        //                                            {
                        //                                                Point3D actualPoint0 = ConvertXYToAxes(set, actual0X, actual0Y);
                        //                                                Point3D actualPoint1 = ConvertXYToAxes(set, actual1X, actual1Y);
                        //                                                Point3D nominalPoint0 = ConvertXYToAxes(set, nominal0X, nominal0Y);
                        //                                                Point3D nominalPoint1 = ConvertXYToAxes(set, nominal1X, nominal1Y);
                        //                                                // 先按定义计算误差向量，再对显示值和阈值值保留两位小数。
                        //                                                double pointsErrorX = Math.Abs(actualPoint1.X - actualPoint0.X) - Math.Abs(nominalPoint1.X - nominalPoint0.X);
                        //                                                double pointsErrorY = Math.Abs(actualPoint1.Y - actualPoint0.Y) - Math.Abs(nominalPoint1.Y - nominalPoint0.Y);
                        //                                                double pointsErrorZ = Math.Abs(actualPoint1.Z - actualPoint0.Z) - Math.Abs(nominalPoint1.Z - nominalPoint0.Z);
                        //                                                PointsDX = (float)Math.Round(pointsErrorX, 2);
                        //                                                PointsDY = (float)Math.Round(pointsErrorY, 2);
                        //                                                PointsDZ = (float)Math.Round(pointsErrorZ, 2);
                        //                                                PointsDD = (float)Math.Round(Math.Sqrt(pointsErrorX * pointsErrorX + pointsErrorY * pointsErrorY + pointsErrorZ * pointsErrorZ), 2);
                        //                                                pointsDistanceOK = EvaluatePointsDistance(set, PointsDX, PointsDY, PointsDZ, PointsDD,
                        //                                                    out PointsColorX, out PointsColorY, out PointsColorZ, out PointsColorD);
                        //                                                if (!pointsDistanceOK)
                        //                                                {
                        //                                                    无超差 = false;
                        //                                                }
                        //                                            }
                        //                                            else
                        //                                            {
                        //                                                ShowMessage($"{camName}_{pointNum} 两孔坐标转换失败", Color.Red);
                        //                                                formShow?.UpDataMessage("两孔坐标转换失败", pointNum.ToString(), set.name);
                        //                                                无异常 = false;
                        //                                            }
                        //                                        }

                        //                                        float DX = float.NaN, DY = float.NaN, DZ = float.NaN;//没测的方向显示白色
                        //                                        switch (carSetting.gSets[pointNum].convertX)
                        //                                        {
                        //                                            case "X": DX = dx; break;
                        //                                            case "-X": DX = -dx; break;
                        //                                            case "Y": DY = dx; break;
                        //                                            case "-Y": DY = -dx; break;
                        //                                            case "Z": DZ = dx; break;
                        //                                            case "-Z": DZ = -dx; break;
                        //                                            default: DX = dx; break;
                        //                                        }
                        //                                        switch (carSetting.gSets[pointNum].convertY)
                        //                                        {
                        //                                            case "X": DX = dy; break;
                        //                                            case "-X": DX = -dy; break;
                        //                                            case "Y": DY = dy; break;
                        //                                            case "-Y": DY = -dy; break;
                        //                                            case "Z": DZ = dy; break;
                        //                                            case "-Z": DZ = -dy; break;
                        //                                            default: DX = dy; break;
                        //                                        }

                        //                                        //匹配成功先赋OK，超差再赋NG
                        //                                        lock (olock_Dict)
                        //                                        {
                        //                                            if (测点结果.ContainsKey(pointNum))
                        //                                            {
                        //                                                测点结果[pointNum] = true;
                        //                                            }
                        //                                            else
                        //                                            {
                        //                                                测点结果.Add(pointNum, true);
                        //                                            }
                        //                                        }
                        //                                        if (set.enablePoints && !pointsDistanceOK)
                        //                                        {
                        //                                            测点结果[pointNum] = false;
                        //                                        }
                        //                                        float L = 0, W = 0, H = 0;
                        //                                        Color colorX = Color.White, colorY = Color.White, colorZ = Color.White;
                        //                                        //判断X
                        //                                        if (!DX.Equals(float.NaN))
                        //                                        {
                        //                                            DX += carSetting.gSets[pointNum].offsetX;//补偿
                        //                                            if (DX < carSetting.gSets[pointNum].minDX_2 || DX > carSetting.gSets[pointNum].maxDX_2)
                        //                                            {
                        //                                                colorX = Color.Red;
                        //                                                无超差 = false;
                        //                                                测点结果[pointNum] = false;
                        //                                            }
                        //                                            else if (DX < carSetting.gSets[pointNum].minDX_1 || DX > carSetting.gSets[pointNum].maxDX_1)
                        //                                            {
                        //                                                colorX = Color.Yellow;
                        //                                            }
                        //                                            else
                        //                                            {
                        //                                                colorX = Color.Green;
                        //                                            }
                        //                                            if (carSetting.gSets[pointNum].相对基准点)
                        //                                            {
                        //                                                重建L.Add(DX);
                        //                                            }
                        //                                            L = DX;
                        //                                        }
                        //                                        //判断Y
                        //                                        if (!DY.Equals(float.NaN))
                        //                                        {
                        //                                            DY += carSetting.gSets[pointNum].offsetY;//补偿
                        //                                            if (DY < carSetting.gSets[pointNum].minDY_2 || DY > carSetting.gSets[pointNum].maxDY_2)
                        //                                            {
                        //                                                colorY = Color.Red;
                        //                                                无超差 = false;
                        //                                                测点结果[pointNum] = false;
                        //                                            }
                        //                                            else if (DY < carSetting.gSets[pointNum].minDY_1 || DY > carSetting.gSets[pointNum].maxDY_1)
                        //                                            {
                        //                                                colorY = Color.Yellow;
                        //                                            }
                        //                                            else
                        //                                            {
                        //                                                colorY = Color.Green;
                        //                                            }
                        //                                            if (carSetting.gSets[pointNum].相对基准点)
                        //                                            {
                        //                                                重建W.Add(DY);
                        //                                            }
                        //                                            W = DY;
                        //                                        }
                        //                                        //判断Z
                        //                                        if (!DZ.Equals(float.NaN))
                        //                                        {
                        //                                            DZ += carSetting.gSets[pointNum].offsetZ;//补偿
                        //                                            if (DZ < carSetting.gSets[pointNum].minDZ_2 || DZ > carSetting.gSets[pointNum].maxDZ_2)
                        //                                            {
                        //                                                colorZ = Color.Red;
                        //                                                无超差 = false;
                        //                                                测点结果[pointNum] = false;
                        //                                            }
                        //                                            else if (DZ < carSetting.gSets[pointNum].minDZ_1 || DZ > carSetting.gSets[pointNum].maxDZ_1)
                        //                                            {
                        //                                                colorZ = Color.Yellow;
                        //                                            }
                        //                                            else
                        //                                            {
                        //                                                colorZ = Color.Green;
                        //                                            }
                        //                                            if (carSetting.gSets[pointNum].相对基准点)
                        //                                            {
                        //                                                重建H.Add(DZ);
                        //                                            }
                        //                                            H = DZ;
                        //                                        }
                        //                                        float dd = (float)Math.Round(Math.Sqrt(L * L + W * W + H * H), 2);

                        //                                        Dictionary<string, string> 测点数据表 = new Dictionary<string, string>();
                        //                                        测点数据表.Add("时间", dateTimeNow.ToString("yyyy-MM-dd HH:mm:ss"));
                        //                                        测点数据表.Add("相机名", camName);
                        //                                        测点数据表.Add("点位号", pointNum.ToString());
                        //                                        测点数据表.Add("DX", L.ToString("0.00"));
                        //                                        测点数据表.Add("DY", W.ToString("0.00"));
                        //                                        测点数据表.Add("DZ", H.ToString("0.00"));
                        //                                        测点数据表.Add("DD", dd.ToString("0.00"));
                        //                                        if (set.enablePoints)
                        //                                        {
                        //                                            // 两孔相对距离误差写入数据库；PLC仍只接收原有单孔测量数据。
                        //                                            测点数据表.Add("PointsDX", PointsDX.ToString("0.00"));
                        //                                            测点数据表.Add("PointsDY", PointsDY.ToString("0.00"));
                        //                                            测点数据表.Add("PointsDZ", PointsDZ.ToString("0.00"));
                        //                                            测点数据表.Add("PointsDD", PointsDD.ToString("0.00"));
                        //                                        }

                        //                                        lock (olock_Dict)
                        //                                        {
                        //                                            if (!point3d给PLC.ContainsKey(pointNum))
                        //                                            {
                        //                                                point3d给PLC.Add(pointNum, new Point3D(L, W, H));
                        //                                                point3d原数据.Add(pointNum, new Point3D(DX, DY, DZ));
                        //                                                point3d原颜色.Add(pointNum, new Point3DColor(colorX, colorY, colorZ));
                        //                                                if (set.enablePoints)
                        //                                                {
                        //                                                    points3dData.Add(pointNum, new Point3D(PointsDX, PointsDY, PointsDZ));
                        //                                                    points3dColors.Add(pointNum, new Point3DColor(PointsColorX, PointsColorY, PointsColorZ));
                        //                                                }
                        //                                                sqlValuePairs.Add(pointNum, 测点数据表);
                        //                                            }
                        //                                            else
                        //                                            {
                        //                                                ShowMessage($"点位号{pointNum}重复", Color.Red);
                        //                                                无异常 = false;
                        //                                            }
                        //                                        }

                        //                                        //显示数据
                        //                                        dataGridViewShow.BeginInvoke(new Action(() =>
                        //                                        {
                        //                                            int index = dataGridViewShow.Rows.Add();
                        //                                            dataGridViewShow.Rows[index].Cells["pointNum"].Value = pointNum;
                        //                                            dataGridViewShow.Rows[index].Cells["cam"].Value = carSetting.gSets[pointNum].CamName;
                        //                                            dataGridViewShow.Rows[index].Cells["name"].Value = carSetting.gSets[pointNum].name;
                        //                                            dataGridViewShow.Rows[index].Cells["dx"].Value = L.ToString("0.00");
                        //                                            dataGridViewShow.Rows[index].Cells["dy"].Value = W.ToString("0.00");
                        //                                            dataGridViewShow.Rows[index].Cells["dz"].Value = H.ToString("0.00");
                        //                                            dataGridViewShow.Rows[index].Cells["dd"].Value = dd.ToString("0.00");

                        //                                            dataGridViewShow.Rows[index].Cells["dx"].Style.BackColor = colorX;
                        //                                            dataGridViewShow.Rows[index].Cells["dy"].Style.BackColor = colorY;
                        //                                            dataGridViewShow.Rows[index].Cells["dz"].Style.BackColor = colorZ;
                        //                                            if (set.enablePoints)
                        //                                            {
                        //                                                dataGridViewShow.Rows[index].Cells["PointsDX"].Value = PointsDX.ToString("0.00");
                        //                                                dataGridViewShow.Rows[index].Cells["PointsDY"].Value = PointsDY.ToString("0.00");
                        //                                                dataGridViewShow.Rows[index].Cells["PointsDZ"].Value = PointsDZ.ToString("0.00");
                        //                                                dataGridViewShow.Rows[index].Cells["PointsDD"].Value = PointsDD.ToString("0.00");
                        //                                                dataGridViewShow.Rows[index].Cells["PointsDX"].Style.BackColor = PointsColorX;
                        //                                                dataGridViewShow.Rows[index].Cells["PointsDY"].Style.BackColor = PointsColorY;
                        //                                                dataGridViewShow.Rows[index].Cells["PointsDZ"].Style.BackColor = PointsColorZ;
                        //                                                dataGridViewShow.Rows[index].Cells["PointsDD"].Style.BackColor = PointsColorD;
                        //                                            }
                        //                                        }));

                        //                                        formShow?.UpDataXYZ(
                        //                                            point3d给PLC[pointNum],
                        //                                            point3d原颜色[pointNum],
                        //                                            pointNum.ToString(),
                        //                                            carSetting.gSets[pointNum].name,
                        //                                            set.enablePoints ? new Point3D(PointsDX, PointsDY, PointsDZ) : null,
                        //                                            set.enablePoints ? new Point3DColor(PointsColorX, PointsColorY, PointsColorZ) : null);////////////////////////////////////
                        //                                    }
                        //                                    else
                        //                                    {
                        //                                        ShowMessage($"{camName}_{pointNum} 未做位姿标定", Color.Red);
                        //                                        formShow?.UpDataMessage($"未做位姿标定", pointNum.ToString(), set.name);
                        //                                        无异常 = false;
                        //                                    }
                        //                                }
                        //                            }
                        //                            else
                        //                            {
                        //                                ShowMessage($"{camName}相机未做标定", Color.Red);
                        //                                formShow?.UpDataMessage($"{camName}相机未标定", pointNum.ToString(), set.name);
                        //                                无异常 = false;
                        //                            }
                        //                        }
                        //                        catch (Exception ex)
                        //                        {
                        //                            ShowMessage($"{camName}_{pointNum} 匹配异常:{ex}", Color.Red);
                        //                            formShow?.UpDataMessage("匹配异常", pointNum.ToString(), set.name);
                        //                            无异常 = false;
                        //                        }

                        //                        //存图
                        //                        if (!checkBox相机仿真.Checked)
                        //                        {
                        //                            if (otherSet.isSaveImage && !bDry_mode)
                        //                            {
                        //                                lock (lockSaveImage)
                        //                                {
                        //                                    if (ListImageKeys.Count < 120)
                        //                                    {
                        //                                        {
                        //                                            string key = $"{path}\\{camName}_{pointNum:00}.png";
                        //                                            ListImageKeys.Add(key);
                        //                                            DicImages.Add(key, hImage2D);
                        //                                        }
                        //                                        {
                        //                                            string key = $"{path}\\{camName}_{pointNum:00}p.jpg";
                        //                                            ListImageKeys.Add(key);
                        //                                            DicImages.Add(key, hImage2D匹配);
                        //                                        }
                        //                                    }
                        //                                    else
                        //                                    {
                        //                                        ShowMessage($"{camName}_{pointNum:00} 内存中有{ListImageKeys.Count}张图未存进硬盘，跳过本次存图", Color.Yellow);
                        //                                    }
                        //                                }
                        //                            }
                        //                        }
                        //                        else
                        //                        {
                        //                            try
                        //                            {
                        //                                hImage2D匹配?.WriteImage("jpeg 80", 0, $"test-Image\\{camName}_{pointNum:00}p.jpg");
                        //                            }
                        //                            catch (Exception ex)
                        //                            {
                        //                                ShowMessage("仿真存图异常" + ex.ToString(), Color.Red);
                        //                            }
                        //                        }
                        //                    }
                        //                }
                        //                else
                        //                {
                        //                    lock (olock_Dict)
                        //                    {
                        //                        if (测点结果.ContainsKey(pointNum))
                        //                        {
                        //                            测点结果[pointNum] = true;
                        //                        }
                        //                        else
                        //                        {
                        //                            测点结果.Add(pointNum, true);
                        //                        }
                        //                    }

                        //                    if (相机参数.sets[camName].gSet.Disabled)
                        //                    {
                        //                        ShowMessage($"车型{carKind}-{TRnum} 相机{camName} 已屏蔽", Color.Yellow);
                        //                        formShow?.UpDataMessage("相机已屏蔽", pointNum.ToString(), set.name, false);
                        //                    }
                        //                    else if (set.keepOK)
                        //                    {
                        //                        ShowMessage($"车型{carKind}-{TRnum} 点位{pointNum} 已屏蔽", Color.Yellow);
                        //                        formShow?.UpDataMessage("测点已屏蔽", pointNum.ToString(), set.name, false);
                        //                    }
                        //                }
                        //            }
                        //            else
                        //            {
                        //                ShowMessage($"车型{carKind}-{TRnum} 相机{camName} 不存在", Color.Red);
                        //                formShow?.UpDataMessage("相机不存在", pointNum.ToString(), set.name);
                        //            }

                        //            //数据排序写入，必须要有
                        //            lock (olock_Dict)
                        //            {
                        //                if (!point3d给PLC.ContainsKey(pointNum))
                        //                {
                        //                    point3d给PLC.Add(pointNum, new Point3D(0, 0, 0));
                        //                }
                        //            }
                                
                                
                        //        }
                        //    }));
                        //}
                    }
                    else
                    {
                        ShowMessage($"{carKind}-{(otherSet.bTRnum ? TRnum : 0)} 未做参数", Color.Red);
                        无异常 = false;
                    }

                    //等待任务完成
                    ShowMessage("等待拍照任务完成");
                    foreach (var key in tasks.Keys)
                    {
                        while (!tasks[key].IsCompleted)
                        {
                            if (stop) return;

                            Thread.Sleep(10);
                        }
                        if (tasks[key].IsFaulted)
                        {
                            string msg = "";
                            msg = tasks[key].Exception?.ToString();
                            ShowMessage($"{key}拍照任务异常：{msg}", Color.Red);
                            无异常 = false;
                        }
                        else if (tasks[key].IsCanceled)
                        {
                            ShowMessage($"{key}拍照任务被取消");
                            无异常 = false;
                        }
                        else
                        {
                            ShowMessage($"{key}拍照任务完成");
                        }
                    }

                    //关灯
                    if (!checkBoxPLC仿真.Checked)
                    {
                        if (!Write(DO.拍照完成, true).IsSuccess)
                        {
                            ShowMessage("拍照完成写入失败", Color.Red);
                        }
                        if (!Write(DO.光源, false).IsSuccess)
                        {
                            ShowMessage("光源写入失败", Color.Red);
                        }
                    }

                    if (bAbort)
                    {
                        continue;
                    }

                    //重建基准
                    bool 重建 = 重建L.Count > 0 || 重建H.Count > 0 || 重建W.Count > 0;
                    bool 无超差重建 = true;
                    Dictionary<int, bool> 测点结果重建 = new Dictionary<int, bool>();
                    Dictionary<int, Point3D> point3d重建数据 = new Dictionary<int, Point3D>();//测点数据
                    if (重建)
                    {
                        formShow?.ResetCarImage();/////////////////////////
                        CarSetting carSetting = 车型参数[$"{carKind}-{(otherSet.bTRnum ? TRnum : 0)}"];
                        foreach (var pointNum in point3d原数据.Keys)
                        {
                            //重新计算
                            float DX = point3d原数据[pointNum].X.Equals(float.NaN) ? float.NaN : point3d原数据[pointNum].X - (重建L.Count > 0 ? 重建L.Average() : 0);
                            float DY = point3d原数据[pointNum].Y.Equals(float.NaN) ? float.NaN : point3d原数据[pointNum].Y - (重建W.Count > 0 ? 重建W.Average() : 0);
                            float DZ = point3d原数据[pointNum].Z.Equals(float.NaN) ? float.NaN : point3d原数据[pointNum].Z - (重建H.Count > 0 ? 重建H.Average() : 0);

                            //判断超差
                            测点结果重建.Add(pointNum, true); //匹配成功先赋OK，超差再赋NG
                            float L = 0, W = 0, H = 0;
                            Color colorX = Color.White, colorY = Color.White, colorZ = Color.White;
                            //判断X
                            if (!DX.Equals(float.NaN))
                            {
                                if (DX < carSetting.gSets[pointNum].minNewDX_2 || DX > carSetting.gSets[pointNum].maxNewDX_2)
                                {
                                    colorX = Color.Red;
                                    无超差重建 = false;
                                    测点结果重建[pointNum] = false;
                                }
                                else if (DX < carSetting.gSets[pointNum].minNewDX_1 || DX > carSetting.gSets[pointNum].maxNewDX_1)
                                {
                                    colorX = Color.Yellow;
                                }
                                else
                                {
                                    colorX = Color.Green;
                                }
                                L = DX;
                            }
                            //判断Y
                            if (!DY.Equals(float.NaN))
                            {
                                if (DY < carSetting.gSets[pointNum].minNewDY_2 || DY > carSetting.gSets[pointNum].maxNewDY_2)
                                {
                                    colorY = Color.Red;
                                    无超差重建 = false;
                                    测点结果重建[pointNum] = false;
                                }
                                else if (DY < carSetting.gSets[pointNum].minNewDY_1 || DY > carSetting.gSets[pointNum].maxNewDY_1)
                                {
                                    colorY = Color.Yellow;
                                }
                                else
                                {
                                    colorY = Color.Green;
                                }
                                W = DY;
                            }
                            //判断Z
                            if (!DZ.Equals(float.NaN))
                            {
                                if (DZ < carSetting.gSets[pointNum].minNewDZ_2 || DZ > carSetting.gSets[pointNum].maxNewDZ_2)
                                {
                                    colorZ = Color.Red;
                                    无超差重建 = false;
                                    测点结果重建[pointNum] = false;
                                }
                                else if (DZ < carSetting.gSets[pointNum].minNewDZ_1 || DZ > carSetting.gSets[pointNum].maxNewDZ_1)
                                {
                                    colorZ = Color.Yellow;
                                }
                                else
                                {
                                    colorZ = Color.Green;
                                }
                                H = DZ;
                            }
                            float dd = (float)Math.Round(Math.Sqrt(L * L + W * W + H * H), 2);

                            point3d重建数据.Add(pointNum, new Point3D(L, W, H));

                            sqlValuePairs[pointNum].Add("新DX", L.ToString("0.00"));
                            sqlValuePairs[pointNum].Add("新DY", W.ToString("0.00"));
                            sqlValuePairs[pointNum].Add("新DZ", H.ToString("0.00"));
                            sqlValuePairs[pointNum].Add("新DD", dd.ToString("0.00"));

                            //显示数据
                            dataGridViewShow.BeginInvoke(new Action(() =>
                            {
                                foreach (DataGridViewRow row in dataGridViewShow.Rows)
                                {
                                    if (int.TryParse(row.Cells["pointNum"].Value.ToString(), out int num))
                                    {
                                        if (num == pointNum)
                                        {
                                            //row.Cells["dx"].Style.ForeColor = row.Cells["dx"].Style.BackColor;
                                            //row.Cells["dy"].Style.ForeColor = row.Cells["dy"].Style.BackColor;
                                            //row.Cells["dz"].Style.ForeColor = row.Cells["dz"].Style.BackColor;
                                            //row.Cells["dx"].Style.BackColor = Color.White;
                                            //row.Cells["dy"].Style.BackColor = Color.White;
                                            //row.Cells["dz"].Style.BackColor = Color.White;
                                            row.Cells["新dx"].Value = L.ToString("0.00");
                                            row.Cells["新dy"].Value = W.ToString("0.00");
                                            row.Cells["新dz"].Value = H.ToString("0.00");
                                            row.Cells["新dd"].Value = dd.ToString("0.00");
                                            row.Cells["新dx"].Style.BackColor = colorX;
                                            row.Cells["新dy"].Style.BackColor = colorY;
                                            row.Cells["新dz"].Style.BackColor = colorZ;
                                            break;
                                        }
                                    }
                                }
                            }));

                            //formShow?.UpDataXYZ(point3d重建数据[pointNum], new Point3DColor(colorX, colorY, colorZ), pointNum.ToString(), carSetting.gSets[pointNum].name);////////////////////////////////////
                            //formShow?.UpDataXYZ新(point3d重建数据[pointNum], new Point3DColor(colorX, colorY, colorZ), pointNum.ToString());////////////////////////////////////
                            //formShow?.UpDataXYZ2(point3d给PLC[pointNum], point3d原颜色[pointNum], point3d重建数据[pointNum], new Point3DColor(colorX, colorY, colorZ), pointNum.ToString(), carSetting.gSets[pointNum].name);
                            ///客户需求：只要统一只显示一个结果
                            bool showPoints = carSetting.gSets[pointNum].enablePoints && points3dData.ContainsKey(pointNum);
                            formShow?.UpDataXYZ(
                                point3d重建数据[pointNum],
                                point3d原颜色[pointNum],
                                pointNum.ToString(),
                                carSetting.gSets[pointNum].name,
                                showPoints ? points3dData[pointNum] : null,
                                showPoints ? points3dColors[pointNum] : null);
                        }
                    }


                    //处理结果
                    bool bResult = 无异常 && 无超差 && 无超差重建;
                    ushort u测点结果 = 0;

                    if (!checkBoxPLC仿真.Checked)
                    {
                        ////固定15个点，按点位号写对应地址，15个点都写入
                        //int[] ints = new int[15 * 3];
                        //for (int i = 0; i < 15; i++)
                        //{
                        //    if (point3d.ContainsKey(i))
                        //    {
                        //        ints[i * 3] = Float2Int(point3d[i].X);
                        //        ints[i * 3 + 1] = Float2Int(point3d[i].Y);
                        //        ints[i * 3 + 2] = Float2Int(point3d[i].Z);
                        //    }
                        //    else//没用到的点位
                        //    {
                        //        ints[i * 3] = Float2Int(0);
                        //        ints[i * 3 + 1] = Float2Int(0);
                        //        ints[i * 3 + 2] = Float2Int(0);
                        //    }
                        //}

                        //按点位号大小升序排列写入
                        int[] ints = new int[point3d给PLC.Count * 3];
                        var temp = point3d给PLC.Keys.OrderBy(n => { return n; }).ToArray();
                        for (int i = 0; i < temp.Length; i++)
                        {
                            if (point3d给PLC.ContainsKey(temp[i]))
                            {
                                ints[i * 3] = Float2Int(point3d给PLC[temp[i]].X);
                                ints[i * 3 + 1] = Float2Int(point3d给PLC[temp[i]].Y);
                                ints[i * 3 + 2] = Float2Int(point3d给PLC[temp[i]].Z);
                                if (重建)
                                {
                                    if (point3d重建数据.ContainsKey(temp[i]))
                                    {
                                        ints[i * 3] = Float2Int(point3d重建数据[temp[i]].X);
                                        ints[i * 3 + 1] = Float2Int(point3d重建数据[temp[i]].Y);
                                        ints[i * 3 + 2] = Float2Int(point3d重建数据[temp[i]].Z);
                                    }
                                    if (测点结果重建.ContainsKey(temp[i]) && 测点结果.ContainsKey(temp[i]))
                                    {
                                        u测点结果 = u测点结果.SetBoolByIndex(i, 测点结果重建[temp[i]] && 测点结果[temp[i]]);
                                    }
                                }
                                else
                                {
                                    if (测点结果.ContainsKey(temp[i]))
                                    {
                                        u测点结果 = u测点结果.SetBoolByIndex(i, 测点结果[temp[i]]);
                                    }
                                }
                            }
                        }

                        //输出信号
                        if (!Write(DO.测量数据, ints).IsSuccess)
                        {
                            ShowMessage("测量数据写入失败", Color.Red);
                        }
                        if (!Write(DO.测点结果, u测点结果).IsSuccess)
                        {
                            ShowMessage("测点结果写入失败", Color.Red);
                        }
                        if (!Write(DO.测量结果, bResult).IsSuccess)
                        {
                            ShowMessage("测量结果写入失败", Color.Red);
                        }
                        if (!Write(DO.测量可读, true).IsSuccess)
                        {
                            ShowMessage("测量结果可读写入失败", Color.Red);
                        }
                        if (!Write(DO.运行中, false).IsSuccess)
                        {
                            ShowMessage("运行中写入失败", Color.Red);
                        }
                    }

                    //数据库写入
                    if (!checkBox相机仿真.Checked && !bDry_mode)
                    {
                        try
                        {
                            if (sqlEnable)
                            {
                                sql?.Open();
                                sql?.BeginTransaction();
                                try
                                {
                                    Dictionary<string, string> 车辆信息表 = new Dictionary<string, string>
                                    {
                                        { "时间", dateTimeNow.ToString("yyyy-MM-dd HH:mm:ss") },
                                        { "车型", carKind.ToString() },
                                        { "连番", carNum.ToString() },
                                        { "托盘号", TRnum.ToString() },
                                        { "车架号", carVIN },
                                        { "测点结果",u测点结果.ToString() },
                                        { "测量结果", bResult?"1":"0"  },
                                    };
                                    int r = sql.InsertRow(sql.Database + ".车辆信息表", 车辆信息表);

                                    foreach (var item in sqlValuePairs.Keys)
                                    {
                                        sql.InsertRow(sql.Database + ".测点数据表", sqlValuePairs[item]);
                                    }

                                    sql.Commit();
                                }
                                catch (Exception ex)
                                {
                                    sql.Rollback();
                                    ShowMessage(ex.ToString(), Color.Red);
                                }
                                sql?.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            ShowMessage($"数据库写入异常：{ex}", Color.Red);
                        }
                    }

                    lblSysStatus.BeginInvoke(new Action(() =>
                    {
                        lblSysStatus.Text = "完成";
                        lblSysStatus.BackColor = Color.White;
                        lblSysStatus.ForeColor = AppTheme.Window;
                        labelResult.Text = bResult ? "OK" : "NG";
                        labelResult.BackColor = bResult ? Color.Green : Color.Red;
                        labelResult.ForeColor = Color.White;
                    }));

                    if (!checkBoxPLC仿真.Checked)
                    {
                        ShowMessage("等待已读信号");
                        while (true)
                        {
                            if (stop) return;
                            var val = ReadBool(DI.已读);
                            if (val.IsSuccess)
                            {
                                if (val.Content == true)
                                {
                                    ShowMessage("收到已读信号");
                                    break;
                                }
                            }
                            else
                            {
                                ShowMessage("已读信号读取失败", Color.Red);
                                return;
                            }
                            var abort = ReadBool(DI.重置);
                            if (!abort.IsSuccess)
                            {
                                ShowMessage("重置流程信号读取失败", Color.Red);
                                return;
                            }
                            if (bAbort || abort.Content)
                            {
                                bAbort = true;
                                ShowMessage("收到重置流程信号", Color.Yellow);
                                break;
                            }
                            Thread.Sleep(50);
                        }
                    }

                    sp.Stop();
                    ShowMessage("流程耗时" + (sp.ElapsedMilliseconds / 1000f).ToString("0.00") + "s");

                    if (checkBoxZero.Checked)
                    {
                        var carSetting = 车型参数[textBoxTestNum.Text];
                        FormZero form = new FormZero(false);
                        foreach (var pointNum in point3d给PLC.Keys)
                        {
                            form.dataPoint3D.Add(pointNum, null);
                            if (carSetting.gSets.ContainsKey(pointNum))
                            {
                                form.dataName.Add(pointNum, carSetting.gSets[pointNum].name);
                            }
                        }
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            foreach (var pointNum in form.dataPoint3D.Keys)
                            {
                                if (carSetting.gSets.ContainsKey(pointNum))
                                {
                                    carSetting.gSets[pointNum].offsetX += form.dataPoint3D[pointNum].X - point3d给PLC[pointNum].X;
                                    carSetting.gSets[pointNum].offsetY += form.dataPoint3D[pointNum].Y - point3d给PLC[pointNum].Y;
                                    carSetting.gSets[pointNum].offsetZ += form.dataPoint3D[pointNum].Z - point3d给PLC[pointNum].Z;

                                    //显示数据
                                    dataGridViewShow.BeginInvoke(new Action(() =>
                                    {
                                        int index = dataGridViewShow.Rows.Add();
                                        dataGridViewShow.Rows[index].Cells["pointNum"].Value = pointNum;
                                        dataGridViewShow.Rows[index].Cells["cam"].Value = carSetting.gSets[pointNum].CamName;
                                        dataGridViewShow.Rows[index].Cells["name"].Value = carSetting.gSets[pointNum].name;
                                        dataGridViewShow.Rows[index].Cells["dx"].Value = form.dataPoint3D[pointNum].X.ToString("0.00");
                                        dataGridViewShow.Rows[index].Cells["dy"].Value = form.dataPoint3D[pointNum].Y.ToString("0.00");
                                        dataGridViewShow.Rows[index].Cells["dz"].Value = form.dataPoint3D[pointNum].Z.ToString("0.00");
                                    }));
                                    bool showPoints = carSetting.gSets[pointNum].enablePoints && points3dData.ContainsKey(pointNum);
                                    formShow?.UpDataXYZ(
                                        form.dataPoint3D[pointNum],
                                        new Point3DColor(Color.Gray, Color.Gray, Color.Gray),
                                        pointNum.ToString(),
                                        carSetting.gSets[pointNum].name,
                                        showPoints ? points3dData[pointNum] : null,
                                        showPoints ? points3dColors[pointNum] : null);////////////////////////////////////
                                }
                            }

                            checkBoxZero.Invoke(new Action(() =>
                            {
                                checkBoxZero.Checked = false;
                            }));
                        }
                    }

                    Thread.Sleep(500);
                    if (checkBoxPLC仿真.Checked) return;
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.ToString(), Color.Red);
            }
            finally
            {
                stop = true;
                alive.Enabled = false;
                threadSaveImage?.Join();
                foreach (var task in tasks.Values)
                {
                    while (!task.IsCompleted)
                    {
                        Thread.Sleep(7);
                    }
                }
                foreach (var cam in 相机.Values)
                {
                    cam?.Close();
                }
                StopState();
            }
        }


        object lockSaveImage = new object();
        List<string> ListImageKeys = new List<string>();
        Dictionary<string, HImage> DicImages = new Dictionary<string, HImage>();
        Thread threadSaveImage = null;
        void SaveImage()
        {
            try
            {
                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                while (!stop)
                {
                    if (ListImageKeys.Count > 0)
                    {
                        var path = ListImageKeys[0];

                        stopwatch.Restart();
                        bool bCreate = true;
                        try
                        {
                            string dir = Path.GetDirectoryName(path);
                            if (!Directory.Exists(dir))
                            {
                                Directory.CreateDirectory(dir);
                                ShowMessage("创建文件夹" + dir);
                            }
                        }
                        catch (Exception ex)
                        {
                            bCreate = false;
                            ShowMessage(ex.ToString(), Color.Red);
                        }
                        stopwatch.Stop();
                        long aTime = stopwatch.ElapsedMilliseconds;

                        stopwatch.Restart();
                        try
                        {
                            if (bCreate)
                            {
                                if (Path.GetExtension(path) == ".png")
                                {
                                    DicImages[path]?.WriteImage("png 1", 0, path);
                                }
                                else
                                {
                                    DicImages[path]?.WriteImage("jpeg 80", 0, path);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ShowMessage("存图异常" + ex.ToString(), Color.Red);
                        }
                        stopwatch.Stop();
                        long bTime = stopwatch.ElapsedMilliseconds;
                        lock (lockSaveImage)
                        {
                            ListImageKeys.RemoveAt(0);
                            DicImages[path]?.Dispose();
                            DicImages.Remove(path);
                            ShowMessage($"{path} 存图进硬盘完成，其中创建目录耗时{aTime}ms，存图耗时{bTime}ms，剩余{ListImageKeys.Count}张");
                        }
                    }
                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("存图线程异常" + ex.ToString(), Color.Red);
            }
        }

        /// <summary>
        /// 将标定后的XY坐标按照测点的轴向配置转换为XYZ坐标。
        /// 未配置到的方向保持为0。
        /// </summary>
        /// <param name="set">当前测点的参数。</param>
        /// <param name="x">GetXY返回的X坐标。</param>
        /// <param name="y">GetXY返回的Y坐标。</param>
        /// <returns>完成轴向转换后的三维坐标。</returns>
        private Point3D ConvertXYToAxes(GeneralSet set, float x, float y)
        {
            Point3D point = new Point3D(0, 0, 0);
            switch (set.convertX)
            {
                case "X": point.X = x; break;
                case "-X": point.X = -x; break;
                case "Y": point.Y = x; break;
                case "-Y": point.Y = -x; break;
                case "Z": point.Z = x; break;
                case "-Z": point.Z = -x; break;
            }
            switch (set.convertY)
            {
                case "X": point.X = y; break;
                case "-X": point.X = -y; break;
                case "Y": point.Y = y; break;
                case "-Y": point.Y = -y; break;
                case "Z": point.Z = y; break;
                case "-Z": point.Z = -y; break;
            }
            return point;
        }

        /// <summary>
        /// 按两孔相对距离的绿色、黄色阈值判断四个结果，并返回各字段颜色。
        /// </summary>
        /// <param name="set">当前测点的参数。</param>
        /// <param name="pointsDX">两孔X轴相对距离误差。</param>
        /// <param name="pointsDY">两孔Y轴相对距离误差。</param>
        /// <param name="pointsDZ">两孔Z轴相对距离误差。</param>
        /// <param name="pointsDD">两孔相对距离误差向量的模长。</param>
        /// <param name="colorX">X轴结果颜色。</param>
        /// <param name="colorY">Y轴结果颜色。</param>
        /// <param name="colorZ">Z轴结果颜色。</param>
        /// <param name="colorD">模长结果颜色。</param>
        /// <returns>四个结果均未超出黄色阈值时返回true。</returns>
        private bool EvaluatePointsDistance(GeneralSet set, float pointsDX, float pointsDY, float pointsDZ, float pointsDD,
            out Color colorX, out Color colorY, out Color colorZ, out Color colorD)
        {
            bool result = true;
            colorX = GetPointsColor(pointsDX, set.minPointsDX_1, set.maxPointsDX_1, set.minPointsDX_2, set.maxPointsDX_2, ref result);
            colorY = GetPointsColor(pointsDY, set.minPointsDY_1, set.maxPointsDY_1, set.minPointsDY_2, set.maxPointsDY_2, ref result);
            colorZ = GetPointsColor(pointsDZ, set.minPointsDZ_1, set.maxPointsDZ_1, set.minPointsDZ_2, set.maxPointsDZ_2, ref result);
            colorD = GetPointsColor(pointsDD, set.minPointsDD_1, set.maxPointsDD_1, set.minPointsDD_2, set.maxPointsDD_2, ref result);
            return result;
        }

        /// <summary>
        /// 判断单个两孔距离字段的阈值颜色；超出黄色阈值时将整体结果置为NG。
        /// </summary>
        private Color GetPointsColor(float value, float minGreen, float maxGreen, float minYellow, float maxYellow, ref bool result)
        {
            if (value < minYellow || value > maxYellow)
            {
                result = false;
                return Color.Red;
            }
            if (value < minGreen || value > maxGreen)
            {
                return Color.Yellow;
            }
            return Color.Green;
        }

        /// <summary>
        /// 将测点浮点结果转换为PLC使用的整数编码。
        /// </summary>
        int Float2Int(float value)
        {
            if (value.Equals(float.NaN)) return 0;
            int num = 0;
            //符号
            if (value < 0)
            {
                byte c = (byte)'-';
                num += c << 16;
            }
            else
            {
                byte c = (byte)'+';
                num += c << 16;
            }
            if (value < 10 && value > -10)
            {
                //个位
                num += ((int)Math.Abs((decimal)value) % 10 * 16 * 16);
                //十分位
                num += ((int)(Math.Abs((decimal)value) * 10) % 10 * 16);
                //百分位
                num += ((int)(Math.Abs((decimal)value) * 10 * 10) % 10);
            }
            else
            {
                num += 0x999;
            }
            return num;
        }

        object iolock = new object();
        OperateResult Write(DO dio, bool value)
        {
            bool bTry = false;
            while (true)
            {
                OperateResult result;
                lock (iolock)
                {
                    result = plc.Write(dio, value);
                }
                if (result.IsSuccess)
                {
                    if (bTry)
                    {
                        bTry = false;
                        ShowMessage("信号重新写入成功", Color.Yellow);
                    }

                    ////通过读取信号判断是否真的写入成功
                    //OperateResult<bool> readResult = ReadBool(dio);
                    //if (readResult.IsSuccess)
                    //{
                    //    if (readResult.Content == value)
                    //    {
                    //        return result;
                    //    }
                    //    else
                    //    {
                    //        ShowMessage("写入值与读取值不一致", Color.Yellow);
                    //        Thread.Sleep(50);
                    //    }
                    //}
                    return result;
                }
                else
                {
                    if (!bTry)
                    {
                        bTry = true;
                        ShowMessage("信号写入失败,重试中...：" + result.Message, Color.Yellow);
                    }
                    Thread.Sleep(1000);
                }
                if (stop)
                {
                    return result;
                }
            }
        }
        OperateResult Write(DO dio, ushort value)
        {
            bool bTry = false;
            while (true)
            {
                OperateResult result;
                lock (iolock)
                {
                    result = plc.Write(dio, value);
                }
                if (result.IsSuccess)
                {
                    if (bTry)
                    {
                        bTry = false;
                        ShowMessage("信号重新写入成功", Color.Yellow);
                    }

                    ////通过读取信号判断是否真的写入成功
                    //OperateResult<bool> readResult = ReadBool(dio);
                    //if (readResult.IsSuccess)
                    //{
                    //    if (readResult.Content == value)
                    //    {
                    //        return result;
                    //    }
                    //    else
                    //    {
                    //        ShowMessage("写入值与读取值不一致", Color.Yellow);
                    //        Thread.Sleep(50);
                    //    }
                    //}
                    return result;
                }
                else
                {
                    if (!bTry)
                    {
                        bTry = true;
                        ShowMessage("信号写入失败,重试中...：" + result.Message, Color.Yellow);
                    }
                    Thread.Sleep(1000);
                }
                if (stop)
                {
                    return result;
                }
            }
        }
        OperateResult Write(DO dio, int[] value)
        {
            bool bTry = false;
            while (true)
            {
                OperateResult result;
                lock (iolock)
                {
                    result = plc.Write(dio, value);
                }
                if (result.IsSuccess)
                {
                    if (bTry)
                    {
                        bTry = false;
                        ShowMessage("信号重新写入成功", Color.Yellow);
                    }

                    ////通过读取信号判断是否真的写入成功
                    //var readResult = ReadInt16(dio, 1);
                    //if (readResult.IsSuccess)
                    //{
                    //    if (readResult.Content == value)
                    //    {
                    //        return result;
                    //    }
                    //    else
                    //    {
                    //        ShowMessage("写入值与读取值不一致", Color.Yellow);
                    //        Thread.Sleep(50);
                    //    }
                    //}
                    return result;
                }
                else
                {
                    if (!bTry)
                    {
                        bTry = true;
                        ShowMessage("信号写入失败,重试中...：" + result.Message, Color.Yellow);
                    }
                    Thread.Sleep(1000);
                }
                if (stop)
                {
                    return result;
                }
            }
        }
        OperateResult<bool> ReadBool(DO dio)
        {
            bool bTry = false;
            while (true)
            {
                OperateResult<bool> result = plc.ReadBool(dio);
                if (result.IsSuccess)
                {
                    if (bTry)
                    {
                        ShowMessage("信号重新读取成功", Color.Yellow);
                    }
                    return result;
                }
                else
                {
                    if (!bTry)
                    {
                        bTry = true;
                        ShowMessage("信号读取失败,重试中...：" + result.Message, Color.Yellow);
                    }
                    Thread.Sleep(1000);
                    //ConnectServer();
                }
                if (stop)
                {
                    return result;
                }
            }
        }
        OperateResult<bool> ReadBool(DI dio)
        {
            bool bTry = false;
            while (true)
            {
                OperateResult<bool> result = plc.ReadBool(dio);
                if (result.IsSuccess)
                {
                    if (bTry)
                    {
                        ShowMessage("信号重新读取成功", Color.Yellow);
                    }
                    return result;
                }
                else
                {
                    if (!bTry)
                    {
                        bTry = true;
                        ShowMessage("信号读取失败,重试中...：" + result.Message, Color.Yellow);
                    }
                    Thread.Sleep(1000);
                    //ConnectServer();
                }
                if (stop)
                {
                    return result;
                }
            }
        }
        OperateResult<ushort> ReadUInt16(DI dio)
        {
            bool bTry = false;
            while (true)
            {
                OperateResult<ushort> result = plc.ReadUInt16(dio);
                if (result.IsSuccess)
                {
                    if (bTry)
                    {
                        ShowMessage("信号重新读取成功", Color.Yellow);
                    }
                    return result;
                }
                else
                {
                    if (!bTry)
                    {
                        bTry = true;
                        ShowMessage("信号读取失败,重试中...：" + result.Message, Color.Yellow);
                    }
                    Thread.Sleep(1000);
                }
                if (stop)
                {
                    return result;
                }
            }
        }
        OperateResult<string> ReadString(DI dio, ushort length)
        {
            bool bTry = false;
            while (true)
            {
                OperateResult<string> result = plc.ReadString(dio, length);
                if (result.IsSuccess)
                {
                    if (bTry)
                    {
                        ShowMessage("信号重新读取成功", Color.Yellow);
                    }
                    return result;
                }
                else
                {
                    if (!bTry)
                    {
                        bTry = true;
                        ShowMessage("信号读取失败,重试中...：" + result.Message, Color.Yellow);
                    }
                    Thread.Sleep(1000);
                }
                if (stop)
                {
                    return result;
                }
            }
        }


        private void buttonStop_Click(object sender, EventArgs e)
        {
            stop = true;
            ShowMessage("用户执行了停止", Color.Red);
        }
        /// <summary>
        /// 运行时控件启用状态
        /// </summary>
        void RunState()
        {
            stop = false;
            buttonRun.Enabled = false;
            buttonStop.Enabled = true;
            groupBox1.Enabled = false;
        }
        /// <summary>
        /// 停止时控件启用状态
        /// </summary>
        void StopState()
        {
            stop = true;
            buttonStop.BeginInvoke(new Action(() =>
            {
                lblSysStatus.Text = "停止";
                lblSysStatus.BackColor = Color.Red;
                lblSysStatus.ForeColor = Color.White;
                buttonRun.Enabled = true;
                buttonStop.Enabled = false;
                groupBox1.Enabled = true;
            }));
        }
        object olock = new object();
        List<string> listLogs = new List<string>();
        void ShowMessage(string message)
        {
            ShowMessage(message, AppTheme.Surface);
        }
        void ShowMessage(string message, Color backColor)
        {
            DateTime now = DateTime.Now;
            string day = now.ToString("yyyy-MM-dd");
            string time = now.ToString("HH:mm:ss.fff");
            try
            {
                dataGridViewLog.BeginInvoke(new Action(() =>
                {
                    if (dataGridViewLog.Rows.Count >= 200)
                    {
                        dataGridViewLog.Rows.RemoveAt(199);
                    }
                    dataGridViewLog.Rows.Insert(0);
                    dataGridViewLog.Rows[0].Cells[0].Value = day + " " + time;
                    dataGridViewLog.Rows[0].Cells[1].Value = message;
                    dataGridViewLog.Rows[0].DefaultCellStyle.BackColor = backColor;
                }));
            }
            catch { }
            lock (olock)
            {
                try
                {
                    if (!Directory.Exists("RunLog"))
                    {
                        Directory.CreateDirectory("RunLog");
                    }
                    while (listLogs.Count > 0)
                    {
                        using (StreamWriter writer = new StreamWriter("RunLog\\" + day + ".log", true))
                        {
                            writer.WriteLine(listLogs[0]);
                            listLogs.RemoveAt(0);
                        }
                    }
                    using (StreamWriter writer = new StreamWriter("RunLog\\" + day + ".log", true))
                    {
                        writer.WriteLine(time + " " + message);
                    }
                }
                catch (Exception)//写入日志到文件失败，记录信息
                {
                    listLogs.Add(time + " " + message);
                }

                if (backColor == Color.Red)
                {
                    try
                    {
                        File.AppendAllText("Error.log", now.ToString("yyyy-MM-dd HH:mm:ss  ") + message + "\r\n\r\n");
                    }
                    catch { }
                }
            }
        }

        private void FormRun_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!stop)
            {
                if (DialogResult.OK != MessageBox.Show("软件正在运行中，确定退出？", "提示", MessageBoxButtons.OKCancel))
                {
                    e.Cancel = true;
                    return;
                }
            }
            if (formShow != null && formShow.IsShow)
            {
                formShow.EnableClose = true;
                formShow.Invoke(new Action(() => { formShow.Close(); }));
            }

            StopState();

            while (mainThread != null && mainThread.ThreadState != ThreadState.Stopped)
            {
                Thread.Sleep(1);
            }
            //关闭plc通讯
            plcClose();

            //关闭光源控制
            lightSerialPort.Close();
            lightSerialPort2.Close();

        }

        #region 光源控制
        private void button_Led_Click(object sender, EventArgs e)
        {
            if (button_Led.BackColor == 原色)
            {
                try
                {
                    //改一下这里的打开方式就好。改为串口通讯打开

                    //OperateResult result;
                    //lock (iolock)
                    //{
                    //    result = plc.Write(DO.光源, true);
                    //}
                    //if (result.IsSuccess)
                    //{
                    //    button_Led.BackColor = Color.Green;
                    //}
                    //else
                    //{
                    //    ShowMessage("开灯异常：" + result.Message, Color.Red);
                    //}

                    bool rt = lightSerialPort.controlLight(true);
                    rt &= lightSerialPort2.controlLight(true);
                    if (!rt)
                    {
                        ShowMessage("开灯异常：" + "通讯连接失败", Color.Red);
                    }
                    else
                    {
                        button_Led.BackColor = Color.Green;
                    }

                }
                catch (Exception ex)
                {
                    ShowMessage("开灯异常：" + ex.Message, Color.Red);
                }
            }
            else
            {
                try
                {
                    //OperateResult result;
                    //lock (iolock)
                    //{
                    //    result = plc.Write(DO.光源, false);
                    //}
                    //if (result.IsSuccess)
                    //{
                    //    button_Led.BackColor = 原色;
                    //}
                    //else
                    //{
                    //    ShowMessage("关灯异常：" + result.Message, Color.Red);
                    //}

                    bool rt = lightSerialPort.controlLight(false);
                    rt &= lightSerialPort2.controlLight(false);


                    if (!rt)
                    {
                        ShowMessage("关灯异常：" + "通讯连接失败", Color.Red);
                    }
                    else
                    {
                        button_Led.BackColor = 原色;
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("关灯异常：" + ex.Message, Color.Red);
                }
            }

            ////测试一下线程控制光源

            //if (button_Led.BackColor == 原色)
            //{
            //    lightState_test.Content = 1;
            //    button_Led.BackColor = Color.Green;
            //}
            //else
            //{
            //    lightState_test.Content = 2;
            //    button_Led.BackColor = 原色;
            //}

        }

        #endregion

        private void buttonSetting_Click(object sender, EventArgs e)
        {
            var form = new FormSet();
            form.ShowDialog();
            if (form.IsSave)
            {
                buttonClear.Enabled = true;
            }
        }
        bool loadAgain = false;
        private void buttonClear_Click(object sender, EventArgs e)
        {
            loadAgain = true;
            buttonClear.Enabled = false;
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            bool b = true;
            foreach (var item in 车型参数.Values)
            {
                if (!item.Save(Car.BaseDirectory + "\\" + item.CarName))
                {
                    b = false;
                    MessageBox.Show(item.CarName + "保存失败！");
                }
            }
            if (b)
            {
                MessageBox.Show("保存成功！");
            }
        }
        bool bSkipStart = false;
        private void buttonSkipStart_Click(object sender, EventArgs e)
        {
            if (!bSkipStart)
            {
                if (DialogResult.OK == MessageBox.Show("是否确定跳过开始信号？", "提示", MessageBoxButtons.OKCancel))
                {
                    bSkipStart = true;
                }
            }
        }

        private void FormRun_Paint(object sender, PaintEventArgs e)
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

        private void buttonPLC_Click(object sender, EventArgs e)
        {
            //如果打开了，要先关闭
            plcClose();
            plc.ShowForm();
            //重新初始化
            plcInit();
        }

        private void buttonCam_Click(object sender, EventArgs e)
        {
            var form = new FormCam();
            form.ShowDialog();
        }

    }
}
