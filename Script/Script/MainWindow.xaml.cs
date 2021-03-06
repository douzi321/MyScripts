﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using APILibrary.API;
using System.Threading;
using System.Windows.Interop;
using Emgu.CV.Structure;
using Emgu.CV;
using System.IO;
using Emgu.CV.OCR;
using Emgu.CV.CvEnum;
using System.ComponentModel;
using System.Globalization;
using System.Management;

namespace Script
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {


        /// <summary>
        /// 狗粮序号和按键的映射
        /// </summary>
        private Dictionary<int, OptionKeys> glsKeys = new Dictionary<int, OptionKeys>()
        {
            { 0, OptionKeys.GL1Key},
            { 1, OptionKeys.GL2Key},
            { 2, OptionKeys.GL3Key},
        };

        /// <summary>
        /// 战利品集合
        /// </summary>
        private Dictionary<Image<Bgr, byte>, NumClass> Spoils = new Dictionary<Image<Bgr, byte>, NumClass> {
            { TempImages.CFImgTemp, new NumClass(0, "超凡石头")},
            { TempImages.CHGuaiTemp, new NumClass(0, "彩虹怪")},
            { TempImages.CSFWTemp, new NumClass(0, "传说符文")},
            { TempImages.CZTemp, new NumClass(0, "厕纸")},
            { TempImages.FSImgTemp, new NumClass(0, "符石石头")},
            { TempImages.HDImgTemp, new NumClass(0, "混沌石头")},
            { TempImages.HSTemp, new NumClass(0, "黄书")},
            { TempImages.HXImgTemp, new NumClass(0, "和谐石头")},{ TempImages.STTemp, new NumClass(0, "召唤石")},
            { TempImages.XYFWTemp, new NumClass(0, "稀有符文")},
            { TempImages.YXFWTemp, new NumClass(0, "英雄符文")},
        };
        /// <summary>
        /// 任务模式
        /// </summary>
        private TaskMode TaskMode = TaskMode.None;
        /// <summary>
        /// 前后台模式
        /// </summary>
        //private RunMode RunMode = RunMode.Fast;
        /// <summary>
        /// 默认大小
        /// </summary>
        private static System.Drawing.Size DefultRectangle = new System.Drawing.Size(1314, 769);
        /// <summary>
        /// 默认窗口模式
        /// </summary>
        private ScreenMode ScreenMode = ScreenMode.Window;
        private Task<bool> runningTask = null;
        /// <summary>
        /// 任务取消标记
        /// </summary>
        private CancellationTokenSource taskCancle = new CancellationTokenSource();
        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static string configPath = "config.config";
        /// <summary>
        /// adb路径
        /// </summary>
        private string vriualExePath = "";
        /// <summary>
        /// 是否停止任务
        /// </summary>
        private bool isStopTask = false;
        /// <summary>
        /// 随机数
        /// </summary>
        private Random Random = new Random();

        /// <summary>
        /// id
        /// </summary>
        private static readonly string UUID = "F28FBD13";

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }



        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            splios.ItemsSource = Spoils;
            window.IsChecked = true;
            //MessageBox.Show(getVriualSize().ToString());
            if(File.Exists(configPath))
            {
                
                using (FileStream stream = File.Open(configPath, FileMode.Open))
                {
                    byte[] datas = new byte[stream.Length];
                    stream.Read(datas, 0, datas.Length);
                    vriualPath.Text = System.Text.Encoding.UTF8.GetString(datas);
                    datas = null;
                    vriualExePath = System.IO.Path.Combine(vriualPath.Text, "bin\\nox_adb.exe");
                    if (!System.IO.File.Exists(vriualExePath))
                    {
                        MessageBox.Show("路径错误，请选择夜神模拟器的根路径,一般是Nox到这里为止就好！");
                    }
                }

            }

            //if(!GetDiskVolumeSerialNumber().Equals(UUID))
            //{
            //    MessageBox.Show("该电脑没有授权!");
            //    Application.Current.Shutdown();
            //}
            //Write(matchtempforsharp(TempImages.CFImgTemp, TempImages.OrigImagePath.ToImageBgr()).ToString());
            //checkSpoils();
        }
        /// <summary>
        /// 按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }
        /// <summary>
        /// 运行程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Run_Click(object sender, RoutedEventArgs e)
        {
            Corrd corrd = new Corrd();
            try
            {
                corrd.Times = Convert.ToInt32(failTimes.Text);
                corrd.StopTime = DateTime.Now + TimeSpan.FromHours(Convert.ToDouble(stopTime.Text));
                corrd.WaitTime = Convert.ToInt32(checkTime.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("参数填写错误!");
                return;
            }
            if (TaskMode == TaskMode.None)
            {
                Write("没有选择任务运行的模式!");
                return;
            }
            else if (TaskMode == TaskMode.DGL)
            {
                InitVriual();
                Write("选择了带狗粮,准备开始进入带狗粮模式!");
                runningTask = RunDGLScript(corrd);
                runTask.IsEnabled = false;
                runningTask.ContinueWith(t =>
                {
                    Write("带狗粮任务结束, 返回结果" + t.Result.ToString());
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        EndInitVriual();
                    }));
                });
            }
            else if (TaskMode == TaskMode.DXC)
            {
                InitVriual();
                Write("选择了地下城,准备开始进入地下城模式!");
                runningTask = RunDungeonsScript(corrd);
                runTask.IsEnabled = false;
                runningTask.ContinueWith(t =>
                {
                    Write("地下城任务结束, 返回结果" + t.Result.ToString());
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        EndInitVriual();
                    }));
                });
            }

        }
        /// <summary>
        /// 停止任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            if(runningTask != null && runningTask.IsCompleted == false)
            {
                taskCancle.Cancel();
                isStopTask = true;
                Write("设置了停止信号等待一轮结束然后停止!");
            }
        }
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="msg"></param>
        public void Write(string m)
        {
            this.Dispatcher.Invoke(new Action<string>((msg) => 
            {
                Run run = new Run(msg + "\r\n");
                if (log.Inlines.Count > 300)
                {
                    log.Inlines.Clear();
                }
                log.Inlines.Add(run);
                logbox.ScrollToEnd();
            }), m);
        }
        /// <summary>
        /// 按键
        /// </summary>
        /// <param name="enumKeyboardKey"></param>
        private void keyoption(OptionKeys enumKeyboardKey)
        {
            if(ScreenMode == ScreenMode.Desktop)
            {
                WindowsAPI.KeyDown(enumKeyboardKey.Key);
                Thread.Sleep(100);
                WindowsAPI.KeyUp(enumKeyboardKey.Key);
            }
            else if(ScreenMode == ScreenMode.Window || ScreenMode == ScreenMode.Vriual)
            {
                string s = "";
                if(enumKeyboardKey.IsDrag)
                {
                    s = "shell input swipe " + (enumKeyboardKey.Position.X + Random.Next(-5, 5)).ToString()
                        + " " + (enumKeyboardKey.Position.Y + Random.Next(-5, 5)).ToString() + " " + (enumKeyboardKey.EndPosition.X + Random.Next(-5, 5)).ToString()
                        + " " + (enumKeyboardKey.EndPosition.Y + Random.Next(-5, 5)).ToString() + " 600";
                }
                else
                {
                    s = "shell input tap " + (enumKeyboardKey.Position.X + Random.Next(-5, 5))
                        + " " + (enumKeyboardKey.Position.Y + Random.Next(-5, 5));
                    //Write(s);
                }
                startADBEXE(s);
            }
            
            Thread.Sleep(Random.Next(200, 4000));
            //Write("按下了 : " + enumKeyboardKey);
        }

        /// <summary>
        /// 模板匹配 匹配origin图像中是否存在模板tempImage
        /// </summary>
        /// <param name="tempImage"></param>
        /// <param name="orginImage"></param>
        /// <returns></returns>
        private bool matchtempforsharp(Image<Bgr, byte> tempImage, Image<Bgr, byte> orginImage, ref Rect rect)
        {
            
            Image<Gray, float> resultImage = orginImage.MatchTemplate(tempImage, Emgu.CV.CvEnum.TemplateMatchingType.SqdiffNormed);
            double[] max;
            double[] min;
            System.Drawing.Point[] min_point;
            System.Drawing.Point[] max_point;
            resultImage.MinMax(out min, out max, out min_point, out max_point);

            rect.X = min_point[0].X;
            rect.Y = min_point[0].Y;
            rect.Size = new Size(tempImage.Width, tempImage.Height);
            
            resultImage.Dispose();
            resultImage = null;

            max = null;
            min = null;
            min_point = null;
            max_point = null;
            return min[0] < 0.6;
        }
        /// <summary>
        /// 模板匹配 匹配origin图像中是否存在模板tempImage
        /// </summary>
        /// <param name="tempImage"></param>
        /// <param name="orginImage"></param>
        /// <returns></returns>
        private bool matchtempforsharp(Image<Bgr, byte> tempImage, double throld = 0.13)
        {
            getPicture();
            double value;
            using (Image<Bgr, byte> orginImage = new Image<Bgr, byte>(TempImages.OrigImagePath))
            {
                Image<Gray, float> resultImage = orginImage.MatchTemplate(tempImage, Emgu.CV.CvEnum.TemplateMatchingType.SqdiffNormed);
                double[] max;
                double[] min;
                
                System.Drawing.Point[] min_point;
                System.Drawing.Point[] max_point;
                resultImage.MinMax(out min, out max, out min_point, out max_point);

                //orginImage.Draw(new System.Drawing.Rectangle(min_point[0].X, min_point[0].Y,
                //    tempImage.Width, tempImage.Height), new Bgr(0, 0, 255), 1);
                //orginImage.Save(namenum++.ToString() +  "wwwwwww.png");

                resultImage.Dispose();
                resultImage = null;
                orginImage.Bitmap.Dispose();
                orginImage.Dispose();

                value = min[0];
                max = null;
                min = null;
                min_point = null;
                max_point = null;
            }
            return value < throld;
        }
        /// <summary>
        /// 模板匹配 匹配origin图像中是否存在模板tempImage
        /// </summary>
        /// <param name="tempImage"></param>
        /// <param name="orginImage"></param>
        /// <returns></returns>
        private double matchtempforsharp(Image<Bgr, byte> tempImage, Image<Bgr, byte> orginImage)
        {
            double value;

            Image<Gray, float> resultImage = orginImage.MatchTemplate(tempImage, Emgu.CV.CvEnum.TemplateMatchingType.SqdiffNormed);
            double[] max;
            double[] min;

            System.Drawing.Point[] min_point;
            System.Drawing.Point[] max_point;
            resultImage.MinMax(out min, out max, out min_point, out max_point);

            resultImage.Dispose();
            resultImage = null;
            //orginImage.Bitmap.Dispose();
            //orginImage.Dispose();

            value = min[0];
            max = null;
            min = null;
            min_point = null;
            max_point = null;
            
            return 1 - value;
        }
        /// <summary>
        /// 匹配数量
        /// </summary>
        /// <param name="tempImage"></param>
        /// <param name="throld"></param>
        /// <returns></returns>

        private bool matchtempforsharp(Image<Bgr, byte> tempImage, Image<Bgr, byte> orginImage, ref int num, double throld = 0.13)
        {
            double value;

            Image<Gray, float> resultImage = orginImage.MatchTemplate(tempImage, Emgu.CV.CvEnum.TemplateMatchingType.SqdiffNormed);
            double[] max;
            double[] min;

            System.Drawing.Point[] min_point;
            System.Drawing.Point[] max_point;
            resultImage.MinMax(out min, out max, out min_point, out max_point);

            resultImage.Dispose();
            resultImage = null;
            orginImage.Bitmap.Dispose();
            orginImage.Dispose();

            num = min.Length;
            value = min[0];
            max = null;
            min = null;
            min_point = null;
            max_point = null;

            return value < throld;
        }
        /// <summary>
        /// 一场游戏是否正常结束
        /// </summary>
        /// <returns></returns>
        private ResultType isGameover()
        {
            bool isover = false;
            DateTime now = DateTime.Now;
            while (!isover && (DateTime.Now - now).TotalMinutes < 10)
            {
                if (matchtempforsharp(TempImages.VectorTemp))
                {
                    Write("游戏胜利，开始开宝箱");
                    return ResultType.Vector;
                }
                else if(matchtempforsharp(TempImages.FailTemp))
                {
                    Write("游戏失败, 进入下一轮!");
                    return ResultType.Fail;
                }
                else if(matchtempforsharp(TempImages.FuHuoTemp))
                {
                    Write("游戏失败, 进入下一轮!");
                    keyoption(OptionKeys.GameFailReturnKey);
                }
                Thread.Sleep(2000);
                //getPicture();
            }
            Write("检测超时!");
            return ResultType.Timeout;
        }
        /// <summary>
        /// 确认回到游戏开始界面
        /// </summary>
        /// <param name="resultType"></param>
        private void backGameStartFace(ResultType resultType)
        {
            int times = 6;
            while (!matchtempforsharp(TempImages.StartTemp) && times-- >= 0)
            {
                if (resultType == ResultType.Vector)
                {
                    Write("开启宝箱!");
                    keyoption(OptionKeys.WhiteKey);
                    keyoption(OptionKeys.WhiteKey);
                    keyoption(OptionKeys.WhiteKey);
                    keyoption(OptionKeys.WhiteKey);
                    if (matchtempforsharp(TempImages.GetTemp))
                    {
                        keyoption(OptionKeys.SailFWKey);
                        keyoption(OptionKeys.SailFWTwo);
                    }
                    else
                    {
                        keyoption(OptionKeys.GetKey);
                    }
                    keyoption(OptionKeys.LoopKey);
                }
                else if (resultType == ResultType.Fail)
                {
                    Write("游戏失败一次!");
                    keyoption(OptionKeys.WhiteKey);
                    keyoption(OptionKeys.GameFailReturnKey);
                    keyoption(OptionKeys.WhiteKey);
                    keyoption(OptionKeys.WhiteKey);
                    keyoption(OptionKeys.LoopKey);
                }
                Write("确认游戏状态!");
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 截一张图用作识别
        /// </summary>
        private void getPicture()
        {
            if (ScreenMode != ScreenMode.Vriual)
            {
                IntPtr hWnd = WindowsAPI.FindWindow(null, "夜神模拟器");
                IntPtr hscrdc = WindowsAPI.GetWindowDC(hWnd);
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle();
                WindowsAPI.GetWindowRect(hWnd, out rect);
                System.Drawing.Bitmap bmp = null;
                if (ScreenMode == ScreenMode.Window)
                {
                    IntPtr hbitmap = WindowsAPI.CreateCompatibleBitmap(hscrdc, rect.Width, rect.Height);
                    IntPtr hmemdc = WindowsAPI.CreateCompatibleDC(hscrdc);
                    WindowsAPI.SelectObject(hmemdc, hbitmap);
                    WindowsAPI.PrintWindow(hWnd, hmemdc, 0);
                    bmp = System.Drawing.Bitmap.FromHbitmap(hbitmap);
                    WindowsAPI.DeleteDC(hmemdc);
                }
                else if (ScreenMode == ScreenMode.Desktop)
                {
                    bmp = WindowsAPI.GetDesktop(rect);
                }
                if (File.Exists(TempImages.OrigImagePath))
                {
                    File.Delete(TempImages.OrigImagePath);
                }
                if (bmp != null)
                {
                    bmp.Save(TempImages.OrigImagePath);

                    bmp.Dispose();
                    bmp = null;
                }
                WindowsAPI.DeleteDC(hscrdc);
            }
            else
            {
                getpictureFromAdb();
            }
        }
        /// <summary>
        /// 从模拟器中获取截图
        /// </summary>
        private void getpictureFromAdb()
        {
            startADBEXE("shell /system/bin/screencap -p /sdcard/screenshot.png");
            startADBEXE("pull /sdcard/screenshot.png " + TempImages.OrigImagePath);
        }
        /// <summary>
        /// 启动adbexe带上s参数
        /// </summary>
        /// <param name="s"></param>
        private void startADBEXE(string s)
        {
            using (Process process = new Process())
            {
                process.StartInfo = new ProcessStartInfo(vriualExePath, s);// 括号里是(程序名,参数)
                process.StartInfo.CreateNoWindow = true;   //不创建窗口
                process.StartInfo.UseShellExecute = false;//不使用系统外壳程序启动，重定向时此处必须设为false
                process.StartInfo.RedirectStandardOutput = true; //重定向输出，而不是默认的显示在dos控制台上
                process.Start();
                process.WaitForExit();
                process.Dispose();
            }
        }
        /// <summary>
        /// 获取模拟器的大小
        /// </summary>
        /// <returns></returns>
        private System.Drawing.Rectangle getVriualSize()
        {
            IntPtr hWnd = WindowsAPI.FindWindow(null, "夜神模拟器");
            IntPtr hscrdc = WindowsAPI.GetWindowDC(hWnd);
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle();
            WindowsAPI.GetWindowRect(hWnd, out rect);
            WindowsAPI.DeleteDC(hscrdc);
            return rect;
        }
        /// <summary>
        /// 运行前初始化
        /// </summary>
        private void InitVriual()
        {
            IntPtr hWnd = WindowsAPI.FindWindow(null, "夜神模拟器");
            IntPtr hscrdc = WindowsAPI.GetWindowDC(hWnd);
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle();
            WindowsAPI.GetWindowRect(hWnd, out rect);
            WindowsAPI.MoveWindow(hWnd, 0, 0, DefultRectangle.Width, DefultRectangle.Height, true);
            WindowsAPI.DeleteDC(hscrdc);



            isStopTask = false;
            selectedBar.IsEnabled = false;
            fwSelected.IsEnabled = false;
            runTask.IsEnabled = false;
        }
        /// <summary>
        /// 任务运行结束或者停止的时候调用
        /// </summary>
        private void EndInitVriual()
        {
            selectedBar.IsEnabled = true;
            fwSelected.IsEnabled = true;
            runTask.IsEnabled = true;
        }
        /// <summary>
        /// 检测和更换狗粮是否满级
        /// </summary>
        private void checkAndChangeGlMaxLevel(bool[] IsMax)
        {
            int times = 3;
            Random random = new Random();
            for (int i = 0; i < IsMax.Length; i++)
            {
                IsMax[i] = false;
            }
            while (times --  >= 0)
            {
                getPicture();
                using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(TempImages.OrigImagePath))
                {
                    Image<Bgr, byte> gl1 = new Image<Bgr, byte>(clipBitmap(bmp, TempImages.GL1_Rect));
                    Image<Bgr, byte> gl2 = new Image<Bgr, byte>(clipBitmap(bmp, TempImages.GL2_Rect));
                    Image<Bgr, byte> gl3 = new Image<Bgr, byte>(clipBitmap(bmp, TempImages.GL3_Rect));

                    if(!IsMax[0] && glIsMaxLevel(gl1))
                    {
                        IsMax[0] = true;
                    }
                    if (!IsMax[1] && glIsMaxLevel(gl2))
                    {
                        IsMax[1] = true;
                    }
                    if (!IsMax[2] && glIsMaxLevel(gl3))
                    {
                        IsMax[2] = true;
                    }

                    gl1 = null;
                    gl2 = null;
                    gl3 = null;
                }
                Thread.Sleep(random.Next(300, 1200));
            }
        }
        /// <summary>
        /// 切换已经满级的狗粮
        /// </summary>
        /// <param name="isMax"></param>
        private void changeGLS(bool [] isMax)
        {
            for (int i = 0; i < isMax.Length; i++)
            {
                if(isMax[i])
                {
                    Write((i+1).ToString() + "号狗粮满级!");
                }
                else
                {
                    Write((i + 1).ToString() + "号狗粮没有满级!");
                }
            }
            if(isMax[0] || isMax[1] || isMax[2])
            {
                foreach (var item in glsKeys)
                {
                    keyoption(item.Value);
                }
                ///打开仓库
                keyoption(OptionKeys.RepertoryKey);
                ///选择强化顺序
                keyoption(OptionKeys.RepertoryForSelectKey);
                ///选择强化顺序为排序方式
                keyoption(OptionKeys.RepertorySelectQHSXKey);

                ///拉到底
                for (int i = 0; i < 10; i++)
                {
                    keyoption(OptionKeys.UpKey);
                }
                ///选择新的3个狗粮
                keyoption(OptionKeys.GetGL1Key);
                keyoption(OptionKeys.GetGL2Key);
                keyoption(OptionKeys.GetGL3Key);
                ///确认关闭仓库
                keyoption(OptionKeys.SureAndCloseRepertoryKey);
            }
            

        }
        /// <summary>
        /// 获取狗粮的状态
        /// </summary>
        /// <param name="gl">狗粮的图片</param>
        /// <param name="starLevel"></param>
        /// <param name="level"></param>
        private bool glIsMaxLevel(Image<Bgr, byte> gl, double throld = 0.75)
        {
            try
            {
                return matchtempforsharp(TempImages.MaxLevelTemp, gl) > throld ||
                    matchtempforsharp(TempImages.MaxLevel2Temp, gl) > throld;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                //gl.Save(namenum++.ToString() + "gl.png");
                gl.Bitmap.Dispose();
                gl.Dispose();
                gl = null;
            }
            
        }
        /// <summary>
        /// 检测战利品，返回是否出售
        /// </summary>
        private bool checkSpoils()
        {
            bool issail = false;
            getPicture();
            NumClass numClass = null;
            double maxvalue = 0, value;

            using (Image<Bgr, byte> image = new Image<Bgr, byte>(TempImages.OrigImagePath))
            {
                using (Image<Bgr, byte> objimg = new Image<Bgr, byte>(clipBitmap(image.Bitmap ,TempImages.ResultObjecRect)))
                {
                    foreach (var item in Spoils)
                    {
                        value = matchtempforsharp(item.Key, objimg);
                        if (value > maxvalue)
                        {
                            numClass = item.Value;
                            maxvalue = value;
                        }
                    }
                    //objimg.Save(namenum++.ToString() + "image.png");
                    objimg.Bitmap.Dispose();
                    objimg.Dispose();
                }
                image.Bitmap.Dispose();
                image.Dispose();
                //image = null;
            }

            if(maxvalue > 0.75)
            {
                numClass.Num++;
                Write("获得战利品 : " + numClass.Name);
            }
            else
            {
                Write("获得战利品 : " + "其他");
            }
            return numClass.IsToSail;
        }
        /// <summary>
        /// 图片裁剪
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        private System.Drawing.Bitmap clipBitmap(System.Drawing.Bitmap bitmap, System.Drawing.Rectangle rectangle)
        {
            System.Drawing.Bitmap result = new System.Drawing.Bitmap(rectangle.Width, rectangle.Height);
            using (System.Drawing.Graphics graphisc = System.Drawing.Graphics.FromImage(result))
            {
                if(this.ScreenMode != ScreenMode.Vriual)
                    graphisc.DrawImage(bitmap, new System.Drawing.Rectangle(0, 0, rectangle.Width, rectangle.Height),
                        rectangle, System.Drawing.GraphicsUnit.Pixel);
                else
                {
                    graphisc.DrawImage(bitmap, new System.Drawing.Rectangle(0, 0, rectangle.Width, rectangle.Height),
                        new System.Drawing.Rectangle(rectangle.X - 5, rectangle.Y - 45, rectangle.Width, rectangle.Height), 
                        System.Drawing.GraphicsUnit.Pixel);
                }
            }
            return result;
        }
#region tasks
        /// <summary>
        /// 运行带狗粮任务
        /// </summary>
        /// <param name="corrd"></param>
        /// <returns></returns>
        public Task<bool> RunDGLScript(Corrd corrd)
        {
            Task<bool> run = Task<bool>.Factory.StartNew(() => {
                try
                {

                    bool[] ismax = new bool[3];
                    Write("准备开始任务5s倒计时!");
                    Thread.Sleep(5000);
                    Write("脚本截止条件:" + "运行次数 : " + corrd.Times.ToString() + ", 或者 运行到 : " + corrd.StopTime.ToString());
                    while (corrd.Times -- >= 0 && DateTime.Now < corrd.StopTime && !isStopTask)
                    {
                        Write("检测是否处于开始界面!");
                        if(matchtempforsharp(TempImages.StartTemp))
                        {
                            Write("检测到游戏开始界面, 准备游戏！");
                        }
                        else
                        {
                            Write("未检测到游戏开始界面，是否没有调整好界面！脚本退出！");
                            return false;
                        }
                        ///切换满级的狗粮
                        changeGLS(ismax);
                        ///开始游戏
                        keyoption(OptionKeys.StartKey);
                        Thread.Sleep(1500);
                        if(matchtempforsharp(TempImages.BarTemp))
                        {
                            Write("检测到进度条， 游戏开始!");
                        }
                        else
                        {
                            Write("未检测到进度条!");
                        }
                        Thread.Sleep(corrd.WaitTime);
                        ResultType resultType = isGameover();
                        Thread.Sleep(Random.Next(2000, 10000));
                        ///检测狗粮是否满级
                        checkAndChangeGlMaxLevel(ismax);

                        backGameStartFace(resultType);
                        Thread.Sleep(1000);
                        if (matchtempforsharp(TempImages.EnergyNotEnoughTemp))
                        {
                            Write("体力不足，退出脚本!");
                            return true;
                        }
                        Thread.Sleep(2000);
                    }
                    Write("脚本正常次数运行完成!");
                    return true;
                }
                catch (Exception ex)
                {
                    Write(ex.ToString());
                    return false;
                }
            }, taskCancle.Token);
            return run;
        }
        /// <summary>
        /// 运行刷地下城的任务
        /// </summary>
        /// <param name="corrd"></param>
        /// <returns></returns>
        public Task<bool> RunDungeonsScript(Corrd corrd)
        {
            Task<bool> run = Task<bool>.Factory.StartNew(() => {
                try
                {
                    Write("准备开始任务5s倒计时!");
                    Thread.Sleep(5000);
                    Write("脚本截止条件:" + "运行次数 : " + corrd.Times.ToString() + ", 或者 运行到 : " + corrd.StopTime.ToString());
                    while (corrd.Times-- >= 0 && DateTime.Now < corrd.StopTime && !isStopTask)
                    {
                        Write("检测是否处于开始界面!");
                        if (matchtempforsharp(TempImages.StartTemp))
                        {
                            Write("检测到游戏开始界面, 准备游戏！");
                        }
                        else
                        {
                            Write("未检测到游戏开始界面，是否没有调整好界面！脚本退出！");
                            return false;
                        }
                        keyoption(OptionKeys.StartKey);
                        Thread.Sleep(1500);
                        if (matchtempforsharp(TempImages.BarTemp))
                        {
                            Write("检测到进度条， 游戏开始!");
                        }
                        else
                        {
                            Write("未检测到进度条!");
                        }
                        Thread.Sleep(corrd.WaitTime);
                        ResultType resultType = isGameover();
                        Thread.Sleep(Random.Next(2000, 10000));
                        if (resultType == ResultType.Vector)
                        {
                            Write("开启宝箱!");
                            keyoption(OptionKeys.WhiteKey);
                            keyoption(OptionKeys.WhiteKey);
                            keyoption(OptionKeys.WhiteKey);
                            keyoption(OptionKeys.WhiteKey);
                            ///检测战利品
                            bool issail = checkSpoils();
                            if (matchtempforsharp(TempImages.GetTemp))
                            {
                                if (!issail)
                                {
                                    keyoption(OptionKeys.SureKey);
                                    Write("不出售！");
                                }
                                else
                                {
                                    keyoption(OptionKeys.SailFWKey);
                                    keyoption(OptionKeys.SailFWTwo);
                                    Write("出售!");
                                }
                            }
                            else
                            {
                                keyoption(OptionKeys.GetKey);
                            }
                            keyoption(OptionKeys.LoopKey);
                        }
                        else if (resultType == ResultType.Fail)
                        {
                            Write("游戏失败一次!");
                            keyoption(OptionKeys.WhiteKey);
                            keyoption(OptionKeys.GameFailReturnKey);
                            keyoption(OptionKeys.WhiteKey);
                            keyoption(OptionKeys.WhiteKey);
                            keyoption(OptionKeys.LoopKey);
                        }
                        Thread.Sleep(1000);
                        if (matchtempforsharp(TempImages.EnergyNotEnoughTemp))
                        {
                            Write("体力不足，退出脚本!");
                            return true;
                        }
                        Thread.Sleep(2000);
                    }
                    Write("脚本正常次数运行完成!");
                    return true;
                }
                catch (Exception ex)
                {
                    Write(ex.ToString());
                    return false;
                }
            }, taskCancle.Token);
            return run;
        }

        /// 取得设备硬盘的卷标号  此方法为取硬盘逻辑分区序列号，重新格式化会改变 
        private string GetDiskVolumeSerialNumber()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
            disk.Get();
            return disk.GetPropertyValue("VolumeSerialNumber").ToString();
        }
#endregion
        private int namenum = 0;
#region checkboxEvent


        /// <summary>
        /// 带狗粮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgl_check(object sender, RoutedEventArgs e)
        {
            dxc.IsChecked = false;
            TaskMode = TaskMode.DGL;
        }
        /// <summary>
        /// 地下城
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dxc_check(object sender, RoutedEventArgs e)
        {
            dgl.IsChecked = false;
            TaskMode = TaskMode.DXC;
        }
        /// <summary>
        /// 符文选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Checked(object sender, RoutedEventArgs e)
        {
            string name = (sender as FrameworkElement).Name;
            if(name.Equals("b"))
            {
                Spoils[TempImages.XYFWTemp].IsToSail = false;
            }
            else if(name.Equals("z"))
            {
                Spoils[TempImages.YXFWTemp].IsToSail = false;
            }
            else if(name.Equals("c"))
            {
                Spoils[TempImages.CSFWTemp].IsToSail = false;
            }
        }
        /// <summary>
        /// 不留
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Unchecked(object sender, RoutedEventArgs e)
        {
            string name = (sender as FrameworkElement).Name;
            if (name.Equals("b"))
            {
                Spoils[TempImages.XYFWTemp].IsToSail = true;
            }
            else if (name.Equals("z"))
            {
                Spoils[TempImages.YXFWTemp].IsToSail = true;
            }
            else if (name.Equals("c"))
            {
                Spoils[TempImages.CSFWTemp].IsToSail = true;
            }
        }
        /// <summary>
        /// 桌面截屏模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void desk_check(object sender, RoutedEventArgs e)
        {
            window.IsChecked = false;
            vriual.IsChecked = false;
            ScreenMode = ScreenMode.Desktop;
        }
        /// <summary>
        /// 指定窗口模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wind_check(object sender, RoutedEventArgs e)
        {
            desktop.IsChecked = false;
            vriual.IsChecked = false;
            ScreenMode = ScreenMode.Window;
        }
        /// <summary>
        /// 模拟器模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void vriual_check(object sender, RoutedEventArgs e)
        {
            desktop.IsChecked = false;
            window.IsChecked = false;
            ScreenMode = ScreenMode.Vriual;

            
        }

        /// <summary>
        /// 选择夜神模拟器路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                vriualPath.Text = fbd.SelectedPath;
                vriualExePath = System.IO.Path.Combine(vriualPath.Text, "bin\\nox_adb.exe");
                if(!System.IO.File.Exists(vriualExePath))
                {
                    MessageBox.Show("路径错误，请选择夜神模拟器的根路径,一般是Nox到这里为止就好！");
                }
                else
                {
                    using (FileStream stream = File.Open(configPath, FileMode.Create))
                    {
                        byte[] datas = System.Text.Encoding.UTF8.GetBytes(vriualPath.Text);
                        stream.Write(datas, 0, datas.Length);
                        datas = null;
                    }
                }
            }
        }
        /// <summary>
        /// 置顶模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void topmode(object sender, RoutedEventArgs e)
        {
            IntPtr intPtr = WindowsAPI.FindWindow(null, "夜神模拟器");
            WindowsAPI.SetWindowPos(intPtr, -1, 0, 0, 0, 0, 1 | 2 | 0x20 | 0x4000 | 0x40);
        }
        /// <summary>
        /// 非置顶模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void untopmode(object sender, RoutedEventArgs e)
        {
            IntPtr intPtr = WindowsAPI.FindWindow(null, "夜神模拟器");
            WindowsAPI.SetWindowPos(intPtr, 0, 0, 0, 0, 0, 1 | 2 | 0x20 | 0x4000 | 0x40);
        }
#endregion


    }

}
