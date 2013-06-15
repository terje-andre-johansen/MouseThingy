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
                User32MouseDefinitions.POINT lpPoint;
                User32Import.GetCursorPos(out lpPoint);
                //bool success = User32.GetCursorPos(out lpPoint);
                // if (!success)

                return lpPoint;
            }
       
    }
}
