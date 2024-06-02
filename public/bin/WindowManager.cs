using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WindowsManager
{
    // 回调函数的委托
    public delegate bool CallBack(int hwnd, int lParam);

    class WinMan
    {
        // win32 API
        [DllImport("user32")]
        public static extern int EnumWindows(CallBack x, int y);

        [DllImport("user32.dll")]
        public static extern int GetWindowText(int hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern int GetClassName(int hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetWindowTextLength(int hWnd);

        [DllImport("user32.dll", EntryPoint = "IsWindowVisible")]
        public static extern bool IsWindowVisible(int hwnd);

        [DllImport("user32.dll")]
        public static extern int GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(int hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool ShowWindow(int hwnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(int hWnd, bool fAltTab);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(int hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        static extern bool DestroyWindow(int hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(int hWnd, int Msg, int wParam, StringBuilder lParam);

        [DllImport("user32.dll", EntryPoint = "SetWindowTextW", CharSet = CharSet.Unicode)]
        private static extern bool SetWindowText(int hWnd, string lpString);

        [DllImport("user32.dll", SetLastError = true)]
        [
            return: MarshalAs(UnmanagedType.Bool)
        ]
        public static extern bool SetLayeredWindowAttributes(int hwnd, int crKey, byte bAlpha, int dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetWindowLong(int hWnd, int nIndex, uint dwNewLong);
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(int hWnd, int nIndex);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern int GetParent(int hWnd);

        public const int GWL_EXSTYLE = (-20);
        public const int GWL_STYLE = (-16);
        //窗体样式 Window Styles 
        const uint WS_OVERLAPPED = 0;
        const uint WS_POPUP = 0x80000000;
        const uint WS_CHILD = 0x40000000;
        const uint WS_MINIMIZE = 0x20000000;
        const uint WS_VISIBLE = 0x10000000;
        const uint WS_DISABLED = 0x8000000;
        const uint WS_CLIPSIBLINGS = 0x4000000;
        const uint WS_CLIPCHILDREN = 0x2000000;
        const uint WS_MAXIMIZE = 0x1000000;
        const uint WS_CAPTION = 0xC00000;
        const uint WS_BORDER = 0x800000;
        const uint WS_DLGFRAME = 0x400000;
        const uint WS_VSCROLL = 0x200000;
        const uint WS_HSCROLL = 0x100000;
        const uint WS_SYSMENU = 0x80000;
        const uint WS_THICKFRAME = 0x40000;
        const uint WS_GROUP = 0x20000;
        const uint WS_TABSTOP = 0x10000;
        const uint WS_MINIMIZEBOX = 0x20000;
        const uint WS_MAXIMIZEBOX = 0x10000;
        const uint WS_TILED = WS_OVERLAPPED;
        const uint WS_ICONIC = WS_MINIMIZE;
        const uint WS_SIZEBOX = WS_THICKFRAME;

        // Extended Window Styles 
        const uint WS_EX_DLGMODALFRAME = 0x0001;
        const uint WS_EX_NOPARENTNOTIFY = 0x0004;
        const uint WS_EX_TOPMOST = 0x0008;
        const uint WS_EX_ACCEPTFILES = 0x0010;
        const uint WS_EX_TRANSPARENT = 0x0020;
        const uint WS_EX_MDICHILD = 0x0040;
        const uint WS_EX_TOOLWINDOW = 0x0080;
        const uint WS_EX_WINDOWEDGE = 0x0100;
        const uint WS_EX_CLIENTEDGE = 0x0200;
        const uint WS_EX_CONTEXTHELP = 0x0400;
        const uint WS_EX_RIGHT = 0x1000;
        const uint WS_EX_LEFT = 0x0000;
        const uint WS_EX_RTLREADING = 0x2000;
        const uint WS_EX_LTRREADING = 0x0000;
        const uint WS_EX_LEFTSCROLLBAR = 0x4000;
        const uint WS_EX_RIGHTSCROLLBAR = 0x0000;
        const uint WS_EX_CONTROLPARENT = 0x10000;
        const uint WS_EX_STATICEDGE = 0x20000;
        const uint WS_EX_APPWINDOW = 0x40000;
        const uint WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE);
        const uint WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST);
        const uint WS_EX_LAYERED = 0x00080000;
        const uint WS_EX_NOINHERITLAYOUT = 0x00100000;
        const uint WS_EX_LAYOUTRTL = 0x00400000;
        const uint WS_EX_COMPOSITED = 0x02000000;
        const uint WS_EX_NOACTIVATE = 0x08000000;

        [DllImport("user32.dll")]
        [
            return: MarshalAs(UnmanagedType.Bool)
        ]
        static extern bool IsIconic(int hWnd);

        [DllImport("user32.dll")]
        static extern bool IsZoomed(int hWnd);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(int hwnd, out int ID);

        [DllImport("user32.dll")]
        [
            return: MarshalAs(UnmanagedType.Bool)
        ]
        static extern bool GetWindowRect(int hWnd, ref RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left; //最左坐标
            public int Top; //最上坐标
            public int Right; //最右坐标
            public int Bottom; //最下坐标
        }

        [DllImport("user32.dll")]
        [
            return: MarshalAs(UnmanagedType.Bool)
        ]
        static extern bool EnableWindow(int hWnd, bool bEnable);

        [DllImport("user32.dll")]
        [
            return: MarshalAs(UnmanagedType.Bool)
        ]
        static extern bool IsWindowEnabled(int hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(int hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private extern static int FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetLayeredWindowAttributes(int hWnd, IntPtr crKey, out byte bAlpha, IntPtr dwFlags);

        // 获取窗口文本
        public static string GetText(int hWnd)
        {
            StringBuilder sb = new StringBuilder(260);
            GetWindowText(hWnd, sb, sb.Capacity);
            return sb.ToString();
        }

        public static string GetClass(int hWnd)
        {
            StringBuilder sb = new StringBuilder(50);
            GetClassName(hWnd, sb, sb.Capacity);
            return sb.ToString();
        }

        // 激活窗口
        public static void ActiveWindow(int hWnd)
        {
            SwitchToThisWindow(hWnd, true);
        }

        // 关闭窗口
        public static IntPtr CloseWindow(int hWnd)
        {
            return SendMessage(hWnd, 0x10, 0, null);
        }

        // 隐藏窗口
        // nCmdShow: 0 隐藏窗口, 1 还原窗口, 2 最大化, 3 最小化, 5 显示窗口
        public static bool HideWindow(int hWnd)
        {
            return ShowWindow(hWnd, 0);
        }

        // 显示窗口
        public static bool ShowHiddenWindow(int hWnd)
        {
            return ShowWindow(hWnd, 5);
        }

        // 还原窗口
        public static bool RestoreWindow(int hWnd)
        {
            return ShowWindow(hWnd, 1);
        }

        // 最小化窗口
        public static bool MinimizeWindow(int hWnd)
        {
            return ShowWindow(hWnd, 2);
        }

        // 最大化窗口
        public static bool MaximizeWindow(int hWnd)
        {
            return ShowWindow(hWnd, 3);
        }

        // 置顶窗口
        // hWndInsertAfter: -1 置顶, -2 取消置顶; uFlags: SWP_NOSIZE 0x0001, SWP_NOMOVE 0x0002
        public static bool TopMostWindow(int hWnd)
        {
            return SetWindowPos(hWnd, new IntPtr(-1), 0, 0, 0, 0, 0x0001 | 0x0002);
        }

        // 取消窗口置顶
        public static bool UnTopMostWindow(int hWnd)
        {
            return SetWindowPos(hWnd, new IntPtr(-2), 0, 0, 0, 0, 0x0001 | 0x0002);
        }

        // 设置透明度
        public static bool SetWindowOpacity(int hWnd, byte bAlpha)
        {
            uint Cur_STYLE = (uint)GetWindowLong(hWnd, GWL_EXSTYLE);
            SetWindowLong(hWnd, GWL_EXSTYLE, (Cur_STYLE | WS_EX_LAYERED));
            return SetLayeredWindowAttributes(hWnd, 0, bAlpha, 2);
        }
        // 更改标题
        public static bool SetTitle(int hWnd, String title)
        {
            return SetWindowText(hWnd, title);
        }
        // 窗口状态
        public static int GetWindowMinMax(int hWnd)
        {
            if (IsIconic(hWnd))
            {
                return -1;
            }
            else if (IsZoomed(hWnd))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static int GetWindowProcessId(int hWnd)
        {
            int ProcessID;
            GetWindowThreadProcessId(hWnd, out ProcessID);
            return ProcessID;
        }

        // 窗口进程路径
        public static string GetWindowPath(int hWnd)
        {
            int ProcessID;
            GetWindowThreadProcessId(hWnd, out ProcessID);
            Process WindowProcess = Process.GetProcessById(ProcessID);
            return WindowProcess.MainModule.FileName;
        }

        // 不含扩展名的进程名
        public static string GetProcessNameByPath(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        // 窗口坐标及长宽
        public static string GetWindowPos(int hWnd)
        {
            RECT fx = new RECT();
            GetWindowRect(hWnd, ref fx);
            int width = fx.Right - fx.Left;
            int height = fx.Bottom - fx.Top;
            int x = fx.Left;
            int y = fx.Top;
            return string.Format("\"width\": \"{0}\", \"height\": \"{1}\", \"x\": \"{2}\", \"y\":\"{3}\"", width, height, x, y);
        }

        // 列出ALTTAB窗口
        public static string WindowsInfo;
        public static string ListAltTabWindow()
        {
            CallBack myCallBack = new CallBack(WinMan.Report);
            WindowsInfo = "[";
            EnumWindows(myCallBack, 0);
            if (WindowsInfo != "[")
            {
                WindowsInfo = WindowsInfo.Substring(0, WindowsInfo.Length - 1).Replace("\\", "/");
            }
            WindowsInfo += "]";
            return WindowsInfo;
        }

        // 是否ALTTAB窗口
        public static bool IsAltTabWindow(int hWnd)
        {
            // 获取窗口风格
            // uint style = (uint) GetWindowLong (hWnd, GWL_STYLE);
            // if (((style | WS_VISIBLE) == style) && ((style | WS_POPUP) != style)) {
            if (IsWindowVisible(hWnd))
            {
                uint exstyle = (uint)GetWindowLong(hWnd, GWL_EXSTYLE);
                if ((exstyle | WS_EX_TOOLWINDOW) != exstyle)
                {
                    return true;
                }
            }
            return false;
        }

        // 枚举窗口回调函数
        public static bool Report(int hWnd, int lParam)
        {
            if (IsAltTabWindow(hWnd))
            {
                string classname = GetClass(hWnd);
                if (classname != "Windows.UI.Core.CoreWindow")
                {
                    string title = GetText(hWnd).Replace("\"", "\\\"");
                    if (!(classname == "ApplicationFrameWindow" && (title == "设置" || title == "Microsoft Store")))
                    {
                        string path;
                        string processname;
                        try
                        {
                            path = GetWindowPath(hWnd);
                            processname = GetProcessNameByPath(path);
                        }
                        catch (System.Exception)
                        {
                            path = "程序路径获取失败，需要管理员权限";
                            processname = "";
                        }
                        string WindowInfo = string.Format("\"id\": \"{0}\", \"title\": \"{1}\", \"path\": \"{2}\", \"class\": \"{3}\",\"process\": \"{4}\"", hWnd, title, path, classname, processname);
                        WindowsInfo += "{" + WindowInfo + "},";
                    }
                }
            }
            return true;
        }

        // 是否置顶窗口
        public static bool IsWindowTopMost(int hWnd)
        {
            uint exstyle = (uint)GetWindowLong(hWnd, GWL_EXSTYLE);
            if ((exstyle | WS_EX_TOPMOST) == exstyle)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // 获取透明度
        public static byte GetWindowOpacity(int hWnd)
        {
            IntPtr crKey = IntPtr.Zero;
            IntPtr dwFlags = IntPtr.Zero;
            byte Alpha;
            bool t = GetLayeredWindowAttributes(hWnd, crKey, out Alpha, dwFlags);
            if (t)
            {
                return Alpha;
            }
            else
            {
                return 255;
            }
        }

        // 获取窗口属性
        public static string GetWindowStatus(int hWnd)
        {
            int pid = GetWindowProcessId(hWnd);
            string path;
            try
            {
                path = GetWindowPath(hWnd);
            }
            catch (System.Exception)
            {
                path = "程序路径获取失败，需要管理员权限";
            }
            string title = GetText(hWnd).Replace("\"", "\\\""); ;
            string classname = GetClass(hWnd);
            int style = GetWindowLong(hWnd, GWL_STYLE);
            int exstyle = GetWindowLong(hWnd, GWL_EXSTYLE);
            string position = GetWindowPos(hWnd);
            int isVisible = IsWindowVisible(hWnd) ? 1 : 0;
            int isEnable = IsWindowEnabled(hWnd) ? 1 : 0;
            int IsTopMost = IsWindowTopMost(hWnd) ? 1 : 0;
            int MinMax = GetWindowMinMax(hWnd);
            byte opacity = GetWindowOpacity(hWnd);
            string status = string.Format("{{\"id\": {0}, \"pid\": {1}, \"path\": \"{2}\", \"title\": \"{3}\", \"class\": \"{4}\", \"style\": {5}, \"exstyle\": {6}, {7}, \"isVisible\": {8}, \"isEnable\": {9}, \"IsTopMost\": {10}, \"MinMax\": {11}, \"opacity\": {12}}}", hWnd, pid, path, title, classname, style, exstyle, position, isVisible, isEnable, IsTopMost, MinMax, opacity);
            return status.Replace("\\", "/");
        }

        // 获取图标
        static string getBase64Icon(string path)
        {
            string[] pathList = path.Replace("\\", "/").Split('|');
            string output = "[";
            foreach (string p in pathList)
            {
                Icon AssociatedIcon = SystemIcons.Application;
                AssociatedIcon = Icon.ExtractAssociatedIcon(p.Trim());
                ImageConverter converter = new ImageConverter();
                byte[] data = (byte[])converter.ConvertTo(AssociatedIcon.ToBitmap(), typeof(byte[]));
                string b64Ico = Convert.ToBase64String(data);
                output += string.Format("{{\"path\": \"{0}\", \"b64Ico\": \"{1}\"}},", p, b64Ico);
            }
            output = output.Substring(0, output.Length - 1);
            output += "]";
            return output;
        }

        // 针对uTools特殊处理
        public static bool MiniuTools()
        {
            int hWnd = FindWindow(null, "窗口管理");
            uint Cur_STYLE = (uint)GetWindowLong(hWnd, GWL_EXSTYLE);
            SetWindowLong(hWnd, GWL_EXSTYLE, (Cur_STYLE | WS_EX_TOOLWINDOW));
            int ScreenWidth = Screen.PrimaryScreen.Bounds.Width;
            RECT fx = new RECT();
            GetWindowRect(hWnd, ref fx);
            int width = fx.Right - fx.Left;
            int height = fx.Bottom - fx.Top;
            int x = fx.Left;
            int y = fx.Top;
            // 置顶，只保留部分的长度，半透明
            SetWindowPos(hWnd, new IntPtr(-1), ScreenWidth - width / 5, y, width, height, 0x0001);
            SetWindowOpacity(hWnd, 125);
            return true;
        }

        // 主函数
        static void Main(string[] args)
        {
            // WindowManager.exe windows
            if (args.Length == 1)
            {
                if (args[0] == "windows")
                {
                    string AltTabWindow = ListAltTabWindow();
                    Console.Write(AltTabWindow);
                }
                else if (args[0] == "MiniuTools")
                {
                    MiniuTools();
                }
            }
            // WindowManager.exe getIcons "C:/Windows/explorer.exe|C:/Windows/system32/cmd.exe|..."
            else if (args.Length == 2)
            {
                if (args[0] == "getIcons")
                {
                    string path = args[1];
                    Console.Write(getBase64Icon(path));
                }
            }
            // WindowManager.exe [active|show|hide...] id $hWnd
            // WindowManager.exe [title|trans] id $hWnd [$TitleText|$TransValue]
            else if (args.Length >= 3)
            {
                if (args[1] == "id")
                {
                    int id = int.Parse(args[2]);
                    switch (args[0])
                    {
                        case "activate":
                            ActiveWindow(id);
                            break;
                        case "show":
                            ShowHiddenWindow(id);
                            break;
                        case "hide":
                            HideWindow(id);
                            break;
                        case "max":
                            MaximizeWindow(id);
                            break;
                        case "min":
                            MinimizeWindow(id);
                            break;
                        case "restore":
                            RestoreWindow(id);
                            break;
                        case "close":
                            CloseWindow(id);
                            break;
                        case "top":
                            TopMostWindow(id);
                            break;
                        case "untop":
                            UnTopMostWindow(id);
                            break;
                        case "trans":
                            byte TransValue = byte.Parse(args[3]);
                            SetWindowOpacity(id, TransValue);
                            break;
                        case "title":
                            string TitleText = args[3];
                            SetWindowText(id, TitleText);
                            break;
                        case "minmax":
                            int minmax = GetWindowMinMax(id);
                            Console.Write(minmax);
                            break;
                        case "enable":
                            EnableWindow(id, true);
                            break;
                        case "disable":
                            EnableWindow(id, false);
                            break;
                        case "status":
                            string status = GetWindowStatus(id);
                            Console.Write(status);
                            break;
                        default:
                            Console.Write("WindowManager, Written By fofolee, 2020");
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("WindowManager, Written By fofolee, 2020");
            }

        }
    }
}
