using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellLang
{
    using TokList = List<Token>;
    using ParsedList = List<AnyType>;

    class Parser
    {
        public TokList from;
        public ErrorStream errorStream;

        public ParsedList result;

        public int index;

        public Parser(TokList tokens, ErrorStream errorStream)
        {
            this.from = tokens;
            this.errorStream = errorStream;
            this.result = new ParsedList();
            this.index = 0;
        }

        public ParseResult Start()
        {
            while (this.index < this.from.Count)
            {
                Token item = this.from[this.index];
                
                if (item.type == TokenNames.NumberToken)
                {
                    this.GenNumberType(item);
                }

                else if (item.type == TokenNames.StringToken)
                {
                    this.GenStringType(item);
                }


                if (this.errorStream.errors.Count > 0)
                    break;

                this.index++;
            }

            return new ParseResult(this.result, this.errorStream);
        }

        public Token NextToken(int n = 1)
        {
            if (n >= this.from.Count)
                return null;
            else
                return this.from[this.index + n];
        }

        public void GenNumberType(Token from_token)
        {
            double parsed;

            if (double.TryParse(from_token.value, out parsed) == false)
            {
                this.errorStream.AddError(new Error(ErrorNames.core_error, "Could not parse the number",
                    from_token.startPos, from_token.endPos));
            }
            else
            {
                this.result.Add(new NumberType(parsed, from_token.startPos, from_token.endPos));
            }
        }
    
        public void GenStringType(Token from_token)
        {
            this.result.Add(new StringType(from_token.value, from_token.startPos, from_token.endPos));
        }
    }

    class ParseResult
    {
        public ParsedList result;
        public ErrorStream errors;
        public bool isError;

        public ParseResult(ParsedList result, ErrorStream errors)
        {
            this.result = result;
            this.errors = errors;
            this.isError = errors.errors.Count > 0;
        }
    }
}
