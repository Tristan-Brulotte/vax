using System;
using System.IO;

namespace Tin
{
    public class TextBufferObject
    {
        public string filePath { get; set; }
        public string[] textBufferLines { get; set; }
        public string textBuffer { get; set; }


        public TextBufferObject(string filePath)
        {
            textBuffer = File.ReadAllText(filePath);
            textBufferLines = File.ReadAllLines(filePath);
        }
    }
}
