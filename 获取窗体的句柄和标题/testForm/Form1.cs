using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using APILibrary.API;

namespace testForm
{
    public partial class Form1 : Form
    {

        private Thread thread = null;
        /// <summary>
        /// 窗口句柄
        /// </summary>
        private static IntPtr windowHand = IntPtr.Zero;
        /// <summary>
        /// 窗口DC
        /// </summary>
        private static IntPtr windowDC = IntPtr.Zero;
        /// <summary>
        /// 图片句柄
        /// </summary>
        private static IntPtr bitmapHand = IntPtr.Zero;
        /// <summary>
        /// 图片的DC
        /// </summary>
        private static IntPtr bitmapDC = IntPtr.Zero;
        /// <summary>
        /// 区域
        /// </summary>
        private static System.Drawing.Rectangle rect;

        public Form1()
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        /// <summary>
        /// 截图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {

            if(textBox1.Text.Equals("") == false)
            {
                windowHand = new IntPtr(Convert.ToInt32(textBox1.Text));
                windowDC = WindowsAPI.GetWindowDC(windowHand);
                WindowsAPI.GetWindowRect(windowHand, out rect);
                bitmapHand = WindowsAPI.CreateCompatibleBitmap(windowDC, rect.Width, rect.Height);
                bitmapDC = WindowsAPI.CreateCompatibleDC(windowDC);

                IntPtr hgdiobjBm = WindowsAPI.SelectObject(bitmapDC, bitmapHand);
                WindowsAPI.BitBlt(bitmapDC, 0, 0, rect.Width, rect.Height, windowDC, rect.X, rect.Y, 13369376);
                System.Drawing.Bitmap bitmap = System.Drawing.Bitmap.FromHbitmap(bitmapHand);
                //bitmap.Save(TempImages.OrigImagePath);
                pictureBox1.Image = bitmap;
                
                WindowsAPI.DeleteDC(windowHand);
                WindowsAPI.DeleteDC(windowDC);
                WindowsAPI.DeleteDC(bitmapHand);
                WindowsAPI.DeleteDC(bitmapDC);
            }
            else
            {
                Write("句柄错误!");
                MessageBox.Show("填写句柄");
            }
        }


        private void Write(string msg)
        {
            this.Invoke(new Action<string>((message)=>{
                log.Text += message + "\r\n";
            }), msg);
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start(object sender, EventArgs e)
        {
            thread = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        Point point = new Point();
                        string text = "";
                        while (true)
                        {
                            APILibrary.API.WindowsAPI.GetCursorPos(ref point);
                            IntPtr ptr = APILibrary.API.WindowsAPI.WindowFromPoint(point);
                            if (ptr == IntPtr.Zero)
                            {
                                Write("未识别窗体的句柄");
                            }
                            else
                            {
                                APILibrary.API.WindowsAPI.GetWindowText(ptr, out text);
                                Write("识别窗体句柄为 + " + ptr.ToString() + "，窗口标题 ：" + text);
                            }
                            Thread.Sleep(1500);
                        }
                    }
                }
                catch (Exception)
                {
                    Write("任务运行结束!");
                }
            })
            { IsBackground = true };
            thread.Start();
        }
        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Stop(object sender, EventArgs e)
        {
            if(thread != null)
                thread.Abort();
        }
    }
}
