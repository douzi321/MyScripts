using EasyHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tools;
using CSharpGL;
using System.Drawing;

namespace HookDll
{
    public class Main : IEntryPoint
    {

        MemoryCopyInterface Interface;
        LocalHook CreateFileHook;
        Stack<String> Queue = new Stack<String>();

        public Main(
            RemoteHooking.IContext InContext,
            String InChannelName)
        {
            // connect to host...
            Interface = RemoteHooking.IpcConnectClient<MemoryCopyInterface>(InChannelName);

            Interface.Ping();
        }

        public void Run(
            RemoteHooking.IContext InContext,
            String InChannelName)
        {
            //// install hook...
            //try
            //{

            //    CreateFileHook = LocalHook.Create(
            //        LocalHook.GetProcAddress("kernel32.dll", "CreateFileW"),
            //        new DCreateFile(CreateFile_Hooked),
            //        this);

            //    CreateFileHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
            //}
            //catch (Exception ExtInfo)
            //{
            //    Interface.ReportException(ExtInfo);

            //    return;
            //}

            Interface.StartInict(true, RemoteHooking.GetCurrentProcessId());

            RemoteHooking.WakeUpProcess();

            // wait for host process termination...
            try
            {
                while (true)
                {
                    Thread.Sleep(500);
                    ///如果开始截图
                    if(Interface.CanPictrue)
                    {
                        Interface.Ping();
                        Save2Picture(0, 0, 400, 300, @"C:\Users\Administrator\Desktop\testForm\test.png");
                        Interface.SetOnePictrue();
                    }
                    
                    //Interface.Ping();
                }
            }
            catch
            {
                // Ping() will raise an exception if host is unreachable
            }
        }




        /// <summary>
        /// 把OpenGL渲染的内容保存到图片文件。
        /// </summary>
        /// <param name="x">左下角坐标为(0, 0)</param>
        /// <param name="y">左下角坐标为(0, 0)</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="filename"></param>
        public void Save2Picture(int x, int y, int width, int height, string filename)
        {
            Interface.OutPutString("截图开始区域为 : " + x.ToString() + "," + y.ToString() +
                "," + width.ToString() + "," + height.ToString() + "---" + filename);
            var format = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
            var lockMode = System.Drawing.Imaging.ImageLockMode.WriteOnly;
            Bitmap bitmap = new Bitmap(width, height, format);
            var bitmapRect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            System.Drawing.Imaging.BitmapData bmpData = bitmap.LockBits(bitmapRect, lockMode, format);
            OpenGL.ReadPixels(x, y, width, height, OpenGL.GL_BGRA, OpenGL.GL_UNSIGNED_BYTE, bmpData.Scan0);
            bitmap.UnlockBits(bmpData);
            //bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);

            bitmap.Save(filename);
            bitmap.Dispose();
            bitmap = null;
        }
    }
}
