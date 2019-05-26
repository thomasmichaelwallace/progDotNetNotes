using System;
using System.Globalization;

namespace Microsoft.SmallBasic.Library
{
   [SmallBasicType]
   public static class Clock
   {
      public static Primitive Date
      {
         get
         {
            var instance = DateTimeFormatInfo.GetInstance(CultureInfo.CurrentCulture);
            return DateTime.Now.ToString(instance.ShortDatePattern, CultureInfo.CurrentUICulture);
         }
      }

      public static Primitive Day
      {
         get
         {
            return DateTime.Now.Day;
         }
      }

      public static Primitive ElapsedMilliseconds
      {
         get
         {
            TimeSpan now = DateTime.Now - new DateTime(1900, 1, 1);
            return (double)now.TotalMilliseconds;
         }
      }

      public static Primitive Hour
      {
         get
         {
            return DateTime.Now.Hour;
         }
      }

      public static Primitive Millisecond
      {
         get
         {
            return DateTime.Now.Millisecond;
         }
      }

      public static Primitive Minute
      {
         get
         {
            return DateTime.Now.Minute;
         }
      }

      public static Primitive Month
      {
         get
         {
            return DateTime.Now.Month;
         }
      }

      public static Primitive Second
      {
         get
         {
            return DateTime.Now.Second;
         }
      }

      public static Primitive Time
      {
         get
         {
            var instance = DateTimeFormatInfo.GetInstance(CultureInfo.CurrentCulture);
            return DateTime.Now.ToString(instance.LongTimePattern, CultureInfo.CurrentUICulture);
         }
      }

      public static Primitive WeekDay
      {
         get
         {
            return DateTime.Now.ToString("dddd", CultureInfo.CurrentUICulture);
         }
      }

      public static Primitive Year
      {
         get
         {
            return DateTime.Now.Year;
         }
      }
   }
}