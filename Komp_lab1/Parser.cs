using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

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
        private int position = 0;
        public List<SyntaxError> Errors = new List<SyntaxError>();

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
            ParsGrammarOfArithmeticExpressions();
            if (Current.Type == TokenType.Separator && Current.Value == ";")
            {
                Eat(TokenType.Separator, ";");
            }
            else
            {
                Error("Ожидалась ';' в конце выражения");
                position = Recover(expectedValue: ";");

                if (Current.Value == ";")
                    Eat(TokenType.Separator, ";");
            }

            if (Current.Type != TokenType.EndOfFile)
            {
                Error("Лишние символы после ';'");
            }

        }
        public void ParsGrammarOfArithmeticExpressions()
        {
            ParseT();
            ParseA();
        }
        private void ParseT()
        {
            ParseF();
            ParseB();
        }

        private void ParseA()
        {
            while (IsOperator("+") || IsOperator("-"))
            {
                string op = Current.Value;
                Eat(TokenType.Operator, op);

                if (Current.Type == TokenType.IntegerLiteral ||
                    Current.Type == TokenType.Identifier ||
                    (Current.Type == TokenType.Separator && Current.Value == "("))
                {
                    ParseT();
                }
                else
                {
                    position = Recover(expectedType: "operand");
                }
            }
        }

        private void ParseF()
        {
            if (Current.Type == TokenType.IntegerLiteral)
            {
                Eat(TokenType.IntegerLiteral);
            }
            else if (Current.Type == TokenType.Identifier)
            {
                Eat(TokenType.Identifier);
            }
            else if (Current.Type == TokenType.Separator && Current.Value == "(")
            {
                Eat(TokenType.Separator, "(");

                ParsGrammarOfArithmeticExpressions();

                if (Current.Type == TokenType.Separator && Current.Value == ")")
                {
                    Eat(TokenType.Separator, ")");
                }
                else
                {
                    position = Recover(expectedValue: ")", insideBracket: true);
                }
            }
            else
            {
                position = Recover(expectedType: "operand");

                if (Current.Type == TokenType.IntegerLiteral ||
                    Current.Type == TokenType.Identifier ||
                    (Current.Type == TokenType.Separator && Current.Value == "("))
                {
                    ParseF();
                }
            }
        }
        private void ParseB()
        {
            while (IsOperator("*") || IsOperator("/") || IsOperator("%") ||
                   IsOperator("**") || IsOperator("//"))
            {
                Eat(TokenType.Operator, Current.Value);

                if (Current.Type == TokenType.IntegerLiteral ||
                    Current.Type == TokenType.Identifier ||
                    (Current.Type == TokenType.Separator && Current.Value == "("))
                {
                    ParseF();
                }
                else
                {
                    position = Recover(expectedType: "operand");

                    if (Current.Type == TokenType.IntegerLiteral ||
                        Current.Type == TokenType.Identifier ||
                        (Current.Type == TokenType.Separator && Current.Value == "("))
                    {
                        ParseF();
                    }
                }
            }
        }

        private void Eat(TokenType type, string value = null)
        {
            if (Current.Type == type && (value == null || Current.Value == value))
            {
                Next();
            }
            else
            {
                Error($"Ожидался {value ?? type.ToString()}, получен {Current.Value}");
                Next();
            }
        }


        private void Next()
        {
            position++;
        }
        private int Recover(string expectedType = null, string expectedValue = null, bool insideBracket = false)
        {
            string errorFragment = Current.Value;

            while (position < tokens.Count)
            {
                if (expectedValue != null && Current.Value == expectedValue)
                    break;

                if (Current.Type == TokenType.Separator && Current.Value == ";")
                    break;

                if (Current.Type == TokenType.Separator && Current.Value == "(")
                    break;

                if (insideBracket && Current.Type == TokenType.Separator && Current.Value == ")")
                    break;

                if (expectedType == "операнд" &&
                    (Current.Type == TokenType.IntegerLiteral || Current.Type == TokenType.Identifier))
                    break;

                if (expectedType == "оператор" &&
                    Current.Type == TokenType.Operator)
                    break;

                errorFragment += Current.Value;
                Next();
            }

            Error($"Ошибка: ожидался {expectedValue ?? expectedType}, получено \"{errorFragment}\"");

            if (expectedValue != null && Current.Value == expectedValue)
                return position + 1;

            if (expectedType == "операнд" &&
                (Current.Type == TokenType.IntegerLiteral || Current.Type == TokenType.Identifier))
                return position + 1;

            if (expectedType == "оператор" &&
                Current.Type == TokenType.Operator)
                return position + 1;

            return position;
        }
        private bool IsOperator(string op)
        {
            return Current.Type == TokenType.Operator && Current.Value == op;
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