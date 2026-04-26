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

        private HashSet<string> errorSet = new HashSet<string>();
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

            ParsGrammarOfArithmeticExpressions(false);

            while (Current.Type == TokenType.Separator && Current.Value == ")")
            {
                Error("Лишняя закрывающая скобка ')'");
                Next();
            }

            if (Current.Type == TokenType.Separator && Current.Value == ";")
            {
                Eat(TokenType.Separator, ";");
            }
            else
            {
                Error("Ожидалась ';'");
                return;
            }
            while (Current.Type == TokenType.Separator && Current.Value == ";")
            {
                Error("Лишняя ';'");
                Next();
            }

            if (Current.Type != TokenType.EndOfFile)
            {
                Error($"Лишний символ после ';' → \"{Current.Value}\"");

                while (Current.Type != TokenType.EndOfFile)
                    Next();
            }
        }
        public string ParsGrammarOfArithmeticExpressions(bool insideBracket = false)
        {
            string left = ParseT(insideBracket);
            return ParseA(left, insideBracket);
        }
        private string ParseT(bool insideBracket)
        {
            string left = ParseF(insideBracket);
            return ParseB(left, insideBracket);
        }

        private string ParseA(string left, bool insideBracket)
        {
            while (IsOperator("+") || IsOperator("-"))
            {
                string op = Current.Value;
                Eat(TokenType.Operator, op);

                string right = ParseT(insideBracket);

                string temp = NewTemp();
                tetrad.Add(new Tetrads(op, left, right, temp));

                left = temp;
            }
            if (insideBracket && Current.Value == ")")
                return left;

            return left;
        }

        private string ParseF(bool insideBracket)
        {
            if (Current.Type == TokenType.IntegerLiteral)
            {
                string value = Current.Value;
                Eat(TokenType.IntegerLiteral);
                return value;
            }

            if (Current.Type == TokenType.Identifier)
            {
                string value = Current.Value;
                Eat(TokenType.Identifier);
                return value;
            }

            if (Current.Type == TokenType.Separator && Current.Value == "(")
            {
                Eat(TokenType.Separator, "(");

                string result = ParsGrammarOfArithmeticExpressions(true);

                if (Current.Type == TokenType.Separator && Current.Value == ")")
                {
                    Eat(TokenType.Separator, ")");
                }
                else
                {
                    Error($"Пропущена ')'");

                    position = Recover(expectedValue: ")", insideBracket: true);

                    if (Current.Value == ")")
                        Eat(TokenType.Separator, ")");
                }

                return result;
            }
            if (Current.Type == TokenType.Separator && Current.Value == ")")
            {
                Error("Ожидался операнд");

                return "error";
            }

            if (Current.Type == TokenType.Operator)
            {
                Error("Ожидался операнд");

                position = Recover(expectedType: "операнд");

                return ParseF(insideBracket);
            }

            Error($"Ожидался операнд, получено \"{Current.Value}\"");

            position = Recover(expectedType: "операнд");

            if (Current.Type == TokenType.IntegerLiteral ||
                Current.Type == TokenType.Identifier ||
                (Current.Type == TokenType.Separator && Current.Value == "("))
            {
                return ParseF(insideBracket);
            }

            return "error";
        }
        private string ParseB(string left, bool insideBracket)
        {
            while (IsOperator("*") || IsOperator("/") || IsOperator("%") ||
                   IsOperator("**") || IsOperator("//"))
            {
                string op = Current.Value;
                Eat(TokenType.Operator, op);

                string right = ParseF(insideBracket);

                string temp = NewTemp();
                tetrad.Add(new Tetrads(op, left, right, temp));

                left = temp;
            }

            if (insideBracket && Current.Value == ")")
                return left;

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
                Next();
            }
        }
        private bool IsOp(string value)
        {
            return value == "+" ||
                   value == "-" ||
                   value == "*" ||
                   value == "**" ||
                   value == "%" ||
                   value == "/" ||
                   value == "//";
        }
        private void Next()
        {
            position++;
        }
        private int Recover(string expectedType = null,
                    string expectedValue = null,
                    bool insideBracket = false,
                    bool skipError = false, bool reportError = false)
        {
            //string errorFragment = Current.Value;
            //Error($"Ошибка: ожидался {expectedValue ?? expectedType}, получено \"{errorFragment}\"");
            if (reportError && expectedValue != null)
            {
                Error($"Ожидался {expectedValue}");
            }

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
                    Current.Type == TokenType.Operator && IsOp(Current.Value))
                    break;

                Next();
            }

            return position;
        }       
        private bool IsOperator(string op)
        {
            return Current.Type == TokenType.Operator && Current.Value == op;
        }
        private void Error(string message)
        {
            string key = $"{Current.Line}:{Current.Position}:{message}";

            if (errorSet.Contains(key))
                return;

            errorSet.Add(key);

            Errors.Add(new SyntaxError
            {
                Fragment = Current.Value,
                Message = message,
                Line = Current.Line,
                Position = Current.Position
            });
        }
    }
}