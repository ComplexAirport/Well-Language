using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellLang
{
    using ErrorList = List<Error>;

    class ErrorStream
    {
        public ErrorList errors;

        public ErrorStream(ErrorList errors)
        {
            this.errors = errors;
        }

        public ErrorStream(params Error[] errors)
        {
            ErrorList temp_errors = new ErrorList();
            foreach (Error err in errors)
            {
                temp_errors.Add(err);
            }
            this.errors = temp_errors;
        }

        public ErrorStream()
        {
            this.errors = new ErrorList();
        }

        public void AddError(Error e)
        {
            this.errors.Add(e);
        }

        public void Clear()
        {
            this.errors = new ErrorList();
        }

        public override string ToString()
        {
            string result = "";
            foreach (Error error in this.errors)
            {
                result += error.ToString();
                result += "\n\n";
            }
            return result;
        }
    }

    class Error
    {
        public string text;
        public string name;
        public string details;
        public string context;

        public Position start_pos;
        public Position end_pos;

        public Error(string error_name, string details, Position start_pos, Position end_pos, string context = null)
        {
            if (start_pos.text != end_pos.text)
                this.text = start_pos.text + end_pos.text;
            else
                this.text = start_pos.text;

            this.name = error_name;
            this.details = details;
            this.context = context;

            this.start_pos = start_pos;
            this.end_pos = end_pos;
        }

        public override string ToString()
        {
            string result = $"{Utils.Magenta}File {Utils.Yellow}{this.end_pos.file_name}{Utils.Reset}";

            if (this.context != null)
            {
                result += ", " + Utils.Cyan + this.context + Utils.Reset + "\n";
            }
            else
            {
                result += "\n";
            }

            if (this.start_pos.line != this.end_pos.line)
                result += "\t" + $"lines: {Utils.Green}{this.start_pos.line}-{this.end_pos.line}{Utils.Reset},\n";
            else
                result += "\t" + $"line: {Utils.Green}{this.start_pos.line}{Utils.Reset},\n";

            result += "\t" + $"columns: {Utils.Green}{this.start_pos.column} - {this.end_pos.column}{Utils.Reset},\n";

            result += "\t" + $"indexes: {Utils.Green}{this.start_pos.index} - {this.end_pos.index}{Utils.Reset}\n";

            result += this.text + "\n";

            // Make underlined color
            result += Utils.RepeatString(" ", this.start_pos.column);
            int col_diff = this.end_pos.column - this.start_pos.column;

            if (!(col_diff <= 0))
            {
                result += Utils.Red;
                result += Utils.RepeatString("~", col_diff + 1);
                result += Utils.Reset + "\n";
            }

            result += $"{Utils.Red}{this.name}: {this.details}{Utils.Reset}\n";
            return result;
        }
    }

    class ErrorNames
    {
        public static string syntax_error = "Syntax-Error";
        public static string core_error = "Core-Error";
        public static string value_error = "Value-Error";
        public static string math_error = "Math-Error";
    }
}