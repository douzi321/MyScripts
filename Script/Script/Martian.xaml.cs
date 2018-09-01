using APILibrary.API;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Script.MatchHelp;
using System.ComponentModel;

namespace Script
{
    /// <summary>
    /// Martian.xaml 的交互逻辑
    /// </summary>
    public partial class Martian : Window
    {
        public Martian()
        {
            InitializeComponent();
            this.Loaded += Martian_Loaded;
            Application.Current.Exit += Current_Exit;



//#if !DEBUG
            //if (GetDiskVolumeSerialNumber() != id)
            //{
            //    MessageBox.Show("不是绑定的电脑!");
            //    this.Close();
            //    return;
            //}
//#endif
        }
        /// <summary>
        /// 应用程序退出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Current_Exit(object sender, ExitEventArgs e)
        {
            MatchHelp.Dispose();
        }

        #region 事件
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Martian_Loaded(object sender, RoutedEventArgs e)
        {
            //AppBoxContent appBoxContent = new AppBoxContent();
            //appBoxContent.Width = DefultRectangle.Width;
            //appBoxContent.Height = DefultRectangle.Height;
            //IntPtr hWnd = WindowsAPI.FindWindow(null, "夜神模拟器");
            ////appBoxContent.SetContent(@"F:\soft\夜神模拟器\Nox\bin\Nox.exe");
            //FWImages.AddImage(TempImages.CSFWTemp.Bitmap.ToBitmapSource());
            //FWImages.ShowDialog();


            splios.ItemsSource = Spoils;
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            Write("脚本程序启动成功!");
            if (File.Exists(configPath))
            {
                Config = configPath.LoadFromFile<Config>();
                vriualPath.Text = Config.VriualExePath;
                Config.VriualExePath = System.IO.Path.Combine(vriualPath.Text, "bin\\nox_adb.exe");
                if (!System.IO.File.Exists(Config.VriualExePath))
                {
                    MessageBox.Show("路径错误，请选择夜神模拟器的根路径,一般是Nox到这里为止就好！");
                }
                else
                {
                    Write("获取配置成功!");
                }
            }
            else
            {
                ///保存一份配置文件
                Config.ToFile(configPath);
            }


            SelectedMode(Config.TaskMode);
            SelectedScreen(Config.ScreenMode);

        }
        /// <summary>
        /// 测试按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MatchHelp.getPicture2();
        }
        /// <summary>
        /// 查看获取的传说符文
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurButtom_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() => {
                this.FWImages.ShowDialog();
            }));
            (sender as CurButtom).IsSelected = false;
        }
        /// <summary>
        /// 收起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            double from = (double)Side.GetValue(Canvas.LeftProperty);
            double to = 0;


            if (isOut == false)
            {
                to = 276;
                SideText.Text = "收起";
                SideText.Margin = new Thickness(20, 120, 20, 100);
            }
            else
            {
                SideText.Text = "查看收获";
                SideText.Margin = new Thickness(20, 90, 20, 80);
                to = 76;
            }
            isOut = !isOut;

            DoubleAnimation doubleAnimation = new DoubleAnimation()
            {
                From = from,
                To = to,
                Duration = TimeSpan.FromSeconds(1),
            };
            Side.BeginAnimation(Canvas.LeftProperty, doubleAnimation);
        }
        /// <summary>
        /// 按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Move_Down(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                this.Cursor = Cursors.Hand;
                isDrag = true;
                forntPoint = e.GetPosition(this);
            }
        }
        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Move_Move(object sender, MouseEventArgs e)
        {
            Point current = e.GetPosition(this);
            if(isDrag == true && e.LeftButton == MouseButtonState.Pressed)
            {
                Point movep = new Point(current.X - forntPoint.X, current.Y - forntPoint.Y);
                this.Left += movep.X;
                this.Top += movep.Y;
                //forntPoint = e.GetPosition(this);
            }
        }
        /// <summary>
        /// 抬起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Move_Up(object sender, MouseButtonEventArgs e)
        {
            isDrag = false;
            this.Cursor = Cursors.Arrow;
        }
        /// <summary>
        /// 离开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mouse_Leave(object sender, MouseEventArgs e)
        {
            isDrag = false;
            this.Cursor = Cursors.Arrow;
        }
        /// <summary>
        /// 模式选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mode_Selected(object sender, MouseButtonEventArgs e)
        {
            CurButtom curButtom = sender as CurButtom;
            if(curButtom.Name== "glMode")
            {
                Config.TaskMode = TaskMode.DGL;
                fwMode.IsSelected = false;
                ftMode.IsSelected = false;
                magicMode.IsSelected = false;
            }
            else if (curButtom.Name == "fwMode")
            {
                Config.TaskMode = TaskMode.DXC;
                glMode.IsSelected = false;
                ftMode.IsSelected = false;
                magicMode.IsSelected = false;
            }
            else if (curButtom.Name == "ftMode")
            {
                Config.TaskMode = TaskMode.ST;
                glMode.IsSelected = false;
                fwMode.IsSelected = false;
                magicMode.IsSelected = false;
            }
            else if(curButtom.Name == "magicMode")
            {
                Config.TaskMode = TaskMode.Magic;
                glMode.IsSelected = false;
                fwMode.IsSelected = false;
                ftMode.IsSelected = false;
            }
        }
        /// <summary>
        /// 运行程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Run_Click(object sender, MouseButtonEventArgs e)
        {

            //bool[] ismax = new bool[3];
            //checkAndChangeGlMaxLevel(ismax);
            
            //return;


            if (runningTask != null && !runningTask.IsCompleted && !runningTask.IsCanceled)
            {
                isStopTask = true;
                Write("发送取消任务请求，当这把打完之后会停止任务!");
                return;
            }
            Corrd corrd = new Corrd() { Times = 1000, StopTime = DateTime.Now + TimeSpan.FromDays(10), WaitTime = 1700};
            //try
            //{
            //    //corrd.Times = Convert.ToInt32(failTimes.Text);
            //    //corrd.StopTime = DateTime.Now + TimeSpan.FromHours(Convert.ToDouble(stopTime.Text));
            //    //corrd.WaitTime = Convert.ToInt32(checkTime.Text);
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("参数填写错误!");
            //    return;
            //}
            Config.TaskMode = GetTaskMode();
            Config.ScreenMode = GetScreenMode();
            MatchHelp.ScreenMode = Config.ScreenMode;
            if (Config.TaskMode == TaskMode.None)
            {
                Write("没有选择任务运行的模式!");
                return;
            }

            InitVriual();
            if (Config.TaskMode == TaskMode.DGL)
            {
                //InitVriual();
                Write("选择了带狗粮,准备开始进入带狗粮模式!");
                runningTask = RunDGLScript(corrd);
                runningTask.ContinueWith(t =>
                {
                    Write("带狗粮任务结束, 返回结果" + t.Result.ToString());
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        EndInitVriual();
                    }));
                });
            }
            else if (Config.TaskMode == TaskMode.DXC)
            {
                Write("选择了地下城,准备开始进入地下城模式!");
                runningTask = RunDungeonsScript(corrd);
                runningTask.ContinueWith(t =>
                {
                    Write("地下城任务结束, 返回结果" + t.Result.ToString());
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        EndInitVriual();
                    }));
                });
            }
            else if(Config.TaskMode == TaskMode.Magic)
            {
                Write("选择了刷魔力石地下城,准备开始进入地下城模式!");
                runningTask = RunMagicScript(corrd);
                runningTask.ContinueWith(t =>
                {
                    Write("刷魔力石地下城任务结束, 返回结果" + t.Result.ToString());
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        EndInitVriual();
                    }));
                });
            }
            else if(Config.TaskMode == TaskMode.ST)
            {
                Write("选择了刷塔模式,准备开始进入爬塔模式!");
                runningTask = RunPagodaScript(corrd);
                runningTask.ContinueWith(t =>
                {
                    Write("选择了刷塔模式, 返回结果" + t.Result.ToString());
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        EndInitVriual();
                    }));
                });
            }
        }

        /// <summary>
        /// 选择夜神模拟器路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VriualPathSelected_Click(object sender, MouseButtonEventArgs e)
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
        /// 符文选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Checked(object sender, RoutedEventArgs e)
        {
            string name = (sender as FrameworkElement).Name;
            if (name.Equals("b"))
            {
                Spoils[TempImages.XYFWTemp].IsToSail = false;
            }
            else if (name.Equals("z"))
            {
                Spoils[TempImages.YXFWTemp].IsToSail = false;
            }
            else if (name.Equals("c"))
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

        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Min_Click(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, MouseButtonEventArgs e)
        {
            Config.TaskMode = GetTaskMode();
            Config.ScreenMode = GetScreenMode();
            Config.VriualExePath = vriualPath.Text;
            Config.ToFile(configPath);
            this.Close();
            Application.Current.Shutdown();
        }
#endregion

#region 属性

        /// <summary>
        /// 战利品集合
        /// </summary>
        private Dictionary<Image<Bgr, byte>, NumClass> Spoils = new Dictionary<Image<Bgr, byte>, NumClass> {
            { TempImages.CFImgTemp, new NumClass(0, "超凡石头", TempImages.CFImgTemp.Bitmap){ Mode= Orientation.Vertical, Height=65, Width=65} },
            { TempImages.HDImgTemp, new NumClass(0, "混沌石头", TempImages.HDImgTemp.Bitmap){ Mode= Orientation.Vertical, Height=65, Width=65} },
            { TempImages.HXImgTemp, new NumClass(0, "和谐石头", TempImages.HXImgTemp.Bitmap){ Mode= Orientation.Vertical, Height=65, Width=65} },
            { TempImages.CHGuaiTemp, new NumClass(0, "彩虹怪", TempImages.CHGuaiTemp.Bitmap){ Mode= Orientation.Vertical, Height=65, Width=65} },
            { TempImages.CSFWTemp, new NumClass(0, "传说符文", TempImages.CSFWTemp.Bitmap){ Mode= Orientation.Horizontal, Height=38, Width=110, Margin = new Thickness(5)} },
            { TempImages.YXFWTemp, new NumClass(0, "英雄符文", TempImages.YXFWTemp.Bitmap){ Mode= Orientation.Horizontal, Height=38, Width=110, Margin = new Thickness(5)} },
            { TempImages.XYFWTemp, new NumClass(0, "稀有符文", TempImages.XYFWTemp.Bitmap){ Mode= Orientation.Horizontal, Height=38, Width=110, Margin = new Thickness(5)} },
            { TempImages.HSTemp, new NumClass(0, "黄书", TempImages.HSTemp.Bitmap){ Mode= Orientation.Vertical, Height=65, Width=65} },
            { TempImages.CZTemp, new NumClass(0, "厕纸", TempImages.CZTemp.Bitmap){ Mode= Orientation.Vertical, Height=65, Width=65} },
            { TempImages.FSImgTemp, new NumClass(0, "符石石头", TempImages.FSImgTemp.Bitmap){ Mode= Orientation.Vertical, Height=65, Width=65} },
            { TempImages.STTemp, new NumClass(0, "召唤石", TempImages.STTemp.Bitmap){ Mode= Orientation.Vertical, Height=65, Width=65} },
            
        };
        /// <summary>
        /// 狗粮序号和按键的映射
        /// </summary>
        private Dictionary<int, OptionKeys> glsKeys = new Dictionary<int, OptionKeys>()
        {
            { 0, OptionKeys.GL1Key},
            { 1, OptionKeys.GL2Key},
            { 2, OptionKeys.GL3Key},
        };

        private static readonly string id = "3E4D7CBE";
        /// <summary>
        /// 符文窗口
        /// </summary>
        private FWImages FWImages = new FWImages();
        /// <summary>
        /// 是否弹出侧边栏
        /// </summary>
        private bool isOut = false;
        /// <summary>
        /// 是否拖动
        /// </summary>
        private bool isDrag = false;
        /// <summary>
        /// 之前的点
        /// </summary>
        private Point forntPoint = new Point();
        /// <summary>
        /// 任务
        /// </summary>
        private Task<bool> runningTask = null;
        /// <summary>
        /// 随机数
        /// </summary>
        private Random Random = new Random();
        /// <summary>
        /// 是否停止任务
        /// </summary>
        private bool isStopTask = false;
        /// <summary>
        /// 配置文件
        /// </summary>
        private Config Config = new Config();
        /// <summary>
        /// 任务取消标记
        /// </summary>
        private CancellationTokenSource taskCancle = new CancellationTokenSource();
        /// <summary>
        /// 默认大小
        /// </summary>
        private static System.Drawing.Size DefultRectangle = new System.Drawing.Size(1314, 769);
        /// <summary>
        /// 配置文件路径
        /// </summary>
        private static string configPath = "config.config";
        private int namenum;

#endregion


#region 自定义方法
        /// <summary>
        /// 选择模式
        /// </summary>
        /// <param name="taskMode"></param>
        private void SelectedMode(TaskMode taskMode)
        {
            if(taskMode == TaskMode.DGL)
            {
                glMode.IsSelected = true;
                fwMode.IsSelected = false;
                ftMode.IsSelected = false;
                magicMode.IsSelected = false;
            }
            else if (taskMode == TaskMode.DXC)
            {
                glMode.IsSelected = false;
                fwMode.IsSelected = true;
                ftMode.IsSelected = false;
                magicMode.IsSelected = false;
            }
            else if (taskMode == TaskMode.ST)
            {
                glMode.IsSelected = false;
                fwMode.IsSelected = false;
                ftMode.IsSelected = true;
                magicMode.IsSelected = false;
            }
            else if (taskMode == TaskMode.Magic)
            {
                glMode.IsSelected = false;
                fwMode.IsSelected = false;
                ftMode.IsSelected = false;
                magicMode.IsSelected = true;
            }
        }
        /// <summary>
        /// 选择截图模式
        /// </summary>
        /// <param name="screenMode"></param>
        private void SelectedScreen(ScreenMode screenMode)
        {
            if (screenMode == ScreenMode.Desktop)
            {
                desktopMode.IsChecked = true;
            }
            else if (screenMode == ScreenMode.Window)
            {
                windowMode.IsChecked = true;
            }
        }
        /// <summary>
        /// 获取任务模式
        /// </summary>
        /// <returns></returns>
        public TaskMode GetTaskMode()
        {
            if (glMode.IsSelected) return TaskMode.DGL;
            else if (fwMode.IsSelected) return TaskMode.DXC;
            else if (ftMode.IsSelected) return TaskMode.ST;
            else if (magicMode.IsSelected) return TaskMode.Magic;
            else return TaskMode.None;
        }
        /// <summary>
        /// 获取屏幕模式
        /// </summary>
        /// <returns></returns>
        public ScreenMode GetScreenMode()
        {
            if (desktopMode.IsChecked == true) return ScreenMode.Desktop;
            else if (windowMode.IsChecked == true) return ScreenMode.Window;
            else return ScreenMode.Window;
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
                if (log.Inlines.Count > 100)
                {
                    log.Inlines.Clear();
                }
                log.Inlines.Add(run);
                logbox.ScrollToEnd();
            }), m);
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
            mode1Selected.IsEnabled = false;
            mode2Selected.IsEnabled = false;
            isTop.IsEnabled = false;
            FWMode.IsEnabled = false;
            run.Text = "停止";

            
        }
        /// <summary>
        /// 任务运行结束或者停止的时候调用
        /// </summary>
        private void EndInitVriual()
        {
            mode1Selected.IsEnabled = true;
            mode2Selected.IsEnabled = true;
            isTop.IsEnabled = true;
            FWMode.IsEnabled = true;

            runningTask.Dispose();
            runningTask = null;
            run.Text = "运行";
            GC.Collect();
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
#endregion
#region 任务
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
                    ResultType resultType = ResultType.Other;
                    bool[] ismax = new bool[3];
                    Write("准备开始任务5s倒计时!");
                    Thread.Sleep(5000);
                    Write("脚本截止条件:" + "运行次数 : " + corrd.Times.ToString() + ", 或者 运行到 : " + corrd.StopTime.ToString()
                        + "停止：" + isStopTask.ToString());
                    while (corrd.Times-- >= 0 && DateTime.Now < corrd.StopTime && !isStopTask)
                    {
                        Write("检测是否处于开始界面!");
                        if (matchtempforsharp(TempImages.StartTemp))
                        {
                            Write("检测到游戏开始界面, 准备游戏！");
                        }
                        else if (resultType == ResultType.Vector)
                        {
                            Write("游戏胜利直接开始下一把");
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
                        if (matchtempforsharp(TempImages.BarTemp))
                        {
                            Write("检测到进度条， 游戏开始!");
                        }
                        else
                        {
                            Write("未检测到进度条!");
                        }
                        Thread.Sleep(corrd.WaitTime);
                        resultType = isGameover();
                        ///检测狗粮是否满级
                        checkAndChangeGlMaxLevel(ismax);
                        ///如果其中有一个满级了则回到更换狗粮界面,或者游戏失败了
                        if (ismax[0] || ismax[1] || ismax[2] || resultType == ResultType.Fail)
                        {
                            Write(resultType == ResultType.Fail ? "游戏失败" : "有狗粮满级");
                            backGameStartFace(resultType);
                        }
                        else
                        {
                            if (againGame(resultType) == false)
                            {
                                return false;
                            }
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
                    ResultType resultType = ResultType.Other;
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
                        else if(resultType == ResultType.Vector)
                        {
                            Write("游戏胜利直接开始下一把!");
                        }
                        else
                        {
                            Write("未检测到游戏开始界面，退出！");
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
                        resultType = isGameover();
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
        /// <summary>
        /// 运行刷魔力地下城的任务
        /// </summary>
        /// <param name="corrd"></param>
        /// <returns></returns>
        public Task<bool> RunMagicScript(Corrd corrd)
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
                            //return false;
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
                        if (resultType == ResultType.Vector)
                        {
                            Write("开启宝箱!");
                            keyoption(OptionKeys.WhiteKey);
                            keyoption(OptionKeys.WhiteKey);
                            keyoption(OptionKeys.WhiteKey);
                            keyoption(OptionKeys.WhiteKey);
                            ///获取魔力石
                            keyoption(OptionKeys.GetMagicKey);
                            keyoption(OptionKeys.GetKey);
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
                        Thread.Sleep(1000);
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
        /// 运行爬塔任务
        /// </summary>
        /// <param name="corrd"></param>
        /// <returns></returns>
        public Task<bool> RunPagodaScript(Corrd corrd)
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
                        if (resultType == ResultType.Vector)
                        {
                            Write("开启宝箱!");
                            keyoption(OptionKeys.WhiteKey);
                            keyoption(OptionKeys.WhiteKey);
                            keyoption(OptionKeys.WhiteKey);
                            keyoption(OptionKeys.WhiteKey);
                            ///获取魔力石
                            keyoption(OptionKeys.GetMagicKey);
                            keyoption(OptionKeys.GetKey);
                            keyoption(OptionKeys.LoopKey);
                        }
                        else if (resultType == ResultType.Fail)
                        {
                            Write("游戏失败!");
                            //keyoption(OptionKeys.WhiteKey);
                            //keyoption(OptionKeys.GameFailReturnKey);
                            //keyoption(OptionKeys.WhiteKey);
                            //keyoption(OptionKeys.WhiteKey);
                            //keyoption(OptionKeys.LoopKey);
                            return false;
                        }
                        Thread.Sleep(1000);
                        if (matchtempforsharp(TempImages.EnergyNotEnoughTemp))
                        {
                            Write("体力不足，退出脚本!");
                            return true;
                        }
                        Thread.Sleep(1000);
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
#endregion
#region 任务相关
        /// 取得设备硬盘的卷标号  此方法为取硬盘逻辑分区序列号，重新格式化会改变 
        private string GetDiskVolumeSerialNumber()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
            disk.Get();
            return disk.GetPropertyValue("VolumeSerialNumber").ToString();
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
                        Write("出售!");
                        keyoption(OptionKeys.SailFWKey);
                        Thread.Sleep(1000);
                        if (matchtempforsharp(TempImages.SureSailTemp))
                        {
                            Write("确认出售!");
                            keyoption(OptionKeys.SailFWTwo);
                        }
                    }
                    else
                    {
                        keyoption(OptionKeys.GetKey);
                    }
                    keyoption(OptionKeys.BackZBKey);
                    //keyoption(OptionKeys.LoopKey);
                }
                else if (resultType == ResultType.Fail)
                {
                    Write("游戏失败一次!");
                    keyoption(OptionKeys.WhiteKey);
                    keyoption(OptionKeys.GameFailReturnKey);
                    keyoption(OptionKeys.WhiteKey);
                    keyoption(OptionKeys.WhiteKey);
                    keyoption(OptionKeys.BackZBKey);
                    //keyoption(OptionKeys.LoopKey);
                }
                Write("确认游戏状态!");
                Thread.Sleep(1000);
            }
        }
        /// <summary>
        /// 直接再来一次，不经过主界面
        /// </summary>
        /// <param name="resultType"></param>
        private bool againGame(ResultType resultType)
        {
            //int times = 6;
            //while (!matchtempforsharp(TempImages.BackZBTemp) && times-- >= 0)
            //{
                if (resultType == ResultType.Vector)
                {
                    Write("开启宝箱!");
                    keyoption(OptionKeys.WhiteKey);
                    keyoption(OptionKeys.WhiteKey);
                    keyoption(OptionKeys.WhiteKey);
                    keyoption(OptionKeys.WhiteKey);
                    if (matchtempforsharp(TempImages.GetTemp))
                    {
                        Write("出售!");
                        keyoption(OptionKeys.SailFWKey);
                        Thread.Sleep(1000);
                        if (matchtempforsharp(TempImages.SureSailTemp))
                        {
                            Write("确认出售!");
                            keyoption(OptionKeys.SailFWTwo);
                        }
                    }
                    else
                    {
                        keyoption(OptionKeys.GetKey);
                    }
                    keyoption(OptionKeys.LoopKey);
                }
                Write("进入下一把,再来一次!");
                Thread.Sleep(1000);
                if (matchtempforsharp(TempImages.EnergyNotEnoughTemp))
                {
                    Write("能量不足，退出脚本!");
                    return false;
                }
                return true;
            //}
            //if (times >= 0)
            //{
            //    keyoption(OptionKeys.LoopKey);
            //    Write("进入下一把游戏~");
            //    return true;
            //}
            //else
            //{
            //    Write("未知的游戏状态~");
            //    return false;
            //}
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
                    Thread.Sleep(Random.Next(Config.WaitForNextValues.X, Config.WaitForNextValues.Y));
                    return ResultType.Vector;
                }
                else if (matchtempforsharp(TempImages.FailTemp))
                {
                    Write("游戏失败, 进入下一轮!");
                    Thread.Sleep(Random.Next(Config.WaitForNextValues.X, Config.WaitForNextValues.Y));
                    return ResultType.Fail;
                }
                else if (matchtempforsharp(TempImages.FuHuoTemp))
                {
                    Write("游戏失败, 进入下一轮!");
                    keyoption(OptionKeys.GameFailReturnKey);
                }
                Thread.Sleep(1000);
                //getPicture();
            }
            Write("检测超时!");
            return ResultType.Timeout;
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
            startADBEXE(s, Config.VriualExePath);

            Thread.Sleep(Random.Next(Config.KeyPressWaitMinTime, Config.KeyPressWaitMaxTime));
            //Write("按下了 : " + enumKeyboardKey);
        }
        /// <summary>
        /// 切换已经满级的狗粮
        /// </summary>
        /// <param name="isMax"></param>
        private void changeGLS(bool[] isMax)
        {
            for (int i = 0; i < isMax.Length; i++)
            {
                if (isMax[i])
                {
                    Write((i + 1).ToString() + "号狗粮满级!");
                }
                else
                {
                    Write((i + 1).ToString() + "号狗粮没有满级!");
                }
            }
            if (isMax[0] || isMax[1] || isMax[2])
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
                for (int i = 0; i < Config.DragTimes; i++)
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
            while (times-- >= 0)
            {
                using (System.Drawing.Bitmap bmp = getPicture())
                {
                    Image<Bgr, byte> gl1 = new Image<Bgr, byte>(clipBitmap(bmp, TempImages.GL1_Rect));
                    Image<Bgr, byte> gl2 = new Image<Bgr, byte>(clipBitmap(bmp, TempImages.GL2_Rect));
                    Image<Bgr, byte> gl3 = new Image<Bgr, byte>(clipBitmap(bmp, TempImages.GL3_Rect));

                    if (!IsMax[0] && glIsMaxLevel(gl1))
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
                graphisc.DrawImage(bitmap, new System.Drawing.Rectangle(0, 0, rectangle.Width, rectangle.Height),
                    new System.Drawing.Rectangle(rectangle.X - 5, rectangle.Y - 45, rectangle.Width, rectangle.Height),
                    System.Drawing.GraphicsUnit.Pixel);
            }
            return result;
        }
        /// <summary>
        /// 检测战利品，返回是否出售
        /// </summary>
        private bool checkSpoils()
        {
            bool issail = false;
            NumClass numClass = null;
            double maxvalue = 0, value;
            Image<Bgr, byte> resultImage = null;
            Image<Bgr, byte> objimg = null;

            using (Image<Bgr, byte> image = new Image<Bgr, byte>(getPicture()))
            {
                objimg = new Image<Bgr, byte>(clipBitmap(image.Bitmap, TempImages.ResultObjecRect));
                
                foreach (var item in Spoils)
                {
                    value = matchtempforsharp(item.Key, objimg);
                    if (value > maxvalue)
                    {
                        numClass = item.Value;
                        maxvalue = value;
                        resultImage = item.Key;
                    }
                }
                //objimg.Save(namenum++.ToString() + "image.png");
                //objimg.Bitmap.Dispose();
                //objimg.Dispose();
                
                image.Bitmap.Dispose();
                image.Dispose();
                //image = null;
            }

            if (maxvalue > 0.75)
            {
                numClass.Num++;
                Write("获得战利品 : " + numClass.Name);
            }
            else
            {
                Write("获得战利品 : " + "其他");
            }
            if(resultImage != null)
            {
                if(resultImage == TempImages.CSFWTemp)
                {
                    this.FWImages.AddImage(objimg.Bitmap.ToBitmapSource());
                }
            }
            objimg.Bitmap.Dispose();
            objimg.Dispose();
            objimg = null;

            return numClass.IsToSail;
        }
#endregion
        /// <summary>
        /// 启动adbexe带上s参数
        /// </summary>
        /// <param name="s"></param>
        public static void startADBEXE(string s, string exepath)
        {
            using (Process process = new Process())
            {
                process.StartInfo = new ProcessStartInfo(exepath, s);// 括号里是(程序名,参数)
                process.StartInfo.CreateNoWindow = true;   //不创建窗口
                process.StartInfo.UseShellExecute = false;//不使用系统外壳程序启动，重定向时此处必须设为false
                process.StartInfo.RedirectStandardOutput = true; //重定向输出，而不是默认的显示在dos控制台上
                process.Start();
                process.WaitForExit();
                process.Dispose();
            }
        }

    }
}
