using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Generator.MainGen.Parametr;
using Generator.MainGen.Structs;
using Generator.Parsing;
using Newtonsoft.Json;

namespace Generator.MainGen
{
    public class GenFunctions
    { 
        public Random Random = new Random();
        private ArithmExpr _arithmExpr = new ArithmExpr();

        public string[] GetArgs(string str, List<Param> parametrs)
        {
            int pos = str.IndexOf('(') + 1;
            str = str.Substring(pos, str.LastIndexOf(')') - pos);
            var args = str.Split('|');
            if (parametrs == null) return args;

            for (int i = 0; i < args.Length; i++)
            {
                args[i] = args[i].Trim(' ', '\n', '\r');
                foreach (var elem in parametrs)
                {
                    if (args[i].Contains($"({elem.Name})"))
                    {
                        args[i] = elem.Value;
                    }
                }
            }
            return args;
        }
        // выражения
        public string Expression(string str, List<Param> parametrs)
        {
            var args = GetArgs(str, parametrs);

            if (args.Length != 1) throw new Exception($"Func #{FuncsEnum.genAE} take 1 parametr ( hardnessOfArithmeticExpression )");

            _arithmExpr.Run(Int32.Parse(args[0]));
            return _arithmExpr.Expression;
        }

        public string ExpressionCodeOnC()
        {
            return _arithmExpr.CodeOnC;
        }
        //------------------------------------------------
    

        // Рандомизация ----------------------------------
        private string GenValue(string a, string b, string type, Random r, string count = "1")
        {
            string str;
            try
            {
                if (type == "int")
                {
                    Int32 A = Int32.Parse(a), B = Int32.Parse(b);
                    str = RndWrapper.NextIMass(A, B, Int32.Parse(count), r);
                }
                else
                {
                    Double A = Double.Parse(a.Replace('.', ',')), B = Double.Parse(b.Replace('.', ','));
                    str = RndWrapper.NextDMass(A, B, Int32.Parse(count), r);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"func #{FuncsEnum.rnd} cannot convert values, maybe it has wrong format | error = {e}");
            }            

            return str;
        }

        public string Rnd(string str, List<Param> parametrs = null)
        {
            var args = GetArgs(str, parametrs);

            if (args.Length != 3 && args.Length != 4) throw new Exception($"Func #{FuncsEnum.rnd} take 3(4) parametrs (min | max| type (| count) )");

            string res;

            if (args.Length < 4)
            {
                res = GenValue(args[0], args[1], args[2], Random);
            }
            else
            {
                res = GenValue(args[0], args[1], args[2], Random, args[3]);
            }
            return res;
        }
        //-----------------------------------------
        public bool CheckTests(List<DataContainer> lDc)
        {

            foreach (DataContainer dc in lDc)
            {
                if (dc.Data.Count == 1) continue;

                foreach (string str in dc.Data)
                {
                    if (str.Contains($"#{FuncsEnum.rnd}"))
                    {
                        return false;
                    }
                }

            }

            return true;
        }

        public List<List<string>> GetTestsFromJson(string json)
        {
            List<List<string>> result = new List<List<string>>();

            var tests = JsonConvert.DeserializeObject<List<DataContainer>>(json);

            foreach(var dc in tests)
            {
                if (dc.Data.Count > 1)
                {
                    result.Add(dc.Data);
                }
                else
                {
                    if (dc.Data[0].Contains($"#{FuncsEnum.rnd}"))
                    {
                        result.Add(new List<string>(Rnd(dc.Data[0]).Split(',')));
                    }
                    else
                    {
                        result.Add(dc.Data);
                    }
                }
            }

            return result;
        }

        //---------------------------------------------------

        public string WhatToDoWithParam(FuncsEnum funcs, string rawData, List<Param> parametrs)
        {
            switch(funcs)
            {
                case FuncsEnum.rnd:
                    return Rnd(rawData, parametrs);
                    
                case FuncsEnum.genAE:
                    return Expression(rawData, parametrs);

                case FuncsEnum.getAEcode:
                    return ExpressionCodeOnC();

            }

            return rawData;
        }

        //--------------------------------------------------------

        private List<string> GetParentsFromArg(string arg)
        {
            string[] res;
            if (arg.Contains('&'))
            {
                res = arg.Split('&');
            }
            else
            {
                res = arg.Split(' ');
            }
            return res.Select(x => x.Trim(' ', '\n', '\r')).Where(x => x.Length > 0).ToList();
            //return res.Where(x => x.Length > 0).ToList();
        }

        private List<int> GetPosesFromArg(string arg)
        {
            var poses = arg.Split(' ');
            List<int> res = new List<int>();
            foreach(var s in poses)
            {
                int pos;
                if (!Int32.TryParse(s, out pos)) throw new Exception("func #{FuncsEnum.parent} posOfParam must be Int32");
                res.Add(pos);
            }
            return res;
        }
        
        public async Task<bool> FindParent(Param param, List<Param> parametrs)
        {
            var args = GetArgs(param.RawData, parametrs);

            if (args.Length != 3) throw new Exception($"func #{FuncsEnum.parent} takes 3 parametrs (value | nameOfParent| posOfParam)");

            param.RawData = param.Value = args[0];
            var parents = await Task.Run( () => GetParentsFromArg(args[1]));
            var poses   = await Task.Run( () => GetPosesFromArg(args[2]));
            bool IsItAnd = args[1].Contains('&');
            int count = parents.Count;


            if (parents.Count != poses.Count) throw new Exception($"func #{FuncsEnum.parent} count nameOfParent == count posOfParam");

            foreach (Param p in parametrs)
            {
                int i = parents.FindIndex(0, x => x == p.Name);
                if (i != -1)
                {
                    bool check = poses[i] != p.Position;
                    if (check && IsItAnd)
                    {
                        return false;
                    }
                    if (!check)
                    {
                        parents.RemoveAt(i);
                        poses.RemoveAt(i);
                    }
                }
            }

            if (parents.Count == 0 || (!IsItAnd && parents.Count < count)) return true;

            return false;
        }

    }
}