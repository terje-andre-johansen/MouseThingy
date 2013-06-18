using System;

namespace MouseThingy
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var ctrGroup = new WindowControlGroups();
            new MouseHooker(ctrGroup);
            new KeyboardHooker(ctrGroup);
            while (true)
            {
             //   Console.WriteLine("t");
                MSG msg;
                User32MouseDefinitions.POINT p;
                User32Import.GetCursorPos(out p);
                var tmp = User32Import.WindowFromPoint(p);
                //var tmpres = User32Import.PeekMessage(out msg, IntPtr.Zero, 0, int.MaxValue, 1);
                Console.WriteLine("first" + (uint) WindowsMessages.KEYFIRST);
                Console.WriteLine("last "+(uint)WindowsMessages.KEYLAST);
                var result = User32Import.GetMessage(out msg, IntPtr.Zero, (uint)WindowsMessages.KEYFIRST, (uint)WindowsMessages.KEYLAST);
                Console.WriteLine(result);
                if (result > 0)
                {
                    User32Import.TranslateMessage(ref msg);
                    User32Import.DispatchMessage(ref msg);
                }
                else if (result == 0)
                {
                    Console.WriteLine("quit");
                    // quiting, remove hooks
                }
                else
                {
                    // error
                }


            }
        }

       

    }
}





