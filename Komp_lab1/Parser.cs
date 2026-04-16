using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace Komp_lab1
{    class SyntaxError
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
                ParseOneStructFSM();
            }
        }
        enum State { S0, S1, S2, S3, S4, S5, S6, S7, S8, ERROR }
        public void ParseOneStructFSM()
        {
            State state = State.S0;

            while (state != State.S8)
            {

                var flag = Current.Type;
                switch (state)
                {
                    case State.S0:
                        if (Match(TokenType.Keyword, "struct"))
                        {
                            Next();
                            state = State.S1;
                        }
                        else
                        {
                            Error("Ожидалось 'struct'");

                            if (position + 1 < tokens.Count && tokens[position + 1].Type == TokenType.Identifier)
                            {
                                if (position + 2 < tokens.Count &&
                                    tokens[position + 2].Type == TokenType.Separator &&
                                    tokens[position + 2].Value == "{")
                                {
                                    Next();
                                    state = State.S1;
                                }
                                else
                                {
                                    position += 2;
                                    Error("Ожидалась '{' после имени структуры");
                                    state = State.ERROR;
                                }
                            }
                            else
                            {
                                state = State.ERROR;
                            }
                        }
                        break;

                    case State.S1:
                        if (Match(TokenType.Identifier))
                        {
                            Next();
                            state = State.S2;
                        }
                        else
                        {
                            Error("Ожидалось имя структуры");
                            state = State.ERROR;
                        }
                        break;

                    case State.S2:
                        if (Match(TokenType.Separator, "{"))
                        {
                            Next();
                            state = State.S3;
                        }
                        else
                        {
                            Error("Ожидалась '{'");
                            state = State.ERROR;
                        }
                        break;

                    case State.S3:
                        if (Match(TokenType.Keyword) && IsType(Current.Value))
                        {
                            Next();
                            state = State.S4;
                        }
                        else if (Match(TokenType.Separator, "}"))
                        {
                            Next();
                            state = State.S7;
                        }
                        else if (Current.Type == TokenType.Variable)
                        {
                            if (Current.Value.Length > 1)
                            {
                                Error("Пропущен тип перед переменной");
                                Next();
                                state = State.S6;
                            }
                            else
                            {
                                Error("Некорректное имя переменной");
                                Next();
                                state = State.ERROR;
                            }
                        }
                        else if (Current.Type == TokenType.EndOfFile)
                        {
                            state = State.S8;
                        }
                        else
                        {
                            Error("Ожидался тип переменной или '}'");

                            if (position + 1 < tokens.Count && tokens[position + 1].Type == TokenType.Variable)
                            {
                                if (position + 2 < tokens.Count &&
                                    tokens[position + 2].Type == TokenType.Separator &&
                                    tokens[position + 2].Value == ";")
                                {
                                    Next();
                                    state = State.S4;
                                }
                                else
                                {
                                    position += 2;
                                    Error("Ожидалась ';' после имени переменнной");
                                    state = State.ERROR;
                                }
                            }
                            else 
                            {
                                state = State.ERROR;
                            }

                        }
                        break;

                    case State.S4:
                        if (Current.Type == TokenType.Variable && Current.Value.Length >=2)
                        {
                            Next();
                            state = State.S6;
                        }
                        else 
                        {
                            Error("Ожидалась переменная $name");
                            state = State.ERROR;
                        }
                        break;

                    case State.S6:
                        if (Match(TokenType.Separator, ";"))
                        {
                            Next();
                            state = State.S3;
                        }
                        else 
                        {
                            Error("Ожидался ';' после переменной");
                            state = State.ERROR;
                        }
                        break;

                    case State.S7:
                        if (Match(TokenType.Separator, ";"))
                        {
                            Next();
                            state = State.S8;
                        }
                        else
                        {
                            Error("Ожидался завершеющий';'");
                            state = State.ERROR;
                        }
                        break;

                    case State.ERROR:
                        var result = Recover();

                        if (flag == TokenType.Identifier)
                        {

                        }

                        switch (result)
                        {
                            case RecoverResult.ToStruct:
                                state = State.S1;
                                break;

                            case RecoverResult.ToFields:
                                state = State.S3;
                                break;

                            case RecoverResult.ToFinal:
                                state = State.S7;
                                break;
                            case RecoverResult.ToEnd:
                                state = State.S8;
                                break;
                        }

                        break;
                }
            }
        }
        private void Next() 
        {
            position++;
        }
        private bool IsType(string value)
        {
            return value == "string" ||
                   value == "int" ||
                   value == "float" ||
                   value == "bool" ||
                   value == "array";
        }
        enum RecoverResult
        {
            ToStruct,
            ToFields,
            ToFinal,
            ToEnd
        }
        private RecoverResult Recover()
        {
            while (Current.Type != TokenType.EndOfFile)
            {
                if (Current.Type == TokenType.Separator && Current.Value == "{")
                {
                    Next(); 
                    return RecoverResult.ToFields;
                }

                if (Current.Type == TokenType.Separator && Current.Value == "}")
                {
                    Next();
                    return RecoverResult.ToFinal;
                }

                if (Current.Type == TokenType.Separator && Current.Value == ";")
                {
                    Next();
                    return RecoverResult.ToFields;
                }

                if (Current.Type == TokenType.Keyword && Current.Value == "struct")
                {
                    return RecoverResult.ToStruct;
                }

                if (Current.Type == TokenType.Keyword && IsType(Current.Value))
                {
                    return RecoverResult.ToFields;
                }

                Next();
            }

            return RecoverResult.ToEnd;
        }

        private bool Match(TokenType type, string value = null)
        {
            if (Current.Type == TokenType.EndOfFile) return false;
            return Current.Type == type && (value == null || Current.Value == value);
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
