using System;
using System.Text;

namespace Microsoft.SmallBasic.Library
{
   [SmallBasicType]
   public static class TextWindow
   {
      public static Primitive BackgroundColor
      {
         get
         {
            return Console.BackgroundColor.ToString();
         }
         set
         {
            try
            {
               Console.BackgroundColor = 
                  (ConsoleColor)Enum.Parse(typeof(ConsoleColor), value, true);
            }
            catch
            {
            }
         }
      }

      public static Primitive CursorLeft
      {
         get
         {
            return new Primitive(Console.CursorLeft);
         }
         set
         {
            Console.CursorLeft = value;
         }
      }

      public static Primitive CursorTop
      {
         get
         {
            return new Primitive(Console.CursorTop);
         }
         set
         {
            Console.CursorTop = value;
         }
      }

      public static Primitive ForegroundColor
      {
         get
         {
            return Console.ForegroundColor.ToString();
         }
         set
         {
            try
            {
               Console.ForegroundColor = 
                  (ConsoleColor)Enum.Parse(typeof(ConsoleColor), value, true);
            }
            catch
            {
            }
         }
      }

      public static Primitive Title
      {
         get
         {
            return new Primitive(Console.Title);
         }
         set
         {
            Console.Title = value;
         }
      }

      public static void Clear()
      {
          Console.Clear();
      }


      public static void Pause()
      {
         Console.WriteLine("Press any key to continue...");
         Console.ReadKey(true);
      }

      public static void PauseWithoutMessage()
      {
         Console.ReadKey(true);
      }

      public static Primitive Read()
      {
         return new Primitive(Console.ReadLine());
      }

      public static Primitive ReadNumber()
      {
         ConsoleKeyInfo consoleKeyInfo;
         StringBuilder builder = new StringBuilder();
         bool hasDecimalSeparator = false;
         int num = 0;
         do
         {
         NextChar:
            consoleKeyInfo = Console.ReadKey(true);
            char keyChar = consoleKeyInfo.KeyChar;
            bool isValidChar = false;
            if (keyChar == '-' && num == 0)
            {
               isValidChar = true;
            }
            else if (keyChar == '.' && !hasDecimalSeparator)
            {
               hasDecimalSeparator = true;
               isValidChar = true;
            }
            else if (keyChar >= '0' && keyChar <= '9')
            {
               isValidChar = true;
            }
            if (!isValidChar)
            {
               if (num <= 0 || consoleKeyInfo.Key != ConsoleKey.Backspace)
               {
                  continue;
               }
               Console.CursorLeft = Console.CursorLeft - 1;
               Console.Write(" ");
               Console.CursorLeft = Console.CursorLeft - 1;
               num--;
               keyChar = builder[num];
               if (keyChar == '.')
               {
                  hasDecimalSeparator = false;
               }
               builder.Remove(num, 1);
               goto NextChar;
            }
            else
            {
               Console.Write(keyChar);
               builder.Append(keyChar);
               num++;
               goto NextChar;
            }
         }
         while (consoleKeyInfo.Key != ConsoleKey.Enter);
         Console.WriteLine();
         if (builder.Length == 0)
         {
            return new Primitive(0);
         }
         return new Primitive(builder.ToString());
      }


      public static void Write(Primitive data)
      {
         Console.Write((string) data);
      }

      public static void WriteLine(Primitive data)
      {
         Console.WriteLine((string) data);
      }
   }
}