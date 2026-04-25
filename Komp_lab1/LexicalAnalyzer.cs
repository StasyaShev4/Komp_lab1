using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Komp_lab1
{
    internal class LexicalAnalyzer
    {
        private string input;
        private int position = 0;
        private int line = 1;

        private readonly HashSet<char> operators = new HashSet<char>
        {
            '=', '-', '+', '*',  '/', '%'
        };
        private readonly HashSet<string> separators = new HashSet<string> {
            "(", ")", ";"
        };
        public LexicalAnalyzer(string text)
        {
            input = text;
        }

        public List<Token> Analize()
        {
            List<Token> tokens = new List<Token>();

            while (position < input.Length)
            {
                char c = input[position];
                if (c == '\r' || c == '\n')
                {
                    if (c == '\r' && position + 1 < input.Length && input[position + 1] == '\n')
                        position++;

                    line++;
                    position++;
                    continue;
                }
                if (position + 3 < input.Length &&
                    input[position] == ' ' &&
                    input[position + 1] == ' ' &&
                    input[position + 2] == ' ' &&
                    input[position + 3] == ' ')
                {
                    position += 4;
                    continue;
                }
                if (c == ' ')
                {
                    tokens.Add(new Token(TokenType.Whitespace, "(пробел)", position, line));
                    position++;
                    continue;
                }
                if (char.IsDigit(c))
                {
                    tokens.Add(ReadNumber());
                    continue;
                }
                if (operators.Contains(c))
                {
                    tokens.Add(ReadOperator());
                    continue;
                }
                if (c == '$')
                {
                    tokens.Add(ReadVariable());
                    continue;
                }
                if (separators.Contains(c.ToString()))
                {
                    tokens.Add(new Token(TokenType.Separator, c.ToString(), position, line));
                    position++;
                    continue;
                }
                tokens.Add(Unknown());

            }
            tokens.Add(new Token(TokenType.EndOfFile, "EndOfFile", position, line));
            return tokens;
        }
        Token Unknown()
        {
            int start = position, startLP = line;
            
            while (position < input.Length &&
                input[position] != ' ' &&
                input[position] != '\n' &&
                input[position] != '\r' &&
                !separators.Contains(input[position].ToString()))
            {
                position++;
            }
            string value = input.Substring(start, position - start);
            return new Token(TokenType.Unknown, value, start, startLP);
        }
        Token ReadVariable()
        {
            int start = position;
            int startLine = line;

            position++;

            while (position < input.Length &&
                   (char.IsLetterOrDigit(input[position]) || input[position] == '_'))
            {
                position++;
            }

            string value = input.Substring(start, position - start);

            return new Token(TokenType.Identifier, value, start, startLine);
        }
        Token ReadNumber()
        {
            int start = position;
            int startLine = line;

            while (position < input.Length && char.IsDigit(input[position]))
            {
                position++;
            }

            string number = input.Substring(start, position - start);

            return new Token(TokenType.IntegerLiteral, number, start, startLine);
        }
        Token ReadOperator()
        {
            int start = position;
            int startLine = line;

            char current = input[position];

            if (position + 1 < input.Length)
            {
                char next = input[position + 1];

                if (current == '*' && next == '*')
                {
                    position += 2;
                    return new Token(TokenType.Operator, "**", start, startLine);
                }

                if (current == '/' && next == '/')
                {
                    position += 2;
                    return new Token(TokenType.Operator, "//", start, startLine);
                }
            }
            position++;
            return new Token(TokenType.Operator, current.ToString(), start, startLine);
        }
    }
}