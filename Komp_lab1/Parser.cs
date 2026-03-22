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
                int startPos = position;
                ParseStruct();
                if (position == startPos)
                {
                    Error("Неожиданный токен");
                    Next();
                }
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
                if (Current.Type == TokenType.Separator && (Current.Value == ";" || Current.Value == "}"))
                {
                    Next();
                    break;
                }
                if (Current.Type == TokenType.Keyword &&
                   (Current.Value == "int" ||
                    Current.Value == "float" ||
                    Current.Value == "string" ||
                    Current.Value == "bool" ||
                    Current.Value == "array"))
                {
                    return;
                }
                if (Current.Type == TokenType.Separator && Current.Value == "}")
                {
                    return;
                }
                Next();
            }
        }
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
            string currentType = null;

            if (Match(TokenType.Keyword, "string")) currentType = "string";
            else if (Match(TokenType.Keyword, "int")) currentType = "int";
            else if (Match(TokenType.Keyword, "float")) currentType = "float";
            else if (Match(TokenType.Keyword, "bool")) currentType = "bool";
            else if (Match(TokenType.Keyword, "array")) currentType = "array";
            else
            {
                Error("Ожидался тип");
                Synchronize();
                return;
            }

            if (!Match(TokenType.Variable))
            {
                Error("Ожидалась переменная вида $name");
                Synchronize();
                return;
            }

            if (Match(TokenType.Operator, "="))
            {
                int beforePos = position;
                ParseDefault(currentType);

                if (beforePos == position)
                {
                    while (Current != null &&
                           !(Current.Type == TokenType.Separator &&
                             (Current.Value == ";" || Current.Value == "}")))
                    {
                        Next();
                    }
                    //ParseDefault(currentType);
                }

                if (!Match(TokenType.Separator, ";"))
                {
                    Error("Ожидался ';'");
                }

            }
        }
        private void ParseValue(string expectedType)
        {
            if (Current == null) return;
            switch (expectedType)
            {
                case "int":
                    if (!Match(TokenType.IntegerLiteral))
                    {
                        Error("Ожидалось целое число");
                        Next();
                    }
                    break;

                case "float":
                    if (Current.Type == TokenType.FloatLiteral)
                    {
                        if (Current.Value.StartsWith("."))
                        {
                            Error("Ожидалась цифра перед точкой");
                            Next();
                            return;
                        }
                        Next();
                    }
                    else
                    {
                        Error("Ожидалось вещественное число");
                        Next();
                    }
                    break;

                case "string":
                    if (!Match(TokenType.StringLiteral))
                    {
                        Error("Ожидалась строка");
                        Next();
                    }
                    break;

                case "bool":
                    if (!Match(TokenType.BooleanLiteral)) 
                    { 
                        Error("Ожидалось true или false");
                        Next();
                    }
                    break;

                case "array":
                    ParseArray(expectedType);
                    break;

                default:
                    Error("Неизвестный тип");
                    break;
            }
        }
        private void ParseArray(string elementType)
        {
            if (!Match(TokenType.Separator, "["))
            {
                Error("Ожидалась '['");
                return;
            }

            SkipWhitespace();

            if (Current != null && Current.Value == "]")
            {
                Match(TokenType.Separator, "]");
                return;
            }

            ParseValue(elementType);

            while (Current != null && Current.Type == TokenType.Separator && Current.Value == ",")
            {
                Next();
                SkipWhitespace();
                if (Current != null && Current.Value == "]")
                {
                    Error("Ожидалось значение после ','");
                    break;
                }
                //if (!Match(TokenType.Separator, ","))
                //{
                //    Error("Ожидалась ',' между элементами массива");
                //    break;
                //}
                //SkipWhitespace();                
                //ParseValue(elementType);
                
            }

            if (!Match(TokenType.Separator, "]"))
            {
                Error("Ожидалась ']'");
            }
        }
        private void ParseDefault(string type)
        {
            if (Match(TokenType.Separator, "["))
            {
                ParseArray(type);
            }
            else
            {
                ParseValue(type);
            }            
        }
        private void Error(string message)
        {
            if (Current == null) return;

            Errors.Add(new SyntaxError
            {
                Fragment = Current?.Value ?? "EOF",
                Message = message,
                Line = Current?.Line ?? 0,
                Position = Current?.Position ?? 0
            });
        }
    }
}
