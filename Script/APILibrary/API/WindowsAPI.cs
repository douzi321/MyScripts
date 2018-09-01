using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace APILibrary.API
{
    public class WindowsAPI
    {
        public delegate bool EnumWindowsProc(IntPtr pHandle, int p_Param);
        public delegate bool CallBack(IntPtr pwnd, int lParam);
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct STRINGBUFFER
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string szText;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Down;
            public override string ToString()
            {
                return string.Concat(new object[]
                {
                    "{Left:",
                    this.Left,
                    ", Up:",
                    this.Top,
                    ", RightPP:",
                    this.Right,
                    "DownPP:",
                    this.Down,
                    "}"
                });
            }
        }
        private struct CURSORINFO
        {
            public int cbSize;
            public int flags;
            public IntPtr hCursor;
            public POINT ptScreenPos;
        }
        public const uint KEYEVENTF_KEYUP = 2u;
        public const uint KEYEVENTF_KEYDOWN = 0u;
        private const int STRINGBUFFER_SizeConst = 512;
        [DllImport("gdi32.dll")]
        public static extern IntPtr DeleteDC(IntPtr hdc);
        [DllImport("user32.dll")]
        public static extern bool PrintWindow(
         IntPtr hwnd,               // Window to copy,Handle to the window that will be copied. 
         IntPtr hdcBlt,             // HDC to print into,Handle to the device context. 
         UInt32 nFlags              // Optional flags,Specifies the drawing options. It can be one of the following values. 
         );
        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        public static extern bool BitBlt(
             IntPtr hdcDest,     //目标设备的句柄
             int XDest,          //目标对象的左上角X坐标
             int YDest,          //目标对象的左上角的Y坐标
             int Width,          //目标对象的宽度
             int Height,         //目标对象的高度
             IntPtr hdcScr,      //源设备的句柄
             int XScr,           //源设备的左上角X坐标
             int YScr,           //源设备的左上角Y坐标
             Int32 drRop         //光栅的操作值
            );
        [DllImport("gdi32.dll")]
        private static extern IntPtr DeleteObject(IntPtr hObject);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobjBm);
        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);
        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();
        public static Bitmap GetDesktop()
        {
            IntPtr dC = WindowsAPI.GetDC(WindowsAPI.GetDesktopWindow());
            IntPtr intPtr = WindowsAPI.CreateCompatibleDC(dC);
            int systemMetrics = WindowsAPI.GetSystemMetrics(0);
            int systemMetrics2 = WindowsAPI.GetSystemMetrics(1);
            IntPtr intPtr2 = WindowsAPI.CreateCompatibleBitmap(dC, systemMetrics, systemMetrics2);
            Bitmap result;
            if (intPtr2 != IntPtr.Zero)
            {
                IntPtr hgdiobjBm = WindowsAPI.SelectObject(intPtr, intPtr2);
                WindowsAPI.BitBlt(intPtr, 0, 0, systemMetrics, systemMetrics2, dC, 0, 0, 13369376);
                WindowsAPI.SelectObject(intPtr, hgdiobjBm);
                WindowsAPI.DeleteDC(intPtr);
                WindowsAPI.ReleaseDC(WindowsAPI.GetDesktopWindow(), dC);
                Bitmap bitmap = Image.FromHbitmap(intPtr2);
                WindowsAPI.DeleteObject(intPtr2);
                GC.Collect();
                result = bitmap;
            }
            else
            {
                result = null;
            }
            return result;
        }
        /// <summary>
        /// 截取桌面指定区域的矩形
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static Bitmap GetDesktop(System.Drawing.Rectangle rectangle)
        {
            IntPtr dC = WindowsAPI.GetDC(WindowsAPI.GetDesktopWindow());
            IntPtr intPtr = WindowsAPI.CreateCompatibleDC(dC);
            IntPtr intPtr2 = WindowsAPI.CreateCompatibleBitmap(dC, rectangle.Width, rectangle.Height);
            Bitmap result;
            if (intPtr2 != IntPtr.Zero)
            {
                IntPtr hgdiobjBm = WindowsAPI.SelectObject(intPtr, intPtr2);
                WindowsAPI.BitBlt(intPtr, 0, 0, rectangle.Width, rectangle.Height, dC, rectangle.X, rectangle.Y, 13369376);
                //WindowsAPI.SelectObject(intPtr, hgdiobjBm);
                WindowsAPI.DeleteDC(intPtr);
                WindowsAPI.ReleaseDC(WindowsAPI.GetDesktopWindow(), dC);
                Bitmap bitmap = Image.FromHbitmap(intPtr2);
                DeleteObject(intPtr2);
                result = bitmap;
                bitmap = null;
            }
            else
            {
                result = null;
            }
            return result;
        }

        public static Bitmap GetInterPtrMap(IntPtr dC, System.Drawing.Rectangle rectangle)
        {
            //IntPtr dC = WindowsAPI.GetDC(WindowsAPI.GetDesktopWindow());
            IntPtr intPtr = WindowsAPI.CreateCompatibleDC(dC);
            IntPtr intPtr2 = WindowsAPI.CreateCompatibleBitmap(dC, rectangle.Width, rectangle.Height);
            Bitmap result;
            if (intPtr2 != IntPtr.Zero)
            {
                IntPtr hgdiobjBm = WindowsAPI.SelectObject(intPtr, intPtr2);
                WindowsAPI.BitBlt(intPtr, 0, 0, rectangle.Width, rectangle.Height, dC, rectangle.X, rectangle.Y, 13369376);
                //WindowsAPI.SelectObject(intPtr, hgdiobjBm);
                WindowsAPI.DeleteDC(intPtr);
                //WindowsAPI.ReleaseDC(WindowsAPI.GetDesktopWindow(), dC);
                Bitmap bitmap = Image.FromHbitmap(intPtr2);
                DeleteObject(intPtr2);
                result = bitmap;
                bitmap = null;
            }
            else
            {
                result = null;
            }
            return result;
        }
        /// <summary>
        /// 设置目标窗体大小，位置
        /// </summary>
        /// <param name="hWnd">目标句柄</param>
        /// <param name="x">目标窗体新位置X轴坐标</param>
        /// <param name="y">目标窗体新位置Y轴坐标</param>
        /// <param name="nWidth">目标窗体新宽度</param>
        /// <param name="nHeight">目标窗体新高度</param>
        /// <param name="BRePaint">是否刷新窗体</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool BRePaint);

        ///// <summary>
        ///// 截取桌面指定区域的bitmap
        ///// </summary>
        ///// <param name="rectangle"></param>
        ///// <returns></returns>
        //public static Bitmap GetRectBitmap(Rectangle rectangle)
        //{

        //}
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);
        [DllImport("gdi32.dll")]
        public static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);
        [DllImport("user32.dll")]
        public static extern void keybd_event(EnumKeyboardKey bVk, byte bScan, uint dwFlags, uint dwExtraInfo);
        [DllImport("user32.dll")]
        public static extern byte MapVirtualKey(EnumKeyboardKey wCode, int wMap);
        public static void KeyDown(EnumKeyboardKey key)
        {
            WindowsAPI.keybd_event(key, WindowsAPI.MapVirtualKey(key, 0), 0u, 0u);
        }
        public static void KeyUp(EnumKeyboardKey key)
        {
            WindowsAPI.keybd_event(key, WindowsAPI.MapVirtualKey(key, 0), 2u, 0u);
        }
        public static void KeyPress(EnumKeyboardKey key)
        {
            WindowsAPI.keybd_event(key, WindowsAPI.MapVirtualKey(key, 0), 0u, 0u);
            Thread.Sleep(0);
            WindowsAPI.keybd_event(key, WindowsAPI.MapVirtualKey(key, 0), 2u, 0u);
        }
        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        public static extern int PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);
        [DllImport("user32")]
        public static extern void mouse_event(EnumVirtualDeviceActionType dwFlags, int dx, int dy, int dwData, UIntPtr dwExtraInfo);
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, StringBuilder lpszClass, StringBuilder lpszWindow);
        [DllImport("user32.dll")]
        public static extern int EnumWindows(WindowsAPI.EnumWindowsProc ewp, int lParam);
        [DllImport("user32.dll")]
        public static extern int EnumChildWindows(IntPtr hWndParent, WindowsAPI.CallBack lpfn, int lParam);
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, out WindowsAPI.STRINGBUFFER text, int nMaxCount);
        public static int GetWindowText(IntPtr hWnd, out string text)
        {
            WindowsAPI.STRINGBUFFER sTRINGBUFFER = default(WindowsAPI.STRINGBUFFER);
            int windowText = WindowsAPI.GetWindowText(hWnd, out sTRINGBUFFER, 512);
            text = sTRINGBUFFER.szText;
            return windowText;
        }
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern int GetClassName(IntPtr hWnd, out WindowsAPI.STRINGBUFFER lpString, int nMaxCount);
        public static int GetClassName(IntPtr hWnd, out string lpString)
        {
            WindowsAPI.STRINGBUFFER sTRINGBUFFER = default(WindowsAPI.STRINGBUFFER);
            int className = WindowsAPI.GetClassName(hWnd, out sTRINGBUFFER, 512);
            lpString = sTRINGBUFFER.szText;
            return className;
        }
        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out WindowsAPI.Rect lpRect);
        public static bool GetWindowRect(IntPtr hWnd, out Rectangle lpRect)
        {
            WindowsAPI.Rect rect = default(WindowsAPI.Rect);
            WindowsAPI.GetWindowRect(hWnd, out rect);
            lpRect = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Down - rect.Top);
            return true;
        }
        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool IsWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hwnd, ref int lpdwProcessId);
        [DllImport("user32.dll")]
        public static extern int GetWindow(int hwnd, int wCmd);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(
         IntPtr hwnd
         );
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern int SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll")]
        public static extern int GetCursorPos(ref Point lpPoint);
        [DllImport("user32.dll", EntryPoint = "ShowWindow", CharSet = CharSet.Auto)]
        public static extern int ShowWindow(IntPtr hWnd, int nCmdShow);
        //切换窗体显示
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        //public static int GetCursorPos(ref Point lpPoint)
        //{
        //    POINT pOINT = new POINT() { x = 0, y = 0 };
        //    int cursorPos = WindowsAPI.GetCursorPos(ref pOINT);
        //    lpPoint.X = (int)pOINT.x;
        //    lpPoint.Y = (int)pOINT.y;
        //    return cursorPos;
        //}
        [DllImport("user32.dll")]
        public static extern bool SetCurorPos(int x, int y);
        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Point Point);
        [DllImport("user32.dll")]
        private static extern IntPtr GetCursor();
        [DllImport("user32.dll")]
        private static extern bool GetCursorInfo(out WindowsAPI.CURSORINFO pci);
        public static string GetCursorShape()
        {
            string result;
            try
            {
                WindowsAPI.CURSORINFO cURSORINFO = default(WindowsAPI.CURSORINFO);
                cURSORINFO.cbSize = Marshal.SizeOf(cURSORINFO);
                WindowsAPI.GetCursorInfo(out cURSORINFO);
                if (cURSORINFO.flags == 1)
                {
                    result = new Cursor(cURSORINFO.hCursor).ToString();
                }
                else
                {
                    result = "(Hidden)";
                }
            }
            catch (Exception)
            {
                result = "(Failure)";
            }
            return result;
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();
    }
    public enum EnumKeyboardKey : byte
    {
        Back = 8,
        Tab,
        Clear = 12,
        Return,
        ShiftLeft = 160,
        ControlLeft = 162,
        ShiftRight = 161,
        ControlRight = 163,
        AltLeft,
        AltRight,
        Menu = 18,
        Pause,
        Capital,
        Escape = 27,
        Space = 32,
        Prior,
        Next,
        End,
        Home,
        Left,
        Up,
        Right,
        Down,
        Select,
        Print,
        Execute,
        Snapshot,
        Insert,
        Delete,
        Help,
        D0,
        D1,
        D2,
        D3,
        D4,
        D5,
        D6,
        D7,
        D8,
        D9,
        A = 65,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        I,
        J,
        K,
        L,
        M,
        N,
        O,
        P,
        Q,
        R,
        S,
        T,
        U,
        V,
        W,
        X,
        Y,
        Z,
        LWindows,
        RWindows,
        Apps,
        NumPad0 = 96,
        NumPad1,
        NumPad2,
        NumPad3,
        NumPad4,
        NumPad5,
        NumPad6,
        NumPad7,
        NumPad8,
        NumPad9,
        Multiply,
        Add,
        Separator,
        Subtract,
        Decimal,
        Divide,
        F1,
        F2,
        F3,
        F4,
        F5,
        F6,
        F7,
        F8,
        F9,
        F10,
        F11,
        F12,
        F13,
        F14,
        F15,
        F16,
        F17,
        F18,
        F19,
        F20,
        F21,
        F22,
        F23,
        F24,
        NumLock = 144,
        Scroll
    }
    public enum EnumVirtualDeviceActionType
    {
        Delay = 33024,
        KeyDown = 33280,
        KeyUp = 33536,
        KeyPress = 30467,
        MoveTo = 32769,
        Move = 1,
        Scroll = 2048,
        LeftDown = 2,
        LeftUp = 4,
        LeftClick = 6,
        RightDown = 8,
        RightUp = 16,
        RightClick = 24,
        MiddleDown = 32,
        MiddleUp = 64,
        MiddleClick = 96
    }
}
