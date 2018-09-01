using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Script
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:Script"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:Script;assembly=Script"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误: 
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:CurButtom/>
    ///
    /// </summary>
    public class CurButtom : Border
    {
        static CurButtom()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CurButtom), new FrameworkPropertyMetadata(typeof(CurButtom)));
        }

        public CurButtom()
        {
            TextBlock textBlock = new TextBlock();
            this.Child = textBlock;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;

            textBlock.BindTo(TextBlock.TextProperty, new Binding() { Source = this, Path = new PropertyPath("Text") });
            textBlock.BindTo(TextBlock.FontFamilyProperty, new Binding() { Source = this, Path = new PropertyPath("FontFamily") });
            textBlock.BindTo(TextBlock.FontSizeProperty, new Binding() { Source = this, Path = new PropertyPath("FontSize") });
            textBlock.BindTo(TextBlock.ForegroundProperty, new Binding() { Source = this, Path = new PropertyPath("Foreground") });
            textBlock.Margin = new Thickness(5, 5, 5, 5);

            this.MouseDown += CurButtom_MouseDown;
            this.MouseUp += CurButtom_MouseUp;
            this.MouseLeave += CurButtom_MouseLeave;
            this.IsSelected = false;
        }
        /// <summary>
        /// 鼠标离开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurButtom_MouseLeave(object sender, MouseEventArgs e)
        {
            IsRelease = false;
            IsPress = false;
        }

        /// <summary>
        /// 鼠标抬起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurButtom_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left && e.LeftButton == MouseButtonState.Released)
            {
                IsRelease = true;
                IsPress = false;

                IsSelected = !IsSelected;
            }
        }

        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurButtom_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                IsPress = true;
                IsRelease = false;
            }
        }



        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(CurButtom));

        public static DependencyProperty FontFamilyProperty = DependencyProperty.Register("FontFamily", typeof(FontFamily), typeof(CurButtom));

        public static DependencyProperty FontSizeProperty = DependencyProperty.Register("FontSize", typeof(double), typeof(CurButtom));

        public static DependencyProperty ForegroundProperty = DependencyProperty.Register("Foreground", typeof(Brush), typeof(CurButtom));


        public static DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(CurButtom));


        public static DependencyProperty IsPressProperty = DependencyProperty.Register("IsPress", typeof(bool), typeof(CurButtom));

        public static DependencyProperty IsReleaseProperty = DependencyProperty.Register("IsRelease", typeof(bool), typeof(CurButtom));


        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public FontFamily FontFamily
        {
            get
            {
                return (FontFamily)GetValue(FontFamilyProperty);
            }
            set
            {
                SetValue(FontFamilyProperty, value);
            }
        }

        public double FontSize
        {
            get
            {
                return (double)GetValue(FontSizeProperty);
            }
            set
            {
                SetValue(FontSizeProperty, value);
            }
        }


        public Brush Foreground
        {
            get
            {
                return (Brush)GetValue(ForegroundProperty);
            }
            set
            {
                SetValue(ForegroundProperty, value);
            }
        }

        public bool IsSelected
        {
            get
            {
                return (bool)GetValue(IsSelectedProperty);
            }
            set
            {
                SetValue(IsSelectedProperty, value);
            }
        }

        public bool IsPress
        {
            get
            {
                return (bool)GetValue(IsPressProperty);
            }
            set
            {
                SetValue(IsPressProperty, value);
            }
        }


        public bool IsRelease
        {
            get
            {
                return (bool)GetValue(IsReleaseProperty);
            }
            set
            {
                SetValue(IsReleaseProperty, value);
            }
        }
    }
}
