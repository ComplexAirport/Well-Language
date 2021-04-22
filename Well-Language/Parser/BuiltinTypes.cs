using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellLang
{
    // Built-in type names
    class TypeNames
    {
        public static string any = "any";
        public static string number = "number";
        public static string str = "string";
    }

    class AnyType
    {
        public string type;
        public dynamic value;
        public Position startPos;
        public Position endPos;

        public AnyType(string type, dynamic value, Position startPos, Position endPos)
        {
            this.type = type;
            this.value = value;
            this.startPos = startPos;
            this.endPos = endPos;
        }

        public virtual AnyType OperatorAdd(AnyType other)
        {
            return new AnyType(this.type, this.value + other.value, this.startPos, other.endPos);
        }

        public virtual AnyType OperatorSub(AnyType other)
        {
            return new AnyType(this.type, this.value - other.value, this.startPos, other.endPos);
        }

        public virtual AnyType OperatorMul(AnyType other)
        {
            return new AnyType(this.type, this.value * other.value, this.startPos, other.endPos);
        }

        public virtual AnyType OperatorDiv(AnyType other)
        {
            return new AnyType(this.type, this.value / other.value, this.startPos, other.endPos);
        }

        public override string ToString()
        {
            return $"{this.value.ToString()}: {this.type}";
        }
    }

    class NumberType : AnyType
    {
        public NumberType(double value, Position startPos, Position endPos)
            : base(TypeNames.number, value, startPos, endPos)
        {

        }
    }

    class StringType : AnyType
    {
        public StringType(string value, Position startPos, Position endPos)
            : base(TypeNames.str, value, startPos, endPos)
        {

        }
    }
}
