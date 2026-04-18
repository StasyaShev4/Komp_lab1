using Komp_lab1;
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
    class StructDeclNode
    {
        public string Name;
        public List<FieldDeclNode> Fields = new List<FieldDeclNode>();
    }
    class FieldDeclNode
    {
        public string Name;
        public string Type;
        public string Value;
    }

    internal class Parser
    {
        private List<Token> tokens;
        private int position = 0;
        public List<SyntaxError> Errors = new List<SyntaxError>();

        public List<StructDeclNode> Structs = new List<StructDeclNode>();
        StructDeclNode currentStruct;
        FieldDeclNode currentField;
        private RichTextBox output;

        string currentType;
        string currentVarName;

        public Parser(List<Token> tokens, RichTextBox output)
        {
            this.tokens = tokens;
            this.output = output;
        }
        private Token Current => tokens[Math.Min(position, tokens.Count - 1)];

        HashSet<string> structNames = new HashSet<string>();
        HashSet<string> fieldNames = new HashSet<string>();
        bool hasErrorInStruct = false;
        bool hasErrorInField = false;

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
                            hasErrorInStruct = false;
                            CheckStructDuplicate(Current.Value);
                            if (!hasErrorInStruct)
                            {
                                currentStruct = new StructDeclNode();
                                currentStruct.Name = Current.Value;

                                Structs.Add(currentStruct);
                            }
                            fieldNames.Clear();

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
                            fieldNames.Clear();
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
                            currentType = Current.Value;
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
                        if (Current.Type == TokenType.Variable && Current.Value.Length >= 2)
                        {
                            hasErrorInField = false;
                            string varName = Current.Value.Substring(1);
                            CheckFieldDuplicate(varName);
                            currentVarName = varName;

                            currentField = new FieldDeclNode();
                            currentField.Name = varName;
                            currentField.Type = currentType;
                            if (!hasErrorInStruct)
                            {
                                currentStruct.Fields.Add(currentField);
                            }

                            Next();
                            if (Match(TokenType.Operator, "="))
                            {
                                Next();
                                state = State.S5;
                            }
                            else
                            {
                                state = State.S6;
                            }
                        }
                        else
                        {
                            Error("Ожидалась переменная $name");
                            state = State.ERROR;
                        }
                        break;

                    case State.S5:
                        if (currentType == "array")
                        {
                            CheckArray();
                            if (!hasErrorInStruct && currentField != null)
                            {
                                currentField.Value = "[]";
                            }
                        }
                        else
                        {
                            string value = Current.Value;
                            CheckType(currentType, value);
                            if (!hasErrorInStruct && currentField != null)
                            {
                                currentField.Value = value;
                            }
                        }
                        state = State.S6;
                        Next();
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

                            case RecoverResult.ToDefault:
                                state = State.S5;
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

        private void CheckStructDuplicate(string name)
        {
            if (structNames.Contains(name))
            {
                Error($"Структура '{name}' уже объявлена");
                hasErrorInStruct = true;
            }
            else
            {
                structNames.Add(name);
            }
        }
        private void CheckFieldDuplicate(string name)
        {
            if (fieldNames.Contains(name))
            {
                Error($"Поле '{name}' уже объявлено");
                hasErrorInField = true;
            }
            else
            {
                fieldNames.Add(name);
            }

        }
        void CheckType(string type, string value)
        {
            switch (type)
            {
                case "int":
                    if (!long.TryParse(value, out long temp))
                    {
                        Error("Некорректное целое число");
                    }
                    else if (temp < int.MinValue || temp > int.MaxValue)
                    {
                        Error("Значение выходит за диапазон int");
                    }
                    break;

                case "float":
                    if (!double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out double d))
                    {
                        Error("Некорректное значение float");
                    }
                    else if (double.IsInfinity(d))
                    {
                        Error("Значение выходит за диапазон float");
                    }
                    break;

                case "bool":
                    if (!bool.TryParse(value, out _))
                        Error($"Переменная должна быть типа bool");
                    break;

                case "string":
                    if (!(value.StartsWith("\"") && value.EndsWith("\"")))
                        Error($"Переменная должна быть строкой");
                    break;
            }
        }
        private void CheckArray()
        {
            if (Current.Value != "[")
            {
                Error("Ожидался символ '[' для массива");
                return;
            }
            Next();

            if (Current.Value != "]")
            {
                Error("Ожидался символ ']' для завершения массива");
                return;
            }
        }
        public void PrintAST()
        {
            output.Clear();

            if (Structs == null)
            {
                output.AppendText("AST пуст\n");
                return;
            }
            foreach (var s in Structs)
            {
                PrintNode(s, " ");
                output.AppendText("\n");
            }
        }

        private void PrintNode(StructDeclNode node, string indent)
        {
            output.AppendText($"{indent}StructDeclNode\n");
            output.AppendText($"{indent}├── name: \"{node.Name}\"\n");
            output.AppendText($"{indent}└── fields:\n");

            for (int i = 0; i < node.Fields.Count; i++)
            {
                var field = node.Fields[i];
                bool isLast = (i == node.Fields.Count - 1);

                string prefix = isLast ? "└──" : "├──";

                output.AppendText($"{indent}    {prefix} FieldDeclNode\n");

                if (isLast)
                {
                    output.AppendText($"{indent}        ├── name: \"{field.Name}\"\n");

                    if (field.Value != null)
                    {
                        output.AppendText($"{indent}        ├── type: {field.Type}\n");
                        output.AppendText($"{indent}        └── value: {field.Value}\n");
                    }
                    else
                    {
                        output.AppendText($"{indent}        └── type: {field.Type}\n");
                    }
                }
                else
                {
                    output.AppendText($"{indent}    │   ├── name: \"{field.Name}\"\n");

                    if (field.Value != null)
                    {
                        output.AppendText($"{indent}    │   ├── type: {field.Type}\n");
                        output.AppendText($"{indent}    │   └── value: {field.Value}\n");
                    }
                    else
                    {
                        output.AppendText($"{indent}    │   └── type: {field.Type}\n");
                    }
                }
            }
        }

        public string GenerateDot()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("digraph AST {");
            sb.AppendLine("node [shape=box, style=filled, fillcolor=lightblue];");

            int id = 0;

            foreach (var s in Structs)
            {
                int structId = id++;
                sb.AppendLine($"node{structId} [label=\"Struct\\n{s.Name}\"];");

                foreach (var f in s.Fields)
                {
                    int fieldId = id++;

                    string safeName = Escape(f.Name);
                    string safeType = Escape(f.Type);

                    string label = $"Field\\nName: {safeName}\\nType: {safeType}";

                    if (f.Value != null)
                    {
                        string safeValue = Escape(f.Value);
                        label += $"\\nValue: {safeValue}";
                    }

                    sb.AppendLine($"node{fieldId} [label=\"{label}\", fillcolor=lightgreen];");
                    sb.AppendLine($"node{structId} -> node{fieldId};");
                }
            }

            sb.AppendLine("}");
            return sb.ToString();
        }
        string Escape(string s)
        {
            return s
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"");
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
            ToDefault,
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

                if (Current.Type == TokenType.Operator && Match(TokenType.Operator, "="))
                {
                    Next();
                    return RecoverResult.ToDefault;
                }

                if (Current.Type == TokenType.StringLiteral || Current.Type == TokenType.IntegerLiteral
                    || Current.Type == TokenType.FloatLiteral || Current.Type == TokenType.BooleanLiteral)
                {
                    return RecoverResult.ToDefault;
                }

                //if (Current.Type == TokenType.StringLiteral && Match(TokenType.Separator, "["))

                //if (Current.Type == TokenType.StringLiteral && Match(TokenType.Separator, "]"))

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