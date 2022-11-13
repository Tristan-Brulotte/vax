using System;
using System.IO;

/***
 *     __   ___   __  __        ___ ___ _  _  ___ _    ___   ___ ___ _    ___   _____ _____  _______   ___ ___ ___ _____ ___  ___ 
 *     \ \ / /_\  \ \/ /  ___  / __|_ _| \| |/ __| |  | __| | __|_ _| |  | __| |_   _| __\ \/ /_   _| | __|   \_ _|_   _/ _ \| _ \
 *      \ V / _ \  >  <  |___| \__ \| || .` | (_ | |__| _|  | _| | || |__| _|    | | | _| >  <  | |   | _|| |) | |  | || (_) |   /
 *       \_/_/ \_\/_/\_\       |___/___|_|\_|\___|____|___| |_| |___|____|___|   |_| |___/_/\_\ |_|   |___|___/___| |_| \___/|_|_\
 *     WRITTEN BY TRISTAN BRULOTTE - NOV 11, 2022         
 *     personal: build with dotnet build --nologo --self-contained true --output C:\Users\trist\Documents\code\cSharp\vax\bin\release                                                                                                             
 */

namespace vax
{
    public class Program
    {

        public static string filepath = "unnamed.txt";
        public static string[] textbuffer = { "" };
        public static int cx, cy, scroll;
        public static int lnumwidth;
        public static char modal;

        public static void Main(string[] args)
        {
            modal = 'i';
            cx = 0;
            cy = 0;
            scroll = 0;

            if (args.Length == 1)
            {
                filepath = args[0];
                if (File.Exists(filepath))
                {
                    textbuffer = File.ReadAllLines(filepath);
                }
            }

            Console.Clear();

            RenderTextBuffer();
            drawStatus();

            while (true)
            {
                lnumwidth = CalculateLineNumberWidth();
                Console.SetCursorPosition(cx + lnumwidth, cy);
                ProcessKeys(Console.ReadKey());
                RenderTextBuffer();
                drawStatus();
            }
        }

        public static void DisplayHelpText()
        {
            filepath = "HELPMENU_DO_NOT_SAVE";
            textbuffer = new string[] {
                " ~ vax 1.0 ~",
                "",
                "modes: ",
                "When you first boot vax, you will be placed into the \'i\' mode.",
                "This is known as \"INSERT\" mode. However when you hit escape, ",
                "you will be switched into \'c\' mode. ",
                "This is known as \"BIND\" mode, you can return to \'i\' mode",
                "by pressing \'i\' or \'a\' on your keyboard",
                "",
                "___________________________",
                "BIND MODE",
                "There are multiple binds you can use whilst in bind mode, these are:",
                "- Arrow keys -",
                "The arrow keys will perform their expected outcome by moving the cursor",
                "around the screen.",
                "",
                "- W and S keys -",
                "Both of these keybinds will save the file, however pressing \'w\' will",
                "also close vax.",
                "",
                "- Q key -",
                "The \'q\' key will ALWAYS exit vax unconditionally. Always save your files!"
            };
        }

        public static int CalculateLineNumberWidth()
        {
            return textbuffer.Length.ToString().Length + 3;
        }

        public static void drawStatus()
        {
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write($" vax | mode: {modal} | {filepath} | {cy + scroll + 1} : {cx}");
        }

        public static void ProcessKeys(ConsoleKeyInfo k)
        {
            if (modal == 'i')
            {
                switch (k.Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.RightArrow:

                        break;

                    case ConsoleKey.Escape:
                        modal = 'c';
                        Console.Clear();
                        break;

                    case ConsoleKey.Backspace:
                        Console.Clear();
                        if (cx > 0)
                        {
                            char[] tempc;
                            tempc = textbuffer[cy + scroll].ToCharArray();
                            tempc[cx - 1] = '\0';
                            textbuffer[cy + scroll] = new string(tempc);
                            cx--;
                        }
                        else if (cy > 0)
                        {
                            List<string> tempbufferTextLiness;
                            tempbufferTextLiness = textbuffer.ToList();
                            tempbufferTextLiness.RemoveAt(cy + scroll);
                            textbuffer = tempbufferTextLiness.ToArray();
                            if (cx == 0)
                            {
                                if(scroll > 0)
                                {
                                    scroll--;
                                    cx = textbuffer[(cy + scroll) - 1].Length;
                                    break;
                                }
                                cx = textbuffer[(cy + scroll) - 1].Length;
                                cy--;
                                break;
                            }

                            cy--;
                        }
                        break;

                    case ConsoleKey.Tab:
                        /* THIS CODE IS FUCKING DISASTEROUS, PREOCEED WITH CAUTION */
                        if (cx > 0 && cx < textbuffer[cy + scroll].Length)
                        { //if somewhere between start and end
                            char[] templine1 = textbuffer[cy + scroll].Substring(0, cx).ToCharArray();
                            char[] templine2 = textbuffer[cy + scroll].Substring(cx).ToCharArray();
                            string templine3 = new string(new string(templine1) + "    " + new string(templine2));

                            textbuffer[cy + scroll] = templine3;
                        }
                        else if (cx == 0)
                        { //if at beginning of line
                            char[] templine4 = textbuffer[cy + scroll].ToCharArray();
                            string templine5 = new string("    " + new string(templine4));

                            textbuffer[cy + scroll] = templine5;
                        }
                        else if (cx == textbuffer[cy + scroll].Length)
                        { //if at end of line
                            char[] templine6 = textbuffer[cy + scroll].ToCharArray();
                            string templine7 = new string(new string(templine6) + "    ");

                            textbuffer[cy + scroll] = templine7;
                        }

                        cx += 4;
                        break;

                    case ConsoleKey.Enter:
                        if (cx < textbuffer[cy + scroll].Length)
                        {
                            List<string> tempbufferTextLines;
                            tempbufferTextLines = textbuffer.ToList();
                            tempbufferTextLines.Insert(cy + 1 + scroll, textbuffer[cy + scroll].Substring(cx, textbuffer[cy + scroll].Length - cx));
                            textbuffer = tempbufferTextLines.ToArray();
                        }
                        else
                        {
                            List<string> tempbufferTextLines;
                            tempbufferTextLines = textbuffer.ToList();
                            tempbufferTextLines.Insert(cy + scroll + 1, "");
                            textbuffer = tempbufferTextLines.ToArray();
                        }
                        Console.Clear();
                        cx = 0;

                        if (cy == Console.WindowHeight - 3)
                        {
                            scroll++;
                            break;
                        }
                        cy++;
                        break;

                    default:
                        char[] temp;
                        temp = textbuffer[cy + scroll].ToCharArray();
                        if (cx > temp.Length - 1)
                        {
                            Array.Resize(ref temp, temp.Length + 1);
                        }
                        temp[cx] = k.KeyChar;
                        string temps = new string(temp);
                        textbuffer[cy + scroll] = temps;
                        cx++;
                        break;
                }

            }
            else if (modal == 'c')
            {

                switch (k.Key)
                {
                    case ConsoleKey.H:
                        DisplayHelpText();
                        break;
                    case ConsoleKey.I:
                    case ConsoleKey.A:
                        modal = 'i';
                        Console.Clear();
                        break;

                    case ConsoleKey.UpArrow:
                        if (cy == 0)
                        {
                            if (scroll == 0)
                            {
                                break;
                            }
                            scroll--;
                            Console.Clear();
                            break;
                        }
                        cy--;
                        break;

                    case ConsoleKey.DownArrow:
                        if (cy == Console.WindowHeight - 3)
                        {
                            if (scroll >= textbuffer.Length - Console.WindowHeight)
                            {
                                break;
                            }
                            scroll++;
                            Console.Clear();
                            break;
                        }
                        cy++;
                        break;

                    case ConsoleKey.LeftArrow:
                        cx--;
                        break;

                    case ConsoleKey.RightArrow:
                        cx++;
                        break;

                    case ConsoleKey.W:
                        File.WriteAllLines(filepath, textbuffer);
                        Environment.Exit(0);
                        break;

                    case ConsoleKey.S:
                        File.WriteAllLines(filepath, textbuffer);
                        break;

                    case ConsoleKey.Q:
                        Environment.Exit(0);
                        break;

                    default:
                        break;
                }
            }
        }

        public static void RenderTextBuffer()
        {
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < textbuffer.Length; i++)
            {
                if (i < Console.WindowHeight - 2)
                {
                    Console.SetCursorPosition(0, i);
                    if (textbuffer[i].Length < Console.BufferWidth)
                    {
                        Console.Write((i + 1 + scroll));
                        for (int l = 0; l < digit(textbuffer.Length) - digit(i); l++)
                        {
                            Console.Write(" ");
                        }

                        Console.Write("   ");

                        //int datatypestartloc = 0, datatypeendloc = 0;
                        //syntax highlighting
                        int ii = 0;
                        foreach (char c in textbuffer[i + scroll])
                        {

                            foreach (char textContainer in textContainers)
                            {
                                if (textbuffer[i + scroll].StartsWith("#"))
                                {
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    break;
                                }

                                if (textbuffer[i + scroll].Contains(textContainer))
                                {
                                    if (ii >= textbuffer[i + scroll].IndexOf(textContainer) && ii < (textbuffer[i + scroll].IndexOf(textContainer, textbuffer[i + scroll].IndexOf(textContainer) + 1)) + 1)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Green;
                                    }
                                }
                            }
                            ////////////////////////////
                            foreach (char bracket in brackets)
                            {
                                if (textbuffer[i + scroll].StartsWith("#"))
                                {
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    break;
                                }

                                if (textbuffer[i + scroll].Contains(bracket))
                                {
                                    if (ii == textbuffer[i + scroll].IndexOf(bracket))
                                    {
                                        Console.ForegroundColor = ConsoleColor.DarkGray;
                                    }
                                }
                            }
                            ////////////////////////////
                            foreach (string ident in identifiers)
                            {
                                if (textbuffer[i + scroll].StartsWith("#"))
                                {
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    break;
                                }

                                if (textbuffer[i + scroll].Contains(ident))
                                {
                                    if (ii >= textbuffer[i + scroll].IndexOf(ident) && ii < textbuffer[i + scroll].IndexOf(ident) + ident.Length)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Blue;
                                    }
                                }
                            }
                            ////////////////////////////
                            foreach (string type in dataTypes)
                            {
                                if (textbuffer[i + scroll].StartsWith("#"))
                                {
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    break;
                                }

                                if (textbuffer[i + scroll].Contains(type))
                                {
                                    if (ii >= textbuffer[i + scroll].IndexOf(type) && ii < textbuffer[i + scroll].IndexOf(type) + type.Length)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Blue;
                                    }
                                }
                            }

                            Console.Write(c);
                            ii++;
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                }
                /////////
            }
        }

        public static int digit(int i)
        {
            return i.ToString().Length;
        }

        public static string[] dataTypes = {
            //else/logic
            "object ",
            "bool ",
            "struct ",
            //numerical
            "byte ",
            "decimal ",
            "int ",
            "double ",
            "float ",
            "long ",
            "short ",
            //alphanumeric
            "string ",
            "char ",

            //arrays ([)
                
            //else/logic
            "object[",
            "bool[",
            "struct[",
            //numerical
            "byte[",
            "decimal[",
            "int[",
            "double[",
            "float[",
            "long[",
            "short[",
            //alphanumeric
            "string[",
            "char[",

            // (*)

            //else/logic
            "object*",
            "bool*",
            "struct*",
            //numerical
            "byte*",
            "decimal*",
            "int*",
            "double*",
            "float*",
            "long*",
            "short*",
            //alphanumeric
            "string*",
            "char*",

            //pointers
            //else/logic
            "&object ",
            "&bool ",
            "&struct ",
            //numerical
            "&byte ",
            "&decimal ",
            "&int ",
            "&double ",
            "&float ",
            "&long ",
            "&short ",
            //alphanumeric
            "&string ",
            "&char ",

        };

        public static char[] textContainers = {
            '\"',
            '\'',
        };

        public static char[] brackets = {
            '{',
            '}',
            '(',
            ')',
            '[',
            ']',
            '<',
            '>'
        };

        public static string[] identifiers = {
            "namespace ",
            "using ",
            "class ",
            "public ",
            "private ",
            "static ",
            "readonly ",
            "void ",
            "signed ",
            "unsigned ",
            "return "
        };
    }
}