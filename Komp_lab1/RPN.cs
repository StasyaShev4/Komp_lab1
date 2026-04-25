using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komp_lab1
{
    internal class RPN
    {
        public List<string> Build(List<Token> tokens)
        {
            List<string> output = new List<string>();
            Stack<string> stack = new Stack<string>();

            foreach (var token in tokens)
            {
                if (token.Type == TokenType.IntegerLiteral)
                {
                    output.Add(token.Value);
                }
                else if (token.Type == TokenType.Operator)
                {
                    while (stack.Count > 0 &&
                           Priority(stack.Peek()) >= Priority(token.Value))
                    {
                        output.Add(stack.Pop());
                    }
                    stack.Push(token.Value);
                }
                else if (token.Value == "(")
                {
                    stack.Push(token.Value);
                }
                else if (token.Value == ")")
                {
                    while (stack.Count > 0 && stack.Peek() != "(")
                        output.Add(stack.Pop());

                    if (stack.Count > 0)
                        stack.Pop();
                }
            }

            while (stack.Count > 0)
                output.Add(stack.Pop());

            return output;
        }

        public int Evaluate(List<string> poliz)
        {
            Stack<int> stack = new Stack<int>();

            foreach (var token in poliz)
            {
                if (int.TryParse(token, out int num))
                {
                    stack.Push(num);
                }
                else
                {
                    int b = stack.Pop();
                    int a = stack.Pop();

                    switch (token)
                    {
                        case "+": stack.Push(a + b); break;
                        case "-": stack.Push(a - b); break;
                        case "*": stack.Push(a * b); break;
                        case "/": stack.Push(a / b); break;
                        case "%": stack.Push(a % b); break;
                    }
                }
            }

            return stack.Pop();
        }

        private int Priority(string op)
        {
            if (op == "+" || op == "-") return 1;
            if (op == "*" || op == "/" || op == "%") return 2;
            if (op == "**" || op == "//") return 3;
            return 0;
        }
    }
}
