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

    // Language built-in types
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

        // Basic operations like +, -, *, /, ^ etc.
        public AnyType OperatorAdd(AnyType other) => new AnyType(this.type, this.value + other.value, this.startPos, other.endPos);

        public AnyType OperatorSub(AnyType other) => null;

        public AnyType OperatorMul(AnyType other) => null;

        public AnyType OperatorDiv(AnyType other) => null;

        // Some methods for language
        public override string ToString() => this.value.ToString();

        public StringType AsString() => new StringType(this.ToString(), this.startPos, this.endPos);
    }

    class NumberType : AnyType
    {
        public double mainValue;
        public NumberType(double value, Position startPos, Position endPos)
            : base(TypeNames.number, new double(), startPos, endPos)
        {
            this.mainValue = value;
        }

        public NumberType OperatorAdd(NumberType other)
            => new NumberType(this.mainValue + other.mainValue, this.startPos, other.endPos);

        public NumberType OperatorSub(NumberType other)
            => new NumberType(this.mainValue - other.mainValue, this.startPos, other.endPos);

        public NumberType OperatorMul(NumberType other)
            => new NumberType(this.mainValue * other.mainValue, this.startPos, other.endPos);

        public NumberType OperatorDiv(NumberType other)
            => new NumberType(this.mainValue / other.mainValue, this.startPos, other.endPos);

        public override string ToString() => this.mainValue.ToString();
    }

    class StringType : AnyType
    {
        public StringType(string value, Position startPos, Position endPos)
            : base(TypeNames.str, value, startPos, endPos)
        {

        }
    }
}
