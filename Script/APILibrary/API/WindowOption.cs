using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace APILibrary.API
{
    public static class WindowOption
    {
        /// <summary>
        /// 获取当前活动窗口的句柄
        /// </summary>
        /// <returns>返回句柄</returns>
        public static IntPtr GetActiveWindowIntPtr()
        {
            return WindowsAPI.GetForegroundWindow();
        }
        /// <summary>
        /// 获取进程列表
        /// </summary>
        /// <returns>进程列表</returns>
        public static Process [] GetAllProcess()
        {
            return Process.GetProcesses();
        }
        /// <summary>
        /// 判断2个进程是否是同一个
        /// </summary>
        /// <param name="proc">进程1</param>
        /// <param name="ptr">进程2的句柄</param>
        /// <returns></returns>
        public static bool Equal(Process proc, IntPtr ptr)
        {
            if (proc.MainWindowHandle == ptr)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 获取屏幕中鼠标所在位置的点的颜色值
        /// </summary>
        /// <returns>返回颜色值</returns>
        public static Color GetMousePointColor()
        {
            IntPtr hdc = WindowsAPI.GetDC(IntPtr.Zero);
            uint pixel = WindowsAPI.GetPixel(hdc, Cursor.Position.X, Cursor.Position.Y);
            WindowsAPI.ReleaseDC(IntPtr.Zero, hdc);
            Color color = Color.FromArgb((int)(pixel & 0x000000FF),
            (int)(pixel & 0x0000FF00) >> 8,
            (int)(pixel & 0x00FF0000) >> 16);
            return color;
        }
        /// <summary>
        /// 获取屏幕中鼠标所在位置的点的颜色值
        /// </summary>
        /// <param name="ptr">获取的范围句柄设定(0为全屏幕)</param>
        /// <returns>返回颜色值</returns>
        public static Color GetMousePointColor(IntPtr ptr)
        {
            IntPtr hdc = WindowsAPI.GetDC(ptr);
            uint pixel = WindowsAPI.GetPixel(hdc, Cursor.Position.X, Cursor.Position.Y);
            WindowsAPI.ReleaseDC(IntPtr.Zero, hdc);
            Color color = Color.FromArgb((int)(pixel & 0x000000FF),
            (int)(pixel & 0x0000FF00) >> 8,
            (int)(pixel & 0x00FF0000) >> 16);
            return color;
        }
    }
}
