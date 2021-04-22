using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellLang
{
    class Utils
    {
        public static string RepeatString(string str, int n)
        {
            string result = "";
            for (int i = 0; i < n; i++)
                result += str;
            return result;
        }

        public static string Red = "\u001b[31m";
        public static string Black = "\u001b[30m";
        public static string Green = "\u001b[32m";
        public static string Yellow = "\u001b[33m";
        public static string Blue = "\u001b[34m";
        public static string Magenta = "\u001b[35m";
        public static string Cyan = "\u001b[36m";
        public static string White = "\u001b[37m";
        public static string Reset = "\u001b[0m";

        public static string digits = "0123456789";
        public static string dot_digits = digits + ".";

        public static string stringTriggers = "'\"`";

        public static (char, char) bracketExpr = ('[', ']');
        public static (char, char) braceExpr = ('{', '}');
        public static (char, char) parenExpr = ('(', ')');
        public static (char, char) angleExpr = ('<', '>');
        public static (char, char)[] exprsTogether = new (char, char)[]
        {
            braceExpr, bracketExpr, parenExpr, angleExpr
        };

        public static void Execute(string fileName, string code, ErrorStream es)
        {
            Lexer lexer = new Lexer(fileName, code, es);
            LexResult lexResult = lexer.Start();
            
            if (lexResult.isError)
            {
                Console.WriteLine(lexResult.errors.ToString());
                return;
            }

            Parser parser = new Parser(lexResult.result, es);
            ParseResult parseResult = parser.Start();

            if (parseResult.isError)
            {
                Console.WriteLine(parseResult.errors.ToString());
                return;
            }

            // todo remove
            foreach (var i in parseResult.result)
            {
                Console.WriteLine(i.ToString());
            }
        }
    }
}
