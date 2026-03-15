using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komp_lab1
{
    public enum TokenType
    {
        Keyword,
        Identifier,
        Variable,
        Separator,
        Whitespace,
        Unknown,
        EndOfFile
    }
    internal class Token
    {
        public string Value { get; set; }
        public int Position { get; set; }
        public int Line { get; set; }
        public TokenType Type { get; set; }

        public Token(TokenType tupe, string value, int pos, int line = 1) 
        {
            Type = tupe;
            Value = value;
            Position = pos;
            Line = line;
        }

    }
}
