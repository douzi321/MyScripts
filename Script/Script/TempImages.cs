using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Script
{
    /// <summary>
    /// 所有的模板图片
    /// </summary>
    public static class TempImages
    {
        /// <summary>
        /// 模拟器固定分辨率
        /// </summary>
        public static Size VirtualBoxSize = new Size(1200, 720);
        /// <summary>
        /// 原图保存路径
        /// </summary>
        public static string OrigImagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OrigImage.png");
        /// <summary>
        /// 模板图片根目录
        /// </summary>
        private static string rootPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temps/");

        /// <summary>
        /// 一号狗粮坐标
        /// </summary>
        public static System.Drawing.Rectangle GL1_Rect = new System.Drawing.Rectangle(597, 470, 160, 100);
        /// <summary>
        /// 2号狗粮坐标
        /// </summary>
        public static System.Drawing.Rectangle GL2_Rect = new System.Drawing.Rectangle(855, 470, 160, 100);
        /// <summary>
        /// 3号狗粮坐标
        /// </summary>
        public static System.Drawing.Rectangle GL3_Rect = new System.Drawing.Rectangle(323, 560, 160, 100);
        /// <summary>
        /// 胜利的模板
        /// </summary>
        public static Image<Bgr, byte> VectorTemp = new Image<Bgr, byte>(new System.Drawing.Bitmap(rootPath.AppendPath("vector.png")));
        /// <summary>
        /// 失败的模板
        /// </summary>
        public static Image<Bgr, byte> FailTemp = new Image<Bgr, byte>(new System.Drawing.Bitmap(rootPath.AppendPath("fail.png")));
        /// <summary>
        /// 进度条的模板
        /// </summary>
        public static Image<Bgr, byte> BarTemp = new Image<Bgr, byte>(new System.Drawing.Bitmap(rootPath.AppendPath("bar.png")));
        /// <summary>
        /// 结束后选择的模板
        /// </summary>
        public static Image<Bgr, byte> EndSelectTemp = new Image<Bgr, byte>(new System.Drawing.Bitmap(rootPath.AppendPath("end.png")));
        /// <summary>
        /// 是否获取道具的模板
        /// </summary>
        public static Image<Bgr, byte> GetTemp = new Image<Bgr, byte>(new System.Drawing.Bitmap(rootPath.AppendPath("get.png")));
        /// <summary>
        /// 是否获取道具的模板
        /// </summary>
        public static Image<Bgr, byte> SureSailTemp = new Image<Bgr, byte>(rootPath.AppendPath("suresail.png"));
        /// <summary>
        /// 开始战斗的模板
        /// </summary>
        public static Image<Bgr, byte> StartTemp = new Image<Bgr, byte>(new System.Drawing.Bitmap(rootPath.AppendPath("start.png")));
        /// <summary>
        /// 体力不足的模板
        /// </summary>
        public static Image<Bgr, byte> EnergyNotEnoughTemp = new Image<Bgr, byte>(new System.Drawing.Bitmap(rootPath.AppendPath("nlg.png")));

        /// <summary>
        /// 回到准备界面
        /// </summary>
        public static Image<Bgr, byte> BackZBTemp = new Image<Bgr, byte>(rootPath.AppendPath("back.png"));
        /// <summary>
        /// 最大等级模板
        /// </summary>
        public static Image<Bgr, byte> MaxLevelTemp = new Image<Bgr, byte>(new System.Drawing.Bitmap(rootPath.AppendPath("maxlevel.png")));
        /// <summary>
        /// 最大等级模板
        /// </summary>
        public static Image<Bgr, byte> MaxLevel2Temp = new Image<Bgr, byte>(new System.Drawing.Bitmap(rootPath.AppendPath("maxlevel2.png")));
        /// <summary>
        /// 最大等级模板
        /// </summary>
        public static Image<Bgr, byte> FuHuoTemp = new Image<Bgr, byte>(new System.Drawing.Bitmap(rootPath.AppendPath("fuhuo.png")));
        /// <summary>
        /// 战利品框
        /// </summary>
        public static System.Drawing.Rectangle ResultObjecRect = new System.Drawing.Rectangle(392, 195, 528, 495);


        #region 战利品模板
        /// <summary>
        /// 最大等级模板
        /// </summary>
        public static Image<Bgr, byte> CFImgTemp = new Image<Bgr, byte>(new System.Drawing.Bitmap(rootPath.AppendPath("cfimg.png")));
        /// <summary>
        /// 最大等级模板
        /// </summary>
        public static Image<Bgr, byte> CHGuaiTemp = new Image<Bgr, byte>(new System.Drawing.Bitmap(rootPath.AppendPath("chguai.png")));
        /// <summary>
        /// 最大等级模板
        /// </summary>
        public static Image<Bgr, byte> CSFWTemp = new Image<Bgr, byte>(new System.Drawing.Bitmap(rootPath.AppendPath("csfw.png")));
        /// <summary>
        /// 最大等级模板
        /// </summary>
        public static Image<Bgr, byte> CZTemp = new Image<Bgr, byte>(new System.Drawing.Bitmap(rootPath.AppendPath("cz.png")));
        /// <summary>
        /// 最大等级模板
        /// </summary>
        public static Image<Bgr, byte> FSImgTemp = new Image<Bgr, byte>(new System.Drawing.Bitmap(rootPath.AppendPath("fsimg.png")));
        /// <summary>
        /// 最大等级模板
        /// </summary>
        public static Image<Bgr, byte> HDImgTemp = new Image<Bgr, byte>(new System.Drawing.Bitmap(rootPath.AppendPath("hdimg.png")));
        /// <summary>
        /// 最大等级模板
        /// </summary>
        public static Image<Bgr, byte> HSTemp = new Image<Bgr, byte>(new System.Drawing.Bitmap(rootPath.AppendPath("hs.png")));
        /// <summary>
        /// 最大等级模板
        /// </summary>
        public static Image<Bgr, byte> HXImgTemp = new Image<Bgr, byte>(new System.Drawing.Bitmap(rootPath.AppendPath("hximg.png")));

        /// <summary>
        /// 最大等级模板
        /// </summary>
        public static Image<Bgr, byte> STTemp = new Image<Bgr, byte>(new System.Drawing.Bitmap(rootPath.AppendPath("st.png")));
        /// <summary>
        /// 最大等级模板
        /// </summary>
        public static Image<Bgr, byte> XYFWTemp = new Image<Bgr, byte>(new System.Drawing.Bitmap(rootPath.AppendPath("xyfw.png")));
        /// <summary>
        /// 最大等级模板
        /// </summary>
        public static Image<Bgr, byte> YXFWTemp = new Image<Bgr, byte>(new System.Drawing.Bitmap(rootPath.AppendPath("yxfw.png")));
        #endregion
    }
}
