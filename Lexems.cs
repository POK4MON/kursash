using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp123
{
    class Lexems
    {
        public static bool IsOperator(string text)
        {
            return text == "var" || text == "else" || text == "integer" || text == "real" || text == "double" || text == "then" || text == "begin" || text == "end" || text == "if" || text == "and";
        }

        public static bool IsSeparator(string text)
        {
            return text == "\n" || text == "," || text == ":" || text == ":=" || text == "==" || text == "="
                || text == "+" || text == "-" || text == "." || text == "*" ||
                text == "/" || text == ";" || text == "(" || text == ")" || text == "<" || text == ">";
        }

        public static bool IsLiteral(string text)
        {
            return int.TryParse(text, out int x);
        }

        public static bool IsErrorLetter(string text)
        {
            return text == "@" || text == "$" || text == "[" || text == "]" || text == "`" || text == "~";
        }

        public static bool IsIDVariable(string text)
        {
            bool Flag = true;
            if (text.Length == 0 || !char.IsLetter(text[0]))
                Flag = false;
            else
                foreach (char s in text)
                {
                    if (s != '_' && !char.IsDigit(s) && !char.IsLetter(s))
                        Flag = false;
                }
            return Flag;
        }
    }
}
