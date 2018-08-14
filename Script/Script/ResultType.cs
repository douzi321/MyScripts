using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Script
{
    /// <summary>
    /// 刷完副本结果
    /// </summary>
    public enum ResultType
    {
        FiveBlue,
        FiveZ,
        FiveC,
        SixBlue,
        SixZ,
        SixC,
        /// <summary>
        /// 厕纸
        /// </summary>
        WC,
        /// <summary>
        /// 小黄书
        /// </summary>
        YellowBook,
        /// <summary>
        /// 符文石
        /// </summary>
        Other,
        /// <summary>
        /// 游戏失败
        /// </summary>
        Fail,

        Timeout,

        Vector,


    }

    public enum TaskMode
    {
        None,
        /// <summary>
        /// 带狗粮
        /// </summary>
        DGL,
        /// <summary>
        /// 地下城
        /// </summary>
        DXC,
        /// <summary>
        /// 刷塔
        /// </summary>
        ST,
        /// <summary>
        /// 魔力石
        /// </summary>
        Magic,
    }

    public enum ScreenMode
    {
        Desktop,
        Window,
        Vriual,
    }


    //public enum RunMode
    //{
    //    Fast,
    //    Last,
    //}
}
