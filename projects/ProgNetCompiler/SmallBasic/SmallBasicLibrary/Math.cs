using System;

namespace Microsoft.SmallBasic.Library
{
   [SmallBasicType]
   public static class Math
   {
      private static Random _random;

      public static Primitive Pi
      {
         get
         {
            return 3.14159265358979;
         }
      }

      public static Primitive Abs(Primitive number)
      {
         return new Primitive(System.Math.Abs((double)number));
      }

      public static Primitive ArcCos(Primitive cosValue)
      {
         return new Primitive(System.Math.Acos((double)cosValue));
      }

      public static Primitive ArcSin(Primitive sinValue)
      {
         return new Primitive(System.Math.Asin((double)sinValue));
      }

      public static Primitive ArcTan(Primitive tanValue)
      {
         return new Primitive(System.Math.Atan((double)tanValue));
      }

      public static Primitive Ceiling(Primitive number)
      {
         return new Primitive(System.Math.Ceiling((double)number));
      }

      public static Primitive Cos(Primitive angle)
      {
         return new Primitive(System.Math.Cos((double)angle));
      }

      public static Primitive Floor(Primitive number)
      {
         return new Primitive(System.Math.Floor((double)number));
      }

      public static Primitive GetDegrees(Primitive angle)
      {
         return new Primitive(180 * (double)angle / 3.14159265358979 % 360);
      }

      public static Primitive GetRadians(Primitive angle)
      {
         return new Primitive((double)angle % 360 * 3.14159265358979 / 180);
      }

      public static Primitive GetRandomNumber(Primitive maxNumber)
      {
         if (Math._random == null)
         {
            Math._random = new Random((int)DateTime.Now.Ticks);
         }
         return Math._random.Next(maxNumber) + 1;
      }

      public static Primitive Log(Primitive number)
      {
         return new Primitive(System.Math.Log10((double)number));
      }

      public static Primitive Max(Primitive number1, Primitive number2)
      {
         return new Primitive(System.Math.Max((double)number1, (double)number2));
      }

      public static Primitive Min(Primitive number1, Primitive number2)
      {
         return new Primitive(System.Math.Min((double)number1, (double)number2));
      }

      public static Primitive NaturalLog(Primitive number)
      {
         return new Primitive(System.Math.Log((double)number));
      }

      public static Primitive Power(Primitive baseNumber, Primitive exponent)
      {
         return new Primitive(System.Math.Pow((double)baseNumber, (double)exponent));
      }

      public static Primitive Remainder(Primitive dividend, Primitive divisor)
      {
         return (double)((double)dividend % (double)divisor);
      }

      public static Primitive Round(Primitive number)
      {
         return new Primitive(System.Math.Round((double)number));
      }

      public static Primitive Sin(Primitive angle)
      {
         return new Primitive(System.Math.Sin((double)angle));
      }

      public static Primitive SquareRoot(Primitive number)
      {
         return new Primitive(System.Math.Sqrt((double)number));
      }

      public static Primitive Tan(Primitive angle)
      {
         return new Primitive(System.Math.Tan((double)angle));
      }
   }
}