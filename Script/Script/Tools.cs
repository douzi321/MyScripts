using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Script
{
    public static class Tools
    {
        /// <summary>
        /// 绑定的扩展方法
        /// </summary>
        /// <param name="frameworkElement"></param>
        /// <param name="dependencyProperty"></param>
        /// <param name="binding"></param>
        public static void BindTo(this FrameworkElement frameworkElement, DependencyProperty dependencyProperty, Binding binding)
        {
            BindingOperations.SetBinding(frameworkElement, dependencyProperty, binding);
        }
        /// <summary>
        /// 最后一个元素
        /// </summary>
        /// <param name="uIElementCollection"></param>
        /// <returns></returns>
        public static UIElement Last(this UIElementCollection uIElementCollection)
        {
            return uIElementCollection[uIElementCollection.Count - 1];
        }
        /// <summary>
        /// 根据字符串获取枚举值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetTFromString<T>(this string name)
        {
            foreach (T item in Enum.GetValues(typeof(T)))
            {
                if (item.ToString() == name)
                {
                    return item;
                }
            }
            return default(T);
        }

        /// <summary>
        /// 加载图片
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Image<Bgr, byte> ToImageBgr(this string url)
        {
            if (!File.Exists(url))
            {
                throw new Exception("不存在图片，" + url);
            }
            string exname = System.IO.Path.GetExtension(url);
            if (exname.Equals(".png") || exname.Equals(".jpg") || exname.Equals(".jpeg")
                || exname.Equals(".bmp"))
            {
                return new Image<Bgr, byte>(url);
            }
            else
            {
                throw new Exception("不是有效的图片，" + url);
            }
        }
        /// <summary>
        /// bitmapToBitmapSource
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static BitmapSource ToBitmapSource(this System.Drawing.Bitmap bitmap)
        {
            MemoryStream memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memoryStream;
            bitmapImage.EndInit();
            //memoryStream.Dispose();
            //memoryStream.Close();
            memoryStream = null;
            return bitmapImage;
        }
        /// <summary>
        /// 给图片打上框和文字
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="rectangle"></param>
        /// <param name="text"></param>
        public static System.Drawing.Bitmap RectBitmap(this System.Drawing.Bitmap bitmap, System.Drawing.Rectangle rectangle, string text = "")
        {
            ///获取绘图句柄
            using (System.Drawing.Graphics graphisc = System.Drawing.Graphics.FromImage(bitmap))
            {
                graphisc.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Red), rectangle);
                graphisc.DrawString(text, new System.Drawing.Font("宋体", 15), System.Drawing.Brushes.Red, new System.Drawing.PointF(rectangle.X, rectangle.Y - 15));
            }
            return bitmap;
        }

        /// <summary>
        /// 追加路径
        /// </summary>
        /// <param name="url"></param>
        /// <param name="appendurl"></param>
        /// <returns></returns>
        public static string AppendPath(this string url, string appendurl)
        {
            url = System.IO.Path.Combine(url, appendurl);
            return url;
        }
        /// <summary>
        /// doubletofloat
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ToFloat(this double value)
        {
            return (float)value
;
        }

        /// <summary>
        /// 二值化
        /// </summary>
        /// <param name="grayimage"></param>
        public static Image<Gray, byte> ToThreshold(this Image<Gray, byte> grayimage, byte threshold)
        {
            //Image<Gray, byte> threshImage = new Image<Gray, byte>(grayimage.Width, grayimage.Height);
            CvInvoke.Threshold(grayimage, grayimage, threshold, 255, ThresholdType.Binary);
            return grayimage;
        }

        /// <summary>
        /// 二值化
        /// </summary>
        /// <param name="grayimage"></param>
        public static Image<Gray, byte> ToThresholdTwo(this Image<Gray, byte> grayimage)
        {
            grayimage = grayimage.ThresholdAdaptive(new Gray(255), AdaptiveThresholdType.MeanC, ThresholdType.Binary, 9, new Gray(5));
            return grayimage;
        }

        public static Image<Gray, byte> ToThresholdThree(this Image<Gray, byte> grayimage, byte threshold)
        {
            CvInvoke.Threshold(grayimage, grayimage, threshold, 255, ThresholdType.Otsu);
            return grayimage;
        }

        /// <summary>
        /// 将对象保存在文件
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool ToFile(this object obj, string path)
        {
            try
            {
                using (FileStream stream = File.Open(path, FileMode.Create))
                {
                    byte[] datas = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));
                    stream.Write(datas, 0, datas.Length);
                    datas = null;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 从文件加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T LoadFromFile<T>(this string path)
        {
            T t;
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                byte[] datas = new byte[stream.Length];
                stream.Read(datas, 0, datas.Length);
                t = JsonConvert.DeserializeObject<T>(System.Text.Encoding.UTF8.GetString(datas));
                datas = null;
                
            }
            return t;
        }
    }

    public class ImageValueChange : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Image<Bgr, byte> image = value as Image<Bgr, byte>;
            return image.Bitmap.ToBitmapSource();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
