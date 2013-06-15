using System;
using System.Runtime.InteropServices;

namespace MouseThingy
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MSG
    {
        public IntPtr hwnd;
        public UInt32 message;
        public IntPtr wParam;
        public IntPtr lParam;
        public UInt32 time;
        public User32MouseDefinitions.POINT pt;
    }

    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public override string ToString()
        {
            return string.Format("left {0} top {1} right {2} bottom {3}", left, top, right, bottom);
        }

    }
}
