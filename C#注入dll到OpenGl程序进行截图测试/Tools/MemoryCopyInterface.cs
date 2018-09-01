using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tools
{
    public class MemoryCopyInterface : MarshalByRefObject
    {
        /// <summary>
        /// 是否允许截图
        /// </summary>
        protected static bool isGetPictrue = false;
        /// <summary>
        /// 截图
        /// </summary>
        protected static Bitmap pictrue = null;
        /// <summary>
        /// 测试通讯是否任然保持连接
        /// </summary>
        public void Ping()
        {
            Console.WriteLine("Process is Ok");
        }
        /// <summary>
        /// 成功注入的时候
        /// </summary>
        public Action<bool> Start = null;

        /// <summary>
        /// 开始注入 （是否成功注入）
        /// </summary>
        /// <param name="isSuccess"></param>
        public void StartInict(bool isSuccess, int inictProcessIntPtr)
        {
            Console.WriteLine("注入" + (isSuccess ? "成功" : "失败") + ",线程号:" + inictProcessIntPtr.ToString());
        }
        /// <summary>
        /// 异步获取截图
        /// </summary>
        public static Task<Bitmap> GetOnePictureAnysc()
        {
            isGetPictrue = true;
            Task<Bitmap> task = new Task<Bitmap>(() =>
            {
                while (isGetPictrue)
                {
                    Thread.Sleep(100);
                }
                return pictrue;
            });
            task.Start();
            return task;
        }

        /// <summary>
        /// 设置截图
        /// </summary>
        /// <param name="bitmap"></param>
        public void SetOnePictrue(Bitmap bitmap = null)
        {
            Console.Write("完成截图");
            pictrue = bitmap;
            isGetPictrue = false;
        }
        /// <summary>
        /// 输出日志
        /// </summary>
        /// <param name="msg"></param>
        public void OutPutString(string msg)
        {
            Console.Write(msg);
        }
        /// <summary>
        /// 是否允许截图
        /// </summary>
        public bool CanPictrue => isGetPictrue;
    }
}
