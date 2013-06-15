using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MouseThingy
{
    internal class Program
    {

      

       
       

        private static User32Import.LowLevelMouseProc myCallbackDelegate = null;


        private const int WH_MOUSEMOVE = 0x0001;

       

        // For Windows Mobile, replace user32.dll with coredll.dll 
        

        private static bool _leftDown = false;
        private static bool _rightDown = false;
        static Point _prevpoint = null;
        private static IntPtr CallbackFunction(int code, IntPtr wParam, IntPtr lParam)
        {
            Console.WriteLine(code);
            if (code == 0)
            {
                var wParamAsInt = wParam.ToString("X");
                Console.WriteLine(wParamAsInt);



                //Console.WriteLine("flags "+hookStruct.flags);
                //Console.WriteLine("extra " + hookStruct.dwExtraInfo);
                //Console.WriteLine("ptX " + hookStruct.pt.X + "ptY "+ hookStruct.pt.Y);
                //Console.WriteLine("mdata " + hookStruct.mouseData.ToString("X"));
                //Console.WriteLine("time " + new DateTime(hookStruct.time));



                User32Definitions.MSLLHOOKSTRUCT hookStruct = (User32Definitions.MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(User32Definitions.MSLLHOOKSTRUCT));
                switch (wParamAsInt)
                {
                    case "200":
                        Console.WriteLine("mousemove");
                        break;
                    case "201":
                        Console.WriteLine("left");
                        _leftDown = true;
                        break;
                    case "202":
                        Console.WriteLine("left up");
                        _leftDown = false;
                        _prevpoint = null;

                        //if (_rightDown)
                        //{
                        //    return IntPtr.Subtract(IntPtr.Zero, -1);
                        //}

                        break;
                    case "204":
                        Console.WriteLine("right down");
                        _rightDown = true;
                        break;
                    case "205":
                        Console.WriteLine("right up");
                        _rightDown = false;
                        _prevpoint = null;

                        //if (_leftDown)
                        //{
                        //    return IntPtr.Subtract(IntPtr.Zero, -1);
                        //}

                        break;
                    case "20A": // wheel
                        Console.WriteLine("mdata " + hookStruct.mouseData.ToString("X"));

                        if (_rightDown)
                        {
                            User32Definitions.POINT p = hookStruct.pt;
                            var tmp = User32Import.WindowFromPoint(p);
                            
                            var rect = new RECT();


                            var rootWindow = User32Import.GetAncestor(tmp, (uint)User32Import.GetAncestorFlags.GetRoot);
                            GetWindowRect(rootWindow, ref rect);
                            Console.WriteLine("top " + rect.top);
                            Console.WriteLine("bottom " + rect.bottom);
                            Console.WriteLine("left " + rect.left);
                            Console.WriteLine("right " + rect.right);


                            switch (hookStruct.mouseData.ToString("X"))
                            {
                                case "780000": // Wheel up
                                    User32Import.MoveWindow(rootWindow, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top + 40, true);

                                    break;
                                case "FF880000": // Wheel down
                                    User32Import.MoveWindow(rootWindow, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top - 40, true);


                                    break;
                                default:
                                    break;
                            }
                        }

                        if (_leftDown)
                        {
                            User32Definitions.POINT p = hookStruct.pt;
                            var tmp = User32Import.WindowFromPoint(p);

                            var rect = new RECT();
                            var rootWindow = User32Import.GetAncestor(tmp, (uint)User32Import.GetAncestorFlags.GetRoot);
                            GetWindowRect(rootWindow, ref rect);


                            Console.WriteLine("top " + rect.top);
                            Console.WriteLine("bottom " + rect.bottom);
                            Console.WriteLine("left "+rect.left);
                            Console.WriteLine("right " + rect.right);


                          
                            switch (hookStruct.mouseData.ToString("X"))
                            {
                                case "780000": // Wheel up
                                    User32Import.MoveWindow(rootWindow, rect.left, rect.top, rect.right - rect.left + 40, rect.bottom - rect.top, true);

                                    break;
                                case "FF880000": // Wheel down
                                    User32Import.MoveWindow(rootWindow, rect.left, rect.top, rect.right - rect.left - 40, rect.bottom - rect.top, true);


                                    break;
                                default:
                                    break;
                            }
                        }

                        break;

                    default:
                        break;

                }

                if (_leftDown && _rightDown)
                {
                    Console.WriteLine("both down");

                    User32Definitions.POINT p = hookStruct.pt;
                    //GetCursorPos(out p);
                    var tmp = User32Import.WindowFromPoint(p);
                    var rect = new RECT();

                    GetWindowRect(tmp, ref rect);


                    Console.WriteLine(" px " + p.X + " py " + p.Y);
                    Console.WriteLine(" rLeft " + rect.left + " rTop " + rect.top);


                    //MoveWindow(tmp, rect.left + 1, rect.top + 1, rect.right - rect.left, rect.bottom - rect.top, true);

                    if (_prevpoint != null)
                    {
                        var deltaX = _prevpoint.X - p.X;
                        var deltaY = _prevpoint.Y - p.Y;


                        var rootWindow = User32Import.GetAncestor(tmp, (uint)User32Import.GetAncestorFlags.GetRoot);
                        GetWindowRect(rootWindow, ref rect);

                        //SetForegroundWindow(rootWindow);

                        User32Import.MoveWindow(rootWindow, rect.left - deltaX, rect.top - deltaY, rect.right - rect.left, rect.bottom - rect.top, true);
                    }


                    _prevpoint = p;



                }

                //Console.WriteLine("hex wParam {0:X}", wParam.ToInt32());
                //Console.WriteLine("binary wParam {0}", Convert.ToString(wParam.ToInt32(), 2));

                //Console.WriteLine("hex lParam {0:X}", lParam.ToInt32());
                //Console.WriteLine("binary lParam {0}", Convert.ToString(lParam.ToInt32(), 2));
            }
            //Console.WriteLine("wParam" + wParam);
            //Console.WriteLine("lParam" + lParam);

            return User32Import.CallNextHookEx(IntPtr.Zero, code, wParam, lParam);

        }

       


        //private static void StartTracing()
        //{
        //    POINT p;
        //    GetCursorPos(out p);
        //    var tmp = WindowFromPoint(p);
        //    if (IsWindow(tmp))
        //    {
        //        //Console.WriteLine("tmp");
        //        if (GetProp(tmp, "mousehook\n") != IntPtr.Zero)
        //        {
        //            Console.WriteLine("tmp2");
        //            return;
        //        }

        //        if (SetProp(tmp, "mousehook\n", IntPtr.Zero))
        //        {
        //            //Console.WriteLine("tmp3");

        //        }
        //        else
        //        {
        //            Console.WriteLine(Marshal.GetLastWin32Error());
        //        }
        //    }

        //}

        [DllImport("user32.dll")]
        static extern IntPtr GetProp(IntPtr hWnd, string lpString);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetProp(IntPtr hWnd, string lpString, IntPtr hData);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern IntPtr RemoveProp(IntPtr hWnd, string lpString);
        [DllImport("user32.dll")]
        static extern sbyte GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);
        [DllImport("user32.dll")]
        private static extern bool TranslateMessage([In] ref MSG lpMsg);
        [DllImport("user32.dll")]
        static extern IntPtr DispatchMessage([In] ref MSG lpmsg);
        [StructLayout(LayoutKind.Sequential)]
        public struct MSG
        {
            public IntPtr hwnd;
            public UInt32 message;
            public IntPtr wParam;
            public IntPtr lParam;
            public UInt32 time;
            public User32Definitions.POINT pt;
        }



        private static void Main(string[] args)
        {
            myCallbackDelegate = new User32Import.LowLevelMouseProc(CallbackFunction);
            User32Import.SetWindowsHookEx(User32Definitions.HookType.WH_MOUSE_LL, myCallbackDelegate, IntPtr.Zero, 0);
            //Thread.CurrentThread.ManagedThreadId);//AppDomain.GetCurrentThreadId());
            var i = 0;

            MSG msg;


            while (true)
            {

                User32Definitions.POINT p;
                User32Import.GetCursorPos(out p);
                var tmp = User32Import.WindowFromPoint(p);

                if (GetMessage(out msg, tmp, 0, 0) > 0)
                {
                    TranslateMessage(ref msg);
                    DispatchMessage(ref msg);
                }

                i++;
                var currentX = (i * 100) % 1920;
                var currentY = (i * 100) % 1200;
                //Thread.Sleep(500);

                //Console.WriteLine("tmp");


                //Console.WriteLine(tmp);
                //Console.WriteLine(Point.GetCursorPosition().X + " " + Point.GetCursorPosition().Y);



                //StartTracing();

                //Console.WriteLine("top: "+rect.top + "bottom: "+rect.bottom+"right "+rect.right+" left: "+ rect.left);
                //MoveWindow(tmp, rect.left + 1, rect.top + 1, rect.right - rect.left, rect.bottom - rect.top, true);
                //SetWindowText(tmp, "Test");

            }
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetWindowText(IntPtr hwnd, String lpString);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
        internal static extern bool GetWindowRect(IntPtr hWnd, ref RECT rect);

        internal struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

       


      



        /// <summary>
        /// Struct representing a point.
        /// </summary>
       


        //[StructLayout(LayoutKind.Sequential)]
        //public struct INPUT
        //{
        //    internal uint type;
        //    internal InputUnion U;
        //    internal static int Size
        //    {
        //        get { return Marshal.SizeOf(typeof(INPUT)); }
        //    }
        //}

        //[StructLayout(LayoutKind.Explicit)]
        //internal struct InputUnion
        //{
        //    [FieldOffset(0)]
        //    internal MOUSEINPUT mi;
        //    //[FieldOffset(0)]
        //    //internal KEYBDINPUT ki;
        //    //[FieldOffset(0)]
        //    //internal HARDWAREINPUT hi;
        //}

        //[DllImport("user32.dll", SetLastError=true)]
        //static extern void MouseProc(long nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, UIntPtr dwExtraInfo);


      


       

       



    }

}





