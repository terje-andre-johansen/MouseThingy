using System;
using System.Runtime.InteropServices;

namespace MouseThingy
{
    public class MouseHooker
    {

       
        private static User32Import.LowLevelMouseProc _myCallbackDelegate = null;
        public MouseHooker()
        {
            _myCallbackDelegate = CallbackFunction;
            User32Import.SetWindowsHookEx(User32Definitions.HookType.WH_MOUSE_LL, _myCallbackDelegate, IntPtr.Zero, 0);
            
        }

        public void DoWork()
        {
            User32Definitions.MSG msg;
            User32Definitions.POINT p;
            User32Import.GetCursorPos(out p);
            var tmp = User32Import.WindowFromPoint(p);

            if (User32Import.GetMessage(out msg, tmp, 0, 0) > 0)
            {
                User32Import.TranslateMessage(ref msg);
                User32Import.DispatchMessage(ref msg);
            }
        }


        private static bool _leftDown = false;
        private static bool _rightDown = false;
        static Point _prevpoint = null;
        public static IntPtr CallbackFunction(int code, IntPtr wParam, IntPtr lParam)
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

                            var rect = new User32Definitions.RECT();


                            var rootWindow = User32Import.GetAncestor(tmp, (uint)User32Import.GetAncestorFlags.GetRoot);
                            User32Import.GetWindowRect(rootWindow, ref rect);
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

                            var rect = new User32Definitions.RECT();
                            var rootWindow = User32Import.GetAncestor(tmp, (uint)User32Import.GetAncestorFlags.GetRoot);
                            User32Import.GetWindowRect(rootWindow, ref rect);


                            Console.WriteLine("top " + rect.top);
                            Console.WriteLine("bottom " + rect.bottom);
                            Console.WriteLine("left " + rect.left);
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
                    var rect = new User32Definitions.RECT();

                    User32Import.GetWindowRect(tmp, ref rect);


                    Console.WriteLine(" px " + p.X + " py " + p.Y);
                    Console.WriteLine(" rLeft " + rect.left + " rTop " + rect.top);


                    //MoveWindow(tmp, rect.left + 1, rect.top + 1, rect.right - rect.left, rect.bottom - rect.top, true);

                    if (_prevpoint != null)
                    {
                        var deltaX = _prevpoint.X - p.X;
                        var deltaY = _prevpoint.Y - p.Y;


                        var rootWindow = User32Import.GetAncestor(tmp, (uint)User32Import.GetAncestorFlags.GetRoot);
                        User32Import.GetWindowRect(rootWindow, ref rect);

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


    }
}
