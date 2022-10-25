using System;

//Colours
using System.Drawing;
using Console = Colorful.Console;

namespace Tin
{
    public class TextWindowBufferObject
    {
        public TextBufferObject buffer;
        public int windowWidth;
        public int windowHeight;
        public bool showLineNumbers;
        public Color bgCol = Color.FromArgb(0, 36, 38, 37);
        public Color fgCol = Color.FromArgb(0, 240, 255, 240);

        public TextWindowBufferObject(TextBufferObject buffer, int windowWidth, int windowHeight, bool showLineNumbers)
        {
            this.buffer = buffer;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            this.showLineNumbers = showLineNumbers;
        }

        private void wipeBg()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            for(int y = 0; y < windowHeight; y++)
            {
                for(int x = 0; x < windowWidth; x++)
                {
                    Console.Write(" ");
                }
                Console.Write("\n");
            }
            Console.Clear();
        }

        public void render()
        {
            Console.ForegroundColor = fgCol;
            Console.BackgroundColor = bgCol;
            wipeBg();
            Console.SetCursorPosition(0, 0);

            foreach(string ln in buffer.textBufferLines)
            {
                Console.WriteLine(ln);
            }
        }
    }
}
