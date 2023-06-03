using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp123
{
    public class Token
    {
        public TokenType Type;
        public string Value;
        public string Qwerty;
        public Token(TokenType type)
        {
            Type = type;
        }
        public override string ToString()
        {
            return string.Format("{0} - {1}", Value, Type);
        }
        public enum TokenType
        {
            IF, VAR, INTEGER, REAL, DOUBLE, DO, NUMBER, BEGIN, END, NETERM, EXPR,
            TO, PLUS, THEN, ELSE, AND, OR, MORE, LESS,
            MINUS, EQUAL, SEMICOLON, MULTIPLY, COMMA, DIVISION, POINT, COLON, VARIABLE, COLONEQUAL, LEFTBRACKET, RIGHTBRACKET, ASSIGNMENT,
            IDENTIFIER,
            LITERAL
        }
        public static TokenType[] Delimiters = new TokenType[]
            { 
               TokenType.PLUS, TokenType.MINUS,
                TokenType.EQUAL, TokenType.LEFTBRACKET, TokenType.RIGHTBRACKET,  TokenType.SEMICOLON, TokenType.MULTIPLY,
                TokenType.COMMA,TokenType.DIVISION, TokenType.POINT, TokenType.COLON, TokenType.ASSIGNMENT, TokenType.MORE, TokenType.LESS 
            };
        public static TokenType[] Words = new TokenType[]
           {
               TokenType.IF, TokenType.VAR,
                TokenType.INTEGER, TokenType.THEN, TokenType.BEGIN,
                TokenType.END, TokenType.ELSE, TokenType.OR, TokenType.AND
           };
        public static bool IsDelimiter(Token token)
        {
            return Delimiters.Contains(token.Type);
        }
        public static bool IsWords(Token token)
        {
            return Words.Contains(token.Type);
        }
        public static Dictionary<string, TokenType> SpecialWords = new Dictionary<string, TokenType>() 
        {
            { "integer", TokenType.INTEGER },
            { "real", TokenType.REAL },
            { "double", TokenType.DOUBLE },
            { "if", TokenType.IF },
            { "var", TokenType.VAR },
            { "then", TokenType.THEN },
            { "and", TokenType.AND },
            { "or", TokenType.OR },
            { "else", TokenType.ELSE },
            { "begin", TokenType.BEGIN },
            { "end", TokenType.END }
        };
        public static bool IsSpecialWord(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return false;
            }
            return (SpecialWords.ContainsKey(word));
        }
        public static Dictionary<string, TokenType> SpecialSymbols = new Dictionary<string, TokenType>()
        {
            { "+", TokenType.PLUS },
            { "-", TokenType.MINUS },
            { "=", TokenType.EQUAL },
            { "*", TokenType.MULTIPLY },
            { "/", TokenType.DIVISION },
            { "(", TokenType.LEFTBRACKET },
            { ")", TokenType.RIGHTBRACKET },
            { ":=", TokenType.ASSIGNMENT},
            { ",", TokenType.COMMA },      
            { ">", TokenType.MORE },
            { "<", TokenType.LESS },
            { ":", TokenType.COLON },
            { ".", TokenType.POINT },
            { ";", TokenType.SEMICOLON },
        };
        public static bool IsSpecialSymbol(string str)
        {
            return SpecialSymbols.ContainsKey(str);
        }
        public static void PrintTokens(System.Windows.Forms.RichTextBox richtextbox, List<Token> list)
        {
            int i = 0;
            richtextbox.Text = "";
            foreach (var t in list)
            {
                i++;
                richtextbox.Text += $"{i} {t}";
                richtextbox.Text += Environment.NewLine;
            }
        }
    }
}
