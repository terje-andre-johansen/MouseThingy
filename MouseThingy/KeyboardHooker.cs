using System;
using System.Runtime.InteropServices;

namespace MouseThingy
{
    public class KeyboardHooker
    {
        private WindowControlGroups _groups;
        private static User32Import.LowLevelKeyboardProc _myCallbackDelegate = null;
        public KeyboardHooker(WindowControlGroups groups)
        {
            _myCallbackDelegate = CallbackFunction;
            _groups = groups;
            User32Import.SetWindowsHookEx(User32MouseDefinitions.HookType.WH_KEYBOARD_LL, _myCallbackDelegate, IntPtr.Zero, 0);

        }


        public static bool LeftCtrDown = false;
        public static bool AltDown = false;

        public IntPtr CallbackFunction(int code, IntPtr wParam, IntPtr lParam)
        {

            //var messageStruct = (WindowsMessages)Marshal.PtrToStructure(wParam, typeof(WindowsMessages));
            var hookStruct = (KEYBDINPUT)Marshal.PtrToStructure(lParam, typeof(KEYBDINPUT));

            //var tmp = (KEYBDINPUT) lParam;

            //Marshal.StructureToPtr();


            if (code == 0)
            {

                var wParamAsHex = wParam.ToString("X");
                //Console.WriteLine(" --- ");
                Console.WriteLine("wParm" + wParamAsHex);
                Console.WriteLine("vks " + hookStruct.wVk);


                switch (wParamAsHex)
                {
                    case "100": // keydown
                        //Console.WriteLine("down");

                        switch (hookStruct.wVk)
                        {

                            case VirtualKeyShort.LCONTROL:
                                Console.WriteLine("ctrl down");
                                LeftCtrDown = true;
                                break;

                            case VirtualKeyShort.LMENU:
                                Console.WriteLine("alt down");
                                AltDown = true;
                                break;


                        }
                        break;
                    case "104": // system key down
                        switch (hookStruct.wVk)
                        {
                            case VirtualKeyShort.KEY_1:
                                Console.WriteLine("Key one and alt down");
                                _groups.SetCurrentControlGroup(VirtualKeyShort.KEY_1.ToString());
                                _groups.ShowControlGroup(VirtualKeyShort.KEY_1.ToString());
                                
                                break;
                            case VirtualKeyShort.KEY_2:
                                Console.WriteLine("Key two and alt down");
                                _groups.SetCurrentControlGroup(VirtualKeyShort.KEY_2.ToString());
                                _groups.ShowControlGroup(VirtualKeyShort.KEY_2.ToString());
                                break;
                            case VirtualKeyShort.KEY_3:
                                Console.WriteLine("Key three and alt down");
                                _groups.SetCurrentControlGroup(VirtualKeyShort.KEY_3.ToString());
                                _groups.ShowControlGroup(VirtualKeyShort.KEY_3.ToString());
                                break;
                            case VirtualKeyShort.KEY_4:
                                Console.WriteLine("Key four and alt down");
                                _groups.SetCurrentControlGroup(VirtualKeyShort.KEY_4.ToString());
                                _groups.ShowControlGroup(VirtualKeyShort.KEY_4.ToString());
                                break;
                            case VirtualKeyShort.LMENU:
                                Console.WriteLine("alt down");
                                AltDown = true;
                                break;
                        }

                        break;
                    case "101": // keyup
                        //Console.WriteLine("up");
                        switch (hookStruct.wVk)
                        {
                            case VirtualKeyShort.LCONTROL:
                                Console.WriteLine("ctrl up");
                                LeftCtrDown = false;
                                break;
                            case VirtualKeyShort.LMENU:
                                AltDown = false;
                                Console.WriteLine("alt up");
                                break;
                        }
                        break;
                }

                //Console.WriteLine("wvk " + hookStruct.wVk);
                //Console.WriteLine("wScan " + hookStruct.wScan);
                //Console.WriteLine("flags " + hookStruct.dwFlags);
                //Console.WriteLine("extra " + hookStruct.dwExtraInfo);
                //Console.WriteLine("time " + hookStruct.time);
            }

            return User32Import.CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
        }
    }
}
