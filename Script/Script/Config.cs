using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Script
{
    public class Config
    {

        private ScreenMode screenMode = ScreenMode.Window;

        /// <summary>
        /// 滑动次数
        /// </summary>
        public int DragTimes { get; set; } = 10;
        /// <summary>
        /// 按键等待随机最小时长
        /// </summary>
        public int KeyPressWaitMinTime { get; set; } = 400;
        /// <summary>
        /// 按键等待随机最大时长
        /// </summary>
        public int KeyPressWaitMaxTime { get; set; } = 4000;
        /// <summary>
        /// 等待下一把的区间
        /// </summary>
        public Point WaitForNextValues { get; set; } = new Point(2000, 10000);
        /// <summary>
        /// 任务模式
        /// </summary>
        public TaskMode TaskMode { get; set; } = TaskMode.DGL;
        /// <summary>
        /// 视窗模式
        /// </summary>
        public ScreenMode ScreenMode
        {
            get
            {
                return screenMode;
            }
            set
            {
                screenMode = value;
            }
        }
        /// <summary>
        /// 虚拟路径
        /// </summary>
        public string VriualExePath { get; set; } = "";
    }
}
