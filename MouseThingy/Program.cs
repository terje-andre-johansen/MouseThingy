namespace MouseThingy
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var mouseHooker = new MouseHooker();
            while (true)
            {
               mouseHooker.DoWork();
            }
        }
    }
}





