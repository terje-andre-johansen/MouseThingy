using System;
using System.Runtime.InteropServices;

namespace MouseThingy
{
    public static class User32Import
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hWnd);

        /// <summary>
        /// Retrieves the handle to the ancestor of the specified window. 
        /// </summary>
        /// <param name="hwnd">A handle to the window whose ancestor is to be retrieved. 
        /// If this parameter is the desktop window, the function returns NULL. </param>
        /// <param name="flags">The ancestor to be retrieved.</param>
        /// <returns>The return value is the handle to the ancestor window.</returns>
        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern IntPtr GetAncestor(IntPtr hwnd, uint flags);
        public enum GetAncestorFlags
        {
            /// <summary>
            /// Retrieves the parent window. This does not include the owner, as it does with the GetParent function. 
            /// </summary>
            GetParent = 1,
            /// <summary>
            /// Retrieves the root window by walking the chain of parent windows.
            /// </summary>
            GetRoot = 2,
            /// <summary>
            /// Retrieves the owned root window by walking the chain of parent and owner windows returned by GetParent. 
            /// </summary>
            GetRootOwner = 3
        }



        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(User32Definitions.HookType hookType, LowLevelMouseProc lpfn, IntPtr hMod, int dwThreadId);
        public delegate IntPtr LowLevelMouseProc(int code, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
        /// <summary>
        /// Retrieves the cursor's position, in screen coordinates.
        /// </summary>
        /// <see>See MSDN documentation for further information.</see>
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out User32Definitions.POINT lpPoint);
        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(User32Definitions.POINT Point);


        /// <summary>
        /// The MoveWindow function changes the position and dimensions of the specified window. For a top-level window, the position and dimensions are relative to the upper-left corner of the screen. For a child window, they are relative to the upper-left corner of the parent window's client area.
        /// </summary>
        /// <param name="hWnd">Handle to the window.</param>
        /// <param name="X">Specifies the new position of the left side of the window.</param>
        /// <param name="Y">Specifies the new position of the top of the window.</param>
        /// <param name="nWidth">Specifies the new width of the window.</param>
        /// <param name="nHeight">Specifies the new height of the window.</param>
        /// <param name="bRepaint">Specifies whether the window is to be repainted. If this parameter is TRUE, the window receives a message. If the parameter is FALSE, no repainting of any kind occurs. This applies to the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent window uncovered as a result of moving a child window.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// <para>If the function fails, the return value is zero. To get extended error information, call GetLastError.</para></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
        internal static extern bool GetWindowRect(IntPtr hWnd, ref User32Definitions.RECT rect);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetWindowText(IntPtr hwnd, String lpString);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, UIntPtr dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern sbyte GetMessage(out User32Definitions.MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);
        [DllImport("user32.dll")]
        public  static extern bool TranslateMessage([In] ref User32Definitions.MSG lpMsg);
        [DllImport("user32.dll")]
        public static extern IntPtr DispatchMessage([In] ref User32Definitions.MSG lpmsg);

        [DllImport("user32.dll")]
        static extern IntPtr GetProp(IntPtr hWnd, string lpString);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetProp(IntPtr hWnd, string lpString, IntPtr hData);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern IntPtr RemoveProp(IntPtr hWnd, string lpString);
       
    
    }
}
