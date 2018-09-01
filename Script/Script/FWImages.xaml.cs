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
        /// <summary>
        /// 图片集合
        /// </summary>
        private ObservableCollection<BitmapSource> ImagesList = new ObservableCollection<BitmapSource>();


        public FWImages()
        {
            InitializeComponent();
            this.Loaded += FWImages_Loaded;
            this.Closing += FWImages_Closing;
        }

        private void FWImages_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

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
        /// 添加一张图片
        /// </summary>
        /// <param name="bitmapImage"></param>
        public void AddImage(BitmapSource bit)
        {
            this.Dispatcher.Invoke(new Action<BitmapSource>((bitmapImage) => {
                if (ImagesList.Count > 50)
                {
                    Clear();
                }
                ImagesList.Add(bitmapImage);
            }), bit);
        }
        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            ImagesList.Clear();
        }
    }
}
