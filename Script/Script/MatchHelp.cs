using APILibrary.API;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Drawing.Imaging;
using EasyHook;

namespace Script
{
    /// <summary>
    /// 图像匹配类
    /// </summary>
    public static class MatchHelp
    {
        /// <summary>
        /// 截屏的模式
        /// </summary>
        public static ScreenMode ScreenMode { get; set; } = ScreenMode.Window;
        #region 匹配函数
        /// <summary>
        /// 模板匹配 匹配origin图像中是否存在模板tempImage
        /// </summary>
        /// <param name="tempImage"></param>
        /// <param name="orginImage"></param>
        /// <returns></returns>
        public static bool matchtempforsharp(Image<Bgr, byte> tempImage, Image<Bgr, byte> orginImage, ref Rect rect)
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
        public static bool matchtempforsharp(Image<Bgr, byte> tempImage, double throld = 0.13, int times = 0)
        {
            if (times >= 5)
            {
                throw new Exception("程序异常");
            }
            try
            {
                double value;
                using (Image<Bgr, byte> orginImage = new Image<Bgr, byte>(getPicture()))
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
        /// 模板匹配 匹配origin图像中是否存在模板tempImage
        /// </summary>
        /// <param name="tempImage"></param>
        /// <param name="orginImage"></param>
        /// <returns></returns>
        public static double matchtempforsharp(Image<Bgr, byte> tempImage, Image<Bgr, byte> orginImage)
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

        public static bool matchtempforsharp(Image<Bgr, byte> tempImage, Image<Bgr, byte> orginImage, ref int num, double throld = 0.13)
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
        /// <summary>
        /// 启动匹配程序
        /// </summary>
        public static void Start()
        {
            windowHand = WindowsAPI.FindWindow(null, "夜神模拟器");
            windowDC = WindowsAPI.GetWindowDC(windowHand);
            WindowsAPI.GetWindowRect(windowHand, out rect);
            bitmapHand = WindowsAPI.CreateCompatibleBitmap(windowDC, rect.Width, rect.Height);
            bitmapDC = WindowsAPI.CreateCompatibleDC(windowDC);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public static void Dispose()
        {

            WindowsAPI.DeleteDC(windowHand);
            WindowsAPI.DeleteDC(windowDC);
            WindowsAPI.DeleteDC(bitmapHand);
            WindowsAPI.DeleteDC(bitmapDC);
        }

        /// <summary>
        /// 截一张图用作识别
        /// </summary>
        public static System.Drawing.Bitmap getPicture2()
        {
            if (windowHand == IntPtr.Zero)
            {
                Start();
            }
            IntPtr hgdiobjBm = WindowsAPI.SelectObject(bitmapDC, bitmapHand);
            WindowsAPI.BitBlt(bitmapDC, 0, 0, rect.Width, rect.Height, windowDC, rect.X, rect.Y, 13369376);
            System.Drawing.Bitmap bitmap = System.Drawing.Bitmap.FromHbitmap(bitmapHand);
            bitmap.Save(TempImages.OrigImagePath);
            return bitmap;
        }
        /// <summary>
        /// 修正版
        /// </summary>
        /// <returns></returns>
        public static System.Drawing.Bitmap getPicture()
        {
            if(windowHand == IntPtr.Zero)
            {
                Start();
            }
            System.Drawing.Bitmap bitmap = null;
            if (ScreenMode == ScreenMode.Window)
            {
                WindowsAPI.SelectObject(bitmapDC, bitmapHand);
                WindowsAPI.PrintWindow(windowHand, bitmapDC, 0);
                bitmap = System.Drawing.Bitmap.FromHbitmap(bitmapHand);
            }
            else if(ScreenMode == ScreenMode.Desktop)
            {
                bitmap = WindowsAPI.GetDesktop(rect);
            }
            if(bitmap == null)
            {
                throw new Exception("识别异常!");
            }
            return bitmap;
        }
        ///// <summary>
        ///// DXHOOK版本
        ///// </summary>
        ///// <returns></returns>
        //public static System.Drawing.Bitmap getPicture3()
        //{

        //}
        #endregion
    }
}
