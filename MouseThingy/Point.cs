using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseThingy
{
   
        public class Point
        {
            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X;
            public int Y;

            public static Point GetCursorPosition()
            {
                User32Definitions.POINT lpPoint;
                User32Import.GetCursorPos(out lpPoint);
                //bool success = User32.GetCursorPos(out lpPoint);
                // if (!success)

                return lpPoint;
            }
       
    }
}
