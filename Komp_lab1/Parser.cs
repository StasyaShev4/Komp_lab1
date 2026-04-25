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

        private List<Tetrads> tetrad = new List<Tetrads>();
        private int tempCounter = 1;
        public List<Tetrads> GetQuads()
        {
            return tetrad;
        }
        private string NewTemp()
        {
            return "t" + tempCounter++;
        }
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
                Error("Ожидалась ';'");
            }
        }
        public string ParsGrammarOfArithmeticExpressions()
        {
            string left = ParseT();
            return ParseA(left);
        }
        private string ParseT()
        {
            string left = ParseF();
            return ParseB(left);
        }

        private string ParseA(string left)
        {
            while (IsOperator("+") || IsOperator("-"))
            {
                string op = Current.Value;
                Eat(TokenType.Operator, op);

                string right = ParseT();

                string temp = NewTemp();
                tetrad.Add(new Tetrads(op, left, right, temp));

                left = temp;
            }

            return left;
        }

        private string ParseF()
        {
            if (Current.Type == TokenType.IntegerLiteral)
            {
                string value = Current.Value;
                Eat(TokenType.IntegerLiteral);
                return value;
            }
            else if (Current.Type == TokenType.Identifier)
            {
                string value = Current.Value;
                Eat(TokenType.Identifier);
                return value;
            }
            else if (Current.Type == TokenType.Separator && Current.Value == "(")
            {
                Eat(TokenType.Separator, "(");

                string result = ParsGrammarOfArithmeticExpressions();

                Eat(TokenType.Separator, ")");
                return result;
            }
            else
            {
                position = Recover(expectedType: "операнд");
                return "error";
            }
        }
        private string ParseB(string left)
        {
            while (IsOperator("*") || IsOperator("/") || IsOperator("%") ||
                   IsOperator("**") || IsOperator("//"))
            {
                string op = Current.Value;
                Eat(TokenType.Operator, op);

                string right = ParseF();

                string temp = NewTemp();
                tetrad.Add(new Tetrads(op, left, right, temp));

                left = temp;
            }

            return left;
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