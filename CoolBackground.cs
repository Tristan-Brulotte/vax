using System;
using System.Drawing;
using Console = Colorful.Console;

namespace Tin
{
    public class CoolBackground
    {
        public static void render(int len)
        {
            Random r = new Random();
            for (int i = 0; i < len; i++){
                int nr = r.Next(128);
                int ng = r.Next(128);
                int nb = r.Next(128);
                //Console.ForegroundColor = Color.FromArgb(0, nr + 128, ng + 128, nb + 128);

                int nx = r.Next(Console.WindowWidth);
                int ny = r.Next(Console.WindowHeight);
                Console.SetCursorPosition(nx, ny);
                Console.Write("*");
            }
        }
    }
}
