using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        private void ParseStruct()
        {
            if (!Match(TokenType.Keyword, "struct"))
            {
                Error("Ожидалось 'struct'");
            }

            if (!Match(TokenType.Identifier))
            {
                Error("Ожидалось имя структуры");
            }

            if (!Match(TokenType.Separator, "{"))
            {
                Error("Ожидалась '{'");
                Synchronize();
            }
            SkipWhitespace();
            if (Current.Value != "}")
            {
                MessageBox.Show("Зашли в тело структуры");
                ParseFields();
                SkipWhitespace();
            }
            if (!Match(TokenType.Separator, "}"))
            {
                Error("Ожидалась '}' в конце структуры");
                Synchronize();
            }
            if (!Match(TokenType.Separator, ";"))
            {
                Error("Ожидалась ';' после определения структуры");
            }
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
                SkipWhitespace();
            }
        }


        private void ParseField()
        {
            string currentType = null;
            string inputType = Current.Value;
            switch (inputType)
            {
                case "string":
                    currentType = "string";
                    break;
                case "int":
                    currentType = "int";
                    break;
                case "float":
                    currentType = "float";
                    break;
                case "bool":
                    currentType = "bool";
                    break;
                case "array":
                    currentType = "array";
                    break;
                default:
                    currentType = null;
                    break;
            }
            if (currentType == null)
            {
                Error("Ожидался тип");
                Synchronize();
            }
            else 
            {
                Next();
                SkipWhitespace();
            }
            
            if (Current == null || !Current.Value.StartsWith("$"))
            {
                Error("Ожидалась переменная вида $name");
            }
            if (Current.Value.Length <= 1)
            {
                Error("После $ должно быть имя переменной");
            }
            Next();
            SkipWhitespace();

            if (Match(TokenType.Operator, "="))
            {
                ParseDefault(currentType);
            }
            if (!Match(TokenType.Separator, ";"))
            {
                Error("Ожидался ';'");
                Synchronize();
            }
        }
        private void ParseDefault(string type)
        {
            string aboba = null;
            int startPos = position;

            SkipWhitespace();

            aboba = Current.Value;
            switch (type)
            {
                case "int":
                    bool isNegative = Match(TokenType.Operator, "-");
                    if (!int.TryParse(aboba, out _))
                    {
                        Error($"Ожидалось целое число (int)");
                        if (!isNegative) Next();
                    }
                    break;

                case "float":
                    bool isNegative1 = Match(TokenType.Operator, "-");
                    if (!float.TryParse(aboba, NumberStyles.Float, CultureInfo.InvariantCulture, out _))
                    {
                        Error($"Ожидалось вещественное число (float) например: 1.2");
                        if (!isNegative1) Next();
                    }
                    break;

                case "string":
                    if (aboba.StartsWith("\"") && !aboba.EndsWith("\""))
                    {
                        Error("Незакрытая кавычка: строка начинается с кавычки, но не заканчивается ей");
                    }
                    if (!aboba.StartsWith("\"") && aboba.EndsWith("\""))
                    {
                        Error("Незакрытая кавычка: строка заканчивается кавычкой, но не начинается с нее");
                    }
                    if (!aboba.StartsWith("\"") && !aboba.EndsWith("\""))
                    {
                        Error("Ожидалась строка (string)");
                    }
                    break;

                case "bool":
                    if (!bool.TryParse(aboba, out _))
                    {
                        Error($"Ожидалось логическое значение (true или false)");
                    }                    
                    break;

                case "array":
                    ParseArray(type); 
                    break;

                default:
                    Error($"Неизвестный тип: {type}");
                    break;
            }
        }
        private void ParseArray(string elementType)
        {
            string aboba = null;
            if (!Match(TokenType.Separator, "["))
            {
                Error("Ожидалась '['");
            }
            Next();
            
            if (Match(TokenType.Separator, "]"))
            {
                return;
            }

            ParseValue();
            do
            {
                if (Match(TokenType.Separator, ","))
                {
                    SkipWhitespace();
                    ParseValue();
                }
                else
                {
                    Error("Ожидалось значение после ','");
                    Next();
                }
                SkipWhitespace();
                aboba = null;
                aboba = Current.Value;
                if (aboba == "]")
                    break;
            }
            while (true);

            if (!Match(TokenType.Separator, "]"))
            {
                Error("Ожидалась ']'");
            }
        }
        private void ParseValue()
        {
            string token = null;
            token = Current.Value;
            if (token.StartsWith("\"") && !token.EndsWith("\""))
            {
                Error("Незакрытая кавычка: строка начинается с кавычки, но не заканчивается ей");
            }
            if (!token.StartsWith("\"") && token.EndsWith("\""))
            {
                Error("Незакрытая кавычка: строка заканчивается кавычкой, но не начинается с нее");
            }

            switch (Current.Type)
            {
                case TokenType.StringLiteral:
                    Next();
                    break;
                case TokenType.IntegerLiteral:
                    Next();
                    break;
                case TokenType.FloatLiteral:
                    Next();
                    break;
                case TokenType.BooleanLiteral:
                    Next();
                    break;

                default:
                    Error($"Неожиданный токен: {Current.Value}");
                    Next();
                    break;
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
    }
}
