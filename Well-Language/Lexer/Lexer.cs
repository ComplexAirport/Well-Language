using System.Collections.Generic;
using System; // Todo: remove

/*  System
 * Lexer <- text, ErrorStream.new()
 * 
 * 
 */

namespace WellLang
{
    using TokList = List<Token>;
    using ErrorList = List<Error>;
    using CodeTokParts = List<List<Token>>;

    class Lexer
    {
        public readonly string text;
        public readonly string file_name;
        public Position currentPos;
        public Position startPos;

        public string token;
        public TokList tokens;

        public ErrorStream errorStream;

        public Lexer(string file_name, string text, ErrorStream errorStream)
        {
            this.file_name = file_name;
            this.text = text;
            this.currentPos = new Position(file_name, text);
            this.startPos = this.currentPos.Copy();

            this.token = "";
            this.tokens = new TokList();

            this.errorStream = errorStream;
        }

        public LexResult Start()
        {
            while (this.currentPos.c_char != '\0')
            {
                this.token += this.currentPos.c_char;

                // Check to be ignored
                if (" \t\n".Contains(this.currentPos.c_char))
                {
                    this.CheckForWord();
                    this.currentPos.Advance();
                    this.SetNewStartPos();
                    continue;
                }

                // Check to be number
                else if (Utils.dot_digits.Contains(this.currentPos.c_char))
                {
                    if (this.token.Length != 1) { } // Check if current token is really number and not
                                                    // object name part
                    else
                    {
                        this.SetNewStartPos();
                        this.AddNumberToken();
                        this.SetNewStartPos();
                        continue;
                    }
                }

                // Check to be string
                else if (Utils.stringTriggers.Contains(this.currentPos.c_char))
                {
                    this.SetNewStartPos();
                    this.AddStringToken();
                    this.SetNewStartPos();
                }

                else if (this.currentPos.c_char == Utils.braceExpr.Item1)
                {
                    this.SetNewStartPos();
                    this.AddBraceExpr();
                    this.SetNewStartPos();
                }

                else if (this.currentPos.c_char == Utils.bracketExpr.Item1)
                {
                    this.SetNewStartPos();
                    this.AddBracketExpr();
                    this.SetNewStartPos();
                }

                else if (this.currentPos.c_char == Utils.parenExpr.Item1)
                {
                    this.SetNewStartPos();
                    this.AddParenExpr();
                    this.SetNewStartPos();
                }

                else if (this.currentPos.c_char == Utils.angleExpr.Item1)
                {
                    this.SetNewStartPos();
                    this.AddAngleExpr();
                    this.SetNewStartPos();
                }

                // Check for errors
                if (this.errorStream.errors.Count != 0)
                    break;

                this.currentPos.Advance();
            }

            // Check if there is some word left in the end
            this.CheckForWord(eof: true);

            // Make up the lex result
            return new LexResult(result: this.tokens, errors: this.errorStream);
        }

        private void AddNumberToken()
        {
            string resultNumber;
            int dotCount = 0;

            if (this.currentPos.c_char == '.')
            {
                dotCount = 1;
                resultNumber = "0.";
            }
            else
            {
                resultNumber = this.currentPos.c_char.ToString();
            }

            this.currentPos.Advance();

            while (this.currentPos.c_char != '\0' && Utils.dot_digits.Contains(this.currentPos.c_char))
            {
                if (this.currentPos.c_char == '.')
                    dotCount++;

                resultNumber += this.currentPos.c_char;
                this.currentPos.Advance();
            }

            if (dotCount > 1)
            {
                this.errorStream.AddError(new Error(
                    ErrorNames.syntax_error, $"Got {dotCount} dots in number, expected max 1", this.startPos, this.currentPos,
                    context: "while parsing floating number"));
            }

            double parsed;
            double.TryParse(resultNumber, out parsed);

            this.tokens.Add(new NumberToken(
                parsed, this.startPos, this.currentPos));
        }

        private void AddStringToken()
        {
            char stringChar = this.currentPos.c_char;
            this.currentPos.Advance();

            string resultString = "";
            bool escape = false;
            bool stringClosed = false;

            while (this.currentPos.c_char != '\0')
            {
                if (this.currentPos.c_char == stringChar && escape == false)
                {
                    stringClosed = true;
                    break;
                }

                else if (this.currentPos.c_char == '\\')
                {
                    escape = true;
                }

                else if (escape == true && this.currentPos.c_char != stringChar)
                {
                    switch (this.currentPos.c_char)
                    {
                        case 'n':
                            resultString += "\n";
                            break;
                        case 't':
                            resultString += "\t";
                            break;
                        case 'b':
                            resultString += "\b";
                            break;
                        case 'f':
                            resultString += "\f";
                            break;
                        case 'r':
                            resultString += "\r";
                            break;
                        case '\\':
                            resultString += "\\";
                            break;
                        default:
                            resultString += "\\" + this.currentPos.c_char;
                            break;
                    }
                    escape = false;
                }

                else if (escape == true && this.currentPos.c_char == stringChar)
                {
                    resultString += stringChar;
                }

                else
                {
                    resultString += this.currentPos.c_char;
                }

                this.currentPos.Advance();
            }

            if (stringClosed == false)
            {
                this.errorStream.AddError(new Error(
                    ErrorNames.syntax_error, "string was never closed", this.startPos, this.currentPos,
                    context: "while inside string"));
            }

            this.tokens.Add(new StringToken(resultString, this.startPos, this.currentPos));
        }

        private void AddBraceExpr()
        {
            string inner = this.GetWrappedExpression(Utils.braceExpr.Item1, Utils.braceExpr.Item2);

            if (inner == null) // Check for error from getting expression
                return;

            this.tokens.Add(new BraceExprToken(inner, this.startPos, this.currentPos));
        }

        private void AddBracketExpr()
        {
            string inner = this.GetWrappedExpression(Utils.bracketExpr.Item1, Utils.bracketExpr.Item2);

            if (inner == null) // Check for error from getting expression
                return;

            this.tokens.Add(new BracketExprToken(inner, this.startPos, this.currentPos));
        }

        private void AddParenExpr()
        {
            string inner = this.GetWrappedExpression(Utils.parenExpr.Item1, Utils.parenExpr.Item2);

            if (inner == null) // Check for error from getting expression
                return;

            this.tokens.Add(new ParenExprToken(inner, this.startPos, this.currentPos));
        }

        private void AddAngleExpr()
        {
            string inner = this.GetWrappedExpression(Utils.angleExpr.Item1, Utils.angleExpr.Item2);

            if (inner == null) // Check for error from getting expression
                return;

            this.tokens.Add(new AngleExprToken(inner, this.startPos, this.currentPos));
        }

        private string GetWrappedExpression(char openChar, char closeChar)
        {
            string result = "";

            if (this.currentPos.c_char != openChar)
                return result;

            this.currentPos.Advance();

            int blockDepth = 1;

            bool isString = false;
            char strChar = '\0';

            while (this.currentPos.c_char != '\0')
            {
                if (isString && this.currentPos.c_char == strChar)
                {
                    isString = false;
                    strChar = '\0';
                }

                else if (!isString && Utils.stringTriggers.Contains(this.currentPos.c_char))
                {
                    isString = true;
                    strChar = this.currentPos.c_char;
                }

                else if (this.currentPos.c_char == closeChar && !isString)
                {
                    blockDepth--;
                    if (blockDepth == 0)
                        break;
                }

                else if (this.currentPos.c_char == openChar && !isString)
                {
                    blockDepth++;
                }

                result += this.currentPos.c_char;
                this.currentPos.Advance();
            }

            if (blockDepth != 0)
            {
                this.errorStream.AddError(new Error(ErrorNames.syntax_error,
                    $"Expected {blockDepth} more closing '{closeChar}'", this.startPos, this.currentPos,
                    context: $"while inside {openChar}{closeChar} with depth {blockDepth}"));
                return null;
            }

            return result;
        } // Get expression inside openChar and closeChar


        private void SetNewStartPos() // Set new start_pos field
        {
            this.token = "";
            this.startPos = this.currentPos.Copy();
        }

        private void CheckForWord(bool eof = false) // Check for some word before space or new line
        {                                           // eof var needed because if it's the end of file no sliceing needed
            string tokenBefore;
            if (!eof)
                tokenBefore = this.GetSlicedToken();
            else
                tokenBefore = this.token;

            if (tokenBefore != "")
            {
                this.tokens.Add(new WordToken(tokenBefore, this.startPos, this.currentPos));
            }
        }

        private string GetSlicedToken(int n = 1) // Get the token sliced from the end [0:-n]
        {
            return this.token.Substring(0, this.token.Length - n); // Slice the token [0:-n]
        }
    }

    class LexResult
    {
        public TokList result;
        public ErrorStream errors;
        public bool isError;

        public LexResult(TokList result, ErrorStream errors)
        {
            this.result = result;
            this.errors = errors;
            this.isError = this.errors.errors.Count > 0;
        }
    }
}