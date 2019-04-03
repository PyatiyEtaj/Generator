using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Generator
{
    public class Gen
    {
        private Parser _pr = new Parser();
        private ArithmExpr _arithmExpr = new ArithmExpr();
        private Random _random = new Random();
        private List<Pair<string, string>> _generated;
        
        public string Template { get; set; }
        public string Code { get; set; }
        public  List<Pair<string, string>>  Tests{ get; set; }

        private string[] GetArgs(string str)
        {
            int pos = str.IndexOf('(')+1;
            str = str.Substring(pos, str.IndexOf(')') - pos);
            var args = str.Split('|');
            if (_generated == null) return args;
            
            for(int i = 0; i < args.Length; i++)
            {
                args[i] = args[i].Trim(' ', '\n', '\r');
                foreach (var elem in _generated)
                {
                    if (args[i].Contains(elem.First))
                    {
                        args[i] = elem.Second;
                    }
                }
            }
            return args;
        }
        
        private string CheckF(string str)
        {
            if (str.Contains(GenFunctions.Funcs[(int) GenFunctions.FuncName.Rnd]))
            {
                var args = GetArgs(str);
                string count;
                if (args.Length < 4)
                {
                    str = GenFunctions.Rnd(args[0], args[1], args[2], _random);
                }
                else
                {
                    str = GenFunctions.Rnd(args[0], args[1], args[2], _random,args[3]);
                }
            }
            else if (str.Contains(GenFunctions.Funcs[(int) GenFunctions.FuncName.GenAE]))
            {
                var args = GetArgs(str);
                _arithmExpr.Run(Int32.Parse(args[0]));
                str = _arithmExpr.Expression;
            }
            else if (str.Contains(GenFunctions.Funcs[(int) GenFunctions.FuncName.GetAECode]))
            {
                str = _arithmExpr.CodeOnC;
            }
            return str;
        }
        
        private List<Pair<string, string>> ProcessData(List<DataContainer> d, List<Pair<string, string>> ls)
        {
            foreach (var sd in d)
            {
                string res = sd.Data[_random.Next(0, sd.Data.Count)];
                res = CheckF(res);
                ls.Add(new Pair<string, string>(sd.Name, res));
            }
            return ls;
        }
        
        public void Run(string fileName)
        {
            var d = _pr.Read(fileName);
            if (d == null) return;

            _generated = new List<Pair<string, string>>();
            ProcessData(d.Sd, _generated);
            
            var tests = new List<Pair<string, string>>();
            ProcessData(d.TestsD, tests);

            foreach (var elem in _generated)
            {
                var pattern = $"({elem.First})";
                d.Template = d.Template.Replace(pattern, elem.Second);
                d.Code = d.Code.Replace(pattern, elem.Second);
            }

            Template = d.Template;
            Code = d.Code;
            Tests = tests;
        }
        
    }
}