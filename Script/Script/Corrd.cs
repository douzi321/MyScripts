using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Script
{
    public class Corrd
    {
        /// <summary>
        /// 每次等待的时间
        /// </summary>
        public int WaitTime { get; set; }
        /// <summary>
        /// 运行次数
        /// </summary>
        public int Times { get; set; }
        /// <summary>
        /// 停止日期
        /// </summary>
        public DateTime StopTime { get; set; }
    }
}
