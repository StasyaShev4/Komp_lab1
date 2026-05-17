using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komp_lab1
{
    class TACInstruction
    {
        public string Op;
        public string Arg1;
        public string Arg2;
        public string Result;

        public TACInstruction(string op, string arg1, string arg2, string result)
        {
            Op = op;
            Arg1 = arg1;
            Arg2 = arg2;
            Result = result;
        }

        public override string ToString()
        {
            return $"({Op}, {Arg1}, {Arg2}, {Result})";
        }
    }
    internal class TACGenerator
    {
        public List<TACInstruction> tac =
            new List<TACInstruction>();

        public List<TACInstruction> optimizedTac1 =
            new List<TACInstruction>();
        public List<TACInstruction> optimizedTac2 =
            new List<TACInstruction>();
        public void Generate(
            List<StructDeclNode> structs)
        {
            tac.Clear();

            foreach (var s in structs)
            {
                tac.Add(new TACInstruction(
                    "struct",
                    s.Name,
                    "-",
                    "-"
                ));

                foreach (var field in s.Fields)
                {
                    foreach (var instr in field.ExprTAC)
                    {
                        tac.Add(instr);
                    }

                    if (field.ExprResult != null)
                    {
                        tac.Add(new TACInstruction(
                            "assign",
                            field.ExprResult,
                            "-",
                            "$" + field.Name
                        ));
                    }
                    else
                    {
                        tac.Add(new TACInstruction(
                            "declare",
                            field.Type,
                            "-",
                            "$" + field.Name
                        ));
                    }
                }
            }
        }

        public void OptimizeConstantFolding()
        {
            optimizedTac1.Clear();

            Dictionary<string, int> constants =
                new Dictionary<string, int>();

            foreach (var instr in tac)
            {
                if (instr.Op == "+" ||
                    instr.Op == "-" ||
                    instr.Op == "*" ||
                    instr.Op == "/")
                {
                    int left;
                    int right;

                    bool leftConst =
                        int.TryParse(instr.Arg1, out left);

                    if (!leftConst &&
                        constants.ContainsKey(instr.Arg1))
                    {
                        left = constants[instr.Arg1];
                        leftConst = true;
                    }

                    bool rightConst =
                        int.TryParse(instr.Arg2, out right);

                    if (!rightConst &&
                        constants.ContainsKey(instr.Arg2))
                    {
                        right = constants[instr.Arg2];
                        rightConst = true;
                    }

                    if (leftConst && rightConst)
                    {
                        int result = 0;

                        switch (instr.Op)
                        {
                            case "+":
                                result = left + right;
                                break;

                            case "-":
                                result = left - right;
                                break;

                            case "*":
                                result = left * right;
                                break;

                            case "/":
                                if (right != 0)
                                    result = left / right;
                                break;
                        }

                        constants[instr.Result] = result;
                    }
                    else
                    {
                        optimizedTac1.Add(instr);
                    }
                }
                else if (instr.Op == "assign")
                {
                    if (constants.ContainsKey(instr.Arg1))
                    {
                        optimizedTac1.Add(
                            new TACInstruction(
                                "assign",
                                constants[instr.Arg1].ToString(),
                                "-",
                                instr.Result
                            )
                        );
                    }
                    else
                    {
                        optimizedTac1.Add(instr);
                    }
                }
                else
                {
                    optimizedTac1.Add(instr);
                }
            }
        }
        public void OptimizeTempVariables()
        {
            optimizedTac2.Clear();

            for (int i = 0; i < tac.Count; i++)
            {
                TACInstruction current = tac[i];

                if (i < optimizedTac1.Count - 1)
                {
                    TACInstruction next = tac[i + 1];

                    bool isAssign =
                        next.Op == "assign";

                    bool tempUsed =
                        next.Arg1 == current.Result;

                    bool isTemp =
                        current.Result.StartsWith("t");

                    if (isAssign &&
                        tempUsed &&
                        isTemp)
                    {
                        optimizedTac2.Add(
                            new TACInstruction(
                                current.Op,
                                current.Arg1,
                                current.Arg2,
                                next.Result
                            )
                        );

                        i++;
                        continue;
                    }
                }

                optimizedTac2.Add(current);
            }
        }
        public string GetTACText(
            List<TACInstruction> instructions,
            string title,
            string description = "")
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(new string('=', 70));
            sb.AppendLine(title);
            sb.AppendLine(new string('=', 70));

            if (!string.IsNullOrEmpty(description))
            {
                sb.AppendLine(description);
                sb.AppendLine();
            }

            sb.AppendLine(
                $"Количество инструкций: {instructions.Count}");

            sb.AppendLine();

            sb.AppendLine(
                $"| {"OP",-10} | {"ARG1",-15} | {"ARG2",-15} | {"RESULT",-15} |");

            sb.AppendLine(new string('-', 70));

            foreach (var instr in instructions)
            {
                sb.AppendLine(
                    $"| {instr.Op,-10} | {instr.Arg1,-15} | {instr.Arg2,-15} | {instr.Result,-15} |");
            }

            sb.AppendLine();
            sb.AppendLine();

            return sb.ToString();
        }
    }
}
