using System;

//for colours
using System.Drawing;
using Console = Colorful.Console;

namespace Tin
{
    internal class Program
    {
        static TextBufferObject buf;
        static TextWindowBufferObject window;
        static string titleText = "~ Tin 1.0 ~";
        static string descText = "A worse VI made by Tristan Brulotte!";
        static string errorMsg = "";
        static int winWMid = Console.WindowWidth / 2;
        static int winHMid = Console.WindowHeight / 2;

        static void Main(string[] args)
        {
            Console.Clear();

            buf = new TextBufferObject(@"c:/test/test.txt");
            CoolBackground.render(15);
            if (args.Length == 0)
            {
                errorMsg = "Error: no file selected! press any key to continue...";
            }

            if (args.Length == 1)
            {
                buf.filePath = args[0];
            }

            if (args.Length >= 2)
            {
                errorMsg = "Error: too many arguments!";
            }

            Console.SetCursorPosition(winWMid - (int)(titleText.Length/2), winHMid);
            Console.WriteLine(titleText);

            Console.SetCursorPosition(winWMid - (int)(descText.Length / 2), winHMid+1);
            Console.WriteLine(descText);

            Console.ForegroundColor = Color.PaleVioletRed;
            Console.SetCursorPosition(winWMid - (int)(errorMsg.Length / 2), winHMid-1);
            Console.WriteLine(errorMsg);
            Console.ForegroundColor = Color.White;

            if (!String.IsNullOrEmpty(errorMsg))
            {
                Console.ReadKey();
                Console.SetCursorPosition(0, 0);
                Console.Write("File path: ");
                buf.filePath = Console.ReadLine();
            }

            Console.Clear();

            window = new TextWindowBufferObject(buf, Console.WindowWidth, Console.WindowHeight, false);
            window.render();

            Console.ReadLine();
            Console.Clear();

        }
    }
}
