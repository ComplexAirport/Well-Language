using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellLang
{
    // Token types as string values
    class TokenNames
    {
        public static string NumberToken = "number";
        public static string StringToken = "string";
        public static string WordToken = "word";

        public static string BracketExprToken = "bracket-expr";
        public static string BraceExprToken = "brace-expr";
        public static string ParenExprToken = "parentheses-expr";
        public static string AngleExprToken = "angle-expr";
    }

    // Tokens
    class Token
    {
        public string type;
        public object value;
        public Position startPos;
        public Position endPos;

        public Token(string type, object value, Position start_pos, Position end_pos)
        {
            this.type = type;
            this.value = value;
            this.startPos = start_pos;
            this.endPos = end_pos;
        }

        public override string ToString()
        {
            return $"{this.value.ToString()}: {this.type.ToString()}";
        }

        public Token Copy()
        {
            return new Token(this.type, this.value, this.startPos, this.endPos);
        }
    }

    class WordToken : Token
    {
        public WordToken(string value, Position startPos, Position endPos)
            : base(TokenNames.WordToken, value, startPos, endPos)
        { }
    }

    class NumberToken : Token
    {
        public NumberToken(double value, Position startPos, Position endPos)
            : base(TokenNames.NumberToken, value, startPos, endPos)
        { }
    }

    class StringToken : Token
    {
        public StringToken(string value, Position startPos, Position endPos)
            : base(TokenNames.StringToken, value, startPos, endPos)
        { }
    }

    class BracketExprToken : Token
    {
        public BracketExprToken(string inner, Position startPos, Position endPos)
            : base(TokenNames.BracketExprToken, inner, startPos, endPos)
        {

        }
    }

    class BraceExprToken : Token
    {
        public BraceExprToken(string inner, Position startPos, Position endPos)
            : base(TokenNames.BraceExprToken, inner, startPos, endPos)
        {

        }
    }

    class ParenExprToken : Token
    {
        public ParenExprToken(string inner, Position startPos, Position endPos)
            : base(TokenNames.ParenExprToken, inner, startPos, endPos)
        {

        }
    }

    class AngleExprToken : Token
    {
        public AngleExprToken(string inner, Position startPos, Position endPos)
            : base(TokenNames.AngleExprToken, inner, startPos, endPos)
        {

        }
    }
}
