using HalconDotNet;
using HslCommunication.Secs.Types;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace OnlineMeasurement
{
    //所有车型下的所有参数
    public class Car
    {
        static public readonly string BaseDirectory = "Data\\Car";
        string _errMsg = string.Empty;
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg => _errMsg;

        /// <summary>
        /// 车型号-托盘号-名称>>车型参数
        /// </summary>
        public Dictionary<string, CarSetting> sets = new Dictionary<string, CarSetting>();

        public bool Load()
        {
            bool result = true;
            _errMsg = string.Empty;
            sets.Clear();
            if (Directory.Exists(BaseDirectory))
            {
                string[] carPaths = Directory.GetDirectories(BaseDirectory);
                foreach (var item in carPaths)
                {
                    string dirName = Path.GetFileNameWithoutExtension(item);
                    string[] strings = dirName.Split('-');
                    if (strings.Length == 3 && int.TryParse(strings[0], out int 车型号) && int.TryParse(strings[1], out int 托盘号))
                    {
                        CarSetting set = new CarSetting(dirName);
                        if (set.Load(item))
                        {
                            sets.Add(dirName, set);
                        }
                        else
                        {
                            _errMsg += "\r\n" + set.ErrMsg;
                            result = false;
                        }
                    }
                    else
                    {
                        _errMsg += $"\r\n目录{item}文件夹名称格式不正确";
                        result = false;
                    }
                }
            }
            else
            {
                _errMsg = $"参数文件夹{BaseDirectory}不存在";
                result = false;
            }
            return result;
        }

        public bool LoadGeneralSet()
        {
            bool result = true;
            _errMsg = string.Empty;
            sets.Clear();
            if (Directory.Exists(BaseDirectory))
            {
                string[] carPaths = Directory.GetDirectories(BaseDirectory);
                foreach (var item in carPaths)
                {
                    string dirName = Path.GetFileNameWithoutExtension(item);
                    string[] strings = dirName.Split('-');
                    if (strings.Length == 3 && int.TryParse(strings[0], out int 车型号) && int.TryParse(strings[1], out int 托盘号))
                    {
                        CarSetting set = new CarSetting(dirName);
                        if (!set.LoadGeneralSet(item))
                        {
                            _errMsg += "\r\n" + set.ErrMsg;
                            result = false;
                        }
                        sets.Add(dirName, set);
                    }
                    else
                    {
                        _errMsg += $"\r\n目录{item}文件夹名称格式不正确";
                        result = false;
                    }
                }
            }
            else
            {
                _errMsg = $"参数文件夹{BaseDirectory}不存在";
                result = false;
            }
            return result;
        }

        public bool Save()
        {
            bool result = true;
            _errMsg = string.Empty;
            string basePath = AppDomain.CurrentDomain.BaseDirectory + BaseDirectory;
            foreach (var item in sets.Keys)//4-0-942B
            {
                string path = basePath + "\\" + item;
                if (!sets[item].Save(path))
                {
                    _errMsg += "\r\n" + sets[item].ErrMsg;
                    result = false;
                }
            }
            return result;
        }
        public bool SaveGeneralSet()
        {
            bool result = true;
            _errMsg = string.Empty;
            string basePath = AppDomain.CurrentDomain.BaseDirectory + BaseDirectory;
            foreach (var item in sets.Keys)//4-0-942B
            {
                string path = basePath + "\\" + item;
                if (!sets[item].SaveGeneralSet(path))
                {
                    _errMsg += "\r\n" + item + "保存失败：" + sets[item].ErrMsg;
                    result = false;
                }
            }
            return result;
        }

        private void CopyDirectory(string sourcePath, string destPath)
        {
            string floderName = Path.GetFileName(sourcePath);
            DirectoryInfo di = Directory.CreateDirectory(Path.Combine(destPath, floderName));
            string[] files = Directory.GetFileSystemEntries(sourcePath);

            foreach (string file in files)
            {
                if (Directory.Exists(file))
                {
                    CopyDirectory(file, di.FullName);
                }
                else
                {
                    File.Copy(file, Path.Combine(di.FullName, Path.GetFileName(file)), true);
                }
            }
        }
    }
    //一个车型下所有参数
    public class CarSetting
    {
        string _errMsg = string.Empty;
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg => _errMsg;

        public string CarName;
        public CarSetting(string carName)
        {
            CarName = carName;
        }

        /// <summary>
        /// 常规参数
        /// </summary>
        public Dictionary<int, GeneralSet> gSets = new Dictionary<int, GeneralSet>();

        /// <summary>
        /// 匹配识别模板
        /// </summary>
        public Dictionary<int, HNCCModel> Models0 = new Dictionary<int, HNCCModel>();
        /// <summary>
        /// 第二个孔的匹配识别模板
        /// </summary>
        public Dictionary<int, HNCCModel> Models1 = new Dictionary<int, HNCCModel>();
        /// <summary>
        /// 匹配识别模板
        /// </summary>
        public Dictionary<int, HShapeModel> ShapeModels0 = new Dictionary<int, HShapeModel>();
        /// <summary>
        /// 第二个孔的匹配识别模板
        /// </summary>
        public Dictionary<int, HShapeModel> ShapeModels1 = new Dictionary<int, HShapeModel>();

        //搜索区域
        public Dictionary<int, HRegion> rectangle0 = new Dictionary<int, HRegion>();
        //第二个孔搜索区域
        public Dictionary<int, HRegion> rectangle1 = new Dictionary<int, HRegion>();

        public Dictionary<int, PointF> modeCenter0 = new Dictionary<int, PointF>();
        //第二个孔模板中心
        public Dictionary<int, PointF> modeCenter1 = new Dictionary<int, PointF>();

        /// <summary>
        /// 相机外参
        /// </summary>
        public Dictionary<int, HPose> camPose = new Dictionary<int, HPose>();

        /// <summary>
        /// 加载车型的常规参数、标定位姿、两个孔的模型、搜索区域和模板中心。
        /// 模型文件名格式为“点位号_孔编号”，孔编号 0 为原孔，1 为第二个孔。
        /// </summary>
        public bool Load(string basePath)
        {
            _errMsg = string.Empty;
            bool result2 = LoadGeneralSet(basePath);

            bool result5 = true;
            try
            {
                string modelBasePath = basePath + "\\Model\\";
                if (Directory.Exists(modelBasePath))
                {
                    string[] shapeModelPaths = Directory.GetFiles(modelBasePath, "*_*.shm");
                    if (shapeModelPaths.Length > 0)
                    {
                        for (int i = 0; i < shapeModelPaths.Length; i++)
                        {
                            if (TryParseHoleFileName(shapeModelPaths[i], out int key, out int holeIndex))
                            {
                                LoadShapeModel(shapeModelPaths[i], key, holeIndex);
                            }
                        }
                    }

                    string[] modelPaths = Directory.GetFiles(modelBasePath, "*_*.ncc");
                    if (modelPaths.Length > 0)
                    {
                        for (int i = 0; i < modelPaths.Length; i++)
                        {
                            if (TryParseHoleFileName(modelPaths[i], out int key, out int holeIndex))
                            {
                                LoadNccModel(modelPaths[i], key, holeIndex);
                            }
                        }
                    }

                    if (shapeModelPaths.Length == 0 && modelPaths.Length == 0)
                    {
                        result5 = false;
                        _errMsg += $"\r\n{modelBasePath}文件夹下不存在shm或ncc文件";
                    }

                    string[] txtPaths = Directory.GetFiles(modelBasePath, "*_*.txt");
                    if (txtPaths.Length > 0)
                    {
                        for (int i = 0; i < txtPaths.Length; i++)
                        {
                            if (TryParseHoleFileName(txtPaths[i], out int key, out int holeIndex))
                            {
                                using (StreamReader reader = new StreamReader(txtPaths[i], Encoding.UTF8))
                                {
                                    string[] strings = reader.ReadLine().Split(',');
                                    if (strings.Length == 4 && double.TryParse(strings[0], out double row1) && double.TryParse(strings[1], out double column1)
                                        && double.TryParse(strings[2], out double row2) && double.TryParse(strings[3], out double column2))
                                    {
                                        new HImage("byte", (int)(column2 + 1), (int)(row2 + 1)).Dispose();//类似于画布，要能覆盖下面创建的区域
                                        SetRectangle(key, holeIndex, new HRegion(row1, column1, row2, column2));
                                    }

                                    string[] printFs = reader.ReadLine().Split(',');
                                    if (printFs.Length == 2 && double.TryParse(printFs[0], out double row) && double.TryParse(printFs[1], out double column))
                                    {
                                        SetModeCenter(key, holeIndex, new PointF((float)column, (float)row));
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        result5 = false;
                        _errMsg += $"\r\n{modelBasePath}文件夹下不存在txt文件";
                    }
                }
                else
                {
                    result5 = false;
                    _errMsg += $"\r\n{modelBasePath}文件夹不存在";
                }
            }
            catch (Exception ex)
            {
                result5 = false;
                _errMsg += "\r\n" + ex.ToString();
            }

            return result2 && result5;
        }

        /// <summary>
        /// 解析模型、区域和中心点文件名，文件名格式为“点位号_孔编号”。
        /// </summary>
        private bool TryParseHoleFileName(string filePath, out int key, out int holeIndex)
        {
            key = 0;
            holeIndex = 0;
            string name = Path.GetFileNameWithoutExtension(filePath);
            int separatorIndex = name.LastIndexOf('_');
            if (separatorIndex <= 0 || separatorIndex >= name.Length - 1)
            {
                return false;
            }

            return int.TryParse(name.Substring(0, separatorIndex), out key)
                && int.TryParse(name.Substring(separatorIndex + 1), out holeIndex)
                && (holeIndex == 0 || holeIndex == 1);
        }

        /// <summary>
        /// 按孔编号保存形状模型，并释放同一位置已经加载的旧模型。
        /// </summary>
        private void LoadShapeModel(string filePath, int key, int holeIndex)
        {
            Dictionary<int, HShapeModel> models = holeIndex == 0 ? ShapeModels0 : ShapeModels1;
            if (models.ContainsKey(key) && models[key] != null)
            {
                models[key].Dispose();
            }
            models[key] = new HShapeModel(filePath);
        }

        /// <summary>
        /// 按孔编号保存 NCC 模型，并释放同一位置已经加载的旧模型。
        /// </summary>
        private void LoadNccModel(string filePath, int key, int holeIndex)
        {
            Dictionary<int, HNCCModel> models = holeIndex == 0 ? Models0 : Models1;
            if (models.ContainsKey(key) && models[key] != null)
            {
                models[key].Dispose();
            }
            models[key] = new HNCCModel(filePath);
        }

        /// <summary>
        /// 按孔编号保存搜索区域。
        /// </summary>
        private void SetRectangle(int key, int holeIndex, HRegion region)
        {
            Dictionary<int, HRegion> rectangles = holeIndex == 0 ? rectangle0 : rectangle1;
            if (rectangles.ContainsKey(key) && rectangles[key] != null)
            {
                rectangles[key].Dispose();
            }
            rectangles[key] = region;
        }

        /// <summary>
        /// 按孔编号保存模板中心。
        /// </summary>
        private void SetModeCenter(int key, int holeIndex, PointF center)
        {
            Dictionary<int, PointF> centers = holeIndex == 0 ? modeCenter0 : modeCenter1;
            centers[key] = center;
        }

        /// <summary>
        /// 判断指定测点和孔编号是否已经具备模型、搜索区域和模板中心。
        /// </summary>
        public bool HasDetectionData(int key, int holeIndex)
        {
            Dictionary<int, HNCCModel> nccModels = holeIndex == 0 ? Models0 : Models1;
            Dictionary<int, HShapeModel> shapeModels = holeIndex == 0 ? ShapeModels0 : ShapeModels1;
            Dictionary<int, HRegion> rectangles = holeIndex == 0 ? rectangle0 : rectangle1;
            Dictionary<int, PointF> centers = holeIndex == 0 ? modeCenter0 : modeCenter1;
            return rectangles.ContainsKey(key) && centers.ContainsKey(key)
                && (nccModels.ContainsKey(key) || shapeModels.ContainsKey(key));
        }

        public bool LoadGeneralSet(string basePath)
        {
            _errMsg = string.Empty;
            bool result2 = false;
            try
            {
                string gPath = basePath + "\\Setting.xml";
                if (File.Exists(gPath))
                {
                    List<GeneralSet> listgSets = new List<GeneralSet>();
                    XmlSerializer xmlgSets = new XmlSerializer(listgSets.GetType());
                    using (FileStream stream = new FileStream(gPath, FileMode.OpenOrCreate))
                    {
                        listgSets = (List<GeneralSet>)xmlgSets.Deserialize(stream);
                    }
                    if (listgSets != null)
                    {
                        gSets = listgSets.ToDictionary(n => { return n.key; });
                        result2 = true;
                    }
                    else
                    {
                        _errMsg = gPath + "文件格式异常";
                    }
                }
                else
                {
                    _errMsg = gPath + "不存在文件";
                }
            }
            catch (Exception ex)
            {
                _errMsg = ex.ToString();
            }

            bool result6 = true;
            try
            {
                string poseBasePath = basePath + "\\CamPose\\";
                if (Directory.Exists(poseBasePath))
                {
                    string[] posePaths = Directory.GetFiles(poseBasePath, "*.dat");
                    if (posePaths.Length > 0)
                    {
                        for (int i = 0; i < posePaths.Length; i++)
                        {
                            string name = Path.GetFileNameWithoutExtension(posePaths[i]);
                            if (int.TryParse(name, out int key))
                            {
                                HPose hPose = new HPose();
                                hPose.ReadPose(posePaths[i]);
                                if (camPose.ContainsKey(key))
                                {
                                    camPose[key] = hPose;
                                }
                                else
                                {
                                    camPose.Add(key, hPose);
                                }
                            }
                        }
                    }
                }
                else
                {
                    result6 = false;
                }
            }
            catch (Exception ex)
            {
                result6 = false;
                _errMsg += "\r\n" + ex.ToString();
            }

            return result2 && result6;
        }

        public bool Save(string basePath)
        {
            _errMsg = string.Empty;
            bool result = SaveGeneralSet(basePath);
            try
            {
                string modelBasePath = basePath + "\\Model\\";
                if (!Directory.Exists(modelBasePath))
                {
                    Directory.CreateDirectory(modelBasePath);
                }
                //foreach (var item in Models0.Keys)
                //{
                //    string name = item.ToString("00");
                //    string modelPath = modelBasePath + name + "_0.ncc";
                //    Models0[item].WriteNccModel(modelPath);
                //}
                //foreach (var item in rectangle0.Keys)
                //{
                //    string name = item.ToString("00");
                //    string txtPath = modelBasePath + name + "_0.txt";
                //    using (StreamWriter writer = new StreamWriter(txtPath, false, Encoding.UTF8))
                //    {
                //        rectangle0[item].SmallestRectangle1(out int row1, out int column1, out int row2, out int column2);
                //        writer.WriteLine($"{row1},{column1},{row2},{column2}");
                //        writer.WriteLine($"{modeCenter0[item].Y},{modeCenter0[item].X}");
                //    }
                //}
            }
            catch (Exception ex)
            {
                result = false;
                _errMsg += "\r\n" + ex.ToString();
            }

            try
            {
                string poseBasePath = basePath + "\\CamPose";
                if (!Directory.Exists(poseBasePath))
                {
                    Directory.CreateDirectory(poseBasePath);
                }
                //foreach (var item in camPose.Keys)
                //{
                //    string name = item.ToString("00");
                //    string posePath = $"{poseBasePath}\\{name}.dat";
                //    camPose[item].WritePose(posePath);
                //}
            }
            catch (Exception ex)
            {
                result = false;
                _errMsg += "\r\n" + ex.ToString();
            }

            return result;
        }

        public bool SaveGeneralSet(string basePath)
        {
            bool result = true;
            try
            {
                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }
                List<GeneralSet> listgSets = gSets.Values.OrderBy(n => { return n.key; }).ToList();//排序
                string fPath = basePath + "\\Setting.xml";
                XmlSerializer xml = new XmlSerializer(listgSets.GetType());
                using (FileStream stream = new FileStream(fPath, FileMode.Create))
                {
                    xml.Serialize(stream, listgSets);
                }
            }
            catch (Exception ex)
            {
                result = false;
                _errMsg = ex.ToString();
            }

            return result;
        }

        public bool Find0PxPy(HImage hImage, int index, out HTuple row, out HTuple column, out double dy, out double dx, out double outScore)
        {
            if (rectangle0.ContainsKey(index) && Models0.ContainsKey(index) && gSets.ContainsKey(index))
            {
                HImage hImageReduced = hImage.ReduceDomain(rectangle0[index]);
                hImageReduced.FindNccModel(Models0[index], -10 * Math.PI / 180, 20 * Math.PI / 180, 0.5, 1, 0.5, "true", 0, out row, out column, out HTuple angle, out HTuple score);
                if (score.Length == 1 && score.D >= gSets[index].score0)
                {
                    dy = row.D - modeCenter0[index].Y;
                    dx = column.D - modeCenter0[index].X;
                    outScore = score.D;
                    return true;
                }
            }
            row = new HTuple();
            column = new HTuple();
            dy = 0;
            dx = 0;
            outScore = 0;
            return false;
        }

        public bool FindPxPy(HImage map, HRegion hRegion, HNCCModel hNCCModel, double inScore, double modeCenterX, double modeCenterY, ref float Px, ref float Py, out double dx, out double dy, out double outScore)
        {
            HImage hImageReduced = map.ReduceDomain(hRegion);
            hImageReduced.FindNccModel(hNCCModel, -10 * Math.PI / 180, 20 * Math.PI / 180, 0.5, 1, 0.5, "true", 0, out HTuple row, out HTuple column, out HTuple angle, out HTuple hscore);
            if (hscore.Length == 1 && hscore.D >= inScore)
            {
                dy = row.D - modeCenterY;
                dx = column.D - modeCenterX;
                Px = (float)column.D;
                Py = (float)row.D;
                outScore = hscore.D;
                return true;
            }
            dy = 0;
            dx = 0;
            outScore = 0;
            return false;
        }
        public bool FindPxPy(HImage map, HRegion hRegion, HShapeModel hShapeModel, bool sharpenProcess, double inScore, double modeCenterX, double modeCenterY, ref float Px, ref float Py, out double dx, out double dy, out double outScore)
        {
            HImage hImageReduced = map.ReduceDomain(hRegion);
            //hImageReduced.FindShapeModel(hShapeModel, -0.1, 0.2, 0.5, 1, 0.5, "least_squares", 0, 0.9, out HTuple row, out HTuple column, out HTuple angle, out HTuple hscore);
            HImage hImageProcess = hImageReduced.MedianImage("circle", 2, "mirrored");

            if (sharpenProcess)
            {
                hImageProcess = hImageProcess.Emphasize(15, 15, 1.5);
            }

            hImageProcess.FindScaledShapeModel(hShapeModel, 0, 0, 0.99, 1.1, 0.5, 1, 0.5, "least_squares", 0, 0.9, out HTuple row, out HTuple column, out HTuple angle, out HTuple scaled, out HTuple hscore);
            if (hscore.Length == 1 && hscore.D >= inScore)
            {
                dy = row.D - modeCenterY;
                dx = column.D - modeCenterX;
                Px = (float)column.D;
                Py = (float)row.D;
                outScore = hscore.D;
                return true;
            }
            dy = 0;
            dx = 0;
            outScore = 0;
            return false;
        }

        /// <summary>
        /// 使用指定测点和孔编号对应的模型、搜索区域完成定位。
        /// </summary>
        public bool FindPxPy(HImage map, int key, int holeIndex, bool sharpenProcess, double inScore, ref float Px, ref float Py, out double dx, out double dy, out double outScore)
        {
            if (!HasDetectionData(key, holeIndex))
            {
                dx = 0;
                dy = 0;
                outScore = 0;
                return false;
            }

            Dictionary<int, HNCCModel> nccModels = holeIndex == 0 ? Models0 : Models1;
            Dictionary<int, HShapeModel> shapeModels = holeIndex == 0 ? ShapeModels0 : ShapeModels1;
            Dictionary<int, HRegion> rectangles = holeIndex == 0 ? rectangle0 : rectangle1;
            Dictionary<int, PointF> centers = holeIndex == 0 ? modeCenter0 : modeCenter1;
            if (shapeModels.ContainsKey(key))
            {
                return FindPxPy(map, rectangles[key], shapeModels[key], sharpenProcess, inScore,
                    centers[key].X, centers[key].Y, ref Px, ref Py, out dx, out dy, out outScore);
            }

            return FindPxPy(map, rectangles[key], nccModels[key], inScore,
                centers[key].X, centers[key].Y, ref Px, ref Py, out dx, out dy, out outScore);
        }
        public HImage GetShowHIamge(HImage hImage, HRegion hRegion, double modeCenterX, double modeCenterY, double px, double py, HShapeModel hShapeModel)
        {
            double w = 1;
            //模板十字架
            HRegion hRegionX = new HRegion();
            hRegionX.GenRectangle2(modeCenterY, modeCenterX, 0, 20, w);
            HRegion hRegionY = new HRegion();
            hRegionY.GenRectangle2(modeCenterY, modeCenterX, 0, w, 20);
            HRegion hRegionXY = hRegionY.Union2(hRegionX);
            hRegionX.Dispose();
            hRegionY.Dispose();
            //匹配十字架
            HRegion hRegionL = new HRegion();
            hRegionL.GenRectangle2(py, px, 0, 20, w);
            HRegion hRegionH = new HRegion();
            hRegionH.GenRectangle2(py, px, 0, w, 20);
            HRegion hRegionLH = hRegionH.Union2(hRegionL);
            hRegionL.Dispose();
            hRegionH.Dispose();
            //匹配模板
            HHomMat2D hHomMat2D = new HHomMat2D();
            hHomMat2D.VectorAngleToRigid(0, 0, 0, py, px, 0);
            HRegion hRegionCont = hShapeModel.GetShapeModelContours(1).AffineTransContourXld(hHomMat2D).GenRegionContourXld("margin");
            //联合所有区域   
            HRegion hRegion0XY = hRegion.ConcatObj(hRegionXY);
            HRegion hRegion0XYLH = hRegion0XY.ConcatObj(hRegionLH);
            hRegion0XY.Dispose();
            HRegion hRegionAll = hRegion0XYLH.ConcatObj(hRegionCont);
            hRegion0XYLH.Dispose();
            //喷涂
            HImage hImage0 = hImage.PaintRegion(hRegionAll, 0d, "margin");
            HImage hImage1 = hImage0.PaintRegion(hRegionLH, 255d, "margin").PaintRegion(hRegionCont, 255d, "margin");//red
            HImage hImage2 = hImage0.PaintRegion(hRegionXY, 255d, "margin");//green
            HImage hImage3 = hImage0.PaintRegion(hRegion, 255d, "margin");//blue
            hRegionXY.Dispose();
            hRegionLH.Dispose();
            hRegionCont.Dispose();
            hRegionAll.Dispose();
            hImage0.Dispose();
            //合成
            HImage hImageAll = hImage1.Compose3(hImage2, hImage3);
            hImage1.Dispose();
            hImage2.Dispose();
            hImage3.Dispose();

            return hImageAll;
        }
        public HImage GetShowHIamge(HImage hImage, HRegion hRegion, double modeCenterX, double modeCenterY, double px, double py)
        {
            double w = 1;
            //模板十字架
            HRegion hRegionX = new HRegion();
            hRegionX.GenRectangle2(modeCenterY, modeCenterX, 0, 20, w);
            HRegion hRegionY = new HRegion();
            hRegionY.GenRectangle2(modeCenterY, modeCenterX, 0, w, 20);
            HRegion hRegionXY = hRegionY.Union2(hRegionX);
            hRegionX.Dispose();
            hRegionY.Dispose();
            //匹配十字架
            HRegion hRegionL = new HRegion();
            hRegionL.GenRectangle2(py, px, 0, 20, w);
            HRegion hRegionH = new HRegion();
            hRegionH.GenRectangle2(py, px, 0, w, 20);
            HRegion hRegionLH = hRegionH.Union2(hRegionL);
            hRegionL.Dispose();
            hRegionH.Dispose();
            //联合所有区域
            HRegion hRegion0XY = hRegion.ConcatObj(hRegionXY);
            HRegion hRegion0XYLH = hRegion0XY.ConcatObj(hRegionLH);
            hRegion0XY.Dispose();
            //喷涂
            HImage hImage0 = hImage.PaintRegion(hRegion0XYLH, 0d, "margin");
            HImage hImage1 = hImage0.PaintRegion(hRegionLH, 255d, "margin");
            HImage hImage2 = hImage0.PaintRegion(hRegionXY, 255d, "margin");
            HImage hImage3 = hImage0.PaintRegion(hRegion, 255d, "margin");
            hRegionXY.Dispose();
            hRegionLH.Dispose();
            hRegion0XYLH.Dispose();
            hImage0.Dispose();
            //合成
            HImage hImageAll = hImage1.Compose3(hImage2, hImage3);
            hImage1.Dispose();
            hImage2.Dispose();
            hImage3.Dispose();

            return hImageAll;
        }
        public HImage GetShowHIamge(HImage hImage, HRegion hRegion1, double modeCenterX1, double modeCenterY1, double px1, double py1, HRegion hRegion2, double modeCenterX2, double modeCenterY2, double px2, double py2)
        {
            double w = 1;
            //搜索区域
            HRegion hRegion = hRegion1.ConcatObj(hRegion2);

            //模板十字架
            HRegion hRegionX1 = new HRegion();
            hRegionX1.GenRectangle2(modeCenterY1, modeCenterX1, 0, 20, w);
            HRegion hRegionY1 = new HRegion();
            hRegionY1.GenRectangle2(modeCenterY1, modeCenterX1, 0, w, 20);
            HRegion hRegionXY1 = hRegionY1.Union2(hRegionX1);

            HRegion hRegionX2 = new HRegion();
            hRegionX2.GenRectangle2(modeCenterY2, modeCenterX2, 0, 20, w);
            HRegion hRegionY2 = new HRegion();
            hRegionY2.GenRectangle2(modeCenterY2, modeCenterX2, 0, w, 20);
            HRegion hRegionXY2 = hRegionY2.Union2(hRegionX2);

            HRegion hRegionXY = hRegionXY1.Union2(hRegionXY2);

            //匹配十字架
            HRegion hRegionL1 = new HRegion();
            hRegionL1.GenRectangle2(py1, px1, 0, 20, w);
            HRegion hRegionH1 = new HRegion();
            hRegionH1.GenRectangle2(py1, px1, 0, w, 20);
            HRegion hRegionLH1 = hRegionH1.Union2(hRegionL1);

            HRegion hRegionL2 = new HRegion();
            hRegionL2.GenRectangle2(py2, px2, 0, 20, w);
            HRegion hRegionH2 = new HRegion();
            hRegionH2.GenRectangle2(py2, px2, 0, w, 20);
            HRegion hRegionLH2 = hRegionH2.Union2(hRegionL2);

            HRegion hRegionLH = hRegionLH1.Union2(hRegionLH2);

            //联合所有区域
            HRegion hRegion0XY = hRegion.ConcatObj(hRegionXY);
            HRegion hRegion0XYLH = hRegion0XY.ConcatObj(hRegionLH);
            //喷涂
            HImage hImage0 = hImage.PaintRegion(hRegion0XYLH, 0d, "margin");
            HImage hImage1 = hImage0.PaintRegion(hRegionLH, 255d, "margin");
            HImage hImage2 = hImage0.PaintRegion(hRegionXY, 255d, "margin");
            HImage hImage3 = hImage0.PaintRegion(hRegion, 255d, "margin");

            return hImage1.Compose3(hImage2, hImage3);
        }
    }

    [Serializable]
    public class GeneralSet
    {
        public int key;//键
        public string name = string.Empty;//名称，别名
        public string CamName = string.Empty;//相机
        public bool keepOK = false;
        public bool 相对基准点 = false;
        public bool 增强对比度 = false;
        public bool sharpenProcess = false;
        public string convertX = "X", convertY = "Y";//坐标数据转换
        public float offsetX = 0, offsetY = 0, offsetZ = 0;//补偿坐标
        //一级绿色公差
        public float minDX_1 = -1.0f, minDY_1 = -1.0f, minDZ_1 = -1.0f;//下限
        public float maxDX_1 = 1.0f, maxDY_1 = 1.0f, maxDZ_1 = 1.0f;//上限
        //二级黄色公差
        public float minDX_2 = -1.5f, minDY_2 = -1.5f, minDZ_2 = -1.5f;//下限
        public float maxDX_2 = 1.5f, maxDY_2 = 1.5f, maxDZ_2 = 1.5f;//上限
        //一级绿色公差
        public float minNewDX_1 = -1.0f, minNewDY_1 = -1.0f, minNewDZ_1 = -1.0f;//下限
        public float maxNewDX_1 = 1.0f, maxNewDY_1 = 1.0f, maxNewDZ_1 = 1.0f;//上限
        //二级黄色公差
        public float minNewDX_2 = -1.5f, minNewDY_2 = -1.5f, minNewDZ_2 = -1.5f;//下限
        public float maxNewDX_2 = 1.5f, maxNewDY_2 = 1.5f, maxNewDZ_2 = 1.5f;//上限

        /// <summary>
        /// 是否启用第二个孔以及两孔相对距离检测，旧配置默认关闭。
        /// </summary>
        public bool enablePoints = false;
        // 两孔相对距离一级绿色阈值
        public float minPointsDX_1 = -1.0f, minPointsDY_1 = -1.0f, minPointsDZ_1 = -1.0f, minPointsDD_1 = -1.0f;//下限
        public float maxPointsDX_1 = 1.0f, maxPointsDY_1 = 1.0f, maxPointsDZ_1 = 1.0f, maxPointsDD_1 = 1.0f;//上限
        // 两孔相对距离二级黄色阈值
        public float minPointsDX_2 = -1.5f, minPointsDY_2 = -1.5f, minPointsDZ_2 = -1.5f, minPointsDD_2 = -1.5f;//下限
        public float maxPointsDX_2 = 1.5f, maxPointsDY_2 = 1.5f, maxPointsDZ_2 = 1.5f, maxPointsDD_2 = 1.5f;//上限

        public int ledExposure = 4000;//曝光

        public double score0 = 0.5;//匹配分数

    }

    [Serializable]
    public class OtherSet
    {
        public bool isSaveImage = true;
        public string imagePath = "D:\\image";
        public bool bTRnum = false;

        string _errMsg = string.Empty;
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg => _errMsg;
        public bool Load()
        {
            bool result2 = false;
            try
            {
                string gPath = "Data\\OtherSet.xml";
                if (File.Exists(gPath))
                {
                    OtherSet set = null;
                    XmlSerializer xmlgSets = new XmlSerializer(this.GetType());
                    using (FileStream stream = new FileStream(gPath, FileMode.OpenOrCreate))
                    {
                        set = (OtherSet)xmlgSets.Deserialize(stream);
                    }
                    if (set != null)
                    {
                        this.isSaveImage = set.isSaveImage;
                        this.imagePath = set.imagePath;
                        bTRnum = set.bTRnum;
                        result2 = true;
                    }
                    else
                    {
                        _errMsg = gPath + "文件格式异常";
                    }
                }
                else
                {
                    _errMsg = gPath + "文件不存在";
                }
            }
            catch (Exception ex)
            {
                _errMsg = ex.ToString();
            }

            return result2;
        }
        public bool Save()
        {
            bool result = true;
            try
            {
                string basePath = "Data";
                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }

                string fPath = basePath + "\\OtherSet.xml";
                XmlSerializer xml = new XmlSerializer(this.GetType());
                using (FileStream stream = new FileStream(fPath, FileMode.Create))
                {
                    xml.Serialize(stream, this);
                }
            }
            catch (Exception ex)
            {
                result = false;
                _errMsg = ex.ToString();
            }

            return result;
        }
    }



    public class Cam
    {
        static public readonly string BaseDirectory = "Data\\Cam";
        string _errMsg = string.Empty;
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg => _errMsg;

        /// <summary>
        /// 相机名-相机参数
        /// </summary>
        public Dictionary<string, CamSetting> sets = new Dictionary<string, CamSetting>();

        public bool Load()
        {
            bool result = true;
            _errMsg = string.Empty;
            sets.Clear();
            if (Directory.Exists(BaseDirectory))
            {
                string[] camPaths = Directory.GetDirectories(BaseDirectory);
                foreach (var item in camPaths)
                {
                    string dirName = Path.GetFileNameWithoutExtension(item);
                    CamSetting set = new CamSetting(dirName);
                    if (set.Load(item))
                    {
                        _errMsg += "\r\n" + set.ErrMsg;
                        result = false;
                    }
                    sets.Add(dirName, set);
                }
            }
            else
            {
                _errMsg = $"参数文件夹{BaseDirectory}不存在";
                result = false;
            }
            return result;
        }

        public bool Save()
        {
            bool result = true;
            _errMsg = string.Empty;
            string basePath = AppDomain.CurrentDomain.BaseDirectory + BaseDirectory;
            foreach (var item in sets.Keys)//4-0-942B
            {
                string path = basePath + "\\" + item;
                if (!sets[item].Save(path))
                {
                    _errMsg += "\r\n" + sets[item].ErrMsg;
                    result = false;
                }
            }
            return result;
        }
    }

    public class CamSetting
    {
        string CamName;
        /// <summary>
        /// 相机内参
        /// </summary>
        public HTuple camparam;

        public CamGenSet gSet = new CamGenSet();

        string _errMsg = string.Empty;
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg => _errMsg;

        public CamSetting(string camName)
        {
            CamName = camName;
        }

        public bool Load(string basePath)
        {
            _errMsg = string.Empty;
            bool result = true;
            try
            {
                string filePath = basePath + "\\camparam.cal";
                if (File.Exists(filePath))
                {
                    //HOperatorSet.ReadCamPar(filePath, out camparam);
                    camparam = HMisc.ReadCamPar(filePath);
                }
                else
                {
                    result = false;
                    _errMsg = filePath + "文件不存在";
                }
            }
            catch (Exception ex)
            {
                result = false;
                _errMsg = ex.ToString();
            }

            bool result2 = false;
            try
            {
                string gPath = basePath + "\\CamSetting.xml";
                if (File.Exists(gPath))
                {
                    CamGenSet camGenSet = new CamGenSet();
                    XmlSerializer xmlgSets = new XmlSerializer(typeof(CamGenSet));
                    using (FileStream stream = new FileStream(gPath, FileMode.OpenOrCreate))
                    {
                        camGenSet = (CamGenSet)xmlgSets.Deserialize(stream);
                    }
                    if (camGenSet != null)
                    {
                        gSet = camGenSet;
                        result2 = true;
                    }
                    else
                    {
                        _errMsg += "\r\n" + gPath + "文件格式异常";
                    }
                }
                else
                {
                    _errMsg += "\r\n" + gPath + "不存在文件";
                }
            }
            catch (Exception ex)
            {
                _errMsg += "\r\n" + ex.ToString();
            }
            if (result2 && gSet.Disabled)
            {
                return true;
            }
            return result && result2;
        }

        public bool Save(string basePath)
        {
            bool result = true;
            try
            {
                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }
                string gPath = basePath + "\\CamSetting.xml";
                XmlSerializer xml = new XmlSerializer(gSet.GetType());
                using (FileStream stream = new FileStream(gPath, FileMode.Create))
                {
                    xml.Serialize(stream, gSet);
                }
                string fPath = basePath + "\\camparam.cal";
                if (camparam != null)
                {
                    HMisc.WriteCamPar(camparam, fPath);
                }
            }
            catch (Exception ex)
            {
                result = false;
                _errMsg = ex.ToString();
            }

            return result;
        }

        /// <summary>
        /// 输入像素坐标，输出物理坐标xy
        /// </summary>
        /// <param name="Px"></param>
        /// <param name="Py"></param>
        /// <param name="z"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public bool GetXY(HPose worldPose, float Px, float Py, out float x, out float y)
        {
            try
            {
                HMisc.ImagePointsToWorldPlane(camparam, worldPose, Py, Px, "mm", out HTuple hx, out HTuple hy);
                x = (float)hx.D;
                y = (float)hy.D;
                return true;
            }
            catch (Exception ex)
            {
                _errMsg = ex.ToString();
                x = 0;
                y = 0;
                return false;
            }
        }


    }
    [Serializable]
    public class CamGenSet
    {
        public bool Disabled = false;
        // ???????????? CamSetting.xml ????????? Basler?
        public CameraType CameraType = OnlineMeasurement.CameraType.Basler;
    }

    [Serializable]
    public class Point3D
    {
        public float X, Y, Z;
        public Point3D(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Point3D(double x, double y, double z)
        {
            X = (float)x;
            Y = (float)y;
            Z = (float)z;
        }
    }
    [Serializable]
    public class Point3DColor
    {
        public Color ColorX;
        public Color ColorY;
        public Color ColorZ;
        public Point3DColor(Color colorX, Color colorY, Color colorZ)
        {
            ColorX = colorX;
            ColorY = colorY;
            ColorZ = colorZ;
        }
    }

    //数模显示参数
    [Serializable]
    public class ShowPoint
    {
        public string Name = " ";
        public Point Point = new Point();
        public Point Location = new Point();
        public int Connection = 0;
    }
    public class ShowCarXYZ
    {
        public string Name;
        public Dictionary<string, ShowPoint> Points = new Dictionary<string, ShowPoint>();
        public bool Load(string filePath)
        {
            bool result2 = false;
            try
            {
                if (File.Exists(filePath))
                {
                    List<ShowPoint> list = new List<ShowPoint>();
                    XmlSerializer xml = new XmlSerializer(list.GetType());
                    using (FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate))
                    {
                        list = (List<ShowPoint>)xml.Deserialize(stream);
                    }
                    if (list != null)
                    {
                        Points = list.ToDictionary(n => { return n.Name; });
                        result2 = true;
                    }
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText("Error.log", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  ") + ex.ToString() + "\r\n\r\n");
            }
            return result2;
        }
        public bool Save(string filePath)
        {
            bool result = true;
            try
            {
                string basePath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }
                List<ShowPoint> list = Points.Values.ToList();
                if (list.Count() == 0)
                {
                    list.Add(new ShowPoint());
                }
                XmlSerializer xml = new XmlSerializer(list.GetType());
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    xml.Serialize(stream, list);
                }
            }
            catch (Exception ex)
            {
                result = false;
                File.AppendAllText("Error.log", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  ") + ex.ToString() + "\r\n\r\n");
            }
            return result;
        }
    }
}
