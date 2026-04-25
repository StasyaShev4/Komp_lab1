using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komp_lab1
{
    internal class Tetrads
    {
        public string Op;
        public string Arg1;
        public string Arg2;
        public string Result;

        public Tetrads(string op, string arg1, string arg2, string result)
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
}
