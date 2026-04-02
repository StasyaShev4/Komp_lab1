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
        private Token Current => tokens[Math.Min(position, tokens.Count - 1)];

        public void RemoveWhitespaceTokens()
        {
            var newTokens = new List<Token>();
            foreach (var token in tokens)
            {
                if (token.Type != TokenType.Whitespace)
                {
                    newTokens.Add(token);
                }
            }
            tokens = newTokens;
            position = 0;
        }

        public void Parse()
        {
            RemoveWhitespaceTokens();
            while (Current.Type != TokenType.EndOfFile)
            {
                int startPos = position;
                ParseStruct();
                if (position == startPos)
                {
                    Error("Неожиданный токен");
                    if (Current.Type == TokenType.EndOfFile)
                        break;
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
                Next();
                if (Current.Type == TokenType.EndOfFile)
                {
                    return;
                }
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
            if (Current.Type != TokenType.EndOfFile && Current.Value != "}")
            {
                ParseFields();
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
            if (Current.Type == TokenType.EndOfFile) return false;
            if (Current.Type == type && (value == null || Current.Value == value))
            {
                Next();
                return true;
            }
            return false;
        }
        private void Synchronize()
        {
            while (Current.Type != TokenType.EndOfFile)
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
            while (Current.Type != TokenType.EndOfFile &&
                   !(Current.Type == TokenType.Separator && Current.Value == "}"))
            {
                ParseField();
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
                Next();
                if (Current.Type == TokenType.EndOfFile) 
                {
                    return;
                }
            }
            else { Next(); }
            

            if (Current.Type == TokenType.EndOfFile || !Current.Value.StartsWith("$"))
            {
                Error("Ожидалась переменная вида $name");
            }
            if (Current.Value.StartsWith("$") && Current.Value.Length <= 1)
            {
                Error("После $ должно быть имя переменной");
            }
            Next();

            if (Match(TokenType.Operator, "="))
            {
                ParseDefault(currentType);
            }
            Next();

            if (!Match(TokenType.Separator, ";"))
            {
                Error("Ожидался ';'");
                Synchronize();
            } 
        }
        private void ParseDefault(string type)
        {
            if (Current.Type == TokenType.EndOfFile)
            {
                Error("Ожидалось значение, но достигнут конец файла");
                return;
            }

            string aboba = null;
            int startPos = position;

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
            
            if (Match(TokenType.Separator, "]"))
            {
                return;
            }

            ParseValue();
            do
            {
                if (Match(TokenType.Separator, ","))
                {
                    ParseValue();
                }
                else
                {
                    Error("Ожидалось значение после ','");
                    Next();
                }
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
            if (Current.Type == TokenType.EndOfFile)
            {
                Error("Ожидалось значение");
                return;
            }
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
                Fragment = Current?.Value ?? "EndOfFile",
                Message = message,
                Line = Current?.Line ?? 0,
                Position = Current?.Position ?? 0
            });
        }
    }
}
