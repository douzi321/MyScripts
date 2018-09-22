using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
    /// FWImages.xaml 的交互逻辑
    /// </summary>
    public partial class FWImages : Window
    {
        


        public FWImages()
        {
            InitializeComponent();
            this.Loaded += FWImages_Loaded;
        }


        /// <summary>
        /// 图片集合
        /// </summary>
        private ObservableCollection<BitmapSource> ImagesList = null;
        /// <summary>
        /// 加载的时候
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FWImages_Loaded(object sender, RoutedEventArgs e)
        {
            this.images.ItemsSource = ImagesList;
        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <param name="bitmaps"></param>
        public void ShowWindow(ObservableCollection<BitmapSource> bitmaps)
        {
            this.ImagesList = bitmaps;
            this.ShowDialog();
        }
    }
}
