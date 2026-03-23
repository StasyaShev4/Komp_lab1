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
            '=', '-'
        };
        private readonly HashSet<string> booleanLiterals = new HashSet<string>
        {
            "true", "false"
        };
        private readonly HashSet<string> keywords = new HashSet<string> {
            "string", "int", "bool", "array", "float", "struct"
        };
        private readonly HashSet<string> separators = new HashSet<string> {
            "{", "}", ";", "[" , "]", "\"", 
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
                if (c == '\'' || c == '\"')
                {
                    tokens.Add(ReadString());
                    continue;
                }
                if (IsLatinLetter(c) || c == '_')
                {
                    tokens.Add(ReadIndentifier());
                    continue;
                }
                if (char.IsDigit(c))
                {
                    tokens.Add(ReadNumber());
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
                if (operators.Contains(c))
                {
                    tokens.Add(new Token(TokenType.Operator, c.ToString(), position++, line));
                    continue;
                }
                tokens.Add(Unknown());
                
            }
            return tokens;
        }
        Token Unknown() 
        {
            int start = position, startLP = line;
            //position++;
            
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
            int start = position, startLP = line;

            position ++;

            while (position < input.Length && char.IsLetterOrDigit(input[position]))
            {
                position++;
            }
            string var = input.Substring(start, position - start);

            return new Token(TokenType.Variable, var, start, startLP);
        }

        Token ReadIndentifier() 
        {
            int start = position, startLP = line;
            while (position < input.Length && char.IsLetterOrDigit(input[position])) 
            {
                position++;
            }
            string word = input.Substring(start, position - start);

            if (keywords.Contains(word))
                return new Token(TokenType.Keyword, word, start, startLP);
            if (booleanLiterals.Contains(word))
                return new Token(TokenType.BooleanLiteral, word, start, line);

            return new Token(TokenType.Identifier, word, start, startLP);
        }
        Token ReadNumber() 
        {
            int start = position;
            bool isFloat = false;

            while (position < input.Length &&
                  (char.IsDigit(input[position]) || input[position] == '.' || input[position] == ','))
            {
                if (input[position] == '.' || input[position] == ',')
                    isFloat = true;

                position++;
            }

            string number = input.Substring(start, position - start);

            if (isFloat)
                return new Token(TokenType.FloatLiteral, number, start, line);

            return new Token(TokenType.IntegerLiteral, number, start, line);
        }
        Token ReadString() 
        {
            int start = position;
            char quote = input[position];
            position++;

            while (position < input.Length && input[position] != quote)
            {
                position++;
            }

            position++;

            string str = input.Substring(start, position - start);

            return new Token(TokenType.StringLiteral, str, start, line);
        }
        private bool IsLatinLetter(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }
    }
}
