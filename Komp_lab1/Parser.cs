using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komp_lab1
{
    class SyntaxError
    {
        public string Fragment;
        public string Message;
        public int Line;
        public int Position;
    }
    internal class Parser
    {
        private List<Token> tokens;
        private int position =0;
        public List<SyntaxError> Errors = new List<SyntaxError> ();

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }
        private Token Current => position < tokens.Count ? tokens[position] : null;

        public void Parse()
        {
            while (Current != null)
            {
                ParseStruct();
            }
        }
        private void Next() 
        {
            position++;
        }

        private bool Match(TokenType type, string value = null)
        {
            SkipWhitespace();
            if (Current == null) return false;
            if (Current.Type == type && (value == null || Current.Value == value))
            {
                Next();
                return true;
            }
            return false;
        }
        private void SkipWhitespace()
        {
            while (Current != null && Current.Type == TokenType.Whitespace)
                Next();
        }

        private void Synchronize()
        {
            while (Current != null)
            {
                if (Current.Value == ";" || Current.Value == "}")
                {
                    Next();
                    return;
                }
                Next();
            }
        }
        private void Error(string message)
        {
            Errors.Add(new SyntaxError
            {
                Fragment = Current?.Value ?? "EOF", 
                Message = message,
                Line = Current?.Line ?? 0,
                Position = Current?.Position ?? 0
            });
        }

        //Token Current => tokens[position];
        private void ParseFields()
        {
            while (Current != null &&
                   !(Current.Type == TokenType.Separator && Current.Value == "}"))
            {
                ParseField();
            }
        }
        private void ParseStruct()
        {
            if (!Match(TokenType.Keyword, "struct"))
            {
                Error("Ожидалось 'struct'");
                Synchronize();
                return;
            }

            if (!Match(TokenType.Identifier))
            {
                Error("Ожидалось имя структуры");
            }

            if (!Match(TokenType.Separator, "{"))
            {
                Error("Ожидалась '{'");
            }

            ParseFields();

            if (!Match(TokenType.Separator, "}"))
            {
                Error("Ожидалась '}'");
            }

            if (!Match(TokenType.Separator, ";"))
            {
                Error("Ожидалась ';'");
            }
        }

        private void ParseField()
        {
            if (!(Match(TokenType.Keyword, "string") ||
                  Match(TokenType.Keyword, "int") ||
                  Match(TokenType.Keyword, "float") ||
                  Match(TokenType.Keyword, "bool") ||
                  Match(TokenType.Keyword, "array")))
            {
                Error("Ожидался тип");
            }

            if (!Match(TokenType.Variable))
            {
                Error("Ожидалась переменная вида $name");
            }

            if (Current != null && Current.Value == "=")
            {
                ParseDefault();
            }

            if (!Match(TokenType.Separator, ";"))
            {
                Error("Ожидался ';'");
                Synchronize();
            }
        }
        private void ParseValue()
        {
            if (Current.Type == TokenType.FloatLiteral)
            {
                if (Current.Value.StartsWith("."))
                {
                    Error("Ожидалась цифра перед десятичной точкой");
                }
                Next();
                return;
            }

            if (Match(TokenType.IntegerLiteral) ||
                Match(TokenType.FloatLiteral) ||
                Match(TokenType.StringLiteral) ||
                Match(TokenType.BooleanLiteral))
            {
                return;
            }
            Error("Ожидалось значение");
        }
        private void ParseArray()
        {
            if (!Match(TokenType.Separator, "["))
            {
                Error("Ожидалась '['");
                return;
            }

            if (Current.Value != "]")
            {
                ParseValue();

                while (Match(TokenType.Separator, ","))
                {
                    ParseValue();
                }
            }

            if (!Match(TokenType.Separator, "]"))
            {
                Error("Ожидалась ']'");
            }
        }
        private void ParseDefault()
        {
            if (!Match(TokenType.Operator, "="))
            {
                Error("Ожидался '='");
                return;
            }

            if (Current.Value == "[")
                ParseArray();
            else
                ParseValue();
        }
    }
}
