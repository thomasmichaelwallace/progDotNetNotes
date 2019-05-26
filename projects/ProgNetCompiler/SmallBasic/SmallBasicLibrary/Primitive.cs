using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace Microsoft.SmallBasic.Library
{
    public struct Primitive
    {
        private string _primitive;

        private decimal? _primitiveAsDecimal;

        private Dictionary<Primitive, Primitive> _arrayMap;

        internal string AsString
        {
            get
            {
                if (this._primitive != null)
                {
                    return this._primitive;
                }
                if (this._primitiveAsDecimal.HasValue)
                {
                    this._primitive = this._primitiveAsDecimal.Value.ToString();
                }
                if (this._arrayMap != null)
                {
                    var builder = new StringBuilder();
                    foreach (var pair in this._arrayMap)
                    {
                        builder.AppendFormat("{0}={1};", 
                           Primitive.Escape(pair.Key), 
                           Primitive.Escape(pair.Value));
                    }
                    this._primitive = builder.ToString();
                }
                return this._primitive;
            }
        }

        internal bool IsArray
        {
            get
            {
                this.ConstructArrayMap();
                return this._arrayMap.Count > 0;
            }
        }

        internal bool IsEmpty
        {
            get
            {
                if (!string.IsNullOrEmpty(this._primitive) || this._primitiveAsDecimal.HasValue)
                {
                    return false;
                }
                if (this._arrayMap == null)
                {
                    return true;
                }
                return this._arrayMap.Count == 0;
            }
        }

        internal bool IsNumber
        {
            get
            {               
                if (this._primitiveAsDecimal.HasValue)
                {
                    return true;
                }
                decimal number;
                return decimal.TryParse(this.AsString, NumberStyles.Float, CultureInfo.InvariantCulture, out number);
            }
        }

        public Primitive this[Primitive index]
        {
            get
            {
                if (!this.ContainsKey(index))
                {
                    return "";
                }
                return this._arrayMap[index];
            }
            set
            {
                Primitive primitive = Primitive.SetArrayValue(value, this, index);
                this._primitive = primitive._primitive;
                this._arrayMap = primitive._arrayMap;
                this._primitiveAsDecimal = primitive._primitiveAsDecimal;
            }
        }

        public Primitive(string primitiveText)
        {
            if (primitiveText == null)
            {
                throw new ArgumentNullException("primitiveText");
            }
            this._primitive = primitiveText;
            this._primitiveAsDecimal = null;
            this._arrayMap = null;
        }

        public Primitive(int primitiveInteger)
        {
            this._primitiveAsDecimal = new decimal?(primitiveInteger);
            this._primitive = null;
            this._arrayMap = null;
        }

        public Primitive(decimal primitiveDecimal)
        {
            this._primitiveAsDecimal = new decimal?(primitiveDecimal);
            this._primitive = null;
            this._arrayMap = null;
        }

        public Primitive(float primitiveFloat) : this((decimal)((float)primitiveFloat))
        {
        }

        public Primitive(double primitiveDouble) : this((decimal)((double)primitiveDouble))
        {
        }

        public Primitive(short primitiveShort) : this((int)primitiveShort)
        {
        }

        public Primitive(long primitiveLong)
        {
            this._primitiveAsDecimal = new decimal?(primitiveLong);
            this._primitive = null;
            this._arrayMap = null;
        }

        public Primitive(object primitiveObject)
        {
            this._primitive = primitiveObject.ToString();
            this._primitiveAsDecimal = null;
            this._arrayMap = null;
        }

        public Primitive(bool primitiveBool)
        {
            this._primitiveAsDecimal = null;
            if (!primitiveBool)
            {
                this._primitive = "False";
            }
            else
            {
                this._primitive = "True";
            }
            this._arrayMap = null;
        }

        public Primitive Add(Primitive addend)
        {
            decimal? lhs = this.TryGetAsDecimal();
            decimal? rhs = addend.TryGetAsDecimal();
            if (lhs.HasValue && rhs.HasValue)
            {
                return lhs.Value + rhs.Value;
            }
            return string.Concat(this.AsString, addend.AsString);
        }

        public Primitive Append(Primitive primitive)
        {
            return new Primitive(string.Concat(this.AsString, primitive.AsString));
        }

        private void ConstructArrayMap()
        {
            if (this._arrayMap != null)
            {
                return;
            }
            this._arrayMap = new Dictionary<Primitive, Primitive>(Primitive.PrimitiveComparer.Instance);
            if (this.IsEmpty)
            {
                return;
            }
            char[] charArray = this.AsString.ToCharArray();
            int num = 0;
            while (true)
            {
                string str = Primitive.Unescape(charArray, ref num);
                if (string.IsNullOrEmpty(str))
                {
                    return;
                }
                string str1 = Primitive.Unescape(charArray, ref num);
                if (str1 == null)
                {
                    break;
                }
                this._arrayMap[str] = str1;
            }
        }

        public Primitive ContainsKey(Primitive key)
        {
            this.ConstructArrayMap();
            return this._arrayMap.ContainsKey(key);
        }

        public Primitive ContainsValue(Primitive value)
        {
            this.ConstructArrayMap();
            return this._arrayMap.ContainsValue(value);
        }

        public static Primitive ConvertFromMap(Dictionary<Primitive, Primitive> map)
        {
            Primitive primitive = new Primitive()
            {
                _primitive = null,
                _primitiveAsDecimal = null,
                _arrayMap = map
            };
            return primitive;
        }

        public static bool ConvertToBoolean(Primitive primitive)
        {
            return primitive;
        }

        public Primitive Divide(Primitive divisor)
        {
            if (divisor.GetAsDecimal() == 0M)
            {
                return 0;
            }
            return new Primitive(this.GetAsDecimal() / divisor.GetAsDecimal());
        }

        public override bool Equals(object obj)
        {
            if (this.AsString == null)
            {
                this._primitive = "";
            }
            if (!(obj is Primitive))
            {
                return false;
            }
            Primitive primitive = (Primitive)obj;
            if (primitive.AsString != null && 
                primitive.AsString.ToLower(CultureInfo.InvariantCulture) == "true" && 
                this.AsString != null && 
                this.AsString.ToLower(CultureInfo.InvariantCulture) == "true")
            {
                return true;
            }
            if (this.IsNumber && primitive.IsNumber)
            {
                return this.GetAsDecimal() == primitive.GetAsDecimal();
            }
            return this.AsString == primitive.AsString;
        }

        public bool EqualTo(Primitive comparer)
        {
            return this.Equals(comparer);
        }

        private static string Escape(string value)
        {
            StringBuilder builder = new StringBuilder();
            string str = value;
            for (int i = 0; i < str.Length; i++)
            {
                char chr = str[i];
                if (chr == '=')
                {
                    builder.Append("\\=");
                }
                else if (chr == ';')
                {
                    builder.Append("\\;");
                }
                else if (chr != '\\')
                {
                    builder.Append(chr);
                }
                else
                {
                    builder.Append("\\\\");
                }
            }
            return builder.ToString();
        }

        public Primitive GetAllIndices()
        {
            this.ConstructArrayMap();
            var primitives = new Dictionary<Primitive, Primitive>(this._arrayMap.Count, Primitive.PrimitiveComparer.Instance);
            int num = 1;
            foreach (Primitive key in this._arrayMap.Keys)
            {
                primitives[num] = key;
                num++;
            }
            return Primitive.ConvertFromMap(primitives);
        }

        public static Primitive GetArrayValue(Primitive array, Primitive indexer)
        {
            Primitive primitive;
            array.ConstructArrayMap();
            if (!array._arrayMap.TryGetValue(indexer, out primitive))
            {
                primitive = new Primitive();
            }
            return primitive;
        }

        internal decimal GetAsDecimal()
        {
            if (this.IsEmpty)
            {
                return new decimal(0);
            }
            if (this._primitiveAsDecimal.HasValue)
            {
                return this._primitiveAsDecimal.Value;
            }
            decimal num = 0M;
            if (decimal.TryParse(this.AsString, NumberStyles.Float, CultureInfo.InvariantCulture, out num))
            {
                this._primitiveAsDecimal = new decimal?(num);
            }
            return num;
        }

        public override int GetHashCode()
        {
            if (this.AsString == null)
            {
                this._primitive = "";
            }
            return this.AsString.ToUpper(CultureInfo.InvariantCulture).GetHashCode();
        }

        public Primitive GetItemCount()
        {
            this.ConstructArrayMap();
            return this._arrayMap.Count;
        }

        public bool GreaterThan(Primitive comparer)
        {
            return this.GetAsDecimal() > comparer.GetAsDecimal();
        }

        public bool GreaterThanOrEqualTo(Primitive comparer)
        {
            return this.GetAsDecimal() >= comparer.GetAsDecimal();
        }

        public bool LessThan(Primitive comparer)
        {
            return this.GetAsDecimal() < comparer.GetAsDecimal();
        }

        public bool LessThanOrEqualTo(Primitive comparer)
        {
            return this.GetAsDecimal() <= comparer.GetAsDecimal();
        }

        public Primitive Multiply(Primitive multiplicand)
        {
            return new Primitive(this.GetAsDecimal() * multiplicand.GetAsDecimal());
        }

        public bool NotEqualTo(Primitive comparer)
        {
            return !this.Equals(comparer);
        }

        public static Primitive operator +(Primitive lhs, Primitive rhs)
        {
            return lhs.Add(rhs);
        }

        public static Primitive op_And(Primitive lhs, Primitive rhs)
        {
            bool result;
            if (!lhs)
            {
                result = false;
            }
            else
            {
                result = rhs;
            }
            return result;
        }

        public static Primitive operator &(Primitive lhs, Primitive rhs)
        {
            return Primitive.op_And(lhs, rhs);
        }

        public static Primitive operator |(Primitive lhs, Primitive rhs)
        {
            return Primitive.op_Or(lhs, rhs);
        }

        public static Primitive operator /(Primitive lhs, Primitive rhs)
        {
            return lhs.Divide(rhs);
        }

        public static Primitive operator ==(Primitive lhs, Primitive rhs)
        {
            return lhs.Equals(rhs);
        }

        public bool IsFalse
        { 
           get 
           {
              return !this;           
           }        
        }

        public static Primitive operator >(Primitive lhs, Primitive rhs)
        {
            return lhs.GreaterThan(rhs);
        }

        public static Primitive operator >=(Primitive lhs, Primitive rhs)
        {
            return lhs.GreaterThanOrEqualTo(rhs);
        }

        public static implicit operator String(Primitive primitive)
        {
            if (primitive.AsString == null)
            {
                return "";
            }
            return primitive.AsString;
        }

        public static implicit operator Int32(Primitive primitive)
        {
            return (int)primitive.GetAsDecimal();
        }

        public static implicit operator Single(Primitive primitive)
        {
            return (float)((float)primitive.GetAsDecimal());
        }

        public static implicit operator Double(Primitive primitive)
        {
            return (double)((double)primitive.GetAsDecimal());
        }

        public static implicit operator Boolean(Primitive primitive)
        {
            if (primitive.AsString != null && primitive.AsString.Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            return false;
        }

        public static implicit operator Primitive(int value)
        {
            return new Primitive(value);
        }

        public static implicit operator Primitive(bool value)
        {
            return new Primitive(value);
        }

        public static implicit operator Primitive(string value)
        {
            if (value == null)
            {
                return new Primitive("");
            }
            return new Primitive(value);
        }

        public static implicit operator Primitive(double value)
        {
            return new Primitive((decimal)((double)value));
        }

        public static implicit operator Primitive(decimal value)
        {
            return new Primitive(value);
        }

        public static implicit operator Primitive(DateTime value)
        {
            return new Primitive((object)value);
        }

        public static Primitive operator !=(Primitive lhs, Primitive rhs)
        {
            return !lhs.Equals(rhs);
        }

        public static Primitive operator <(Primitive lhs, Primitive rhs)
        {
            return lhs.LessThan(rhs);
        }

        public static Primitive operator <=(Primitive lhs, Primitive rhs)
        {
            return lhs.LessThanOrEqualTo(rhs);
        }

        public static Primitive operator *(Primitive lhs, Primitive rhs)
        {
            return lhs.Multiply(rhs);
        }

        public static Primitive op_Or(Primitive lhs, Primitive rhs)
        {
            bool result;
            if (lhs)
            {
                result = true;
            }
            else
            {
                result = rhs;
            }
            return result;
        }

        public static Primitive operator -(Primitive primitive1, Primitive primitive2)
        {
            return primitive1.Subtract(primitive2);
        }

       public bool IsTrue
       {
           get 
           {
              return this;
           }
       }

        public static Primitive operator -(Primitive primitive)
        {
            return -primitive.GetAsDecimal();
        }

        public static Primitive SetArrayValue(Primitive value, Primitive array, Primitive indexer)
        {
            array.ConstructArrayMap();
            var primitives = new Dictionary<Primitive, Primitive>(array._arrayMap, Primitive.PrimitiveComparer.Instance);
            if (!value.IsEmpty)
            {
                primitives[indexer] = value;
            }
            else
            {
                primitives.Remove(indexer);
            }
            return Primitive.ConvertFromMap(primitives);
        }

        public Primitive Subtract(Primitive addend)
        {
            return new Primitive(this.GetAsDecimal() - addend.GetAsDecimal());
        }

        public override string ToString()
        {
            return this.AsString;
        }

        internal decimal? TryGetAsDecimal()
        {
            if (this.IsEmpty)
            {
                return null;
            }
            if (this._primitiveAsDecimal.HasValue)
            {
                return this._primitiveAsDecimal;
            }
            decimal number = new decimal(0);
            if (decimal.TryParse(this.AsString, NumberStyles.Float, CultureInfo.InvariantCulture, out number))
            {
                this._primitiveAsDecimal = new decimal?(number);
            }
            return this._primitiveAsDecimal;
        }

        private static string Unescape(char[] source, ref int index)
        {
            bool skip = false;
            bool isPending = true;
            int length = (int)source.Length;
            StringBuilder stringBuilder = new StringBuilder();
            while (index < length)
            {
                char chr = source[index];
                index = index + 1;
                if (skip)
                {
                    skip = false;
                }
                else if (chr != '\\')
                {
                    if (chr == '=')
                    {
                        break;
                    }
                    if (chr == ';')
                    {
                        break;
                    }
                }
                else
                {
                    skip = true;
                    continue;
                }
                isPending = false;
                stringBuilder.Append(chr);
            }
            if (isPending)
            {
                return null;
            }
            return stringBuilder.ToString();
        }

        private class PrimitiveComparer : IEqualityComparer<Primitive>
        {
            private static Primitive.PrimitiveComparer _instance;

            public static Primitive.PrimitiveComparer Instance
            {
                get
                {
                    return Primitive.PrimitiveComparer._instance;
                }
            }

            static PrimitiveComparer()
            {
                Primitive.PrimitiveComparer._instance = new Primitive.PrimitiveComparer();
            }

            private PrimitiveComparer()
            {
            }

            public bool Equals(Primitive x, Primitive y)
            {
                return string.Equals(x.AsString, y.AsString, StringComparison.InvariantCultureIgnoreCase);
            }

            public int GetHashCode(Primitive obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}