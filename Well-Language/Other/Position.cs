using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellLang
{
    class Position
    {
        public readonly string text;
        public int index;
        public int column;
        public int line;
        public char c_char;
        public readonly string file_name;

        public Position(string file_name, string text, int idx = -1, int col = -1, int line = 1)
        {
            this.file_name = file_name;
            this.text = text;
            this.index = idx;
            this.column = col;
            this.line = line;
            this.Advance();
        }

        public void Advance()
        {

            this.index++;

            if (this.index >= this.text.Length)
            {
                this.c_char = '\0';
                return;
            }

            this.c_char = this.text[this.index];

            if (this.c_char == '\n')
            {
                this.line++;
                this.column = 0;
            }

            else this.column++;
        }

        public Position Copy()
        {
            return new Position(this.file_name, this.text, this.index, this.column, this.line);
        }

        public string GetLineText()
        {
            return this.GetLineText(this.line - 1);
        }

        public string GetLineText(int line_index)
        {
            return this.text.Split('\n')[line_index];
        }

        public override string ToString()
        {
            return $"(Position line: {this.line}, index: {this.index}, column: {this.column})";
        }
    }
}
