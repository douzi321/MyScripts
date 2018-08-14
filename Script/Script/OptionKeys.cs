using APILibrary.API;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Script
{
    /// <summary>
    /// 动作按键规定
    /// </summary>
    public class OptionKeys
    {
        /// <summary>
        /// 开始游戏按键
        /// </summary>
        public static OptionKeys StartKey = new OptionKeys(EnumKeyboardKey.E, new Point(1110, 517));
        /// <summary>
        /// 再来一次按键
        /// </summary>
        public static OptionKeys LoopKey = new OptionKeys(EnumKeyboardKey.O, new Point(375, 399));
        /// <summary>
        /// 空白位置按键
        /// </summary>
        public static OptionKeys WhiteKey = new OptionKeys(EnumKeyboardKey.U, new Point(912, 213));
        /// <summary>
        /// 确认获取符文按键
        /// </summary>
        public static OptionKeys SureKey = new OptionKeys(EnumKeyboardKey.F, new Point(765, 595));
        /// <summary>
        /// 出售符文
        /// </summary>
        public static OptionKeys SailFWKey = new OptionKeys(EnumKeyboardKey.P, new Point(533, 595));
        /// <summary>
        /// 出售符文二次确认按键
        /// </summary>
        public static OptionKeys SailFWTwo = new OptionKeys(EnumKeyboardKey.H, new Point(525, 445));
        /// <summary>
        /// 获取符文以外的物品按键
        /// </summary>
        public static OptionKeys GetKey = new OptionKeys(EnumKeyboardKey.I, new Point(651, 563));
        /// <summary>
        /// 游戏失败的时候退出买红水再来一次的按键
        /// </summary>
        public static OptionKeys GameFailReturnKey = new OptionKeys(EnumKeyboardKey.G, new Point(846, 484));
        /// <summary>
        /// 体力不足的时候返回按键
        /// </summary>
        public static OptionKeys NlgReturnKey = new OptionKeys(EnumKeyboardKey.J, new Point(768, 448));

        /// <summary>
        /// 上
        /// </summary>
        public static OptionKeys UpKey = new OptionKeys(EnumKeyboardKey.W, new Point(671, 432), true, 672, 279);
        /// <summary>
        /// 下
        /// </summary>
        public static OptionKeys DownKey = new OptionKeys(EnumKeyboardKey.D, new Point(735, 370), true ,569, 367);
        /// <summary>
        /// 左
        /// </summary>
        public static OptionKeys LeftKey = new OptionKeys(EnumKeyboardKey.A, new Point(569, 367), true, 735, 370);
        /// <summary>
        /// 右
        /// </summary>
        public static OptionKeys RightKey = new OptionKeys(EnumKeyboardKey.D, new Point(672, 279), true, 671, 432);

        /// <summary>
        /// 狗粮1
        /// </summary>
        public static OptionKeys GL1Key = new OptionKeys(EnumKeyboardKey.D1, new Point(192, 257));
        /// <summary>
        /// 狗粮2
        /// </summary>
        public static OptionKeys GL2Key = new OptionKeys(EnumKeyboardKey.D2, new Point(325, 324));
        /// <summary>
        /// 狗粮3
        /// </summary>
        public static OptionKeys GL3Key = new OptionKeys(EnumKeyboardKey.D3, new Point(466, 254));
        /// <summary>
        /// 获取狗粮1
        /// </summary>
        public static OptionKeys GetGL1Key = new OptionKeys(EnumKeyboardKey.D7, new Point(818, 551));
        /// <summary>
        /// 获取狗粮2
        /// </summary>
        public static OptionKeys GetGL2Key = new OptionKeys(EnumKeyboardKey.D8 ,new Point(924, 555));
        /// <summary>
        /// 获取狗粮3
        /// </summary>
        public static OptionKeys GetGL3Key = new OptionKeys(EnumKeyboardKey.D9, new Point(1032, 551));

        /// <summary>
        /// 仓库
        /// </summary>
        public static OptionKeys RepertoryKey = new OptionKeys(EnumKeyboardKey.R, new Point(74, 496));
        /// <summary>
        /// 仓库里面选择排列顺序
        /// </summary>
        public static OptionKeys RepertoryForSelectKey = new OptionKeys(EnumKeyboardKey.T, new Point(318, 142));

        /// <summary>
        /// 仓库选择强化顺序为排列顺序
        /// </summary>
        public static OptionKeys RepertorySelectQHSXKey = new OptionKeys(EnumKeyboardKey.Y, new Point(308, 327));
        /// <summary>
        /// 确认并且关闭仓库
        /// </summary>
        public static OptionKeys SureAndCloseRepertoryKey = new OptionKeys(EnumKeyboardKey.K, new Point(655, 646));
        /// <summary>
        /// 获取魔力石头
        /// </summary>
        public static OptionKeys GetMagicKey = new OptionKeys(EnumKeyboardKey.C, new Point(650, 650));

        /// <summary>
        /// 回到准备界面的按钮
        /// </summary>
        public static OptionKeys BackZBKey = new OptionKeys(EnumKeyboardKey.V, new Point(776, 598));

        /// <summary>
        /// 按键
        /// </summary>
        public EnumKeyboardKey Key { get; private set; }
        /// <summary>
        /// 坐标
        /// </summary>
        public Point Position { get; private set; }
        /// <summary>
        /// 如果是滑动的话 结束点的坐标
        /// </summary>
        public Point EndPosition { get; private set; }
        /// <summary>
        /// 是否是滑动
        /// </summary>
        public bool IsDrag { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="position"></param>
        /// <param name="isdrag"></param>
        /// <param name="endx"></param>
        /// <param name="endy"></param>
        public OptionKeys(EnumKeyboardKey key, Point position, bool isdrag = false, int endx = 0, int endy = 0)
        {
            this.Key = key;
            this.Position = position;
            this.IsDrag = isdrag;
            if(isdrag)
            {
                this.EndPosition = new Point(endx, endy);
            }
        }
    }
}
