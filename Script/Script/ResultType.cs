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

    public class TaskModeAttribute : Attribute
    {

        public string Description { get; set; } = "";

        public TaskModeAttribute(string description)
        {
            this.Description = description;
        }
    }


    public enum TaskMode
    {
        [TaskMode("没选择模式")]
        None,
        /// <summary>
        /// 带狗粮
        /// </summary>
        [TaskMode("带狗粮模式")]
        DGL,
        /// <summary>
        /// 地下城
        /// </summary>
        [TaskMode("地下城模式")]
        DXC,
        /// <summary>
        /// 刷塔
        /// </summary>
        [TaskMode("刷塔模式")]
        ST,
        /// <summary>
        /// 魔力石
        /// </summary>
        [TaskMode("刷魔力石模式")]
        Magic,
    }

    public enum ScreenMode
    {
        Desktop,
        Window,
    }


    //public enum RunMode
    //{
    //    Fast,
    //    Last,
    //}
}
