using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace Script
{
    public class NumClass : INotifyPropertyChanged
    {

        private int num = 0;
        private bool isSail = false;
        private string name = "";
        private double width;
        private double height;
        private System.Windows.Controls.Orientation mode;
        private Thickness margin = new Thickness();


        public Thickness Margin
        {
            get
            {
                return margin;
            }
            set
            {
                margin = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Margin"));
            }
        }

        public System.Windows.Controls.Orientation Mode
        {
            get
            {
                return mode;
            }
            set
            {
                mode = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Mode"));
            }
        }
        public double Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Width"));
            }
        }
        public double Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Height"));
            }
        }


        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }
        public int Num
        {
            get
            {
                return num;
            }
            set
            {
                num = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Num"));
            }
        }
        /// <summary>
        /// 是否出售
        /// </summary>
        public bool IsToSail
        {
            get
            {
                return isSail;
            }
            set
            {
                isSail = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsToSail"));
            }
        }

        public NumClass(int num, string name)
        {
            Num = num;
            this.Name = name;
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
