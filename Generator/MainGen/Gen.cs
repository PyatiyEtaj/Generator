using System;
using System.Collections.Generic;

namespace Generator
{
    public class Gen
    {
        private Parser _pr = new Parser();
        private Random _random = new Random();
        private List<Pair<string, string>> _generated;
        private GenFunctions _genFunctions = new GenFunctions();


        public string Template { get; set; }
        public string Code { get; set; }
        public  List<Pair<string, string>>  Tests{ get; set; }

        // Выполнение необходимой функции (указаны в файле GenFunctions)
        private string CheckF(string str)
        {
            if (str.Contains(GenFunctions.FuncName.Rnd.Value))
            {
                var args = _genFunctions.GetArgs(str, _generated);

                if (args.Length < 4)
                {
                    str = _genFunctions.Rnd(args[0], args[1], args[2], _random);
                }
                else
                {
                    str = _genFunctions.Rnd(args[0], args[1], args[2], _random,args[3]);
                }
            }
            else if (str.Contains(GenFunctions.FuncName.GenAE.Value))
            {
                str = _genFunctions.Expression(str, _generated);
            }
            else if (str.Contains(GenFunctions.FuncName.GetAECode.Value))
            {
                str = _genFunctions.ExpressionCodeOnC();
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