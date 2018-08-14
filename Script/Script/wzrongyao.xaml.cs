using APILibrary.API;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Script
{
    /// <summary>
    /// wzrongyao.xaml 的交互逻辑
    /// </summary>
    public partial class wzrongyao : Window
    {


        /// <summary>
        /// 模板图片根目录
        /// </summary>
        private static string rootPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WZTemps/");
        /// <summary>
        /// 下一步的模板
        /// </summary>
        public static Image<Bgr, byte> NextTemp = new Image<Bgr, byte>(rootPath.AppendPath("next.png"));
        /// <summary>
        /// 下一步的按键
        /// </summary>
        public static OptionKeys NextKey = new OptionKeys(EnumKeyboardKey.Y, new System.Drawing.Point(1105, 697));
        /// <summary>
        /// 开始界面
        /// </summary>
        public static Image<Bgr, byte> StartTemp = new Image<Bgr, byte>(rootPath.AppendPath("start.png"));
        /// <summary>
        /// 开始按键
        /// </summary>
        public static OptionKeys StartKey = new OptionKeys(EnumKeyboardKey.Q, new System.Drawing.Point(1000, 650));
        /// <summary>
        /// 跳过
        /// </summary>
        public static Image<Bgr, byte> JumpYellowTemp = new Image<Bgr, byte>(rootPath.AppendPath("jump.png"));
        /// <summary>
        /// 跳过蓝色
        /// </summary>
        public static Image<Bgr, byte> JumpBlueTemp = new Image<Bgr, byte>(rootPath.AppendPath("jumpblue.png"));
        /// <summary>
        /// 跳过按键
        /// </summary>
        public static OptionKeys JumpYellowKey = new OptionKeys(EnumKeyboardKey.Tab, new System.Drawing.Point(1227, 74));

        /// <summary>
        /// 再来一次
        /// </summary>
        public static Image<Bgr, byte> AgainTemp = new Image<Bgr, byte>(rootPath.AppendPath("again.png"));
        /// <summary>
        /// 再来一次
        /// </summary>
        public static OptionKeys AgainKey = new OptionKeys(EnumKeyboardKey.Y, new System.Drawing.Point(1090, 697));

        /// <summary>
        /// 再来一次
        /// </summary>
        public static Image<Bgr, byte> VecTemp = new Image<Bgr, byte>(rootPath.AppendPath("vec.png"));
        /// <summary>
        /// 是否停止
        /// </summary>
        private bool IsStop = false;
        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static string configPath = "wzconfig.config";
        /// <summary>
        /// 随机数
        /// </summary>
        private Random Random = new Random();


        private Thread Task = null;

        private Config Config = new Config();

        public wzrongyao()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if(Task != null && Task.IsAlive)
            {
                Task.Abort();
                Task = null;
            }
            else
            {
                Task = new Thread(() =>
                {
                    try
                    {
                        startTask();
                        while (true)
                        {
                            if(!matchtempforsharp(StartTemp))
                            {
                                return;
                            }
                            keyoption(StartKey);
                            while (!matchtempforsharp(AgainTemp))
                            {
                                if(matchtempforsharp(JumpBlueTemp) || matchtempforsharp(JumpYellowTemp))
                                {
                                    keyoption(JumpYellowKey);
                                }
                                keyoption(AgainKey);
                                Thread.Sleep(2000);
                            }
                            keyoption(AgainKey);
                            Thread.Sleep(3000);
                            //if (!matchtempforsharp(AgainTemp))
                            //{
                            //    return;
                            //}
                            //keyoption(AgainKey);
                            //Thread.Sleep(3000);
                        }
                    }
                    catch (ThreadAbortException abth)
                    {
                        return;
                    }
                    catch(Exception ex)
                    {
                        return;
                    }
                    finally
                    {
                        stopTask();
                    }
                })
                { IsBackground = true};
                Task.Start();
            }
        }

        private void startTask()
        {
            this.Dispatcher.Invoke(new Action(() => {
                this.task.Content = "停止";
            }));
        }

        private void stopTask()
        {
            this.Dispatcher.Invoke(new Action(() => {
                this.task.Content = "开始";
            }));
        }

        /// <summary>
        /// 按键
        /// </summary>
        /// <param name="enumKeyboardKey"></param>
        private void keyoption(OptionKeys enumKeyboardKey)
        {
            //if (Config.ScreenMode == ScreenMode.Desktop)
            //{
            //    WindowsAPI.KeyDown(enumKeyboardKey.Key);
            //    Thread.Sleep(300);
            //    WindowsAPI.KeyUp(enumKeyboardKey.Key);
            //    Write("按下:" + enumKeyboardKey.Key.ToString());
            //}
            //else if (Config.ScreenMode == ScreenMode.Window || Config.ScreenMode == ScreenMode.Vriual)
            //{
            string s = "";
            if (enumKeyboardKey.IsDrag)
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
            Martian.startADBEXE(s, Config.VriualExePath);
            //}

            Thread.Sleep(Random.Next(Config.KeyPressWaitMinTime, Config.KeyPressWaitMaxTime));
            //Write("按下了 : " + enumKeyboardKey);
        }


        private void Selected_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                vriualPath.Text = fbd.SelectedPath;
                Config.VriualExePath = fbd.SelectedPath;
                string path = System.IO.Path.Combine(vriualPath.Text, "bin\\nox_adb.exe");
                if (!System.IO.File.Exists(path))
                {
                    MessageBox.Show("路径错误，请选择夜神模拟器的根路径,一般是Nox到这里为止就好！");
                }
                else
                {
                    Config.ToFile(configPath);
                    Config.VriualExePath = path;
                }
            }
        }


        /// <summary>
        /// 模板匹配 匹配origin图像中是否存在模板tempImage
        /// </summary>
        /// <param name="tempImage"></param>
        /// <param name="orginImage"></param>
        /// <returns></returns>
        private bool matchtempforsharp(Image<Bgr, byte> tempImage, double throld = 0.13, int times = 0)
        {
            if (times >= 5)
            {
                throw new Exception("程序异常");
            }
            try
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
            catch (Exception)
            {
                return matchtempforsharp(tempImage, throld, times++);
            }
        }

        /// <summary>
        /// 截一张图用作识别
        /// </summary>
        private void getPicture()
        {
            IntPtr hWnd = WindowsAPI.FindWindow(null, "夜神模拟器");
            IntPtr hscrdc = WindowsAPI.GetWindowDC(hWnd);
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle();
            WindowsAPI.GetWindowRect(hWnd, out rect);
            System.Drawing.Bitmap bmp = null;
            if (Config.ScreenMode == ScreenMode.Window)
            {
                IntPtr hbitmap = WindowsAPI.CreateCompatibleBitmap(hscrdc, rect.Width, rect.Height);
                IntPtr hmemdc = WindowsAPI.CreateCompatibleDC(hscrdc);
                WindowsAPI.SelectObject(hmemdc, hbitmap);
                WindowsAPI.PrintWindow(hWnd, hmemdc, 0);
                bmp = System.Drawing.Bitmap.FromHbitmap(hbitmap);
                WindowsAPI.DeleteDC(hmemdc);
            }
            else if (Config.ScreenMode == ScreenMode.Desktop)
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
    }
}
