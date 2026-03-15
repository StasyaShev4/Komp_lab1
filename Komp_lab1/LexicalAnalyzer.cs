using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komp_lab1
{
    internal class LexicalAnalyzer
    {
        private string input;
        private int position = 0;
        private int line = 1;
        //private int linePosition = 1;

        private readonly HashSet<string> keywords = new HashSet<string> {
            "string", "int", "bool", "array", "float", "struct"
        };
        private readonly HashSet<string> separators = new HashSet<string> {
            "{", "}", ";"
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
                if (char.IsLetter(c)) 
                {
                    tokens.Add(ReadIndentifier());
                    continue;
                }
                if (c == '$') 
                {
                    tokens.Add(ReadVariable());
                    continue;
                }
                //if (c == '{' || c == '}' || c == ';')
                if (separators.Contains(c.ToString()))
                {
                    tokens.Add(new Token(TokenType.Separator, c.ToString(), position, line));
                    position++;
                    continue;
                }

                tokens.Add(new Token(TokenType.Unknown, c.ToString(), position, line));
                position ++;
            }
            return tokens;
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

            return new Token(TokenType.Identifier, word, start, startLP);
        }
    }
}
