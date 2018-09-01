using EasyHook;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels.Ipc;
using System.Text;
using System.Threading.Tasks;
using Tools;
using APILibrary;
using System.Diagnostics;
using System.Threading;
using System.Drawing;

namespace MyOpenGLHookCSharp
{
    class Program
    {
        public static string channelName = null;

        public static bool isStop = false;
        static void Main(string[] args)
        {
            
            string pathName = "";
            "程序开始运行,创建共享内存".ToOutPut();

            IpcServerChannel memory = RemoteHooking.IpcCreateServer<MemoryCopyInterface>(ref channelName, System.Runtime.Remoting.WellKnownObjectMode.SingleCall);

            ("Ipc name is " + memory.ChannelName).ToOutPut();

            //(memory.ChannelData as MemoryCopyInterface).Ping();

            ("获取程序集路径 : " + (pathName = @"C:\Users\Administrator\Desktop\testForm\MyOpenGLHookCSharp\HookDll\bin\x86\Debug\HookDll.dll")
                ).ToOutPut();
            "获取目标进程......".ToOutPut();

            IntPtr intPtr = new IntPtr(1706608);//APILibrary.API.WindowsAPI.FindWindow(null, "ScreenBoardClassWindow");
            //"开始循环检测其他窗口的句柄!".ToOutPut();
            //Task<bool> task = new Task<bool>(() =>
            //{
            //    Point point = new Point();
            //    string text = "";
            //    while (!isStop)
            //    {
            //        APILibrary.API.WindowsAPI.GetCursorPos(ref point);
            //        IntPtr ptr = APILibrary.API.WindowsAPI.WindowFromPoint(point);
            //        if (ptr == IntPtr.Zero)
            //        {
            //            "未识别窗体的句柄".ToOutPut();
            //        }
            //        else
            //        {
            //            APILibrary.API.WindowsAPI.GetWindowText(ptr, out text);
            //            ("识别窗体句柄为 + " + ptr.ToString() + "，窗口标题 ：" + text).ToOutPut();
            //        }
            //        Thread.Sleep(1500);
            //    }
            //    "识别窗体句柄线程结束!".ToOutPut();
            //    return true;
            //});
            //task.Start();
            //IntPtr intPtr = new IntPtr(Convert.ToInt32(Console.ReadLine()));
            //isStop = true;
            //task.Result.ToString().ToOutPut();


            int processId = 0;
            if(intPtr != IntPtr.Zero)
            {
                "成功获取目标进程夜神模拟器的句柄".ToOutPut();
                APILibrary.API.WindowsAPI.GetWindowThreadProcessId(intPtr, ref processId);
                ("获取进程id : " + processId).ToOutPut();
            }
            else
            {
                "获取目标进程句柄失败".ToOutPut();
                "<Press any key to continue!>".ToOutPut();
                Console.ReadKey();
                return;
            }

            "开始注入dll到目标进程!".ToOutPut();
            ("目标进程id : " + processId).ToOutPut();
            ("文件地址 ： " + pathName +
                (File.Exists(pathName) ? "----存在" : "-----不存在")).ToOutPut();
            try
            {
                
                RemoteHooking.Inject(
                        processId,
                        pathName,
                        pathName,
                        channelName);

                ("注入成功!").ToOutPut();
                "等待输入任意键去截图".ToOutPut();
                Console.ReadKey();
                Bitmap bitmap = MemoryCopyInterface.GetOnePictureAnysc().Result;
                //bitmap.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.png"));
            }
            catch (Exception ex)
            {
                ("注入失败!Message : " + ex.ToString()).ToOutPut();
            }

            "<Press any key to continue!>".ToOutPut();
            Console.ReadKey();

        }

        
    }


    public static class Tolo
    {
        /// <summary>
        /// 输出到控制台
        /// </summary>
        /// <param name="content"></param>
        public static void ToOutPut(this string content)
        {
            Console.WriteLine(content);
        }
    }
}
