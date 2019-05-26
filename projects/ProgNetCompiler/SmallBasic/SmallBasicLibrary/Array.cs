using System;
using System.Collections.Generic;

namespace Microsoft.SmallBasic.Library
{
   [SmallBasicType]
   public static class Array
   {
      private static Dictionary<Primitive, Dictionary<Primitive, Primitive>> _arrayMap;

      static Array()
      {
         Array._arrayMap = new Dictionary<Primitive, Dictionary<Primitive, Primitive>>();
      }

      public static Primitive ContainsIndex(Primitive array, Primitive index)
      {
         return array.ContainsKey(index);
      }

      public static Primitive ContainsValue(Primitive array, Primitive value)
      {
         return array.ContainsValue(value);
      }

      public static Primitive GetAllIndices(Primitive array)
      {
         return array.GetAllIndices();
      }

      internal static Dictionary<Primitive, Primitive> GetArray(Primitive arrayName)
      {
         Dictionary<Primitive, Primitive> primitives;
         if (Array._arrayMap.TryGetValue(arrayName, out primitives))
         {
            return primitives;
         }
         return null;
      }

      public static Primitive GetItemCount(Primitive array)
      {
         return array.GetItemCount();
      }

      [HideFromIntellisense]
      public static Primitive GetValue(Primitive arrayName, Primitive index)
      {         
         var array = Array.GetArray(arrayName);
         if (array == null)
         {
            return "";
         }
         Primitive primitive;
         if (array.TryGetValue(index, out primitive))
         {
            return primitive;
         }
         return "";
      }

      public static Primitive IsArray(Primitive array)
      {
         return array.IsArray;
      }

      [HideFromIntellisense]
      public static void RemoveValue(Primitive arrayName, Primitive index)
      {
         Dictionary<Primitive, Primitive> array = Array.GetArray(arrayName);
         if (array != null)
         {
            array.Remove(index);
         }
      }

      [HideFromIntellisense]
      public static void SetValue(Primitive arrayName, Primitive index, Primitive value)
      {
         Dictionary<Primitive, Primitive> primitives;
         if (arrayName.IsEmpty)
         {
            return;
         }
         if (!Array._arrayMap.TryGetValue(arrayName, out primitives))
         {
            primitives = new Dictionary<Primitive, Primitive>();
            Array._arrayMap[arrayName] = primitives;
         }
         primitives[index] = value;
      }
   }
}